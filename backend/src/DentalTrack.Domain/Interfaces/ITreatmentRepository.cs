using DentalTrack.Domain.Entities;
using DentalTrack.Domain.ValueObjects;

namespace DentalTrack.Domain.Interfaces;

public interface ITreatmentRepository : IRepository<Treatment>
{
    Task<IEnumerable<Treatment>> GetByPatientIdAsync(Guid patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Treatment>> GetByStatusAsync(TreatmentStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Treatment>> GetByTypeAsync(TreatmentType type, CancellationToken cancellationToken = default);
    Task<IEnumerable<Treatment>> GetActiveTreatmentsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Treatment>> GetCompletedTreatmentsAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<Treatment?> GetWithPhotosAsync(Guid treatmentId, CancellationToken cancellationToken = default);
    Task<Treatment?> GetWithAnalysesAsync(Guid treatmentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Treatment>> GetUpcomingTreatmentsAsync(int days = 7, CancellationToken cancellationToken = default);
    Task<(IList<Treatment> Items, int TotalCount)> GetPagedAsync(
        int page, 
        int pageSize, 
        Guid? patientId = null, 
        string? status = null, 
        string? sortBy = null, 
        bool sortDescending = false, 
        CancellationToken cancellationToken = default);
}