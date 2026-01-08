using System.ComponentModel.DataAnnotations;
using TheLeague.Core.Enums;

namespace TheLeague.Api.DTOs;

public record EventDto(
    Guid Id,
    string Title,
    string? Description,
    EventType Type,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string? Location,
    int? Capacity,
    int CurrentAttendees,
    int? AvailableSpots,
    bool IsTicketed,
    decimal? TicketPrice,
    decimal? MemberTicketPrice,
    DateTime? TicketSalesEndDate,
    bool RequiresRSVP,
    DateTime? RSVPDeadline,
    bool IsCancelled,
    string? CancellationReason,
    bool IsPublished,
    string? ImageUrl,
    VenueDto? Venue
);

public record EventListDto(
    Guid Id,
    string Title,
    EventType Type,
    DateTime StartDateTime,
    DateTime EndDateTime,
    int? Capacity,
    int CurrentAttendees,
    bool IsTicketed,
    decimal? TicketPrice,
    bool IsCancelled,
    bool IsPublished,
    string? ImageUrl
);

public record EventCreateRequest(
    [Required] string Title,
    string? Description,
    EventType Type,
    [Required] DateTime StartDateTime,
    [Required] DateTime EndDateTime,
    string? Location,
    Guid? VenueId,
    int? Capacity,
    bool IsTicketed,
    decimal? TicketPrice,
    decimal? MemberTicketPrice,
    DateTime? TicketSalesEndDate,
    bool RequiresRSVP,
    DateTime? RSVPDeadline,
    string? ImageUrl,
    bool IsPublished = true
);

public record EventUpdateRequest(
    string? Title,
    string? Description,
    EventType? Type,
    DateTime? StartDateTime,
    DateTime? EndDateTime,
    string? Location,
    Guid? VenueId,
    int? Capacity,
    bool? IsTicketed,
    decimal? TicketPrice,
    decimal? MemberTicketPrice,
    DateTime? TicketSalesEndDate,
    bool? RequiresRSVP,
    DateTime? RSVPDeadline,
    string? ImageUrl,
    bool? IsPublished,
    bool? IsCancelled,
    string? CancellationReason
);

public record EventTicketDto(
    Guid Id,
    Guid EventId,
    string EventTitle,
    Guid MemberId,
    string MemberName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalAmount,
    string TicketCode,
    bool IsUsed,
    DateTime? UsedAt,
    DateTime PurchasedAt
);

public record PurchaseTicketRequest(
    [Required] int Quantity
);

public record EventRSVPDto(
    Guid Id,
    Guid EventId,
    Guid MemberId,
    string MemberName,
    RSVPResponse Response,
    int GuestCount,
    string? Notes,
    DateTime RespondedAt
);

public record RSVPRequest(
    [Required] RSVPResponse Response,
    int GuestCount = 0,
    string? Notes = null
);

public record EventFilterRequest(
    DateTime? DateFrom = null,
    DateTime? DateTo = null,
    EventType? Type = null,
    bool? IsTicketed = null,
    bool? IncludeCancelled = null,
    bool? IncludeUnpublished = null,
    int Page = 1,
    int PageSize = 20
);

public record EventAttendeesDto(
    IEnumerable<EventTicketDto> TicketHolders,
    IEnumerable<EventRSVPDto> RSVPs,
    int TotalTickets,
    int TotalAttending,
    int TotalMaybe,
    int TotalNotAttending
);
