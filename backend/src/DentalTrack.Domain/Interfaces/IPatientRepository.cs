using DentalTrack.Domain.Entities;

namespace DentalTrack.Domain.Interfaces;

public interface IPatientRepository : IRepository<Patient>
{
    Task<Patient?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Patient>> GetActivePatients(CancellationToken cancellationToken = default);
    Task<IEnumerable<Patient>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<Patient>> GetPatientsWithActiveTreatmentsAsync(CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, Guid? excludePatientId = null, CancellationToken cancellationToken = default);
}