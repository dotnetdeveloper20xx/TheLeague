using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Enums;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FeesController : ControllerBase
{
    private readonly IFeeService _feeService;
    private readonly ITenantService _tenantService;

    public FeesController(IFeeService feeService, ITenantService tenantService)
    {
        _feeService = feeService;
        _tenantService = tenantService;
    }

    private Guid GetClubId()
    {
        var clubIdClaim = User.FindFirst("clubId")?.Value;
        if (Guid.TryParse(clubIdClaim, out var clubId))
            return clubId;
        return _tenantService.CurrentTenantId ?? Guid.Empty;
    }

    private string? GetUserId() => User.FindFirst("sub")?.Value ?? User.FindFirst("userId")?.Value;

    /// <summary>
    /// Get all fees with optional filtering and pagination
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<FeeListDto>>> GetAll([FromQuery] FeeFilterRequest filter)
    {
        var clubId = GetClubId();
        var fees = await _feeService.GetFeesAsync(clubId, filter);
        return Ok(fees);
    }

    /// <summary>
    /// Get all fees without pagination (for dropdowns)
    /// </summary>
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<FeeListDto>>> GetAllFees([FromQuery] bool includeInactive = false)
    {
        var clubId = GetClubId();
        var fees = await _feeService.GetAllFeesAsync(clubId, includeInactive);
        return Ok(fees);
    }

    /// <summary>
    /// Get a specific fee by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<FeeDto>> GetById(Guid id)
    {
        var clubId = GetClubId();
        var fee = await _feeService.GetFeeByIdAsync(clubId, id);
        if (fee == null)
            return NotFound();
        return Ok(fee);
    }

    /// <summary>
    /// Get fees by type
    /// </summary>
    [HttpGet("by-type/{type}")]
    public async Task<ActionResult<IEnumerable<FeeListDto>>> GetByType(FeeType type)
    {
        var clubId = GetClubId();
        var fees = await _feeService.GetFeesByTypeAsync(clubId, type);
        return Ok(fees);
    }

    /// <summary>
    /// Create a new fee
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<FeeDto>> Create([FromBody] FeeCreateRequest request)
    {
        var clubId = GetClubId();
        var userId = GetUserId();
        var fee = await _feeService.CreateFeeAsync(clubId, request, userId);
        return CreatedAtAction(nameof(GetById), new { id = fee.Id }, fee);
    }

    /// <summary>
    /// Update an existing fee
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<FeeDto>> Update(Guid id, [FromBody] FeeUpdateRequest request)
    {
        var clubId = GetClubId();
        var userId = GetUserId();
        var fee = await _feeService.UpdateFeeAsync(clubId, id, request, userId);
        if (fee == null)
            return NotFound();
        return Ok(fee);
    }

    /// <summary>
    /// Delete (soft delete) a fee
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var clubId = GetClubId();
        var result = await _feeService.DeleteFeeAsync(clubId, id);
        if (!result)
            return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Toggle the active status of a fee
    /// </summary>
    [HttpPost("{id}/toggle-active")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult> ToggleActive(Guid id)
    {
        var clubId = GetClubId();
        var result = await _feeService.ToggleActiveAsync(clubId, id);
        if (!result)
            return NotFound();
        return Ok();
    }
}
