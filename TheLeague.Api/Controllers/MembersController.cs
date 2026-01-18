using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Controllers;

[Authorize(Roles = "ClubManager,SuperAdmin")]
public class MembersController : BaseApiController
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService, ITenantService tenantService)
        : base(tenantService)
    {
        _memberService = memberService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<MemberListDto>>> GetMembers([FromQuery] MemberFilterRequest filter)
    {
        var clubId = GetClubId();
        var result = await _memberService.GetMembersAsync(clubId, filter);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MemberDto>> GetMember(Guid id)
    {
        var clubId = GetClubId();
        var member = await _memberService.GetMemberByIdAsync(clubId, id);
        if (member == null)
            return NotFound();
        return Ok(member);
    }

    [HttpPost]
    public async Task<ActionResult<MemberDto>> CreateMember([FromBody] MemberCreateRequest request)
    {
        var clubId = GetClubId();
        var member = await _memberService.CreateMemberAsync(clubId, request);
        return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MemberDto>> UpdateMember(Guid id, [FromBody] MemberUpdateRequest request)
    {
        var clubId = GetClubId();
        var member = await _memberService.UpdateMemberAsync(clubId, id, request);
        if (member == null)
            return NotFound();
        return Ok(member);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMember(Guid id)
    {
        var clubId = GetClubId();
        var result = await _memberService.DeleteMemberAsync(clubId, id);
        if (!result)
            return NotFound();
        return NoContent();
    }

    // Family Members
    [HttpGet("{id}/family")]
    public async Task<ActionResult<IEnumerable<FamilyMemberDto>>> GetFamilyMembers(Guid id)
    {
        var clubId = GetClubId();
        var members = await _memberService.GetFamilyMembersAsync(clubId, id);
        return Ok(members);
    }

    [HttpPost("{id}/family")]
    public async Task<ActionResult<FamilyMemberDto>> AddFamilyMember(Guid id, [FromBody] FamilyMemberCreateRequest request)
    {
        var clubId = GetClubId();
        var member = await _memberService.AddFamilyMemberAsync(clubId, id, request);
        return Ok(member);
    }

    [HttpPut("{id}/family/{familyMemberId}")]
    public async Task<ActionResult<FamilyMemberDto>> UpdateFamilyMember(Guid id, Guid familyMemberId, [FromBody] FamilyMemberCreateRequest request)
    {
        var clubId = GetClubId();
        var member = await _memberService.UpdateFamilyMemberAsync(clubId, id, familyMemberId, request);
        if (member == null)
            return NotFound();
        return Ok(member);
    }

    [HttpDelete("{id}/family/{familyMemberId}")]
    public async Task<ActionResult> RemoveFamilyMember(Guid id, Guid familyMemberId)
    {
        var clubId = GetClubId();
        var result = await _memberService.RemoveFamilyMemberAsync(clubId, id, familyMemberId);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
