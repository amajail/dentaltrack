using DentalTrack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalTrack.Infrastructure.Tests;

public abstract class TestBase : IDisposable
{
    protected readonly DentalTrackDbContext Context;

    protected TestBase()
    {
        var options = new DbContextOptionsBuilder<DentalTrackDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new DentalTrackDbContext(options);
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context?.Dispose();
    }
}