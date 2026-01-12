using Microsoft.EntityFrameworkCore;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Entities;
using TheLeague.Core.Enums;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Services;

public class SessionService : ISessionService
{
    private readonly ApplicationDbContext _context;

    public SessionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<SessionListDto>> GetSessionsAsync(Guid clubId, SessionFilterRequest filter)
    {
        var query = _context.Sessions.IgnoreQueryFilters()
            .Where(s => s.ClubId == clubId)
            .Include(s => s.Venue)
            .AsQueryable();

        if (filter.DateFrom.HasValue)
            query = query.Where(s => s.StartTime >= filter.DateFrom.Value);
        if (filter.DateTo.HasValue)
            query = query.Where(s => s.StartTime <= filter.DateTo.Value);
        if (filter.Category.HasValue)
            query = query.Where(s => s.Category == filter.Category.Value);
        if (filter.VenueId.HasValue)
            query = query.Where(s => s.VenueId == filter.VenueId.Value);
        if (!filter.IncludeCancelled.GetValueOrDefault())
            query = query.Where(s => !s.IsCancelled);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(s => s.StartTime)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(s => new SessionListDto(
                s.Id,
                s.Title,
                s.Category,
                s.StartTime,
                s.EndTime,
                s.Capacity,
                s.CurrentBookings,
                s.Capacity - s.CurrentBookings,
                s.SessionFee,
                s.IsCancelled,
                s.Venue != null ? s.Venue.Name : null
            ))
            .ToListAsync();

        return new PagedResult<SessionListDto>(
            items,
            totalCount,
            filter.Page,
            filter.PageSize,
            (int)Math.Ceiling(totalCount / (double)filter.PageSize)
        );
    }

    public async Task<SessionDto?> GetSessionByIdAsync(Guid clubId, Guid id)
    {
        var session = await _context.Sessions.IgnoreQueryFilters()
            .Where(s => s.ClubId == clubId && s.Id == id)
            .Include(s => s.Venue)
            .Include(s => s.Bookings).ThenInclude(b => b.Member)
            .Include(s => s.Bookings).ThenInclude(b => b.FamilyMember)
            .FirstOrDefaultAsync();

        return session == null ? null : MapToDto(session);
    }

    public async Task<SessionDto> CreateSessionAsync(Guid clubId, SessionCreateRequest request)
    {
        var session = new Session
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Capacity = request.Capacity,
            VenueId = request.VenueId,
            SessionFee = request.SessionFee
        };

        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();

