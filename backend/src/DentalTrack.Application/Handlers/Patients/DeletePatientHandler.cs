using DentalTrack.Application.Commands.Patients;
using DentalTrack.Domain.Interfaces;
using MediatR;

namespace DentalTrack.Application.Handlers.Patients;

public class DeletePatientHandler : IRequestHandler<DeletePatientCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePatientHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(request.PatientId, cancellationToken);
        if (patient == null)
        {
            return false;
        }

        // Check if patient has any active treatments
        var activeTreatments = await _unitOfWork.Treatments.GetByPatientIdAsync(request.PatientId, cancellationToken);
        var hasActiveTreatments = activeTreatments.Any(t => t.IsActive());

        if (hasActiveTreatments)
        {
            throw new InvalidOperationException("Cannot delete patient with active treatments. Please complete or cancel all treatments first.");
        }

        // Soft delete by deactivating the patient instead of hard delete
        patient.Deactivate();
        await _unitOfWork.Patients.UpdateAsync(patient, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}