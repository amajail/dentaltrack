using DentalTrack.Application.Common;
using DentalTrack.Application.DTOs;
using MediatR;

namespace DentalTrack.Application.Queries.Patients;

public record GetAllPatientsQuery : IRequest<PagedResult<PatientDto>>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? Search { get; init; }
    public string? SortBy { get; init; } = "LastName";
    public bool SortDescending { get; init; } = false;
};