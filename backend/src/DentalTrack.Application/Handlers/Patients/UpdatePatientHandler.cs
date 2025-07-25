using AutoMapper;
using DentalTrack.Application.Commands.Patients;
using DentalTrack.Application.DTOs;
using DentalTrack.Domain.Interfaces;
using MediatR;

namespace DentalTrack.Application.Handlers.Patients;

public class UpdatePatientHandler : IRequestHandler<UpdatePatientCommand, PatientDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePatientHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PatientDto> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(request.PatientId, cancellationToken);
        if (patient == null)
        {
            throw new InvalidOperationException($"Patient with ID {request.PatientId} not found");
        }

        if (await _unitOfWork.Patients.EmailExistsAsync(request.PatientDto.Email, request.PatientId, cancellationToken))
        {
            throw new InvalidOperationException($"Patient with email {request.PatientDto.Email} already exists");
        }

        patient.UpdatePersonalInfo(
            request.PatientDto.FirstName,
            request.PatientDto.LastName,
            request.PatientDto.Email,
            request.PatientDto.DateOfBirth,
            request.PatientDto.Phone,
            request.PatientDto.Address);

        patient.UpdateMedicalInfo(
            request.PatientDto.EmergencyContact,
            request.PatientDto.EmergencyPhone,
            request.PatientDto.MedicalHistory,
            request.PatientDto.Allergies);

        await _unitOfWork.Patients.UpdateAsync(patient, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PatientDto>(patient);
    }
}