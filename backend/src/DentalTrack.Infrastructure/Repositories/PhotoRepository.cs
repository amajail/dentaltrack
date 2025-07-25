using DentalTrack.Domain.Entities;
using DentalTrack.Domain.Interfaces;
using DentalTrack.Domain.ValueObjects;
using DentalTrack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalTrack.Infrastructure.Repositories;

public class PhotoRepository : Repository<Photo>, IPhotoRepository
{
    public PhotoRepository(DentalTrackDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Photo>> GetByTreatmentIdAsync(Guid treatmentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(p => p.TreatmentId == treatmentId).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Photo>> GetByTypeAsync(PhotoType type, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(p => p.Type == type).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Photo>> GetByQualityAsync(PhotoQuality quality, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(p => p.Quality == quality).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Photo>> GetUnprocessedPhotosAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(p => !p.IsProcessed).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Photo>> GetPhotosRequiringReviewAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.Quality == PhotoQuality.Pending || p.Quality == PhotoQuality.Low)
            .ToListAsync(cancellationToken);
    }

    public async Task<Photo?> GetWithAnalysesAsync(Guid photoId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Analyses)
            .FirstOrDefaultAsync(p => p.Id == photoId, cancellationToken);
    }

    public async Task<IEnumerable<Photo>> GetByToothNumberAsync(int toothNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(p => p.ToothNumber == toothNumber).ToListAsync(cancellationToken);
    }

    public async Task<long> GetTotalFileSizeAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.SumAsync(p => p.FileSize, cancellationToken);
    }
}