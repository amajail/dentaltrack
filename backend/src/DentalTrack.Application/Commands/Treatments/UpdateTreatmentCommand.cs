using DentalTrack.Application.DTOs;
using MediatR;

namespace DentalTrack.Application.Commands.Treatments;

public record UpdateTreatmentCommand(Guid TreatmentId, UpdateTreatmentDto TreatmentDto) : IRequest<TreatmentDto>;