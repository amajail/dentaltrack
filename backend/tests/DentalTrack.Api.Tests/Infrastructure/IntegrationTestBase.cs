using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DentalTrack.Infrastructure.Data;
using Testcontainers.MsSql;
using System.Data.Common;

namespace DentalTrack.Api.Tests.Infrastructure;

public class IntegrationTestBase : IClassFixture<IntegrationTestWebApplicationFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestWebApplicationFactory Factory;
    protected readonly HttpClient Client;
    protected readonly IServiceScope Scope;
    protected readonly DentalTrackDbContext DbContext;

    public IntegrationTestBase(IntegrationTestWebApplicationFactory factory)
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

public class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("DentalTrack123!")
        .WithCleanUp(true)
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            services.RemoveAll(typeof(DbContextOptions<DentalTrackDbContext>));
            services.RemoveAll(typeof(DentalTrackDbContext));

            // Add the test database connection
            services.AddDbContext<DentalTrackDbContext>(options =>
            {
                options.UseSqlServer(_dbContainer.GetConnectionString());
            });

            // Add fake authentication for testing
            services.AddAuthentication("Test")
                .AddScheme<TestAuthenticationSchemeOptions, TestAuthenticationHandler>(
                    "Test", options => { });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await base.DisposeAsync();
    }
}