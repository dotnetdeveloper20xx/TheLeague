using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents a venue/location where club activities take place.
/// Contains facilities, operating hours, and venue details.
/// </summary>
public class Venue
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }

    // Basic Info
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsPrimary { get; set; }

    // Address
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? County { get; set; }
    public string? PostCode { get; set; }
    public string? Country { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? WhatThreeWords { get; set; }

    // Contact
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? ContactName { get; set; }

    // Capacity & Size
    public int? TotalCapacity { get; set; }
    public decimal? TotalArea { get; set; } // Square meters
    public string? AreaUnit { get; set; }

    // Ownership
    public string? OwnershipType { get; set; } // Owned, Leased, Shared
    public string? LandlordName { get; set; }
    public string? LandlordContact { get; set; }
    public DateTime? LeaseStartDate { get; set; }
    public DateTime? LeaseEndDate { get; set; }
    public decimal? MonthlyRent { get; set; }

    // Operating Hours (JSON stored)
    public string? OperatingHours { get; set; } // JSON: { "monday": { "open": "09:00", "close": "22:00" }, ... }
    public string? SpecialHours { get; set; } // JSON for holidays/special days
    public bool Open24Hours { get; set; }
    public string? TimeZone { get; set; }

    // Amenities
    public bool HasParking { get; set; }
    public int? ParkingSpaces { get; set; }
    public bool HasDisabledAccess { get; set; }
    public bool HasChangingRooms { get; set; }
    public bool HasShowers { get; set; }
    public bool HasLockers { get; set; }
    public bool HasCatering { get; set; }
    public bool HasWifi { get; set; }
    public bool HasFirstAid { get; set; }
    public bool HasDefibrillator { get; set; }
    public string? AdditionalAmenities { get; set; } // JSON array

    // Images & Media
    public string? ImageUrl { get; set; }
    public string? GalleryUrls { get; set; } // JSON array
    public string? VirtualTourUrl { get; set; }
    public string? FloorPlanUrl { get; set; }

    // Booking Settings
    public bool AllowOnlineBooking { get; set; } = true;
    public int? MinBookingDuration { get; set; } // Minutes
    public int? MaxBookingDuration { get; set; } // Minutes
    public int? BookingSlotDuration { get; set; } // Minutes (30, 60, etc.)
    public int? AdvanceBookingDays { get; set; } // How far in advance can book
    public int? CancellationNoticePeriod { get; set; } // Hours required for cancellation
    public decimal? CancellationFee { get; set; }
    public string? BookingInstructions { get; set; }

    // Insurance & Compliance
    public string? InsuranceProvider { get; set; }
    public string? InsurancePolicyNumber { get; set; }
    public DateTime? InsuranceExpiryDate { get; set; }
    public DateTime? LastInspectionDate { get; set; }
    public DateTime? NextInspectionDue { get; set; }
    public string? ComplianceCertificates { get; set; } // JSON array

    // Emergency
    public string? EmergencyContact { get; set; }
    public string? EmergencyProcedures { get; set; }
    public string? EvacuationPlanUrl { get; set; }
    public string? NearestHospital { get; set; }

    // Notes
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }
    public string? AccessInstructions { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public ICollection<Facility> Facilities { get; set; } = new List<Facility>();
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<VenueOperatingSchedule> OperatingSchedules { get; set; } = new List<VenueOperatingSchedule>();
    public ICollection<VenueHoliday> Holidays { get; set; } = new List<VenueHoliday>();
}

/// <summary>
/// Represents the regular operating schedule for a venue.
/// </summary>
public class VenueOperatingSchedule
{
    public Guid Id { get; set; }
    public Guid VenueId { get; set; }

    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }
    public bool IsClosed { get; set; }

    // Peak/Off-peak
    public TimeOnly? PeakStartTime { get; set; }
    public TimeOnly? PeakEndTime { get; set; }

    // Effective dates for seasonal schedules
    public DateTime? EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public string? SeasonName { get; set; }

    // Navigation
    public Venue Venue { get; set; } = null!;
}

/// <summary>
/// Represents holidays and special closures for a venue.
/// </summary>
public class VenueHoliday
{
    public Guid Id { get; set; }
    public Guid VenueId { get; set; }

    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public bool IsClosed { get; set; } = true;
    public TimeOnly? OpenTime { get; set; }
    public TimeOnly? CloseTime { get; set; }
    public bool IsRecurringAnnually { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public Venue Venue { get; set; } = null!;
}
