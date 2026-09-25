using DeskFlow.API.Data;
using Microsoft.EntityFrameworkCore;
using DeskFlow.API.Repositories;
using DeskFlow.API.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {

        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());


        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<CategoriaRepository>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ChamadoRepository>();
builder.Services.AddScoped<ChamadoService>();

var app = builder.Build();

app.MapOpenApi();

app.UseSwaggerUI(op =>
{
    op.SwaggerEndpoint("/openapi/v1.json", "v1");
});

app.UseHttpsRedirection();

app.MapControllers();
app.Run();