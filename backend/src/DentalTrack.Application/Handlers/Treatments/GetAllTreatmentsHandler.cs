using AutoMapper;
using DentalTrack.Application.Common;
using DentalTrack.Application.DTOs;
using DentalTrack.Application.Queries.Treatments;
using DentalTrack.Domain.Interfaces;
using MediatR;

namespace DentalTrack.Application.Handlers.Treatments;

public class GetAllTreatmentsHandler : IRequestHandler<GetAllTreatmentsQuery, PagedResult<TreatmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllTreatmentsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<TreatmentDto>> Handle(GetAllTreatmentsQuery request, CancellationToken cancellationToken)
    {
        var treatments = await _unitOfWork.Treatments.GetPagedAsync(
            page: request.Page,
            pageSize: request.PageSize,
            patientId: request.PatientId,
            status: request.Status,
            sortBy: request.SortBy,
            sortDescending: request.SortDescending,
            cancellationToken: cancellationToken);

        var treatmentDtos = _mapper.Map<IList<TreatmentDto>>(treatments.Items);
        
        return new PagedResult<TreatmentDto>(treatmentDtos, treatments.TotalCount, request.Page, request.PageSize);
    }
}