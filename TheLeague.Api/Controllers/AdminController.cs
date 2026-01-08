using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;

namespace TheLeague.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "SuperAdmin")]
public class AdminController : ControllerBase
{
    private readonly IClubService _clubService;

    public AdminController(IClubService clubService)
    {
        _clubService = clubService;
    }

    [HttpGet("clubs")]
    public async Task<ActionResult<IEnumerable<ClubDto>>> GetAllClubs()
    {
        var clubs = await _clubService.GetAllClubsAsync();
        return Ok(clubs);
    }

    [HttpPost("clubs")]
    public async Task<ActionResult<ClubDto>> CreateClub([FromBody] ClubCreateRequest request)
    {
        var club = await _clubService.CreateClubAsync(request);
        return CreatedAtAction(nameof(GetClub), new { id = club.Id }, club);
    }

    [HttpGet("clubs/{id}")]
    public async Task<ActionResult<ClubDto>> GetClub(Guid id)
    {
        var club = await _clubService.GetClubByIdAsync(id);
        if (club == null)
            return NotFound();
        return Ok(club);
    }

    [HttpPut("clubs/{id}")]
    public async Task<ActionResult<ClubDto>> UpdateClub(Guid id, [FromBody] ClubUpdateRequest request)
    {
        var club = await _clubService.UpdateClubAsync(id, request);
        if (club == null)
            return NotFound();
        return Ok(club);
    }

    [HttpDelete("clubs/{id}")]
    public async Task<ActionResult> DeleteClub(Guid id)
    {
        var result = await _clubService.DeleteClubAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }

    [HttpGet("clubs/{id}/stats")]
    public async Task<ActionResult<ClubDashboardDto>> GetClubStats(Guid id)
    {
        var stats = await _clubService.GetClubDashboardAsync(id);
        return Ok(stats);
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult> GetSystemDashboard()
    {
        var clubs = await _clubService.GetAllClubsAsync();
        var totalClubs = clubs.Count();
        var activeClubs = clubs.Count(c => c.IsActive);
        var totalMembers = clubs.Sum(c => c.MemberCount);

        return Ok(new
        {
            TotalClubs = totalClubs,
            ActiveClubs = activeClubs,
            TotalMembers = totalMembers,
            Clubs = clubs.Take(10)
        });
    }
}
