using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Middleware;
using PortfolioApi.Services;

var builder = WebApplication.CreateBuilder(args);

const string CorsPolicy = "FrontendPolicy";

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Banco SQLite em arquivo local (simples para o workshop).
builder.Services.AddDbContext<PortfolioContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=portfolio.db"));

builder.Services.AddScoped<AtivoService>();
builder.Services.AddScoped<OrdemService>();
builder.Services.AddScoped<WatchlistService>();
builder.Services.AddScoped<PortfolioService>();
builder.Services.AddSingleton<ICotacaoProvider, CotacaoSimuladaProvider>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Cria o banco e popula dados de exemplo.
// EnsureCreated() é adequado para o workshop (recria o schema conforme o modelo).
// Para evolução controlada de schema em produção, migrar para EF Core Migrations
// (dotnet ef migrations add / context.Database.Migrate()).
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PortfolioContext>();
    context.Database.EnsureCreated();
    DbSeeder.Seed(context);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsPolicy);
app.MapControllers();

app.Run();

// Necessário para os testes de integração (WebApplicationFactory).
public partial class Program { }
