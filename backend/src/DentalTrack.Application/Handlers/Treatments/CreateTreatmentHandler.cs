using AutoMapper;
using DentalTrack.Application.Commands.Treatments;
using DentalTrack.Application.DTOs;
using DentalTrack.Domain.Entities;
using DentalTrack.Domain.Interfaces;
using MediatR;

namespace DentalTrack.Application.Handlers.Treatments;

public class CreateTreatmentHandler : IRequestHandler<CreateTreatmentCommand, TreatmentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTreatmentHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TreatmentDto> Handle(CreateTreatmentCommand request, CancellationToken cancellationToken)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(request.TreatmentDto.PatientId, cancellationToken);
        if (patient == null)
        {
            throw new InvalidOperationException($"Patient with ID {request.TreatmentDto.PatientId} not found");
        }

        var treatment = _mapper.Map<Treatment>(request.TreatmentDto);
        
        await _unitOfWork.Treatments.AddAsync(treatment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TreatmentDto>(treatment);
    }
}