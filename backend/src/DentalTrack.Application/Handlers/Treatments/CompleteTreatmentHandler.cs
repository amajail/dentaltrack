using AutoMapper;
using DentalTrack.Application.Commands.Treatments;
using DentalTrack.Application.DTOs;
using DentalTrack.Domain.Interfaces;
using MediatR;

namespace DentalTrack.Application.Handlers.Treatments;

public class CompleteTreatmentHandler : IRequestHandler<CompleteTreatmentCommand, TreatmentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CompleteTreatmentHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TreatmentDto> Handle(CompleteTreatmentCommand request, CancellationToken cancellationToken)
    {
        var treatment = await _unitOfWork.Treatments.GetByIdAsync(request.TreatmentId, cancellationToken);
        if (treatment == null)
        {
            throw new InvalidOperationException($"Treatment with ID {request.TreatmentId} not found");
        }

        treatment.Complete(request.CompletionDto.ActualCost, request.CompletionDto.Notes);

        await _unitOfWork.Treatments.UpdateAsync(treatment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TreatmentDto>(treatment);
    }
}