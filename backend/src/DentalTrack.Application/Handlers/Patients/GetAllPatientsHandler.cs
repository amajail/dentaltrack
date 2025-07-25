using AutoMapper;
using DentalTrack.Application.DTOs;
using DentalTrack.Application.Queries.Patients;
using DentalTrack.Domain.Interfaces;
using MediatR;

namespace DentalTrack.Application.Handlers.Patients;

public class GetAllPatientsHandler : IRequestHandler<GetAllPatientsQuery, IEnumerable<PatientDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllPatientsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PatientDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
    {
        var patients = await _unitOfWork.Patients.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<PatientDto>>(patients);
    }
}