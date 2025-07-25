using AutoMapper;
using DentalTrack.Application.DTOs;
using DentalTrack.Application.Queries.Treatments;
using DentalTrack.Domain.Interfaces;
using MediatR;

namespace DentalTrack.Application.Handlers.Treatments;

public class GetTreatmentsByPatientHandler : IRequestHandler<GetTreatmentsByPatientQuery, IEnumerable<TreatmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTreatmentsByPatientHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TreatmentDto>> Handle(GetTreatmentsByPatientQuery request, CancellationToken cancellationToken)
    {
        var treatments = await _unitOfWork.Treatments.GetByPatientIdAsync(request.PatientId, cancellationToken);
        return _mapper.Map<IEnumerable<TreatmentDto>>(treatments);
    }
}