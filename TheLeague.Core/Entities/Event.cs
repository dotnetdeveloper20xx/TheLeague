using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

public class Event
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid? VenueId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public EventType Type { get; set; }

    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? Location { get; set; }

    public int? Capacity { get; set; }
    public int CurrentAttendees { get; set; }

    public bool IsTicketed { get; set; }
    public decimal? TicketPrice { get; set; }
    public decimal? MemberTicketPrice { get; set; }
    public DateTime? TicketSalesEndDate { get; set; }

    public bool RequiresRSVP { get; set; }
    public DateTime? RSVPDeadline { get; set; }

    public bool IsCancelled { get; set; }
    public string? CancellationReason { get; set; }
    public bool IsPublished { get; set; } = true;

    public string? ImageUrl { get; set; }

    public Club Club { get; set; } = null!;
    public Venue? Venue { get; set; }
    public ICollection<EventTicket> Tickets { get; set; } = new List<EventTicket>();
    public ICollection<EventRSVP> RSVPs { get; set; } = new List<EventRSVP>();
}
