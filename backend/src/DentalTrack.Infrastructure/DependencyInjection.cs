using DentalTrack.Domain.Interfaces;
using DentalTrack.Infrastructure.Data;
using DentalTrack.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DentalTrack.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Entity Framework
        services.AddDbContext<DentalTrackDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Add repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<ITreatmentRepository, TreatmentRepository>();
        services.AddScoped<IPhotoRepository, PhotoRepository>();
        services.AddScoped<IAnalysisRepository, AnalysisRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Add data seeder
        services.AddScoped<DataSeeder>();

        // Add Health Checks
        services.AddHealthChecks()
            .AddDbContextCheck<DentalTrackDbContext>();

        return services;
    }

    public static async Task<IServiceProvider> ApplyMigrationsAndSeedAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DentalTrackDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DataSeeder>>();
        
        try
        {
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migrations applied successfully");
            
            var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
            await seeder.SeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while setting up the database");
            throw;
        }

        return serviceProvider;
    }
}