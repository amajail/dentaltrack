using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DentalTrack.Infrastructure.Data;

namespace DentalTrack.Api.Tests.Infrastructure;

/// <summary>
/// Integration test base that uses in-memory database instead of TestContainers
/// Use this for CI environments where Docker is not available
/// </summary>
public class InMemoryIntegrationTestBase : IClassFixture<InMemoryTestWebApplicationFactory>, IAsyncLifetime
{
    protected readonly InMemoryTestWebApplicationFactory Factory;
    protected readonly HttpClient Client;
    protected readonly IServiceScope Scope;
    protected readonly DentalTrackDbContext DbContext;

    public InMemoryIntegrationTestBase(InMemoryTestWebApplicationFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
        Scope = factory.Services.CreateScope();
        DbContext = Scope.ServiceProvider.GetRequiredService<DentalTrackDbContext>();
    }

    public virtual async Task InitializeAsync()
    {
        await DbContext.Database.EnsureCreatedAsync();
    }

    public virtual async Task DisposeAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
        DbContext.Dispose();
        Scope.Dispose();
        Client.Dispose();
    }

    protected async Task<T> ExecuteDbContextAsync<T>(Func<DentalTrackDbContext, Task<T>> action)
    {
        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DentalTrackDbContext>();
        
        var result = await action(context);
        return result;
    }

    protected async Task ExecuteDbContextAsync(Func<DentalTrackDbContext, Task> action)
    {
        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DentalTrackDbContext>();
        
        await action(context);
    }
}

public class InMemoryTestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            services.RemoveAll(typeof(DbContextOptions<DentalTrackDbContext>));
            services.RemoveAll(typeof(DentalTrackDbContext));

            // Add in-memory database for testing
            services.AddDbContext<DentalTrackDbContext>(options =>
            {
                options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}");
                options.EnableSensitiveDataLogging();
            });

            // Add fake authentication for testing
            services.AddAuthentication("Test")
                .AddScheme<TestAuthenticationSchemeOptions, TestAuthenticationHandler>(
                    "Test", options => { });
        });
    }
}