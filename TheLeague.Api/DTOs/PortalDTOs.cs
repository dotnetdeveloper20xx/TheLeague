namespace TheLeague.Api.DTOs;

public record MemberPortalDashboardDto(
    MemberDto Profile,
    MembershipSummaryDto? CurrentMembership,
    IEnumerable<UpcomingBookingDto> UpcomingBookings,
    IEnumerable<EventListDto> UpcomingEvents,
    IEnumerable<PaymentListDto> RecentPayments,
    int TotalBookingsThisMonth,
    int AttendedSessionsThisMonth
);

public record UpcomingBookingDto(
    Guid BookingId,
    Guid SessionId,
    string SessionTitle,
    DateTime StartTime,
    DateTime EndTime,
    string? VenueName,
    string? FamilyMemberName,
    bool CanCancel
);

public record MyBookingsDto(
    IEnumerable<UpcomingBookingDto> UpcomingBookings,
    IEnumerable<PastBookingDto> PastBookings,
    IEnumerable<RecurringBookingDto> RecurringBookings
);

public record PastBookingDto(
    Guid BookingId,
    Guid SessionId,
    string SessionTitle,
    DateTime Date,
    bool Attended,
    string? FamilyMemberName
);

public record AvailableSessionDto(
    Guid Id,
    string Title,
    string? Description,
    string Category,
    DateTime StartTime,
    DateTime EndTime,
    int AvailableSpots,
    int Capacity,
    decimal? SessionFee,
    string? VenueName,
    bool IsBooked,
    bool IsOnWaitlist,
    int? WaitlistPosition
);

public record MyTicketsDto(
    IEnumerable<EventTicketDto> UpcomingTickets,
    IEnumerable<EventTicketDto> PastTickets
);

public record ProfileUpdateRequest(
    string? FirstName,
    string? LastName,
    string? Phone,
    string? Address,
    string? City,
    string? PostCode,
    string? EmergencyContactName,
    string? EmergencyContactPhone,
    string? EmergencyContactRelation,
    string? MedicalConditions,
    string? Allergies
);

public record SessionBookRequest(
    Guid? FamilyMemberId
);

public record EventRegistrationRequest(
    int? TicketQuantity
);
