using System.Diagnostics;
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

var app = builder.Build();

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
    const int chunkAreaInSquareMeters = 1;
    
    var (yMin, xMin, yMax, xMax) = input;
    GeometryFactory geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);
    
    var rectangleCoordinates = new[]
    {
        new Coordinate(yMin, xMin),
        new Coordinate(yMin, xMax),
        new Coordinate(yMax, xMax),
        new Coordinate(yMax, xMin),
        new Coordinate(yMin, xMin)
    };
    
    bool IsCounterClockwise(Coordinate[] coordinates)
    {
        double sum = 0;

        for (int i = 0; i < coordinates.Length - 1; i++)
        {
            sum += (coordinates[i + 1].X - coordinates[i].X) * (coordinates[i + 1].Y + coordinates[i].Y);
        }

        return sum > 0;
    }
    
    if(IsCounterClockwise(rectangleCoordinates))
        rectangleCoordinates = rectangleCoordinates.Reverse().ToArray();
    
    var rectangle = geometryFactory.CreatePolygon(rectangleCoordinates);
    
    var chunks = await context.AreaChunks
        .Where(x => rectangle.Contains(x.BottomRightVertex4326) && rectangle.Contains(x.UpperLeftVertex4326) 
                                                                && x.TreeClassification == 5)
        .Select(x => new AreaChunkResponse(x.Id, x.UpperLeftVertex4326, x.BottomRightVertex4326, x.TreeId))
        .ToListAsync(cancellationToken);
    
    var rectangleArea = AreaCalculator.CalculateArea(rectangle);
    var treeCoverage = chunks.Count * chunkAreaInSquareMeters / rectangleArea;

    return new GetAreaResponse(rectangleArea, treeCoverage, chunks);
});

app.Run();
