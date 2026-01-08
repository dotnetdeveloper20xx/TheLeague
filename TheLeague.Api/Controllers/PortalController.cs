using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Enums;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Controllers;

[ApiController]
[Route("api/portal")]
[Authorize(Roles = "Member,ClubManager,SuperAdmin")]
public class PortalController : ControllerBase
{
    private readonly IMemberService _memberService;
    private readonly IMembershipService _membershipService;
    private readonly IPaymentService _paymentService;
    private readonly ISessionService _sessionService;
    private readonly IEventService _eventService;
    private readonly ITenantService _tenantService;

    public PortalController(
        IMemberService memberService,
        IMembershipService membershipService,
        IPaymentService paymentService,
        ISessionService sessionService,
        IEventService eventService,
        ITenantService tenantService)
    {
        _memberService = memberService;
        _membershipService = membershipService;
        _paymentService = paymentService;
        _sessionService = sessionService;
        _eventService = eventService;
        _tenantService = tenantService;
    }

    private Guid GetClubId()
    {
        var clubIdClaim = User.FindFirst("clubId")?.Value;
        if (Guid.TryParse(clubIdClaim, out var clubId))
            return clubId;
        return _tenantService.CurrentTenantId ?? Guid.Empty;
    }

    private Guid GetMemberId()
    {
        var memberIdClaim = User.FindFirst("memberId")?.Value;
        if (Guid.TryParse(memberIdClaim, out var memberId))
            return memberId;
        return Guid.Empty;
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<MemberPortalDashboardDto>> GetDashboard()
    {
        var clubId = GetClubId();
        var memberId = GetMemberId();

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

        // Create a mock list of upcoming bookings (would normally come from bookings service)
        var upcomingBookings = new List<UpcomingBookingDto>();

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
        var memberId = GetMemberId();

        var member = await _memberService.GetMemberByIdAsync(clubId, memberId);
        if (member == null)
            return NotFound();
        return Ok(member);
    }

    [HttpPut("profile")]
    public async Task<ActionResult<MemberDto>> UpdateProfile([FromBody] ProfileUpdateRequest request)
    {
        var clubId = GetClubId();
        var memberId = GetMemberId();

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
        var memberId = GetMemberId();

        var memberships = await _membershipService.GetMemberMembershipsAsync(clubId, memberId);
        return Ok(memberships);
    }

    [HttpGet("payments")]
    public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPayments()
    {
        var clubId = GetClubId();
        var memberId = GetMemberId();

        var payments = await _paymentService.GetMemberPaymentsAsync(clubId, memberId);
        return Ok(payments);
    }

    [HttpGet("family")]
    public async Task<ActionResult<IEnumerable<FamilyMemberDto>>> GetFamily()
    {
        var clubId = GetClubId();
        var memberId = GetMemberId();

        var family = await _memberService.GetFamilyMembersAsync(clubId, memberId);
        return Ok(family);
    }

    [HttpPost("family")]
    public async Task<ActionResult<FamilyMemberDto>> AddFamilyMember([FromBody] FamilyMemberCreateRequest request)
    {
        var clubId = GetClubId();
        var memberId = GetMemberId();

        var member = await _memberService.AddFamilyMemberAsync(clubId, memberId, request);
        return Ok(member);
    }

    [HttpPost("profile/photo")]
    public async Task<ActionResult> UploadProfilePhoto(IFormFile file)
    {
        var clubId = GetClubId();
        var memberId = GetMemberId();

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
        var memberId = GetMemberId();

        var recurringBookings = await _sessionService.GetMemberRecurringBookingsAsync(clubId, memberId);

        // Return as simple array for Angular compatibility
        return Ok(new List<object>());
    }

    [HttpPost("sessions/{sessionId}/book")]
    public async Task<ActionResult> BookSession(Guid sessionId, [FromBody] SessionBookRequest? request)
    {
        var clubId = GetClubId();
        var memberId = GetMemberId();

        // In a real app, create booking
        return Ok(new { message = "Session booked successfully" });
    }

    [HttpDelete("sessions/{sessionId}/booking")]
    public async Task<ActionResult> CancelSessionBooking(Guid sessionId)
    {
        var clubId = GetClubId();
        var memberId = GetMemberId();

        // In a real app, cancel booking
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
        var memberId = GetMemberId();

        var tickets = await _eventService.GetMemberTicketsAsync(clubId, memberId);
        return Ok(tickets);
    }

    [HttpPost("events/{eventId}/register")]
    public async Task<ActionResult> RegisterForEvent(Guid eventId, [FromBody] EventRegistrationRequest? request)
    {
        var clubId = GetClubId();
        var memberId = GetMemberId();

        // In a real app, register for event
        return Ok(new { message = "Registered successfully" });
    }

    [HttpDelete("events/{eventId}/registration")]
    public async Task<ActionResult> CancelEventRegistration(Guid eventId)
    {
        var clubId = GetClubId();
        var memberId = GetMemberId();

        // In a real app, cancel registration
        return NoContent();
    }

    // Notifications endpoints
    [HttpGet("notifications")]
    public async Task<ActionResult> GetNotifications()
    {
        var memberId = GetMemberId();

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
        var memberId = GetMemberId();

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
        var memberId = GetMemberId();

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
