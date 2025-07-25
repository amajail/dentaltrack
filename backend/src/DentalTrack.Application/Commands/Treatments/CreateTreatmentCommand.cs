using DentalTrack.Application.DTOs;
using MediatR;

namespace DentalTrack.Application.Commands.Treatments;

public record CreateTreatmentCommand(CreateTreatmentDto TreatmentDto) : IRequest<TreatmentDto>;