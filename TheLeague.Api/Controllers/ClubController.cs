using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Controllers;

[Route("api/club")]
[Authorize(Roles = "ClubManager,SuperAdmin")]
public class ClubController : BaseApiController
{
    private readonly IClubService _clubService;

    public ClubController(IClubService clubService, ITenantService tenantService)
        : base(tenantService)
    {
        _clubService = clubService;
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
