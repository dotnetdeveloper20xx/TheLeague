using Microsoft.EntityFrameworkCore;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Entities;
using TheLeague.Core.Enums;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Services;

public class MemberService : IMemberService
{
    private readonly ApplicationDbContext _context;

    public MemberService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<MemberListDto>> GetMembersAsync(Guid clubId, MemberFilterRequest filter)
    {
        var query = _context.Members.IgnoreQueryFilters()
            .Where(m => m.ClubId == clubId)
            .Include(m => m.Memberships)
                .ThenInclude(ms => ms.MembershipType)
            .Include(m => m.FamilyMembers)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            var term = filter.SearchTerm.ToLower();
            query = query.Where(m =>
                m.FirstName.ToLower().Contains(term) ||
                m.LastName.ToLower().Contains(term) ||
                m.Email.ToLower().Contains(term));
        }

        if (filter.Status.HasValue)
            query = query.Where(m => m.Status == filter.Status.Value);

        if (filter.MembershipTypeId.HasValue)
            query = query.Where(m => m.Memberships.Any(ms => ms.MembershipTypeId == filter.MembershipTypeId.Value && ms.Status == MembershipStatus.Active));

        if (filter.IsFamilyAccount.HasValue)
            query = query.Where(m => m.IsFamilyAccount == filter.IsFamilyAccount.Value);

        if (filter.JoinedAfter.HasValue)
            query = query.Where(m => m.JoinedDate >= filter.JoinedAfter.Value);

        if (filter.JoinedBefore.HasValue)
            query = query.Where(m => m.JoinedDate <= filter.JoinedBefore.Value);

        // Apply sorting
        query = filter.SortBy?.ToLower() switch
        {
            "name" => filter.SortDescending ? query.OrderByDescending(m => m.LastName) : query.OrderBy(m => m.LastName),
            "email" => filter.SortDescending ? query.OrderByDescending(m => m.Email) : query.OrderBy(m => m.Email),
            "status" => filter.SortDescending ? query.OrderByDescending(m => m.Status) : query.OrderBy(m => m.Status),
            _ => filter.SortDescending ? query.OrderByDescending(m => m.JoinedDate) : query.OrderBy(m => m.JoinedDate)
        };

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(m => new MemberListDto(
                m.Id,
                m.FirstName + " " + m.LastName,
                m.Email,
                m.Phone,
                m.Status,
                m.JoinedDate,
                m.Memberships.Where(ms => ms.Status == MembershipStatus.Active).Select(ms => ms.MembershipType.Name).FirstOrDefault(),
                m.Memberships.Where(ms => ms.Status == MembershipStatus.Active).Select(ms => ms.EndDate).FirstOrDefault(),
                m.IsFamilyAccount,
                m.FamilyMembers.Count
            ))
            .ToListAsync();

