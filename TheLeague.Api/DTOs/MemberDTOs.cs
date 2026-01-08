using System.ComponentModel.DataAnnotations;
using TheLeague.Core.Enums;

namespace TheLeague.Api.DTOs;

public record MemberDto(
    Guid Id,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    string? Phone,
    DateTime? DateOfBirth,
    string? Address,
    string? City,
    string? PostCode,
    string? ProfilePhotoUrl,
    string? EmergencyContactName,
    string? EmergencyContactPhone,
    string? EmergencyContactRelation,
    string? MedicalConditions,
    string? Allergies,
    bool IsFamilyAccount,
    MemberStatus Status,
    DateTime JoinedDate,
    bool IsActive,
    bool EmailVerified,
    MembershipSummaryDto? CurrentMembership,
    IEnumerable<FamilyMemberDto>? FamilyMembers
);

public record MemberListDto(
    Guid Id,
    string FullName,
    string Email,
    string? Phone,
    MemberStatus Status,
    DateTime JoinedDate,
    string? MembershipType,
    DateTime? MembershipExpiry,
    bool IsFamilyAccount,
    int FamilyMemberCount
);

public record MemberCreateRequest(
    [Required] string FirstName,
    [Required] string LastName,
    [Required, EmailAddress] string Email,
    string? Phone,
    DateTime? DateOfBirth,
    string? Address,
    string? City,
    string? PostCode,
    string? EmergencyContactName,
    string? EmergencyContactPhone,
    string? EmergencyContactRelation,
    string? MedicalConditions,
    string? Allergies,
    bool IsFamilyAccount,
    Guid? MembershipTypeId,
    IEnumerable<FamilyMemberCreateRequest>? FamilyMembers
);

public record MemberUpdateRequest(
    string? FirstName,
    string? LastName,
    string? Phone,
    DateTime? DateOfBirth,
    string? Address,
    string? City,
    string? PostCode,
    string? EmergencyContactName,
    string? EmergencyContactPhone,
    string? EmergencyContactRelation,
    string? MedicalConditions,
    string? Allergies,
    MemberStatus? Status,
    bool? IsActive
);

public record FamilyMemberDto(
    Guid Id,
    string FirstName,
    string LastName,
    string FullName,
    DateTime? DateOfBirth,
    FamilyMemberRelation Relation,
    string? MedicalConditions,
    string? Allergies,
    bool IsActive
);

public record FamilyMemberCreateRequest(
    [Required] string FirstName,
    [Required] string LastName,
    DateTime? DateOfBirth,
    FamilyMemberRelation Relation,
    string? MedicalConditions,
    string? Allergies
);

public record MembershipSummaryDto(
    Guid Id,
    string MembershipType,
    DateTime StartDate,
    DateTime EndDate,
    MembershipStatus Status,
    decimal AmountDue,
    bool AutoRenew
);

public record MemberFilterRequest(
    string? SearchTerm,
    MemberStatus? Status,
    Guid? MembershipTypeId,
    bool? IsFamilyAccount,
    DateTime? JoinedAfter,
    DateTime? JoinedBefore,
    int Page = 1,
    int PageSize = 20,
    string SortBy = "JoinedDate",
    bool SortDescending = true
);

public record PagedResult<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);
