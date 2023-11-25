using ColoboTree.Context;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.IO.Converters;

var builder = WebApplication.CreateBuilder(args);
//Database
builder.Services.AddDbContext<ColoboTreeContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), cfg => cfg.UseNetTopologySuite()));
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


app.Run();
