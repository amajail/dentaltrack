using DentalTrack.Api.Middleware;
using DentalTrack.Application;
using DentalTrack.Infrastructure;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/dentaltrack-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();

// Add layer dependencies
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("DentalTrackPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3001") // React frontend
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "DentalTrack API",
        Version = "v1",
        Description = "A comprehensive dental practice management system API built with Clean Architecture. " +
                     "Provides endpoints for managing patients, treatments, photos, and AI-powered dental analysis.",
        Contact = new() { Name = "DentalTrack Team", Email = "support@dentaltrack.com" },
        License = new() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
    });

    // Include XML comments for better documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // Add response examples and better schemas
    c.EnableAnnotations();
    c.UseInlineDefinitionsForEnums();

    // Group endpoints by tags
    c.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });
    c.DocInclusionPredicate((name, api) => true);
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DentalTrack API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

// Use middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("DentalTrackPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Map health check endpoints
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new()
{
    Predicate = check => check.Tags.Contains("ready")
});
app.MapHealthChecks("/health/live", new()
{
    Predicate = _ => false
});

// Auto-migrate database and seed data in development
if (app.Environment.IsDevelopment())
{
    await app.Services.ApplyMigrationsAndSeedAsync();
}

Log.Information("DentalTrack API starting up...");

app.Run();

// Make Program class accessible for integration testing
public partial class Program { }
