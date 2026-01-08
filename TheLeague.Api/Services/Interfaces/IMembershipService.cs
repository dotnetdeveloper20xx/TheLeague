using TheLeague.Api.DTOs;

namespace TheLeague.Api.Services.Interfaces;

public interface IMembershipService
{
    // Membership Types
    Task<IEnumerable<MembershipTypeDto>> GetMembershipTypesAsync(Guid clubId);
    Task<MembershipTypeDto?> GetMembershipTypeByIdAsync(Guid clubId, Guid id);
    Task<MembershipTypeDto> CreateMembershipTypeAsync(Guid clubId, MembershipTypeCreateRequest request);
    Task<MembershipTypeDto?> UpdateMembershipTypeAsync(Guid clubId, Guid id, MembershipTypeUpdateRequest request);
    Task<bool> DeleteMembershipTypeAsync(Guid clubId, Guid id);

    // Memberships
    Task<PagedResult<MembershipDto>> GetMembershipsAsync(Guid clubId, int page = 1, int pageSize = 20);
    Task<MembershipDto?> GetMembershipByIdAsync(Guid clubId, Guid id);
    Task<IEnumerable<MembershipDto>> GetMemberMembershipsAsync(Guid clubId, Guid memberId);
    Task<MembershipDto> CreateMembershipAsync(Guid clubId, MembershipCreateRequest request);
    Task<MembershipDto?> UpdateMembershipAsync(Guid clubId, Guid id, MembershipUpdateRequest request);
    Task<MembershipDto?> RenewMembershipAsync(Guid clubId, Guid id, MembershipRenewRequest request);
    Task<bool> CancelMembershipAsync(Guid clubId, Guid id);
}
