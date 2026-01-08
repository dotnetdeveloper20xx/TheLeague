using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Controllers;

[ApiController]
[Route("api/membership-types")]
[Authorize]
public class MembershipTypesController : ControllerBase
{
    private readonly IMembershipService _membershipService;
    private readonly ITenantService _tenantService;

    public MembershipTypesController(IMembershipService membershipService, ITenantService tenantService)
    {
        _membershipService = membershipService;
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
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<MembershipTypeDto>>> GetAll([FromQuery] Guid? clubId)
    {
        var id = clubId ?? GetClubId();
        if (id == Guid.Empty)
            return BadRequest("Club ID required");

        var types = await _membershipService.GetMembershipTypesAsync(id);
        return Ok(types);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MembershipTypeDto>> GetById(Guid id)
    {
        var clubId = GetClubId();
        var type = await _membershipService.GetMembershipTypeByIdAsync(clubId, id);
        if (type == null)
            return NotFound();
        return Ok(type);
    }

    [HttpPost]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<MembershipTypeDto>> Create([FromBody] MembershipTypeCreateRequest request)
    {
        var clubId = GetClubId();
        var type = await _membershipService.CreateMembershipTypeAsync(clubId, request);
        return CreatedAtAction(nameof(GetById), new { id = type.Id }, type);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<MembershipTypeDto>> Update(Guid id, [FromBody] MembershipTypeUpdateRequest request)
    {
        var clubId = GetClubId();
        var type = await _membershipService.UpdateMembershipTypeAsync(clubId, id, request);
        if (type == null)
            return NotFound();
        return Ok(type);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var clubId = GetClubId();
        var result = await _membershipService.DeleteMembershipTypeAsync(clubId, id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
