using DentalTrack.Application.DTOs;
using MediatR;

namespace DentalTrack.Application.Queries.Treatments;

public record GetAllTreatmentsQuery : IRequest<IEnumerable<TreatmentDto>>;