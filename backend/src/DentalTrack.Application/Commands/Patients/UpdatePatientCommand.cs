using DentalTrack.Application.DTOs;
using MediatR;

namespace DentalTrack.Application.Commands.Patients;

public record UpdatePatientCommand(Guid PatientId, UpdatePatientDto PatientDto) : IRequest<PatientDto>;