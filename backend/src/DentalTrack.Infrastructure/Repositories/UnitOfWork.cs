using DentalTrack.Domain.Interfaces;
using DentalTrack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace DentalTrack.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DentalTrackDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(DentalTrackDbContext context)
    {
        _context = context;
        Patients = new PatientRepository(_context);
        Treatments = new TreatmentRepository(_context);
        Photos = new PhotoRepository(_context);
        Analyses = new AnalysisRepository(_context);
        Users = new UserRepository(_context);
    }

    public IPatientRepository Patients { get; }
    public ITreatmentRepository Treatments { get; }
    public IPhotoRepository Photos { get; }
    public IAnalysisRepository Analyses { get; }
    public IUserRepository Users { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}