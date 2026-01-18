using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Enums;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Controllers;

[Route("api/portal")]
[Authorize(Roles = "Member,ClubManager,SuperAdmin")]
public class PortalController : BaseApiController
{
    private readonly IMemberService _memberService;
    private readonly IMembershipService _membershipService;
    private readonly IPaymentService _paymentService;
    private readonly ISessionService _sessionService;
    private readonly IEventService _eventService;

    public PortalController(
        IMemberService memberService,
        IMembershipService membershipService,
        IPaymentService paymentService,
        ISessionService sessionService,
        IEventService eventService,
        ITenantService tenantService)
        : base(tenantService)
    {
        _memberService = memberService;
        _membershipService = membershipService;
        _paymentService = paymentService;
        _sessionService = sessionService;
        _eventService = eventService;
    }

    private Guid GetMemberIdValue() => GetMemberId() ?? Guid.Empty;

    [HttpGet("dashboard")]
    public async Task<ActionResult<MemberPortalDashboardDto>> GetDashboard()
    {
        var clubId = GetClubId();
        var memberId = GetMemberIdValue();

        var profile = await _memberService.GetMemberByIdAsync(clubId, memberId);
        if (profile == null)
            return NotFound();

        var memberships = await _membershipService.GetMemberMembershipsAsync(clubId, memberId);
        var currentMembership = memberships.FirstOrDefault(m => m.Status == MembershipStatus.Active);

        var payments = await _paymentService.GetMemberPaymentsAsync(clubId, memberId);
        var recentPayments = payments.Take(5).Select(p => new PaymentListDto(
            p.Id, p.MemberName, p.Amount, p.Currency, p.Status, p.Method, p.Type, p.PaymentDate, p.ReceiptNumber
        ));

        var filter = new SessionFilterRequest(DateFrom: DateTime.UtcNow, DateTo: DateTime.UtcNow.AddDays(30));
        var sessions = await _sessionService.GetSessionsAsync(clubId, filter);

        var eventFilter = new EventFilterRequest(DateFrom: DateTime.UtcNow, DateTo: DateTime.UtcNow.AddMonths(2));
        var events = await _eventService.GetEventsAsync(clubId, eventFilter);

        // Get actual upcoming bookings
        var memberBookings = await _sessionService.GetMemberBookingsAsync(clubId, memberId);
        var upcomingBookings = memberBookings
            .Where(b => b.StartTime > DateTime.UtcNow)
            .Take(5)
            .Select(b => new UpcomingBookingDto(
                b.BookingId,
                b.SessionId,
                b.SessionTitle,
                b.StartTime,
                b.EndTime,
                b.VenueName,
                b.FamilyMemberName,
                b.CanCancel
            ))
            .ToList();

        MembershipSummaryDto? membershipSummary = null;
        if (currentMembership != null)
        {
            membershipSummary = new MembershipSummaryDto(
                currentMembership.Id,
                currentMembership.MembershipTypeName,
                currentMembership.StartDate,
                currentMembership.EndDate,
                currentMembership.Status,
                currentMembership.AmountDue,
                currentMembership.AutoRenew
            );
        }

        return Ok(new MemberPortalDashboardDto(
            profile,
            membershipSummary,
            upcomingBookings,
            events.Items,
            recentPayments,
            0,
            0
        ));
    }

    [HttpGet("profile")]
    public async Task<ActionResult<MemberDto>> GetProfile()
    {
        var clubId = GetClubId();
        var memberId = GetMemberIdValue();

        var member = await _memberService.GetMemberByIdAsync(clubId, memberId);
        if (member == null)
            return NotFound();
        return Ok(member);
    }

    [HttpPut("profile")]
    public async Task<ActionResult<MemberDto>> UpdateProfile([FromBody] ProfileUpdateRequest request)
    {
        var clubId = GetClubId();
        var memberId = GetMemberIdValue();

        var updateRequest = new MemberUpdateRequest(
            request.FirstName,
            request.LastName,
            request.Phone,
            null,
            request.Address,
            request.City,
            request.PostCode,
            request.EmergencyContactName,
            request.EmergencyContactPhone,
            request.EmergencyContactRelation,
            request.MedicalConditions,
            request.Allergies,
            null,
            null
        );

        var member = await _memberService.UpdateMemberAsync(clubId, memberId, updateRequest);
        if (member == null)
            return NotFound();
        return Ok(member);
    }

