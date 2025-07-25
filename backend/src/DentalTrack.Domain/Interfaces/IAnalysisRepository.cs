using DentalTrack.Domain.Entities;
using DentalTrack.Domain.ValueObjects;

namespace DentalTrack.Domain.Interfaces;

public interface IAnalysisRepository : IRepository<Analysis>
{
    Task<IEnumerable<Analysis>> GetByPhotoIdAsync(Guid photoId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Analysis>> GetByTypeAsync(AnalysisType type, CancellationToken cancellationToken = default);
    Task<IEnumerable<Analysis>> GetByStatusAsync(AnalysisStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Analysis>> GetPendingAnalysesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Analysis>> GetFailedAnalysesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Analysis>> GetCompletedAnalysesAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<Analysis>> GetHighConfidenceAnalysesAsync(decimal minConfidence = 0.8m, CancellationToken cancellationToken = default);
    Task<double> GetAverageProcessingTimeAsync(AnalysisType? type = null, CancellationToken cancellationToken = default);
}