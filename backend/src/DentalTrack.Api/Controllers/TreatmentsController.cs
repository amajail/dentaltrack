using DentalTrack.Application.Commands.Treatments;
using DentalTrack.Application.DTOs;
using DentalTrack.Application.Queries.Treatments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DentalTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TreatmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TreatmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TreatmentDto>>> GetAllTreatments()
    {
        var treatments = await _mediator.Send(new GetAllTreatmentsQuery());
        return Ok(treatments);
    }

    [HttpGet("patient/{patientId}")]
    public async Task<ActionResult<IEnumerable<TreatmentDto>>> GetTreatmentsByPatient(Guid patientId)
    {
        var treatments = await _mediator.Send(new GetTreatmentsByPatientQuery(patientId));
        return Ok(treatments);
    }

    [HttpPost]
    public async Task<ActionResult<TreatmentDto>> CreateTreatment([FromBody] CreateTreatmentDto createTreatmentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var treatment = await _mediator.Send(new CreateTreatmentCommand(createTreatmentDto));
            return CreatedAtAction(nameof(GetTreatmentsByPatient), 
                new { patientId = treatment.PatientId }, treatment);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TreatmentDto>> UpdateTreatment(Guid id, [FromBody] UpdateTreatmentDto updateTreatmentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var treatment = await _mediator.Send(new UpdateTreatmentCommand(id, updateTreatmentDto));
            return Ok(treatment);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{id}/start")]
    public async Task<ActionResult<TreatmentDto>> StartTreatment(Guid id)
    {
        try
        {
            var treatment = await _mediator.Send(new StartTreatmentCommand(id));
            return Ok(treatment);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/complete")]
    public async Task<ActionResult<TreatmentDto>> CompleteTreatment(Guid id, [FromBody] CompleteTreatmentDto completeTreatmentDto)
    {
        try
        {
            var treatment = await _mediator.Send(new CompleteTreatmentCommand(id, completeTreatmentDto));
            return Ok(treatment);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}