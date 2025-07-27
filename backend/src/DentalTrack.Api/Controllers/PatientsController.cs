using DentalTrack.Application.Commands.Patients;
using DentalTrack.Application.DTOs;
using DentalTrack.Application.Queries.Patients;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DentalTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetAllPatients()
    {
        var patients = await _mediator.Send(new GetAllPatientsQuery());
        return Ok(patients);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PatientDto>> GetPatient(Guid id)
    {
        var patient = await _mediator.Send(new GetPatientByIdQuery(id));

        if (patient == null)
        {
            return NotFound($"Patient with ID {id} not found");
        }

        return Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<PatientDto>> CreatePatient([FromBody] CreatePatientDto createPatientDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var patient = await _mediator.Send(new CreatePatientCommand(createPatientDto));
            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PatientDto>> UpdatePatient(Guid id, [FromBody] UpdatePatientDto updatePatientDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var patient = await _mediator.Send(new UpdatePatientCommand(id, updatePatientDto));
            return Ok(patient);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePatient(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new DeletePatientCommand(id));

            if (!result)
            {
                return NotFound($"Patient with ID {id} not found");
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}