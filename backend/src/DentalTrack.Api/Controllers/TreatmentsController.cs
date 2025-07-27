using DentalTrack.Application.Commands.Treatments;
using DentalTrack.Application.Common;
using DentalTrack.Application.DTOs;
using DentalTrack.Application.Queries.Treatments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DentalTrack.Api.Controllers;

/// <summary>
/// Controller for managing treatments
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TreatmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TreatmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all treatments with pagination and filtering
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    /// <param name="patientId">Filter by patient ID</param>
    /// <param name="status">Filter by treatment status (Planned, InProgress, Completed, Cancelled)</param>
    /// <param name="sortBy">Sort field (StartDate, EndDate, Status, Type, PatientName, CreatedAt)</param>
    /// <param name="sortDescending">Sort direction (default: true for StartDate)</param>
    /// <returns>Paged list of treatments</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TreatmentDto>), 200)]
    public async Task<ActionResult<PagedResult<TreatmentDto>>> GetAllTreatments(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? patientId = null,
        [FromQuery] string? status = null,
        [FromQuery] string? sortBy = "StartDate",
        [FromQuery] bool sortDescending = true)
    {
        // Validate pagination parameters
        page = Math.Max(1, page);
        pageSize = Math.Min(100, Math.Max(1, pageSize));

        var query = new GetAllTreatmentsQuery
        {
            Page = page,
            PageSize = pageSize,
            PatientId = patientId,
            Status = status,
            SortBy = sortBy,
            SortDescending = sortDescending
        };

        var treatments = await _mediator.Send(query);
        return Ok(treatments);
    }

    /// <summary>
    /// Get a specific treatment by ID
    /// </summary>
    /// <param name="id">Treatment ID</param>
    /// <returns>Treatment details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TreatmentDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<TreatmentDto>> GetTreatment(Guid id)
    {
        // For now, we'll use the existing GetTreatmentsByPatient logic
        // In a real implementation, you'd create a GetTreatmentByIdQuery
        var treatments = await _mediator.Send(new GetAllTreatmentsQuery { Page = 1, PageSize = 1000 });
        var treatment = treatments.Items.FirstOrDefault(t => t.Id == id);
        
        if (treatment == null)
        {
            return NotFound($"Treatment with ID {id} not found");
        }

        return Ok(treatment);
    }

    /// <summary>
    /// Get treatments by patient ID
    /// </summary>
    /// <param name="patientId">Patient ID</param>
    /// <returns>List of treatments for the patient</returns>
    [HttpGet("patient/{patientId}")]
    [ProducesResponseType(typeof(IEnumerable<TreatmentDto>), 200)]
    public async Task<ActionResult<IEnumerable<TreatmentDto>>> GetTreatmentsByPatient(Guid patientId)
    {
        var treatments = await _mediator.Send(new GetTreatmentsByPatientQuery(patientId));
        return Ok(treatments);
    }

    /// <summary>
    /// Create a new treatment
    /// </summary>
    /// <param name="createTreatmentDto">Treatment creation data</param>
    /// <returns>Created treatment</returns>
    [HttpPost]
    [ProducesResponseType(typeof(TreatmentDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<TreatmentDto>> CreateTreatment([FromBody] CreateTreatmentDto createTreatmentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var treatment = await _mediator.Send(new CreateTreatmentCommand(createTreatmentDto));
            
            return CreatedAtAction(nameof(GetTreatment), 
                new { id = treatment.Id }, treatment);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update an existing treatment
    /// </summary>
    /// <param name="id">Treatment ID</param>
    /// <param name="updateTreatmentDto">Treatment update data</param>
    /// <returns>Updated treatment</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TreatmentDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
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

    /// <summary>
    /// Start a treatment
    /// </summary>
    /// <param name="id">Treatment ID</param>
    /// <returns>Updated treatment</returns>
    [HttpPost("{id}/start")]
    [ProducesResponseType(typeof(TreatmentDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
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

    /// <summary>
    /// Complete a treatment
    /// </summary>
    /// <param name="id">Treatment ID</param>
    /// <param name="completeTreatmentDto">Treatment completion data</param>
    /// <returns>Updated treatment</returns>
    [HttpPost("{id}/complete")]
    [ProducesResponseType(typeof(TreatmentDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
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