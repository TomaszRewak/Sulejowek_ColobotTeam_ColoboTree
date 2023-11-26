using ColoboTree.Context;
using ColoboTree.Functions;
using ColoboTree.Inputs;
using ColoboTree.Responses;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;

var builder = WebApplication.CreateBuilder(args);
//Database
builder.Services.AddDbContext<ColoboTreeContext>(opt =>
{
    opt.EnableSensitiveDataLogging();
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), cfg => cfg.UseNetTopologySuite());
});
builder.Services.ConfigureHttpJsonOptions(x => x.SerializerOptions.Converters.Add(new GeoJsonConverterFactory()));
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

app.UseCors("corsapp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", async (ColoboTreeContext context, CancellationToken cancellationToken) =>
{
    var testArea = await context.AreaChunks.FirstOrDefaultAsync(cancellationToken);

    return testArea;
});

app.MapPost("/chunks", async (GetAreaChunksInput input, ColoboTreeContext context, CancellationToken cancellationToken) =>
{ 
    var (yMin, xMin, yMax, xMax) = input;
    GeometryFactory geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);
    
    var rectangleCoordinates = new[]
    {
        new Coordinate(xMin, yMin),
        new Coordinate(xMin, yMax),
        new Coordinate(xMax, yMax),
        new Coordinate(xMax, yMin),
        new Coordinate(xMin, yMin)
    };
    
    if(IsCounterClockwise(rectangleCoordinates))
        rectangleCoordinates = rectangleCoordinates.Reverse().ToArray();
    
    var rectangle = geometryFactory.CreatePolygon(rectangleCoordinates);
    var rectangleResolution = ResolutionFilter.GetResolutionForRectangle(rectangle);
    
    var chunks = await context.AreaChunks
        .Where(x => rectangle.Contains(x.BottomRightVertex4326) && rectangle.Contains(x.UpperLeftVertex4326) 
                                                                && x.Resolution == 1)
        .Select(x => new
        {
            Response = new AreaChunkResponse(x.Id, x.UpperLeftVertex4326, x.BottomRightVertex4326, x.TreeId),
            x.TreeCoveragePercentage
        })
        .ToListAsync(cancellationToken);
    
    var rectangleArea = AreaCalculator.CalculateArea(rectangle);
    var treeCoverage = (double)chunks.Sum(x => x.TreeCoveragePercentage ?? 0) * Math.Pow(rectangleResolution, 2) / rectangleArea;

    return new GetAreaResponse(rectangleArea, treeCoverage, chunks.Select(x => x.Response).ToList());
});


app.MapPost("/plot", async (GetPlotInput input, ColoboTreeContext context, CancellationToken cancellationToken) =>
{
    GeometryFactory geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);

    var rectangleCoordinates = input.polygon.Select(point => new Coordinate(point.Y, point.X)).ToArray();

    if (IsCounterClockwise(rectangleCoordinates))
        rectangleCoordinates = rectangleCoordinates.Reverse().ToArray();

    var rectangle = geometryFactory.CreatePolygon(rectangleCoordinates);
    var chunks = await context.AreaChunks
            .Where(x => rectangle.Contains(x.BottomRightVertex4326) && rectangle.Contains(x.UpperLeftVertex4326)
                                                                    && x.Resolution == 1)
            .Select(x => new
            {
                Response = new AreaChunkResponse(x.Id, x.UpperLeftVertex4326, x.BottomRightVertex4326, x.TreeId),
                x.TreeCoveragePercentage
            })
            .ToListAsync(cancellationToken);

    var metricPoints = input.polygon.Select(point =>
    {
        var converted = CoordinateConverter.ConvertLatLonToUTM(point.X, point.Y);
        return new Coordinate(converted.Easting, converted.Northing);
    }).ToArray();

    var metricRectangle = geometryFactory.CreatePolygon(metricPoints);
    var treeCoverage = (double)chunks.Sum(x => x.TreeCoveragePercentage ?? 0) / metricRectangle.Area;
    var co2 = chunks.GroupBy(c => c.Response.TreeId)
        .Select(x => Co2SequestrationFunctions.CalculateTreeLifetimeCo2Sequestration(x.Count())).Sum();

    return new GetPlotResponse(metricRectangle.Area, treeCoverage, co2);
});

bool IsCounterClockwise(Coordinate[] coordinates)
{
    double sum = 0;

    for (int i = 0; i < coordinates.Length - 1; i++)
    {
        sum += (coordinates[i + 1].X - coordinates[i].X) * (coordinates[i + 1].Y + coordinates[i].Y);
    }

    return sum > 0;
}

app.Run();
