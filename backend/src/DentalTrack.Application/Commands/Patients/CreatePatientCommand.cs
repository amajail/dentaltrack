using DentalTrack.Application.DTOs;
using MediatR;

namespace DentalTrack.Application.Commands.Patients;

public record CreatePatientCommand(CreatePatientDto PatientDto) : IRequest<PatientDto>;