using Microsoft.EntityFrameworkCore;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Entities;
using TheLeague.Core.Enums;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Services;

public class EventService : IEventService
{
    private readonly ApplicationDbContext _context;

    public EventService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<EventListDto>> GetEventsAsync(Guid clubId, EventFilterRequest filter)
    {
        var query = _context.Events.IgnoreQueryFilters()
            .Where(e => e.ClubId == clubId)
            .AsQueryable();

        if (filter.DateFrom.HasValue)
            query = query.Where(e => e.StartDateTime >= filter.DateFrom.Value);
        if (filter.DateTo.HasValue)
            query = query.Where(e => e.StartDateTime <= filter.DateTo.Value);
        if (filter.Type.HasValue)
            query = query.Where(e => e.Type == filter.Type.Value);
        if (filter.IsTicketed.HasValue)
            query = query.Where(e => e.IsTicketed == filter.IsTicketed.Value);
        if (!filter.IncludeCancelled.GetValueOrDefault())
            query = query.Where(e => !e.IsCancelled);
        if (!filter.IncludeUnpublished.GetValueOrDefault())
            query = query.Where(e => e.IsPublished);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(e => e.StartDateTime)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(e => new EventListDto(
                e.Id,
                e.Title,
                e.Type,
                e.StartDateTime,
                e.EndDateTime,
                e.Capacity,
                e.CurrentAttendees,
                e.IsTicketed,
                e.TicketPrice,
                e.IsCancelled,
                e.IsPublished,
                e.ImageUrl
            ))
            .ToListAsync();

        return new PagedResult<EventListDto>(
            items,
            totalCount,
            filter.Page,
            filter.PageSize,
            (int)Math.Ceiling(totalCount / (double)filter.PageSize)
        );
    }

    public async Task<EventDto?> GetEventByIdAsync(Guid clubId, Guid id)
    {
        var evt = await _context.Events.IgnoreQueryFilters()
            .Include(e => e.Venue)
            .FirstOrDefaultAsync(e => e.ClubId == clubId && e.Id == id);

        return evt == null ? null : MapToDto(evt);
    }

    public async Task<EventDto> CreateEventAsync(Guid clubId, EventCreateRequest request)
    {
        var evt = new Event
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Title = request.Title,
            Description = request.Description,
            Type = request.Type,
            StartDateTime = request.StartDateTime,
            EndDateTime = request.EndDateTime,
            Location = request.Location,
            VenueId = request.VenueId,
            Capacity = request.Capacity,
            IsTicketed = request.IsTicketed,
            TicketPrice = request.TicketPrice,
            MemberTicketPrice = request.MemberTicketPrice,
            TicketSalesEndDate = request.TicketSalesEndDate,
            RequiresRSVP = request.RequiresRSVP,
            RSVPDeadline = request.RSVPDeadline,
            ImageUrl = request.ImageUrl,
            IsPublished = request.IsPublished
        };

        _context.Events.Add(evt);
        await _context.SaveChangesAsync();

