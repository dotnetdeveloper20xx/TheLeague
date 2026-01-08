using System.ComponentModel.DataAnnotations;

namespace TheLeague.Api.DTOs;

public record VenueDto(
    Guid Id,
    string Name,
    string? Description,
    string? Address,
    string? PostCode,
    decimal? Latitude,
    decimal? Longitude,
    int? Capacity,
    string? Facilities,
    string? ImageUrl,
    bool IsActive,
    bool IsPrimary,
    int SessionCount,
    int EventCount
);

public record VenueCreateRequest(
    [Required] string Name,
    string? Description,
    string? Address,
    string? PostCode,
    decimal? Latitude,
    decimal? Longitude,
    int? Capacity,
    string? Facilities,
    string? ImageUrl,
    bool IsPrimary = false
);

public record VenueUpdateRequest(
    string? Name,
    string? Description,
    string? Address,
    string? PostCode,
    decimal? Latitude,
    decimal? Longitude,
    int? Capacity,
    string? Facilities,
    string? ImageUrl,
    bool? IsActive,
    bool? IsPrimary
);

public record VenueScheduleDto(
    Guid VenueId,
    string VenueName,
    IEnumerable<SessionListDto> UpcomingSessions,
    IEnumerable<EventListDto> UpcomingEvents
);
