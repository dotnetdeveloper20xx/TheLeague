using System.ComponentModel.DataAnnotations;
using TheLeague.Core.Enums;

namespace TheLeague.Api.DTOs;

public record MembershipTypeDto(
    Guid Id,
    string Name,
    string? Description,
    decimal AnnualFee,
    decimal? MonthlyFee,
    decimal? SessionFee,
    int? MinAge,
    int? MaxAge,
    int? MaxFamilyMembers,
    bool IsActive,
    bool AllowOnlineSignup,
    int SortOrder,
    bool IncludesBooking,
    bool IncludesEvents,
    int? MaxSessionsPerWeek,
    int MemberCount
);

public record MembershipTypeCreateRequest(
    [Required] string Name,
    [Required] decimal AnnualFee,
    string? Description = null,
    decimal? MonthlyFee = null,
    decimal? SessionFee = null,
    int? MinAge = null,
    int? MaxAge = null,
    int? MaxFamilyMembers = null,
    bool AllowOnlineSignup = true,
    int SortOrder = 0,
    bool IncludesBooking = true,
    bool IncludesEvents = true,
    int? MaxSessionsPerWeek = null
);

public record MembershipTypeUpdateRequest(
    string? Name,
    string? Description,
    decimal? AnnualFee,
    decimal? MonthlyFee,
    decimal? SessionFee,
    int? MinAge,
    int? MaxAge,
    int? MaxFamilyMembers,
    bool? IsActive,
    bool? AllowOnlineSignup,
    int? SortOrder,
    bool? IncludesBooking,
    bool? IncludesEvents,
    int? MaxSessionsPerWeek
);

public record MembershipDto(
    Guid Id,
    Guid MemberId,
    string MemberName,
    Guid MembershipTypeId,
    string MembershipTypeName,
    DateTime StartDate,
    DateTime EndDate,
    BillingCycle BillingCycle,
    MembershipStatus Status,
    decimal AmountPaid,
    decimal AmountDue,
    DateTime? LastPaymentDate,
    DateTime? NextPaymentDate,
    bool AutoRenew,
    string? Notes
);

public record MembershipCreateRequest(
    [Required] Guid MemberId,
    [Required] Guid MembershipTypeId,
    [Required] DateTime StartDate,
    [Required] DateTime EndDate,
    BillingCycle BillingCycle = BillingCycle.Annual,
    bool AutoRenew = false,
    string? Notes = null
);

public record MembershipUpdateRequest(
    DateTime? StartDate,
    DateTime? EndDate,
    BillingCycle? BillingCycle,
    MembershipStatus? Status,
    bool? AutoRenew,
    string? Notes
);

public record MembershipRenewRequest(
    BillingCycle BillingCycle = BillingCycle.Annual,
    bool AutoRenew = false
);