        return MapToDto(session);
    }

    public async Task<SessionDto?> UpdateSessionAsync(Guid clubId, Guid id, SessionUpdateRequest request)
    {
        var session = await _context.Sessions.IgnoreQueryFilters()
            .Include(s => s.Venue)
            .FirstOrDefaultAsync(s => s.ClubId == clubId && s.Id == id);

        if (session == null) return null;

        if (request.Title != null) session.Title = request.Title;
        if (request.Description != null) session.Description = request.Description;
        if (request.Category.HasValue) session.Category = request.Category.Value;
        if (request.StartTime.HasValue) session.StartTime = request.StartTime.Value;
        if (request.EndTime.HasValue) session.EndTime = request.EndTime.Value;
        if (request.Capacity.HasValue) session.Capacity = request.Capacity.Value;
        if (request.VenueId.HasValue) session.VenueId = request.VenueId;
        if (request.SessionFee.HasValue) session.SessionFee = request.SessionFee;
        if (request.IsCancelled.HasValue) session.IsCancelled = request.IsCancelled.Value;
        if (request.CancellationReason != null) session.CancellationReason = request.CancellationReason;

        await _context.SaveChangesAsync();
        return MapToDto(session);
    }

    public async Task<bool> CancelSessionAsync(Guid clubId, Guid id, string? reason)
    {
        var session = await _context.Sessions.IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.ClubId == clubId && s.Id == id);

        if (session == null) return false;

        session.IsCancelled = true;
        session.CancellationReason = reason;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<SessionBookingDto>> GetSessionBookingsAsync(Guid clubId, Guid sessionId)
    {
        var bookings = await _context.SessionBookings.IgnoreQueryFilters()
            .Where(b => b.ClubId == clubId && b.SessionId == sessionId)
            .Include(b => b.Member)
            .Include(b => b.FamilyMember)
            .ToListAsync();

        return bookings.Select(MapBookingToDto);
    }

    public async Task<IEnumerable<MemberBookingDto>> GetMemberBookingsAsync(Guid clubId, Guid memberId)
    {
        var bookings = await _context.SessionBookings.IgnoreQueryFilters()
            .Where(b => b.ClubId == clubId && b.MemberId == memberId && b.Status == BookingStatus.Confirmed)
            .Include(b => b.Session).ThenInclude(s => s.Venue)
            .Include(b => b.FamilyMember)
            .OrderBy(b => b.Session.StartTime)
            .ToListAsync();

        return bookings.Select(b => new MemberBookingDto(
            b.Id,
            b.SessionId,
            b.Session?.Title ?? "Unknown",
            b.Session?.StartTime ?? DateTime.MinValue,
            b.Session?.EndTime ?? DateTime.MinValue,
            b.Session?.Venue?.Name,
            b.FamilyMember?.FullName,
            b.Session != null && b.Session.StartTime > DateTime.UtcNow.AddHours(24), // Can cancel if > 24h before
            b.Status.ToString()
        ));
    }

    public async Task<SessionBookingDto> BookSessionAsync(Guid clubId, Guid sessionId, Guid memberId, BookSessionRequest request)
    {
        var session = await _context.Sessions.FindAsync(sessionId);
        if (session == null) throw new InvalidOperationException("Session not found");
        if (session.CurrentBookings >= session.Capacity) throw new InvalidOperationException("Session is full");

        var existingBooking = await _context.SessionBookings.IgnoreQueryFilters()
            .FirstOrDefaultAsync(b =>
                b.ClubId == clubId &&
                b.SessionId == sessionId &&
                b.MemberId == memberId &&
                b.FamilyMemberId == request.FamilyMemberId &&
                b.Status == BookingStatus.Confirmed);

        if (existingBooking != null) throw new InvalidOperationException("Already booked");

        var booking = new SessionBooking
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            SessionId = sessionId,
            MemberId = memberId,
            FamilyMemberId = request.FamilyMemberId,
            Notes = request.Notes,
            Status = BookingStatus.Confirmed,
            BookedAt = DateTime.UtcNow
        };

        _context.SessionBookings.Add(booking);
        session.CurrentBookings++;
        await _context.SaveChangesAsync();

        // Use IgnoreQueryFilters since we just created this booking with the correct ClubId
        booking = await _context.SessionBookings
            .IgnoreQueryFilters()
            .Include(b => b.Member)
            .Include(b => b.FamilyMember)
            .FirstAsync(b => b.Id == booking.Id);

        return MapBookingToDto(booking);
    }

    public async Task<bool> CancelBookingAsync(Guid clubId, Guid sessionId, Guid memberId, Guid? familyMemberId)
    {
        var booking = await _context.SessionBookings.IgnoreQueryFilters()
            .FirstOrDefaultAsync(b =>
                b.ClubId == clubId &&
                b.SessionId == sessionId &&
                b.MemberId == memberId &&
                b.FamilyMemberId == familyMemberId &&
                b.Status == BookingStatus.Confirmed);

        if (booking == null) return false;

        booking.Status = BookingStatus.Cancelled;
        booking.CancelledAt = DateTime.UtcNow;

        var session = await _context.Sessions.FindAsync(sessionId);
        if (session != null) session.CurrentBookings = Math.Max(0, session.CurrentBookings - 1);

        // Check waitlist and notify first person
        var waitlistEntry = await _context.Waitlists.IgnoreQueryFilters()
            .Where(w => w.ClubId == clubId && w.SessionId == sessionId)
            .OrderBy(w => w.Position)
            .FirstOrDefaultAsync();

        if (waitlistEntry != null)
        {
            waitlistEntry.NotificationSent = true;
            waitlistEntry.NotificationSentAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAttendanceAsync(Guid clubId, Guid sessionId, BulkAttendanceRequest request)
    {
        foreach (var attendance in request.Attendances)
        {
            var booking = await _context.SessionBookings.FindAsync(attendance.BookingId);
            if (booking != null)
            {
                booking.Attended = attendance.Attended;
                booking.CheckedInAt = attendance.Attended ? DateTime.UtcNow : null;
                booking.Status = attendance.Attended ? BookingStatus.Attended : BookingStatus.NoShow;
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<WaitlistDto>> GetSessionWaitlistAsync(Guid clubId, Guid sessionId)
    {
        var entries = await _context.Waitlists.IgnoreQueryFilters()
            .Where(w => w.ClubId == clubId && w.SessionId == sessionId)
            .Include(w => w.Member)
            .OrderBy(w => w.Position)
            .ToListAsync();

        return entries.Select(w => new WaitlistDto(
            w.Id,
            w.SessionId,
            w.MemberId,
            w.Member?.FullName ?? "Unknown",
            w.FamilyMemberId,
            null,
            w.Position,
            w.JoinedAt,
            w.NotificationSent,
            w.NotificationSentAt
        ));
    }

    public async Task<WaitlistDto> JoinWaitlistAsync(Guid clubId, Guid sessionId, Guid memberId, Guid? familyMemberId)
    {
        var maxPosition = await _context.Waitlists.IgnoreQueryFilters()
            .Where(w => w.ClubId == clubId && w.SessionId == sessionId)
            .MaxAsync(w => (int?)w.Position) ?? 0;

        var entry = new Waitlist
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            SessionId = sessionId,
            MemberId = memberId,
            FamilyMemberId = familyMemberId,
            Position = maxPosition + 1,
            JoinedAt = DateTime.UtcNow
        };

        _context.Waitlists.Add(entry);
        await _context.SaveChangesAsync();

        var member = await _context.Members.FindAsync(memberId);
        return new WaitlistDto(
            entry.Id,
            entry.SessionId,
            entry.MemberId,
            member?.FullName ?? "Unknown",
            entry.FamilyMemberId,
            null,
            entry.Position,
            entry.JoinedAt,
            entry.NotificationSent,
            entry.NotificationSentAt
        );
    }

    public async Task<bool> LeaveWaitlistAsync(Guid clubId, Guid sessionId, Guid memberId, Guid? familyMemberId)
    {
        var entry = await _context.Waitlists.IgnoreQueryFilters()
            .FirstOrDefaultAsync(w =>
                w.ClubId == clubId &&
                w.SessionId == sessionId &&
                w.MemberId == memberId &&
                w.FamilyMemberId == familyMemberId);

        if (entry == null) return false;

        _context.Waitlists.Remove(entry);
        await _context.SaveChangesAsync();
        return true;
    }

    // Recurring Schedules
    public async Task<IEnumerable<RecurringScheduleDto>> GetRecurringSchedulesAsync(Guid clubId)
    {
        var schedules = await _context.RecurringSchedules.IgnoreQueryFilters()
            .Where(rs => rs.ClubId == clubId)
            .Include(rs => rs.Venue)
            .Include(rs => rs.GeneratedSessions)
            .ToListAsync();

        return schedules.Select(MapScheduleToDto);
    }

    public async Task<RecurringScheduleDto?> GetRecurringScheduleByIdAsync(Guid clubId, Guid id)
    {
        var schedule = await _context.RecurringSchedules.IgnoreQueryFilters()
            .Include(rs => rs.Venue)
            .Include(rs => rs.GeneratedSessions)
            .FirstOrDefaultAsync(rs => rs.ClubId == clubId && rs.Id == id);

        return schedule == null ? null : MapScheduleToDto(schedule);
    }

    public async Task<RecurringScheduleDto> CreateRecurringScheduleAsync(Guid clubId, RecurringScheduleCreateRequest request)
    {
        var schedule = new RecurringSchedule
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            DayOfWeek = request.DayOfWeek,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Capacity = request.Capacity,
            VenueId = request.VenueId,
            ScheduleStartDate = request.ScheduleStartDate,
            ScheduleEndDate = request.ScheduleEndDate,
            SessionFee = request.SessionFee,
            IsActive = true
        };

        _context.RecurringSchedules.Add(schedule);
        await _context.SaveChangesAsync();

        return MapScheduleToDto(schedule);
    }

    public async Task<RecurringScheduleDto?> UpdateRecurringScheduleAsync(Guid clubId, Guid id, RecurringScheduleUpdateRequest request)
    {
        var schedule = await _context.RecurringSchedules.IgnoreQueryFilters()
            .Include(rs => rs.Venue)
            .Include(rs => rs.GeneratedSessions)
            .FirstOrDefaultAsync(rs => rs.ClubId == clubId && rs.Id == id);

        if (schedule == null) return null;

        if (request.Title != null) schedule.Title = request.Title;
        if (request.Description != null) schedule.Description = request.Description;
        if (request.Category.HasValue) schedule.Category = request.Category.Value;
        if (request.StartTime.HasValue) schedule.StartTime = request.StartTime.Value;
        if (request.EndTime.HasValue) schedule.EndTime = request.EndTime.Value;
        if (request.Capacity.HasValue) schedule.Capacity = request.Capacity.Value;
        if (request.VenueId.HasValue) schedule.VenueId = request.VenueId;
        if (request.ScheduleEndDate.HasValue) schedule.ScheduleEndDate = request.ScheduleEndDate;
        if (request.IsActive.HasValue) schedule.IsActive = request.IsActive.Value;
        if (request.SessionFee.HasValue) schedule.SessionFee = request.SessionFee;

        await _context.SaveChangesAsync();
        return MapScheduleToDto(schedule);
    }

    public async Task<bool> DeleteRecurringScheduleAsync(Guid clubId, Guid id)
    {
        var schedule = await _context.RecurringSchedules.IgnoreQueryFilters()
            .FirstOrDefaultAsync(rs => rs.ClubId == clubId && rs.Id == id);

        if (schedule == null) return false;

        schedule.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> GenerateSessionsAsync(Guid clubId, Guid scheduleId, GenerateSessionsRequest request)
    {
        var schedule = await _context.RecurringSchedules.FindAsync(scheduleId);
        if (schedule == null) return 0;

        var currentDate = request.FromDate;
        var count = 0;

        while (currentDate <= request.ToDate)
        {
            if (currentDate.DayOfWeek == schedule.DayOfWeek)
            {
                var existingSession = await _context.Sessions.IgnoreQueryFilters()
                    .FirstOrDefaultAsync(s =>
                        s.RecurringScheduleId == scheduleId &&
                        s.StartTime.Date == currentDate.Date);

                if (existingSession == null)
                {
                    var session = new Session
                    {
                        Id = Guid.NewGuid(),
                        ClubId = clubId,
                        RecurringScheduleId = scheduleId,
                        Title = schedule.Title,
                        Description = schedule.Description,
                        Category = schedule.Category,
                        StartTime = currentDate.Date + schedule.StartTime,
                        EndTime = currentDate.Date + schedule.EndTime,
                        Capacity = schedule.Capacity,
                        VenueId = schedule.VenueId,
                        SessionFee = schedule.SessionFee,
                        IsRecurring = true
                    };

                    _context.Sessions.Add(session);
                    count++;
                }
            }
            currentDate = currentDate.AddDays(1);
        }

        await _context.SaveChangesAsync();
        return count;
    }

    public async Task<IEnumerable<RecurringBookingDto>> GetMemberRecurringBookingsAsync(Guid clubId, Guid memberId)
    {
        var bookings = await _context.RecurringBookings.IgnoreQueryFilters()
            .Where(rb => rb.ClubId == clubId && rb.MemberId == memberId)
            .Include(rb => rb.RecurringSchedule)
            .Include(rb => rb.Member)
            .Include(rb => rb.FamilyMember)
            .ToListAsync();

        return bookings.Select(rb => new RecurringBookingDto(
            rb.Id,
            rb.MemberId,
            rb.Member?.FullName ?? "Unknown",
            rb.FamilyMemberId,
            rb.FamilyMember?.FullName,
            rb.RecurringScheduleId,
            rb.RecurringSchedule?.Title ?? "Unknown",
            rb.StartDate,
            rb.EndDate,
            rb.IsActive,
            rb.Notes,
            rb.CreatedAt
        ));
    }

    public async Task<RecurringBookingDto> CreateRecurringBookingAsync(Guid clubId, Guid memberId, RecurringBookingCreateRequest request)
    {
        var booking = new RecurringBooking
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            MemberId = memberId,
            FamilyMemberId = request.FamilyMemberId,
            RecurringScheduleId = request.RecurringScheduleId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Notes = request.Notes,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.RecurringBookings.Add(booking);
        await _context.SaveChangesAsync();

        booking = await _context.RecurringBookings
            .Include(rb => rb.RecurringSchedule)
            .Include(rb => rb.Member)
            .FirstAsync(rb => rb.Id == booking.Id);

        return new RecurringBookingDto(
            booking.Id,
            booking.MemberId,
            booking.Member?.FullName ?? "Unknown",
            booking.FamilyMemberId,
            null,
            booking.RecurringScheduleId,
            booking.RecurringSchedule?.Title ?? "Unknown",
            booking.StartDate,
            booking.EndDate,
            booking.IsActive,
            booking.Notes,
            booking.CreatedAt
        );
    }

    public async Task<bool> CancelRecurringBookingAsync(Guid clubId, Guid id)
    {
        var booking = await _context.RecurringBookings.IgnoreQueryFilters()
            .FirstOrDefaultAsync(rb => rb.ClubId == clubId && rb.Id == id);

        if (booking == null) return false;

        booking.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    private static SessionDto MapToDto(Session s) => new(
        s.Id,
        s.Title,
        s.Description,
        s.Category,
        s.StartTime,
        s.EndTime,
        s.Capacity,
        s.CurrentBookings,
        s.Capacity - s.CurrentBookings,
        s.IsRecurring,
        s.RecurringScheduleId,
        s.SessionFee,
        s.IsCancelled,
        s.CancellationReason,
        s.Venue == null ? null : new VenueDto(
            s.Venue.Id,
            s.Venue.Name,
            s.Venue.Description,
            s.Venue.AddressLine1,
            s.Venue.PostCode,
            s.Venue.Latitude,
            s.Venue.Longitude,
            s.Venue.TotalCapacity,
            s.Venue.AdditionalAmenities,
            s.Venue.ImageUrl,
            s.Venue.IsActive,
            s.Venue.IsPrimary,
            0, 0
        ),
        s.Bookings?.Select(MapBookingToDto)
    );

    private static SessionBookingDto MapBookingToDto(SessionBooking b) => new(
        b.Id,
        b.MemberId,
        b.Member?.FullName ?? "Unknown",
        b.FamilyMemberId,
        b.FamilyMember?.FullName,
        b.BookedAt,
        b.Status,
        b.Attended,
        b.CheckedInAt,
        b.Notes
    );

    private static RecurringScheduleDto MapScheduleToDto(RecurringSchedule rs) => new(
        rs.Id,
        rs.Title,
        rs.Description,
        rs.Category,
        rs.DayOfWeek,
        rs.StartTime,
        rs.EndTime,
        rs.Capacity,
        rs.ScheduleStartDate,
        rs.ScheduleEndDate,
        rs.IsActive,
        rs.SessionFee,
        rs.Venue == null ? null : new VenueDto(
            rs.Venue.Id,
            rs.Venue.Name,
            rs.Venue.Description,
            rs.Venue.AddressLine1,
            rs.Venue.PostCode,
            rs.Venue.Latitude,
            rs.Venue.Longitude,
            rs.Venue.TotalCapacity,
            rs.Venue.AdditionalAmenities,
            rs.Venue.ImageUrl,
            rs.Venue.IsActive,
            rs.Venue.IsPrimary,
            0, 0
        ),
        rs.GeneratedSessions?.Count ?? 0
    );
}
