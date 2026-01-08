namespace TheLeague.Core.Entities;

public class Venue
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? PostCode { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    public int? Capacity { get; set; }
    public string? Facilities { get; set; }
    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsPrimary { get; set; }

    public Club Club { get; set; } = null!;
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
    public ICollection<Event> Events { get; set; } = new List<Event>();
}
