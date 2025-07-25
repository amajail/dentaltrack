using DentalTrack.Application.DTOs;
using MediatR;

namespace DentalTrack.Application.Commands.Treatments;

public record StartTreatmentCommand(Guid TreatmentId) : IRequest<TreatmentDto>;