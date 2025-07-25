using DentalTrack.Application.DTOs;
using MediatR;

namespace DentalTrack.Application.Queries.Treatments;

public record GetTreatmentsByPatientQuery(Guid PatientId) : IRequest<IEnumerable<TreatmentDto>>;