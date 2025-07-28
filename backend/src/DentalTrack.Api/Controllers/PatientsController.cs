using DentalTrack.Application.Commands.Patients;
using DentalTrack.Application.Common;
using DentalTrack.Application.DTOs;
using DentalTrack.Application.Queries.Patients;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DentalTrack.Api.Controllers;

/// <summary>
/// Controller for managing patients
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PatientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all patients with pagination and search
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    /// <param name="search">Search term for name, email, or phone</param>
    /// <param name="sortBy">Sort field (FirstName, LastName, Email, DateOfBirth, CreatedAt)</param>
    /// <param name="sortDescending">Sort direction (default: false)</param>
    /// <returns>Paged list of patients</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<PatientDto>), 200)]
    public async Task<ActionResult<PagedResult<PatientDto>>> GetAllPatients(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = "LastName",
        [FromQuery] bool sortDescending = false)
    {
        // Validate pagination parameters
        page = Math.Max(1, page);
        pageSize = Math.Min(100, Math.Max(1, pageSize));

        var query = new GetAllPatientsQuery
        {
            Page = page,
            PageSize = pageSize,
            Search = search,
            SortBy = sortBy,
            SortDescending = sortDescending
        };

        var patients = await _mediator.Send(query);
        return Ok(patients);
    }

    /// <summary>
    /// Get a specific patient by ID
    /// </summary>
    /// <param name="id">Patient ID</param>
    /// <returns>Patient details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PatientDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<PatientDto>> GetPatient(Guid id)
    {
        var patient = await _mediator.Send(new GetPatientByIdQuery(id));

        if (patient == null)
        {
            return NotFound($"Patient with ID {id} not found");
        }

        return Ok(patient);
    }

    /// <summary>
    /// Create a new patient
    /// </summary>
    /// <param name="createPatientDto">Patient creation data</param>
    /// <returns>Created patient</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PatientDto), 201)]
    [ProducesResponseType(400)]
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

    /// <summary>
    /// Update an existing patient
    /// </summary>
    /// <param name="id">Patient ID</param>
    /// <param name="updatePatientDto">Patient update data</param>
    /// <returns>Updated patient</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PatientDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
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

    /// <summary>
    /// Delete a patient (soft delete)
    /// </summary>
    /// <param name="id">Patient ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
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