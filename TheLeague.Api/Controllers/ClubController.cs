using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Controllers;

[ApiController]
[Route("api/club")]
[Authorize(Roles = "ClubManager,SuperAdmin")]
public class ClubController : ControllerBase
{
    private readonly IClubService _clubService;
    private readonly ITenantService _tenantService;

    public ClubController(IClubService clubService, ITenantService tenantService)
    {
        _clubService = clubService;
        _tenantService = tenantService;
    }

    private Guid GetClubId()
    {
        var clubIdClaim = User.FindFirst("clubId")?.Value;
        if (Guid.TryParse(clubIdClaim, out var clubId))
            return clubId;
        return _tenantService.CurrentTenantId ?? Guid.Empty;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<ClubDto>> GetProfile()
    {
        var clubId = GetClubId();
        if (clubId == Guid.Empty)
            return BadRequest("Club not found");

        var club = await _clubService.GetClubByIdAsync(clubId);
        if (club == null)
            return NotFound();
        return Ok(club);
    }

    [HttpPut("profile")]
    public async Task<ActionResult<ClubDto>> UpdateProfile([FromBody] ClubUpdateRequest request)
    {
        var clubId = GetClubId();
        var club = await _clubService.UpdateClubAsync(clubId, request);
        if (club == null)
            return NotFound();
        return Ok(club);
    }

    [HttpGet("settings")]
    public async Task<ActionResult<ClubSettingsDto>> GetSettings()
    {
        var clubId = GetClubId();
        var settings = await _clubService.GetClubSettingsAsync(clubId);
        if (settings == null)
            return NotFound();
        return Ok(settings);
    }

    [HttpPut("settings")]
    public async Task<ActionResult<ClubSettingsDto>> UpdateSettings([FromBody] ClubSettingsUpdateRequest request)
    {
        var clubId = GetClubId();
        var settings = await _clubService.UpdateClubSettingsAsync(clubId, request);
        if (settings == null)
            return NotFound();
        return Ok(settings);
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<ClubDashboardDto>> GetDashboard()
    {
        var clubId = GetClubId();
        var dashboard = await _clubService.GetClubDashboardAsync(clubId);
        return Ok(dashboard);
    }
}
