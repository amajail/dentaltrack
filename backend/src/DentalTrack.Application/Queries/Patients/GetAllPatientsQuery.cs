using DentalTrack.Application.DTOs;
using MediatR;

namespace DentalTrack.Application.Queries.Patients;

public record GetAllPatientsQuery : IRequest<IEnumerable<PatientDto>>;