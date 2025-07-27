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

    public async Task<(IList<Patient> Items, int TotalCount)> GetPagedAsync(
        int page, 
        int pageSize, 
        string? search = null, 
        string? sortBy = null, 
        bool sortDescending = false, 
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(p => p.IsActive);

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => 
                p.FirstName.Contains(search) || 
                p.LastName.Contains(search) || 
                p.Email.Contains(search) ||
                p.Phone.Contains(search));
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply sorting
        query = sortBy?.ToLower() switch
        {
            "firstname" => sortDescending ? query.OrderByDescending(p => p.FirstName) : query.OrderBy(p => p.FirstName),
            "lastname" => sortDescending ? query.OrderByDescending(p => p.LastName) : query.OrderBy(p => p.LastName),
            "email" => sortDescending ? query.OrderByDescending(p => p.Email) : query.OrderBy(p => p.Email),
            "dateofbirth" => sortDescending ? query.OrderByDescending(p => p.DateOfBirth) : query.OrderBy(p => p.DateOfBirth),
            "createdat" => sortDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
            _ => sortDescending ? query.OrderByDescending(p => p.LastName) : query.OrderBy(p => p.LastName)
        };

        // Apply pagination
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}