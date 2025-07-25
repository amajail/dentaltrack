using AutoMapper;
using DentalTrack.Application.DTOs;
using DentalTrack.Application.Queries.Patients;
using DentalTrack.Domain.Interfaces;
using MediatR;

namespace DentalTrack.Application.Handlers.Patients;

public class GetPatientByIdHandler : IRequestHandler<GetPatientByIdQuery, PatientDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPatientByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PatientDto?> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(request.PatientId, cancellationToken);
        return patient == null ? null : _mapper.Map<PatientDto>(patient);
    }
}