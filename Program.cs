using DeskFlow.API.Data;
using Microsoft.EntityFrameworkCore;
using DeskFlow.API.Repositories;
using DeskFlow.API.Services;
using DeskFlow.API.Middlewares;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configuração do JSON: Converte Enums para texto e evita erros de loop infinito nos relacionamentos
builder.Services.AddOpenApi(options =>
{
    options.AddSchemaTransformer((schema, context, cancellationToken) =>
    {
        if (schema.Type.HasValue && schema.Type.Value.HasFlag(Microsoft.OpenApi.JsonSchemaType.Integer))
        {
            schema.Type = Microsoft.OpenApi.JsonSchemaType.Integer;
            schema.Pattern = null;
        }
        return Task.CompletedTask;
    });
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {

        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());


        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Configuração da conexão com o banco de dados SQL Server via Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Injeção de Dependências: Registra Repositórios e Serviços no ciclo de vida Scoped    
builder.Services.AddScoped<CategoriaRepository>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ChamadoRepository>();
builder.Services.AddScoped<ChamadoService>();

// RNF03: Middleware customizado para captura global de exceções e retorno de JSON limpo
var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapOpenApi();

app.UseSwaggerUI(op =>
{
    op.SwaggerEndpoint("/openapi/v1.json", "v1");
});

app.UseHttpsRedirection();

app.MapControllers();
app.Run();