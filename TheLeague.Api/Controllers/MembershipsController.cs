using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Controllers;

[Authorize(Roles = "ClubManager,SuperAdmin")]
public class MembershipsController : BaseApiController
{
    private readonly IMembershipService _membershipService;

    public MembershipsController(IMembershipService membershipService, ITenantService tenantService)
        : base(tenantService)
    {
        _membershipService = membershipService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<MembershipDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var clubId = GetClubId();
        var result = await _membershipService.GetMembershipsAsync(clubId, page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MembershipDto>> GetById(Guid id)
    {
        var clubId = GetClubId();
        var membership = await _membershipService.GetMembershipByIdAsync(clubId, id);
        if (membership == null)
            return NotFound();
        return Ok(membership);
    }

    [HttpPost]
    public async Task<ActionResult<MembershipDto>> Create([FromBody] MembershipCreateRequest request)
    {
        var clubId = GetClubId();
        var membership = await _membershipService.CreateMembershipAsync(clubId, request);
        return CreatedAtAction(nameof(GetById), new { id = membership.Id }, membership);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MembershipDto>> Update(Guid id, [FromBody] MembershipUpdateRequest request)
    {
        var clubId = GetClubId();
        var membership = await _membershipService.UpdateMembershipAsync(clubId, id, request);
        if (membership == null)
            return NotFound();
        return Ok(membership);
    }

    [HttpPost("{id}/renew")]
    public async Task<ActionResult<MembershipDto>> Renew(Guid id, [FromBody] MembershipRenewRequest request)
    {
        var clubId = GetClubId();
        var membership = await _membershipService.RenewMembershipAsync(clubId, id, request);
        if (membership == null)
            return NotFound();
        return Ok(membership);
    }

    [HttpPost("{id}/cancel")]
    public async Task<ActionResult> Cancel(Guid id)
    {
        var clubId = GetClubId();
        var result = await _membershipService.CancelMembershipAsync(clubId, id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