    [HttpGet("membership")]
    public async Task<ActionResult<IEnumerable<MembershipDto>>> GetMemberships()
    {
        var clubId = GetClubId();
        var memberId = GetMemberIdValue();

        var memberships = await _membershipService.GetMemberMembershipsAsync(clubId, memberId);
        return Ok(memberships);
    }

    [HttpGet("payments")]
    public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPayments()
    {
        var clubId = GetClubId();
        var memberId = GetMemberIdValue();

        var payments = await _paymentService.GetMemberPaymentsAsync(clubId, memberId);
        return Ok(payments);
    }

    [HttpGet("family")]
    public async Task<ActionResult<IEnumerable<FamilyMemberDto>>> GetFamily()
    {
        var clubId = GetClubId();
        var memberId = GetMemberIdValue();

        var family = await _memberService.GetFamilyMembersAsync(clubId, memberId);
        return Ok(family);
    }

    [HttpPost("family")]
    public async Task<ActionResult<FamilyMemberDto>> AddFamilyMember([FromBody] FamilyMemberCreateRequest request)
    {
        var clubId = GetClubId();
        var memberId = GetMemberIdValue();

        var member = await _memberService.AddFamilyMemberAsync(clubId, memberId, request);
        return Ok(member);
    }

    [HttpPost("profile/photo")]
    public async Task<ActionResult> UploadProfilePhoto(IFormFile file)
    {
        var clubId = GetClubId();
        var memberId = GetMemberIdValue();

        // In a real app, save the file and update member
        var url = $"/uploads/members/{memberId}/photo{Path.GetExtension(file.FileName)}";
        return Ok(new { profilePhotoUrl = url });
    }

    // Sessions endpoints - matching Angular expected routes
    [HttpGet("sessions/upcoming")]
    public async Task<ActionResult> GetUpcomingSessions([FromQuery] SessionFilterRequest? filter)
    {
        var clubId = GetClubId();
        filter ??= new SessionFilterRequest();
        filter = filter with { DateFrom = DateTime.UtcNow, DateTo = DateTime.UtcNow.AddDays(30) };

        var sessions = await _sessionService.GetSessionsAsync(clubId, filter);
        return Ok(sessions.Items);
    }

    [HttpGet("sessions/my-bookings")]
    public async Task<ActionResult> GetMySessionBookings()
    {
        var clubId = GetClubId();
        var memberId = GetMemberIdValue();

        var bookings = await _sessionService.GetMemberBookingsAsync(clubId, memberId);
        return Ok(bookings);
    }

