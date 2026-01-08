using TheLeague.Api.DTOs;

namespace TheLeague.Api.Services.Interfaces;

public interface IEventService
{
    Task<PagedResult<EventListDto>> GetEventsAsync(Guid clubId, EventFilterRequest filter);
    Task<EventDto?> GetEventByIdAsync(Guid clubId, Guid id);
    Task<EventDto> CreateEventAsync(Guid clubId, EventCreateRequest request);
    Task<EventDto?> UpdateEventAsync(Guid clubId, Guid id, EventUpdateRequest request);
    Task<bool> CancelEventAsync(Guid clubId, Guid id, string? reason);

    // Tickets
    Task<EventTicketDto> PurchaseTicketAsync(Guid clubId, Guid eventId, Guid memberId, PurchaseTicketRequest request);
    Task<IEnumerable<EventTicketDto>> GetMemberTicketsAsync(Guid clubId, Guid memberId);
    Task<bool> ValidateTicketAsync(Guid clubId, string ticketCode);

    // RSVP
    Task<EventRSVPDto> RSVPToEventAsync(Guid clubId, Guid eventId, Guid memberId, RSVPRequest request);
    Task<EventAttendeesDto> GetEventAttendeesAsync(Guid clubId, Guid eventId);
}
