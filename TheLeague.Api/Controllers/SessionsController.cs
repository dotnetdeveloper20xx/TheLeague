using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Controllers;

[Authorize]
public class SessionsController : BaseApiController
{
    private readonly ISessionService _sessionService;

    public SessionsController(ISessionService sessionService, ITenantService tenantService)
        : base(tenantService)
    {
        _sessionService = sessionService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<SessionListDto>>> GetAll([FromQuery] SessionFilterRequest filter)
    {
        var clubId = GetClubId();
        var result = await _sessionService.GetSessionsAsync(clubId, filter);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SessionDto>> GetById(Guid id)
    {
        var clubId = GetClubId();
        var session = await _sessionService.GetSessionByIdAsync(clubId, id);
        if (session == null)
            return NotFound();
        return Ok(session);
    }

    [HttpPost]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<SessionDto>> Create([FromBody] SessionCreateRequest request)
    {
        var clubId = GetClubId();
        var session = await _sessionService.CreateSessionAsync(clubId, request);
        return CreatedAtAction(nameof(GetById), new { id = session.Id }, session);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<SessionDto>> Update(Guid id, [FromBody] SessionUpdateRequest request)
    {
        var clubId = GetClubId();
        var session = await _sessionService.UpdateSessionAsync(clubId, id, request);
        if (session == null)
            return NotFound();
        return Ok(session);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult> Cancel(Guid id, [FromQuery] string? reason)
    {
        var clubId = GetClubId();
        var result = await _sessionService.CancelSessionAsync(clubId, id, reason);
        if (!result)
            return NotFound();
        return NoContent();
    }

    // Bookings
    [HttpGet("{id}/bookings")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<IEnumerable<SessionBookingDto>>> GetBookings(Guid id)
    {
        var clubId = GetClubId();
        var bookings = await _sessionService.GetSessionBookingsAsync(clubId, id);
        return Ok(bookings);
    }

    [HttpPost("{id}/book")]
    public async Task<ActionResult<SessionBookingDto>> Book(Guid id, [FromBody] BookSessionRequest request)
    {
        var clubId = GetClubId();
        var memberId = GetMemberId() ?? Guid.Empty;
        var booking = await _sessionService.BookSessionAsync(clubId, id, memberId, request);
        return Ok(booking);
    }

    [HttpDelete("{id}/book")]
    public async Task<ActionResult> CancelBooking(Guid id, [FromQuery] Guid? familyMemberId)
    {
        var clubId = GetClubId();
        var memberId = GetMemberId() ?? Guid.Empty;
        var result = await _sessionService.CancelBookingAsync(clubId, id, memberId, familyMemberId);
        if (!result)
            return NotFound();
        return NoContent();
    }

    [HttpPost("{id}/attendance")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult> UpdateAttendance(Guid id, [FromBody] BulkAttendanceRequest request)
    {
        var clubId = GetClubId();
        await _sessionService.UpdateAttendanceAsync(clubId, id, request);
        return Ok();
    }

    // Waitlist
    [HttpGet("{id}/waitlist")]
    public async Task<ActionResult<IEnumerable<WaitlistDto>>> GetWaitlist(Guid id)
    {
        var clubId = GetClubId();
        var waitlist = await _sessionService.GetSessionWaitlistAsync(clubId, id);
        return Ok(waitlist);
    }

    [HttpPost("{id}/waitlist")]
    public async Task<ActionResult<WaitlistDto>> JoinWaitlist(Guid id, [FromQuery] Guid? familyMemberId)
    {
        var clubId = GetClubId();
        var memberId = GetMemberId() ?? Guid.Empty;
        var entry = await _sessionService.JoinWaitlistAsync(clubId, id, memberId, familyMemberId);
        return Ok(entry);
    }

    [HttpDelete("{id}/waitlist")]
    public async Task<ActionResult> LeaveWaitlist(Guid id, [FromQuery] Guid? familyMemberId)
    {
        var clubId = GetClubId();
        var memberId = GetMemberId() ?? Guid.Empty;
        var result = await _sessionService.LeaveWaitlistAsync(clubId, id, memberId, familyMemberId);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
