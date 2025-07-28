using AutoMapper;
using DentalTrack.Application.Commands.Patients;
using DentalTrack.Application.DTOs;
using DentalTrack.Domain.Entities;
using DentalTrack.Domain.Interfaces;
using MediatR;

namespace DentalTrack.Application.Handlers.Patients;

public class CreatePatientHandler : IRequestHandler<CreatePatientCommand, PatientDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePatientHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PatientDto> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Patients.EmailExistsAsync(request.PatientDto.Email, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException($"Patient with email {request.PatientDto.Email} already exists");
        }

        var patient = _mapper.Map<Patient>(request.PatientDto);

        await _unitOfWork.Patients.AddAsync(patient, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PatientDto>(patient);
    }
}