        return MapToDto(evt);
    }

    public async Task<EventDto?> UpdateEventAsync(Guid clubId, Guid id, EventUpdateRequest request)
    {
        var evt = await _context.Events.IgnoreQueryFilters()
            .Include(e => e.Venue)
            .FirstOrDefaultAsync(e => e.ClubId == clubId && e.Id == id);

        if (evt == null) return null;

        if (request.Title != null) evt.Title = request.Title;
        if (request.Description != null) evt.Description = request.Description;
        if (request.Type.HasValue) evt.Type = request.Type.Value;
        if (request.StartDateTime.HasValue) evt.StartDateTime = request.StartDateTime.Value;
        if (request.EndDateTime.HasValue) evt.EndDateTime = request.EndDateTime.Value;
        if (request.Location != null) evt.Location = request.Location;
        if (request.VenueId.HasValue) evt.VenueId = request.VenueId;
        if (request.Capacity.HasValue) evt.Capacity = request.Capacity;
        if (request.IsTicketed.HasValue) evt.IsTicketed = request.IsTicketed.Value;
        if (request.TicketPrice.HasValue) evt.TicketPrice = request.TicketPrice;
        if (request.MemberTicketPrice.HasValue) evt.MemberTicketPrice = request.MemberTicketPrice;
        if (request.TicketSalesEndDate.HasValue) evt.TicketSalesEndDate = request.TicketSalesEndDate;
        if (request.RequiresRSVP.HasValue) evt.RequiresRSVP = request.RequiresRSVP.Value;
        if (request.RSVPDeadline.HasValue) evt.RSVPDeadline = request.RSVPDeadline;
        if (request.ImageUrl != null) evt.ImageUrl = request.ImageUrl;
        if (request.IsPublished.HasValue) evt.IsPublished = request.IsPublished.Value;
        if (request.IsCancelled.HasValue) evt.IsCancelled = request.IsCancelled.Value;
        if (request.CancellationReason != null) evt.CancellationReason = request.CancellationReason;

        await _context.SaveChangesAsync();
        return MapToDto(evt);
    }

    public async Task<bool> CancelEventAsync(Guid clubId, Guid id, string? reason)
    {
        var evt = await _context.Events.IgnoreQueryFilters()
            .FirstOrDefaultAsync(e => e.ClubId == clubId && e.Id == id);

        if (evt == null) return false;

        evt.IsCancelled = true;
        evt.CancellationReason = reason;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<EventTicketDto> PurchaseTicketAsync(Guid clubId, Guid eventId, Guid memberId, PurchaseTicketRequest request)
    {
        var evt = await _context.Events.FindAsync(eventId);
        if (evt == null) throw new InvalidOperationException("Event not found");
        if (!evt.IsTicketed) throw new InvalidOperationException("Event is not ticketed");

        var member = await _context.Members.FindAsync(memberId);
        var unitPrice = member != null ? (evt.MemberTicketPrice ?? evt.TicketPrice ?? 0) : (evt.TicketPrice ?? 0);

        var ticket = new EventTicket
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            EventId = eventId,
            MemberId = memberId,
            TicketNumber = GenerateTicketCode(),
            Price = unitPrice,
            FinalPrice = unitPrice * request.Quantity,
            CreatedAt = DateTime.UtcNow
        };

        _context.EventTickets.Add(ticket);
        evt.CurrentAttendees += request.Quantity;
        await _context.SaveChangesAsync();

        ticket = await _context.EventTickets
            .Include(t => t.Event)
            .Include(t => t.Member)
            .FirstAsync(t => t.Id == ticket.Id);

        return MapTicketToDto(ticket);
    }

    public async Task<IEnumerable<EventTicketDto>> GetMemberTicketsAsync(Guid clubId, Guid memberId)
    {
        var tickets = await _context.EventTickets.IgnoreQueryFilters()
            .Where(t => t.ClubId == clubId && t.MemberId == memberId)
            .Include(t => t.Event)
            .Include(t => t.Member)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return tickets.Select(MapTicketToDto);
    }

    public async Task<bool> ValidateTicketAsync(Guid clubId, string ticketCode)
    {
        var ticket = await _context.EventTickets.IgnoreQueryFilters()
            .FirstOrDefaultAsync(t => t.ClubId == clubId && t.TicketNumber == ticketCode);

        if (ticket == null || ticket.IsCheckedIn) return false;

        ticket.IsCheckedIn = true;
        ticket.CheckedInAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<EventRSVPDto> RSVPToEventAsync(Guid clubId, Guid eventId, Guid memberId, RSVPRequest request)
    {
        var existing = await _context.EventRSVPs.IgnoreQueryFilters()
            .FirstOrDefaultAsync(r => r.ClubId == clubId && r.EventId == eventId && r.MemberId == memberId);

        if (existing != null)
        {
            existing.Response = request.Response;
            existing.GuestCount = request.GuestCount;
            existing.Notes = request.Notes;
            existing.RespondedAt = DateTime.UtcNow;
        }
        else
        {
            existing = new EventRSVP
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                EventId = eventId,
                MemberId = memberId,
                Response = request.Response,
                GuestCount = request.GuestCount,
                Notes = request.Notes,
                RespondedAt = DateTime.UtcNow
            };
            _context.EventRSVPs.Add(existing);
        }

        // Update attendee count
        var evt = await _context.Events.FindAsync(eventId);
        if (evt != null && request.Response == RSVPResponse.Attending)
        {
            evt.CurrentAttendees = await _context.EventRSVPs.IgnoreQueryFilters()
                .Where(r => r.EventId == eventId && r.Response == RSVPResponse.Attending)
                .SumAsync(r => 1 + r.GuestCount);
        }

        await _context.SaveChangesAsync();

        existing = await _context.EventRSVPs
            .Include(r => r.Member)
            .FirstAsync(r => r.Id == existing.Id);

        return MapRSVPToDto(existing);
    }

    public async Task<EventAttendeesDto> GetEventAttendeesAsync(Guid clubId, Guid eventId)
    {
        var tickets = await _context.EventTickets.IgnoreQueryFilters()
            .Where(t => t.ClubId == clubId && t.EventId == eventId)
            .Include(t => t.Event)
            .Include(t => t.Member)
            .ToListAsync();

        var rsvps = await _context.EventRSVPs.IgnoreQueryFilters()
            .Where(r => r.ClubId == clubId && r.EventId == eventId)
            .Include(r => r.Member)
            .ToListAsync();

        return new EventAttendeesDto(
            tickets.Select(MapTicketToDto),
            rsvps.Select(MapRSVPToDto),
            tickets.Count,
            rsvps.Count(r => r.Response == RSVPResponse.Attending),
            rsvps.Count(r => r.Response == RSVPResponse.Maybe),
            rsvps.Count(r => r.Response == RSVPResponse.NotAttending)
        );
    }

    private static string GenerateTicketCode() => $"TKT-{Guid.NewGuid().ToString()[..8].ToUpper()}";

    private static EventDto MapToDto(Event e) => new(
        e.Id,
        e.Title,
        e.Description,
        e.Type,
        e.StartDateTime,
        e.EndDateTime,
        e.Location,
        e.Capacity,
        e.CurrentAttendees,
        e.Capacity.HasValue ? e.Capacity - e.CurrentAttendees : null,
        e.IsTicketed,
        e.TicketPrice,
        e.MemberTicketPrice,
        e.TicketSalesEndDate,
        e.RequiresRSVP,
        e.RSVPDeadline,
        e.IsCancelled,
        e.CancellationReason,
        e.IsPublished,
        e.ImageUrl,
        e.Venue == null ? null : new VenueDto(
            e.Venue.Id, e.Venue.Name, e.Venue.Description, e.Venue.AddressLine1,
            e.Venue.PostCode, e.Venue.Latitude, e.Venue.Longitude, e.Venue.TotalCapacity,
            e.Venue.AdditionalAmenities, e.Venue.ImageUrl, e.Venue.IsActive, e.Venue.IsPrimary, 0, 0
        )
    );

    private static EventTicketDto MapTicketToDto(EventTicket t) => new(
        t.Id,
        t.EventId,
        t.Event?.Title ?? "Unknown",
        t.MemberId ?? Guid.Empty,
        t.Member?.FullName ?? "Unknown",
        1,
        t.Price,
        t.FinalPrice,
        t.TicketNumber,
        t.IsCheckedIn,
        t.CheckedInAt,
        t.CreatedAt
    );

    private static EventRSVPDto MapRSVPToDto(EventRSVP r) => new(
        r.Id,
        r.EventId,
        r.MemberId ?? Guid.Empty,
        r.Member?.FullName ?? r.Name ?? "Unknown",
        r.Response,
        r.GuestCount,
        r.Notes,
        r.RespondedAt
    );
}
