using DentalTrack.Application.DTOs;
using MediatR;

namespace DentalTrack.Application.Queries.Patients;

public record GetPatientByIdQuery(Guid PatientId) : IRequest<PatientDto?>;