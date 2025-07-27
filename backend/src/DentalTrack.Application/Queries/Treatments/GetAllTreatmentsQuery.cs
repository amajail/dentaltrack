using DentalTrack.Application.Common;
using DentalTrack.Application.DTOs;
using MediatR;

namespace DentalTrack.Application.Queries.Treatments;

public record GetAllTreatmentsQuery : IRequest<PagedResult<TreatmentDto>>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public Guid? PatientId { get; init; }
    public string? Status { get; init; }
    public string? SortBy { get; init; } = "StartDate";
    public bool SortDescending { get; init; } = true;
};