using Microsoft.EntityFrameworkCore;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Entities;
using TheLeague.Core.Enums;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Services;

public class MembershipService : IMembershipService
{
    private readonly ApplicationDbContext _context;

    public MembershipService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Membership Types
    public async Task<IEnumerable<MembershipTypeDto>> GetMembershipTypesAsync(Guid clubId)
    {
        var types = await _context.MembershipTypes.IgnoreQueryFilters()
            .Where(mt => mt.ClubId == clubId)
            .Include(mt => mt.Memberships)
            .OrderBy(mt => mt.SortOrder)
            .AsNoTracking()
            .ToListAsync();

        return types.Select(MapTypeToDto);
    }

    public async Task<MembershipTypeDto?> GetMembershipTypeByIdAsync(Guid clubId, Guid id)
    {
        var type = await _context.MembershipTypes.IgnoreQueryFilters()
            .Include(mt => mt.Memberships)
            .FirstOrDefaultAsync(mt => mt.ClubId == clubId && mt.Id == id);

        return type == null ? null : MapTypeToDto(type);
    }

    public async Task<MembershipTypeDto> CreateMembershipTypeAsync(Guid clubId, MembershipTypeCreateRequest request)
    {
        var membershipType = new MembershipType
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Name = request.Name,
            Description = request.Description,
            AnnualFee = request.AnnualFee,
            MonthlyFee = request.MonthlyFee,
            SessionFee = request.SessionFee,
            MinAge = request.MinAge,
            MaxAge = request.MaxAge,
            MaxFamilyMembers = request.MaxFamilyMembers,
            AllowOnlineSignup = request.AllowOnlineSignup,
            SortOrder = request.SortOrder,
            IncludesBooking = request.IncludesBooking,
            IncludesEvents = request.IncludesEvents,
            MaxSessionsPerWeek = request.MaxSessionsPerWeek,
            IsActive = true
        };

        _context.MembershipTypes.Add(membershipType);
        await _context.SaveChangesAsync();

