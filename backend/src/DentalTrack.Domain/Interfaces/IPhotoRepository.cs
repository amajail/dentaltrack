using DentalTrack.Domain.Entities;
using DentalTrack.Domain.ValueObjects;

namespace DentalTrack.Domain.Interfaces;

public interface IPhotoRepository : IRepository<Photo>
{
    Task<IEnumerable<Photo>> GetByTreatmentIdAsync(Guid treatmentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Photo>> GetByTypeAsync(PhotoType type, CancellationToken cancellationToken = default);
    Task<IEnumerable<Photo>> GetByQualityAsync(PhotoQuality quality, CancellationToken cancellationToken = default);
    Task<IEnumerable<Photo>> GetUnprocessedPhotosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Photo>> GetPhotosRequiringReviewAsync(CancellationToken cancellationToken = default);
    Task<Photo?> GetWithAnalysesAsync(Guid photoId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Photo>> GetByToothNumberAsync(int toothNumber, CancellationToken cancellationToken = default);
    Task<long> GetTotalFileSizeAsync(CancellationToken cancellationToken = default);
}