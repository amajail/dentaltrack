using DentalTrack.Domain.Entities;
using DentalTrack.Domain.Interfaces;
using DentalTrack.Domain.ValueObjects;
using DentalTrack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalTrack.Infrastructure.Repositories;

public class AnalysisRepository : Repository<Analysis>, IAnalysisRepository
{
    public AnalysisRepository(DentalTrackDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Analysis>> GetByPhotoIdAsync(Guid photoId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(a => a.PhotoId == photoId).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Analysis>> GetByTypeAsync(AnalysisType type, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(a => a.Type == type).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Analysis>> GetByStatusAsync(AnalysisStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(a => a.Status == status).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Analysis>> GetPendingAnalysesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.Status == AnalysisStatus.Pending)
            .Include(a => a.Photo)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Analysis>> GetFailedAnalysesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.Status == AnalysisStatus.Failed)
            .Include(a => a.Photo)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Analysis>> GetCompletedAnalysesAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(a => a.Status == AnalysisStatus.Completed);

        if (fromDate.HasValue)
        {
            query = query.Where(a => a.CompletedAt >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(a => a.CompletedAt <= toDate.Value);
        }

        return await query.Include(a => a.Photo).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Analysis>> GetHighConfidenceAnalysesAsync(decimal minConfidence = 0.8m, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.Status == AnalysisStatus.Completed && a.ConfidenceScore >= minConfidence)
            .ToListAsync(cancellationToken);
    }

    public async Task<double> GetAverageProcessingTimeAsync(AnalysisType? type = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(a => a.Status == AnalysisStatus.Completed);

        if (type.HasValue)
        {
            query = query.Where(a => a.Type == type.Value);
        }

        return await query.AverageAsync(a => a.ProcessingTimeMs, cancellationToken);
    }
}