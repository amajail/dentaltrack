using AutoMapper;
using DentalTrack.Application.Common;
using DentalTrack.Application.DTOs;
using DentalTrack.Application.Queries.Patients;
using DentalTrack.Domain.Interfaces;
using MediatR;

namespace DentalTrack.Application.Handlers.Patients;

public class GetAllPatientsHandler : IRequestHandler<GetAllPatientsQuery, PagedResult<PatientDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllPatientsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<PatientDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
    {
        var patients = await _unitOfWork.Patients.GetPagedAsync(
            page: request.Page,
            pageSize: request.PageSize,
            search: request.Search,
            sortBy: request.SortBy,
            sortDescending: request.SortDescending,
            cancellationToken: cancellationToken);

        var patientDtos = _mapper.Map<IList<PatientDto>>(patients.Items);

        return new PagedResult<PatientDto>(patientDtos, patients.TotalCount, request.Page, request.PageSize);
    }
}