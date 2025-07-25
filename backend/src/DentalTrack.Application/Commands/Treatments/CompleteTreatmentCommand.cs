using DentalTrack.Application.DTOs;
using MediatR;

namespace DentalTrack.Application.Commands.Treatments;

public record CompleteTreatmentCommand(Guid TreatmentId, CompleteTreatmentDto CompletionDto) : IRequest<TreatmentDto>;