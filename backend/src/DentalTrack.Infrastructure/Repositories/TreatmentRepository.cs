using DentalTrack.Domain.Entities;
using DentalTrack.Domain.Interfaces;
using DentalTrack.Domain.ValueObjects;
using DentalTrack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalTrack.Infrastructure.Repositories;

public class TreatmentRepository : Repository<Treatment>, ITreatmentRepository
{
    public TreatmentRepository(DentalTrackDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Treatment>> GetByPatientIdAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(t => t.PatientId == patientId).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Treatment>> GetByStatusAsync(TreatmentStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(t => t.Status == status).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Treatment>> GetByTypeAsync(TreatmentType type, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(t => t.Type == type).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Treatment>> GetActiveTreatmentsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => t.Status == TreatmentStatus.InProgress)
            .Include(t => t.Patient)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Treatment>> GetCompletedTreatmentsAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(t => t.Status == TreatmentStatus.Completed);

        if (fromDate.HasValue)
        {
            query = query.Where(t => t.EndDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(t => t.EndDate <= toDate.Value);
        }

        return await query.Include(t => t.Patient).ToListAsync(cancellationToken);
    }

    public async Task<Treatment?> GetWithPhotosAsync(Guid treatmentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Photos)
            .FirstOrDefaultAsync(t => t.Id == treatmentId, cancellationToken);
    }

    public async Task<Treatment?> GetWithAnalysesAsync(Guid treatmentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Analyses)
            .FirstOrDefaultAsync(t => t.Id == treatmentId, cancellationToken);
    }

    public async Task<IEnumerable<Treatment>> GetUpcomingTreatmentsAsync(int days = 7, CancellationToken cancellationToken = default)
    {
        var futureDate = DateTime.UtcNow.AddDays(days);
        return await _dbSet
            .Where(t => t.Status == TreatmentStatus.Planned && t.StartDate <= futureDate)
            .Include(t => t.Patient)
            .OrderBy(t => t.StartDate)
            .ToListAsync(cancellationToken);
    }
}