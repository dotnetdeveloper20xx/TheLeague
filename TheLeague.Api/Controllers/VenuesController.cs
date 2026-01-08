using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VenuesController : ControllerBase
{
    private readonly IVenueService _venueService;
    private readonly ITenantService _tenantService;

    public VenuesController(IVenueService venueService, ITenantService tenantService)
    {
        _venueService = venueService;
        _tenantService = tenantService;
    }

    private Guid GetClubId()
    {
        var clubIdClaim = User.FindFirst("clubId")?.Value;
        if (Guid.TryParse(clubIdClaim, out var clubId))
            return clubId;
        return _tenantService.CurrentTenantId ?? Guid.Empty;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VenueDto>>> GetAll()
    {
        var clubId = GetClubId();
        var venues = await _venueService.GetVenuesAsync(clubId);
        return Ok(venues);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VenueDto>> GetById(Guid id)
    {
        var clubId = GetClubId();
        var venue = await _venueService.GetVenueByIdAsync(clubId, id);
        if (venue == null)
            return NotFound();
        return Ok(venue);
    }

    [HttpPost]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<VenueDto>> Create([FromBody] VenueCreateRequest request)
    {
        var clubId = GetClubId();
        var venue = await _venueService.CreateVenueAsync(clubId, request);
        return CreatedAtAction(nameof(GetById), new { id = venue.Id }, venue);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<VenueDto>> Update(Guid id, [FromBody] VenueUpdateRequest request)
    {
        var clubId = GetClubId();
        var venue = await _venueService.UpdateVenueAsync(clubId, id, request);
        if (venue == null)
            return NotFound();
        return Ok(venue);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var clubId = GetClubId();
        var result = await _venueService.DeleteVenueAsync(clubId, id);
        if (!result)
            return NotFound();
        return NoContent();
    }

    [HttpGet("{id}/schedule")]
    public async Task<ActionResult<VenueScheduleDto>> GetSchedule(Guid id, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
    {
        var clubId = GetClubId();
        var schedule = await _venueService.GetVenueScheduleAsync(clubId, id, fromDate, toDate);
        return Ok(schedule);
    }
}
