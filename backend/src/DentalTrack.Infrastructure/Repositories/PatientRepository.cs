using DentalTrack.Domain.Entities;
using DentalTrack.Domain.Interfaces;
using DentalTrack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalTrack.Infrastructure.Repositories;

public class PatientRepository : Repository<Patient>, IPatientRepository
{
    public PatientRepository(DentalTrackDbContext context) : base(context)
    {
    }

    public async Task<Patient?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<Patient>> GetActivePatients(CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(p => p.IsActive).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Patient>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.FirstName.Contains(searchTerm) || p.LastName.Contains(searchTerm))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Patient>> GetPatientsWithActiveTreatmentsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Treatments)
            .Where(p => p.Treatments.Any(t => t.Status == Domain.ValueObjects.TreatmentStatus.InProgress))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, Guid? excludePatientId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(p => p.Email == email);

        if (excludePatientId.HasValue)
        {
            query = query.Where(p => p.Id != excludePatientId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }
}