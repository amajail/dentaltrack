using DentalTrack.Api.Middleware;
using DentalTrack.Application.Handlers.Patients;
using DentalTrack.Application.Mappings;
using DentalTrack.Domain.Interfaces;
using DentalTrack.Infrastructure.Data;
using DentalTrack.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
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

// Add Entity Framework
builder.Services.AddDbContext<DentalTrackDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePatientHandler).Assembly));

// Add FluentValidation (if needed - comment out if not available)
// builder.Services.AddFluentValidationAutoValidation();

// Add repositories and Unit of Work
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<ITreatmentRepository, TreatmentRepository>();
builder.Services.AddScoped<IPhotoRepository, PhotoRepository>();
builder.Services.AddScoped<IAnalysisRepository, AnalysisRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<DentalTrackDbContext>();

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
        Description = "A dental practice management system API built with Clean Architecture"
    });
    
    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
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

// Auto-migrate database in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DentalTrackDbContext>();
    try
    {
        context.Database.EnsureCreated();
        Log.Information("Database ensured created successfully");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while creating the database");
    }
}

Log.Information("DentalTrack API starting up...");

app.Run();