        return MapTypeToDto(membershipType);
    }

    public async Task<MembershipTypeDto?> UpdateMembershipTypeAsync(Guid clubId, Guid id, MembershipTypeUpdateRequest request)
    {
        var type = await _context.MembershipTypes.IgnoreQueryFilters()
            .Include(mt => mt.Memberships)
            .FirstOrDefaultAsync(mt => mt.ClubId == clubId && mt.Id == id);

        if (type == null) return null;

        if (request.Name != null) type.Name = request.Name;
        if (request.Description != null) type.Description = request.Description;
        if (request.AnnualFee.HasValue) type.AnnualFee = request.AnnualFee.Value;
        if (request.MonthlyFee.HasValue) type.MonthlyFee = request.MonthlyFee;
        if (request.SessionFee.HasValue) type.SessionFee = request.SessionFee;
        if (request.MinAge.HasValue) type.MinAge = request.MinAge;
        if (request.MaxAge.HasValue) type.MaxAge = request.MaxAge;
        if (request.MaxFamilyMembers.HasValue) type.MaxFamilyMembers = request.MaxFamilyMembers;
        if (request.IsActive.HasValue) type.IsActive = request.IsActive.Value;
        if (request.AllowOnlineSignup.HasValue) type.AllowOnlineSignup = request.AllowOnlineSignup.Value;
        if (request.SortOrder.HasValue) type.SortOrder = request.SortOrder.Value;
        if (request.IncludesBooking.HasValue) type.IncludesBooking = request.IncludesBooking.Value;
        if (request.IncludesEvents.HasValue) type.IncludesEvents = request.IncludesEvents.Value;
        if (request.MaxSessionsPerWeek.HasValue) type.MaxSessionsPerWeek = request.MaxSessionsPerWeek;

        await _context.SaveChangesAsync();
        return MapTypeToDto(type);
    }

    public async Task<bool> DeleteMembershipTypeAsync(Guid clubId, Guid id)
    {
        var type = await _context.MembershipTypes.IgnoreQueryFilters()
            .FirstOrDefaultAsync(mt => mt.ClubId == clubId && mt.Id == id);

        if (type == null) return false;

        type.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    // Memberships
    public async Task<PagedResult<MembershipDto>> GetMembershipsAsync(Guid clubId, int page = 1, int pageSize = 20)
    {
        var query = _context.Memberships.IgnoreQueryFilters()
            .Where(m => m.ClubId == clubId)
            .Include(m => m.Member)
            .Include(m => m.MembershipType)
            .OrderByDescending(m => m.StartDate);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<MembershipDto>(
            items.Select(MapToDto),
            totalCount,
            page,
            pageSize,
            (int)Math.Ceiling(totalCount / (double)pageSize)
        );
    }

    public async Task<MembershipDto?> GetMembershipByIdAsync(Guid clubId, Guid id)
    {
        var membership = await _context.Memberships.IgnoreQueryFilters()
            .Include(m => m.Member)
            .Include(m => m.MembershipType)
            .FirstOrDefaultAsync(m => m.ClubId == clubId && m.Id == id);

        return membership == null ? null : MapToDto(membership);
    }

    public async Task<IEnumerable<MembershipDto>> GetMemberMembershipsAsync(Guid clubId, Guid memberId)
    {
        var memberships = await _context.Memberships.IgnoreQueryFilters()
            .Where(m => m.ClubId == clubId && m.MemberId == memberId)
            .Include(m => m.Member)
            .Include(m => m.MembershipType)
            .OrderByDescending(m => m.StartDate)
            .ToListAsync();

        return memberships.Select(MapToDto);
    }

    public async Task<MembershipDto> CreateMembershipAsync(Guid clubId, MembershipCreateRequest request)
    {
        var membershipType = await _context.MembershipTypes.FindAsync(request.MembershipTypeId);

        var membership = new Membership
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            MemberId = request.MemberId,
            MembershipTypeId = request.MembershipTypeId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            BillingCycle = request.BillingCycle,
            Status = MembershipStatus.PendingPayment,
            AmountDue = membershipType?.AnnualFee ?? 0,
            AutoRenew = request.AutoRenew,
            Notes = request.Notes
        };

        _context.Memberships.Add(membership);

        // Update member status
        var member = await _context.Members.FindAsync(request.MemberId);
        if (member != null)
        {
            member.Status = MemberStatus.Pending;
        }

        await _context.SaveChangesAsync();

        // Reload with includes
        membership = await _context.Memberships
            .Include(m => m.Member)
            .Include(m => m.MembershipType)
            .FirstAsync(m => m.Id == membership.Id);

        return MapToDto(membership);
    }

    public async Task<MembershipDto?> UpdateMembershipAsync(Guid clubId, Guid id, MembershipUpdateRequest request)
    {
        var membership = await _context.Memberships.IgnoreQueryFilters()
            .Include(m => m.Member)
            .Include(m => m.MembershipType)
            .FirstOrDefaultAsync(m => m.ClubId == clubId && m.Id == id);

        if (membership == null) return null;

        if (request.StartDate.HasValue) membership.StartDate = request.StartDate.Value;
        if (request.EndDate.HasValue) membership.EndDate = request.EndDate.Value;
        if (request.BillingCycle.HasValue) membership.BillingCycle = request.BillingCycle.Value;
        if (request.Status.HasValue) membership.Status = request.Status.Value;
        if (request.AutoRenew.HasValue) membership.AutoRenew = request.AutoRenew.Value;
        if (request.Notes != null) membership.Notes = request.Notes;

        await _context.SaveChangesAsync();
        return MapToDto(membership);
    }

    public async Task<MembershipDto?> RenewMembershipAsync(Guid clubId, Guid id, MembershipRenewRequest request)
    {
        var currentMembership = await _context.Memberships.IgnoreQueryFilters()
            .Include(m => m.MembershipType)
            .FirstOrDefaultAsync(m => m.ClubId == clubId && m.Id == id);

        if (currentMembership == null) return null;

        // Create new membership starting from end of current one
        var newStartDate = currentMembership.EndDate > DateTime.UtcNow ? currentMembership.EndDate : DateTime.UtcNow;

        var newMembership = new Membership
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            MemberId = currentMembership.MemberId,
            MembershipTypeId = currentMembership.MembershipTypeId,
            StartDate = newStartDate,
            EndDate = newStartDate.AddYears(1),
            BillingCycle = request.BillingCycle,
            Status = MembershipStatus.PendingPayment,
            AmountDue = currentMembership.MembershipType?.AnnualFee ?? 0,
            AutoRenew = request.AutoRenew
        };

        _context.Memberships.Add(newMembership);
        await _context.SaveChangesAsync();

        newMembership = await _context.Memberships
            .Include(m => m.Member)
            .Include(m => m.MembershipType)
            .FirstAsync(m => m.Id == newMembership.Id);

        return MapToDto(newMembership);
    }

    public async Task<bool> CancelMembershipAsync(Guid clubId, Guid id)
    {
        var membership = await _context.Memberships.IgnoreQueryFilters()
            .FirstOrDefaultAsync(m => m.ClubId == clubId && m.Id == id);

        if (membership == null) return false;

        membership.Status = MembershipStatus.Cancelled;
        await _context.SaveChangesAsync();
        return true;
    }

    private static MembershipTypeDto MapTypeToDto(MembershipType mt) => new(
        mt.Id,
        mt.Name,
        mt.Description,
        mt.AnnualFee,
        mt.MonthlyFee,
        mt.SessionFee,
        mt.MinAge,
        mt.MaxAge,
        mt.MaxFamilyMembers,
        mt.IsActive,
        mt.AllowOnlineSignup,
        mt.SortOrder,
        mt.IncludesBooking,
        mt.IncludesEvents,
        mt.MaxSessionsPerWeek,
        mt.Memberships?.Count(m => m.Status == MembershipStatus.Active) ?? 0
    );

    private static MembershipDto MapToDto(Membership m) => new(
        m.Id,
        m.MemberId,
        m.Member?.FullName ?? "Unknown",
        m.MembershipTypeId,
        m.MembershipType?.Name ?? "Unknown",
        m.StartDate,
        m.EndDate,
        m.BillingCycle,
        m.Status,
        m.AmountPaid,
        m.AmountDue,
        m.LastPaymentDate,
        m.NextPaymentDate,
        m.AutoRenew,
        m.Notes
    );
}
