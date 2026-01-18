using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Controllers;

[Authorize]
public class EventsController : BaseApiController
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService, ITenantService tenantService)
        : base(tenantService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<EventListDto>>> GetAll([FromQuery] EventFilterRequest filter)
    {
        var clubId = GetClubId();
        var result = await _eventService.GetEventsAsync(clubId, filter);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetById(Guid id)
    {
        var clubId = GetClubId();
        var evt = await _eventService.GetEventByIdAsync(clubId, id);
        if (evt == null)
            return NotFound();
        return Ok(evt);
    }

    [HttpPost]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<EventDto>> Create([FromBody] EventCreateRequest request)
    {
        var clubId = GetClubId();
        var evt = await _eventService.CreateEventAsync(clubId, request);
        return CreatedAtAction(nameof(GetById), new { id = evt.Id }, evt);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<EventDto>> Update(Guid id, [FromBody] EventUpdateRequest request)
    {
        var clubId = GetClubId();
        var evt = await _eventService.UpdateEventAsync(clubId, id, request);
        if (evt == null)
            return NotFound();
        return Ok(evt);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult> Cancel(Guid id, [FromQuery] string? reason)
    {
        var clubId = GetClubId();
        var result = await _eventService.CancelEventAsync(clubId, id, reason);
        if (!result)
            return NotFound();
        return NoContent();
    }

    [HttpGet("{id}/attendees")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<EventAttendeesDto>> GetAttendees(Guid id)
    {
        var clubId = GetClubId();
        var attendees = await _eventService.GetEventAttendeesAsync(clubId, id);
        return Ok(attendees);
    }

    [HttpPost("{id}/rsvp")]
    public async Task<ActionResult<EventRSVPDto>> RSVP(Guid id, [FromBody] RSVPRequest request)
    {
        var clubId = GetClubId();
        var memberId = GetMemberId() ?? Guid.Empty;
        var rsvp = await _eventService.RSVPToEventAsync(clubId, id, memberId, request);
        return Ok(rsvp);
    }

    [HttpPost("{id}/purchase")]
    public async Task<ActionResult<EventTicketDto>> PurchaseTicket(Guid id, [FromBody] PurchaseTicketRequest request)
    {
        var clubId = GetClubId();
        var memberId = GetMemberId() ?? Guid.Empty;
        var ticket = await _eventService.PurchaseTicketAsync(clubId, id, memberId, request);
        return Ok(ticket);
    }

    [HttpPost("validate-ticket/{ticketCode}")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult> ValidateTicket(string ticketCode)
    {
        var clubId = GetClubId();
        var valid = await _eventService.ValidateTicketAsync(clubId, ticketCode);
        if (!valid)
            return BadRequest("Invalid or already used ticket");
        return Ok(new { Valid = true });
    }
}
