using MediatR;

namespace DentalTrack.Application.Commands.Patients;

public record DeletePatientCommand(Guid PatientId) : IRequest<bool>;