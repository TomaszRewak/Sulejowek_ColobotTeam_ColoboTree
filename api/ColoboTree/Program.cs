using ColoboTree.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Database
builder.Services.AddDbContext<ColoboTreeContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), cfg => cfg.UseNetTopologySuite()));

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
    var treesCount = await context.Trees.CountAsync(cancellationToken);

    return $"Current trees count: {treesCount}";
});


app.Run();
