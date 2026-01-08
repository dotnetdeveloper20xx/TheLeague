using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Controllers;

[ApiController]
[Route("api/clubs")]
[Authorize]
public class ClubsController : ControllerBase
{
    private readonly IClubService _clubService;
    private readonly ITenantService _tenantService;

    public ClubsController(IClubService clubService, ITenantService tenantService)
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

    // SuperAdmin: Get all clubs
    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<PagedResult<ClubDto>>> GetClubs(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null)
    {
        var clubs = await _clubService.GetAllClubsAsync();

        // Apply search filter
        if (!string.IsNullOrEmpty(searchTerm))
        {
            clubs = clubs.Where(c =>
                c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                (c.ContactEmail != null && c.ContactEmail.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
        }

        var totalCount = clubs.Count();
        var items = clubs.Skip((page - 1) * pageSize).Take(pageSize);

        return Ok(new PagedResult<ClubDto>(
            items,
            totalCount,
            page,
            pageSize,
            (int)Math.Ceiling(totalCount / (double)pageSize)
        ));
    }

    // SuperAdmin: Get club by ID
    [HttpGet("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<ClubDto>> GetClub(Guid id)
    {
        var club = await _clubService.GetClubByIdAsync(id);
        if (club == null)
            return NotFound();
        return Ok(club);
    }

    // SuperAdmin: Get club by slug
    [HttpGet("slug/{slug}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<ClubDto>> GetClubBySlug(string slug)
    {
        var clubs = await _clubService.GetAllClubsAsync();
        var club = clubs.FirstOrDefault(c =>
            c.Name.ToLower().Replace(" ", "-") == slug.ToLower());
        if (club == null)
            return NotFound();
        return Ok(club);
    }

    // SuperAdmin: Create club
    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<ClubDto>> CreateClub([FromBody] ClubCreateRequest request)
    {
        var club = await _clubService.CreateClubAsync(request);
        return CreatedAtAction(nameof(GetClub), new { id = club.Id }, club);
    }

    // SuperAdmin: Update club
    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<ClubDto>> UpdateClub(Guid id, [FromBody] ClubUpdateRequest request)
    {
        var club = await _clubService.UpdateClubAsync(id, request);
        if (club == null)
            return NotFound();
        return Ok(club);
    }

    // SuperAdmin: Delete club
    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult> DeleteClub(Guid id)
    {
        var result = await _clubService.DeleteClubAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }

    // ClubManager: Get club dashboard
    [HttpGet("dashboard")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<ClubDashboardDto>> GetDashboard()
    {
        var clubId = GetClubId();
        if (clubId == Guid.Empty)
            return BadRequest("Club not found");

        var dashboard = await _clubService.GetClubDashboardAsync(clubId);
        return Ok(dashboard);
    }

    // SuperAdmin: Upload club logo
    [HttpPost("{id}/logo")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult> UploadLogo(Guid id, IFormFile file)
    {
        // In a real app, save the file and return the URL
        return Ok(new { logoUrl = $"/uploads/clubs/{id}/logo{Path.GetExtension(file.FileName)}" });
    }
}