    [HttpPost("sessions/{sessionId}/book")]
    public async Task<ActionResult> BookSession(Guid sessionId, [FromBody] SessionBookRequest? request)
    {
        var clubId = GetClubId();
        var memberId = GetMemberIdValue();

        try
        {
            var bookRequest = new BookSessionRequest(request?.FamilyMemberId, null);
            var booking = await _sessionService.BookSessionAsync(clubId, sessionId, memberId, bookRequest);
            return Ok(new { message = "Session booked successfully", bookingId = booking.Id });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("sessions/{sessionId}/booking")]
    public async Task<ActionResult> CancelSessionBooking(Guid sessionId, [FromQuery] Guid? familyMemberId)
    {
        var clubId = GetClubId();
        var memberId = GetMemberIdValue();

        var result = await _sessionService.CancelBookingAsync(clubId, sessionId, memberId, familyMemberId);
        if (!result)
            return NotFound(new { message = "Booking not found" });

        return NoContent();
    }

    // Events endpoints - matching Angular expected routes
    [HttpGet("events/upcoming")]
    public async Task<ActionResult> GetUpcomingEvents([FromQuery] EventFilterRequest? filter)
    {
        var clubId = GetClubId();
        filter ??= new EventFilterRequest();
        filter = filter with { DateFrom = DateTime.UtcNow };

        var events = await _eventService.GetEventsAsync(clubId, filter);
        return Ok(events.Items);
    }

    [HttpGet("events/my-registrations")]
    public async Task<ActionResult> GetMyEventRegistrations()
    {
        var clubId = GetClubId();
        var memberId = GetMemberIdValue();

        var tickets = await _eventService.GetMemberTicketsAsync(clubId, memberId);
        return Ok(tickets);
    }

    [HttpPost("events/{eventId}/register")]
    public async Task<ActionResult> RegisterForEvent(Guid eventId, [FromBody] EventRegistrationRequest? request)
    {
        var clubId = GetClubId();
        var memberId = GetMemberIdValue();

        try
        {
            // Get event to determine if it's ticketed or RSVP
            var evt = await _eventService.GetEventByIdAsync(clubId, eventId);
            if (evt == null)
                return NotFound(new { message = "Event not found" });

            if (evt.IsTicketed)
            {
                // Purchase ticket
                var ticketRequest = new PurchaseTicketRequest(request?.TicketQuantity ?? 1);
                var ticket = await _eventService.PurchaseTicketAsync(clubId, eventId, memberId, ticketRequest);
                return Ok(new { message = "Ticket purchased successfully", ticketId = ticket.Id, ticketCode = ticket.TicketCode });
            }
            else if (evt.RequiresRSVP)
            {
                // RSVP to event
                var rsvpRequest = new RSVPRequest(RSVPResponse.Attending, 0, null);
                var rsvp = await _eventService.RSVPToEventAsync(clubId, eventId, memberId, rsvpRequest);
                return Ok(new { message = "RSVP submitted successfully", rsvpId = rsvp.Id });
            }
            else
            {
                return BadRequest(new { message = "This event does not require registration" });
            }
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("events/{eventId}/registration")]
    public async Task<ActionResult> CancelEventRegistration(Guid eventId)
    {
        var clubId = GetClubId();
        var memberId = GetMemberIdValue();

        // Cancel RSVP by setting response to NotAttending
        var evt = await _eventService.GetEventByIdAsync(clubId, eventId);
        if (evt == null)
            return NotFound(new { message = "Event not found" });

        if (evt.RequiresRSVP)
        {
            var rsvpRequest = new RSVPRequest(RSVPResponse.NotAttending, 0, "Cancelled");
            await _eventService.RSVPToEventAsync(clubId, eventId, memberId, rsvpRequest);
        }

        return NoContent();
    }

    // Notifications endpoints
    [HttpGet("notifications")]
    public async Task<ActionResult> GetNotifications()
    {
        var memberId = GetMemberIdValue();

        // Return mock notifications
        var notifications = new List<object>
        {
            new { id = Guid.NewGuid(), type = "info", title = "Welcome!", message = "Welcome to the member portal.", createdAt = DateTime.UtcNow.AddDays(-1), isRead = true },
            new { id = Guid.NewGuid(), type = "reminder", title = "Session Reminder", message = "You have a session tomorrow.", createdAt = DateTime.UtcNow, isRead = false }
        };
        return Ok(notifications);
    }

    [HttpPost("notifications/{id}/read")]
    public async Task<ActionResult> MarkNotificationRead(Guid id)
    {
        // In a real app, mark as read in database
        return Ok();
    }

    [HttpPost("notifications/read-all")]
    public async Task<ActionResult> MarkAllNotificationsRead()
    {
        // In a real app, mark all as read in database
        return Ok();
    }

    // Legacy endpoints (keeping for backward compatibility)
    [HttpGet("bookings")]
    public async Task<ActionResult> GetMyBookings()
    {
        var clubId = GetClubId();
        var memberId = GetMemberIdValue();

        var recurringBookings = await _sessionService.GetMemberRecurringBookingsAsync(clubId, memberId);

        return Ok(new
        {
            UpcomingBookings = new List<UpcomingBookingDto>(),
            PastBookings = new List<PastBookingDto>(),
            RecurringBookings = recurringBookings
        });
    }

    [HttpGet("tickets")]
    public async Task<ActionResult<IEnumerable<EventTicketDto>>> GetMyTickets()
    {
        var clubId = GetClubId();
        var memberId = GetMemberIdValue();

        var tickets = await _eventService.GetMemberTicketsAsync(clubId, memberId);
        return Ok(tickets);
    }

    [HttpGet("events")]
    public async Task<ActionResult<PagedResult<EventListDto>>> GetEvents([FromQuery] EventFilterRequest? filter)
    {
        var clubId = GetClubId();
        filter ??= new EventFilterRequest();
        filter = filter with { DateFrom = DateTime.UtcNow };

        var events = await _eventService.GetEventsAsync(clubId, filter);
        return Ok(events);
    }
}
