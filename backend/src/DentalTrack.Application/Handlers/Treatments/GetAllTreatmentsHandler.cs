using AutoMapper;
using DentalTrack.Application.DTOs;
using DentalTrack.Application.Queries.Treatments;
using DentalTrack.Domain.Interfaces;
using MediatR;

namespace DentalTrack.Application.Handlers.Treatments;

public class GetAllTreatmentsHandler : IRequestHandler<GetAllTreatmentsQuery, IEnumerable<TreatmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllTreatmentsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TreatmentDto>> Handle(GetAllTreatmentsQuery request, CancellationToken cancellationToken)
    {
        var treatments = await _unitOfWork.Treatments.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<TreatmentDto>>(treatments);
    }
}