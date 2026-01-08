using TheLeague.Api.DTOs;

namespace TheLeague.Api.Services.Interfaces;

public interface IMemberService
{
    Task<PagedResult<MemberListDto>> GetMembersAsync(Guid clubId, MemberFilterRequest filter);
    Task<MemberDto?> GetMemberByIdAsync(Guid clubId, Guid id);
    Task<MemberDto?> GetMemberByUserIdAsync(string userId);
    Task<MemberDto> CreateMemberAsync(Guid clubId, MemberCreateRequest request, string? userId = null);
    Task<MemberDto?> UpdateMemberAsync(Guid clubId, Guid id, MemberUpdateRequest request);
    Task<bool> DeleteMemberAsync(Guid clubId, Guid id);

    // Family Members
    Task<IEnumerable<FamilyMemberDto>> GetFamilyMembersAsync(Guid clubId, Guid memberId);
    Task<FamilyMemberDto> AddFamilyMemberAsync(Guid clubId, Guid memberId, FamilyMemberCreateRequest request);
    Task<FamilyMemberDto?> UpdateFamilyMemberAsync(Guid clubId, Guid memberId, Guid familyMemberId, FamilyMemberCreateRequest request);
    Task<bool> RemoveFamilyMemberAsync(Guid clubId, Guid memberId, Guid familyMemberId);
}