        return new PagedResult<MemberListDto>(
            items,
            totalCount,
            filter.Page,
            filter.PageSize,
            (int)Math.Ceiling(totalCount / (double)filter.PageSize)
        );
    }

    public async Task<MemberDto?> GetMemberByIdAsync(Guid clubId, Guid id)
    {
        var member = await _context.Members.IgnoreQueryFilters()
            .Where(m => m.ClubId == clubId && m.Id == id)
            .Include(m => m.Memberships)
                .ThenInclude(ms => ms.MembershipType)
            .Include(m => m.FamilyMembers)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return member == null ? null : MapToDto(member);
    }

    public async Task<MemberDto?> GetMemberByUserIdAsync(string userId)
    {
        var member = await _context.Members
            .Include(m => m.Memberships)
                .ThenInclude(ms => ms.MembershipType)
            .Include(m => m.FamilyMembers)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.UserId == userId);

        return member == null ? null : MapToDto(member);
    }

    public async Task<MemberDto> CreateMemberAsync(Guid clubId, MemberCreateRequest request, string? userId = null)
    {
        var member = new Member
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            UserId = userId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            DateOfBirth = request.DateOfBirth,
            Address = request.Address,
            City = request.City,
            PostCode = request.PostCode,
            EmergencyContactName = request.EmergencyContactName,
            EmergencyContactPhone = request.EmergencyContactPhone,
            EmergencyContactRelation = request.EmergencyContactRelation,
            MedicalConditions = request.MedicalConditions,
            Allergies = request.Allergies,
            IsFamilyAccount = request.IsFamilyAccount,
            Status = MemberStatus.Pending,
            JoinedDate = DateTime.UtcNow
        };

        _context.Members.Add(member);

        // Add family members if provided
        if (request.FamilyMembers != null && request.IsFamilyAccount)
        {
            foreach (var fm in request.FamilyMembers)
            {
                var familyMember = new FamilyMember
                {
                    Id = Guid.NewGuid(),
                    ClubId = clubId,
                    PrimaryMemberId = member.Id,
                    FirstName = fm.FirstName,
                    LastName = fm.LastName,
                    DateOfBirth = fm.DateOfBirth,
                    Relation = fm.Relation,
                    MedicalConditions = fm.MedicalConditions,
                    Allergies = fm.Allergies
                };
                _context.FamilyMembers.Add(familyMember);
            }
        }

        // Create initial membership if type provided
        if (request.MembershipTypeId.HasValue)
        {
            var membershipType = await _context.MembershipTypes.FindAsync(request.MembershipTypeId.Value);
            if (membershipType != null)
            {
                var membership = new Membership
                {
                    Id = Guid.NewGuid(),
                    ClubId = clubId,
                    MemberId = member.Id,
                    MembershipTypeId = membershipType.Id,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddYears(1),
                    PaymentType = MembershipPaymentType.Annual,
                    Status = MembershipStatus.PendingPayment,
                    AmountDue = membershipType.AnnualFee
                };
                _context.Memberships.Add(membership);
            }
        }

        await _context.SaveChangesAsync();

        return MapToDto(member);
    }

    public async Task<MemberDto?> UpdateMemberAsync(Guid clubId, Guid id, MemberUpdateRequest request)
    {
        var member = await _context.Members.IgnoreQueryFilters()
            .Include(m => m.Memberships)
                .ThenInclude(ms => ms.MembershipType)
            .Include(m => m.FamilyMembers)
            .FirstOrDefaultAsync(m => m.ClubId == clubId && m.Id == id);

        if (member == null) return null;

        if (request.FirstName != null) member.FirstName = request.FirstName;
        if (request.LastName != null) member.LastName = request.LastName;
        if (request.Phone != null) member.Phone = request.Phone;
        if (request.DateOfBirth.HasValue) member.DateOfBirth = request.DateOfBirth;
        if (request.Address != null) member.Address = request.Address;
        if (request.City != null) member.City = request.City;
        if (request.PostCode != null) member.PostCode = request.PostCode;
        if (request.EmergencyContactName != null) member.EmergencyContactName = request.EmergencyContactName;
        if (request.EmergencyContactPhone != null) member.EmergencyContactPhone = request.EmergencyContactPhone;
        if (request.EmergencyContactRelation != null) member.EmergencyContactRelation = request.EmergencyContactRelation;
        if (request.MedicalConditions != null) member.MedicalConditions = request.MedicalConditions;
        if (request.Allergies != null) member.Allergies = request.Allergies;
        if (request.Status.HasValue) member.Status = request.Status.Value;
        if (request.IsActive.HasValue) member.IsActive = request.IsActive.Value;

        await _context.SaveChangesAsync();
        return MapToDto(member);
    }

    public async Task<bool> DeleteMemberAsync(Guid clubId, Guid id)
    {
        var member = await _context.Members.IgnoreQueryFilters()
            .FirstOrDefaultAsync(m => m.ClubId == clubId && m.Id == id);

        if (member == null) return false;

        member.IsActive = false;
        member.Status = MemberStatus.Cancelled;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<FamilyMemberDto>> GetFamilyMembersAsync(Guid clubId, Guid memberId)
    {
        var familyMembers = await _context.FamilyMembers.IgnoreQueryFilters()
            .Where(fm => fm.ClubId == clubId && fm.PrimaryMemberId == memberId)
            .AsNoTracking()
            .ToListAsync();

        return familyMembers.Select(MapFamilyMemberToDto);
    }

    public async Task<FamilyMemberDto> AddFamilyMemberAsync(Guid clubId, Guid memberId, FamilyMemberCreateRequest request)
    {
        var familyMember = new FamilyMember
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            PrimaryMemberId = memberId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            Relation = request.Relation,
            MedicalConditions = request.MedicalConditions,
            Allergies = request.Allergies
        };

        _context.FamilyMembers.Add(familyMember);
        await _context.SaveChangesAsync();

        return MapFamilyMemberToDto(familyMember);
    }

    public async Task<FamilyMemberDto?> UpdateFamilyMemberAsync(Guid clubId, Guid memberId, Guid familyMemberId, FamilyMemberCreateRequest request)
    {
        var familyMember = await _context.FamilyMembers.IgnoreQueryFilters()
            .FirstOrDefaultAsync(fm => fm.ClubId == clubId && fm.PrimaryMemberId == memberId && fm.Id == familyMemberId);

        if (familyMember == null) return null;

        familyMember.FirstName = request.FirstName;
        familyMember.LastName = request.LastName;
        familyMember.DateOfBirth = request.DateOfBirth;
        familyMember.Relation = request.Relation;
        familyMember.MedicalConditions = request.MedicalConditions;
        familyMember.Allergies = request.Allergies;

        await _context.SaveChangesAsync();
        return MapFamilyMemberToDto(familyMember);
    }

    public async Task<bool> RemoveFamilyMemberAsync(Guid clubId, Guid memberId, Guid familyMemberId)
    {
        var familyMember = await _context.FamilyMembers.IgnoreQueryFilters()
            .FirstOrDefaultAsync(fm => fm.ClubId == clubId && fm.PrimaryMemberId == memberId && fm.Id == familyMemberId);

        if (familyMember == null) return false;

        familyMember.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    private static MemberDto MapToDto(Member m)
    {
        var activeMembership = m.Memberships?
            .Where(ms => ms.Status == MembershipStatus.Active)
            .OrderByDescending(ms => ms.EndDate)
            .FirstOrDefault();

        MembershipSummaryDto? membershipSummary = null;
        if (activeMembership != null)
        {
            membershipSummary = new MembershipSummaryDto(
                activeMembership.Id,
                activeMembership.MembershipType?.Name ?? "Unknown",
                activeMembership.StartDate,
                activeMembership.EndDate,
                activeMembership.Status,
                activeMembership.AmountDue,
                activeMembership.AutoRenew
            );
        }

        return new MemberDto(
            m.Id,
            m.FirstName,
            m.LastName,
            m.FullName,
            m.Email,
            m.Phone,
            m.DateOfBirth,
            m.Address,
            m.City,
            m.PostCode,
            m.ProfilePhotoUrl,
            m.EmergencyContactName,
            m.EmergencyContactPhone,
            m.EmergencyContactRelation,
            m.MedicalConditions,
            m.Allergies,
            m.IsFamilyAccount,
            m.Status,
            m.JoinedDate,
            m.IsActive,
            m.EmailVerified,
            membershipSummary,
            m.FamilyMembers?.Select(MapFamilyMemberToDto)
        );
    }

    private static FamilyMemberDto MapFamilyMemberToDto(FamilyMember fm) => new(
        fm.Id,
        fm.FirstName,
        fm.LastName,
        fm.FullName,
        fm.DateOfBirth,
        fm.Relation,
        fm.MedicalConditions,
        fm.Allergies,
        fm.IsActive
    );
}
