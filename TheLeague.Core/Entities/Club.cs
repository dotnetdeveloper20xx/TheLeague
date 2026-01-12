using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

public class Club
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string PrimaryColor { get; set; } = "#1E40AF";
    public string SecondaryColor { get; set; } = "#3B82F6";
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string? Address { get; set; }
    public string? Website { get; set; }
    public ClubType ClubType { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RenewalDate { get; set; }

    // Payment Settings
    public PaymentProvider PreferredPaymentProvider { get; set; } = PaymentProvider.Stripe;
    public string? StripeAccountId { get; set; }
    public string? PayPalClientId { get; set; }

    // SendGrid Settings
    public string? SendGridApiKey { get; set; }
    public string? FromEmail { get; set; }
    public string? FromName { get; set; }

    // Navigation
    public ICollection<Member> Members { get; set; } = new List<Member>();
    public ICollection<MembershipType> MembershipTypes { get; set; } = new List<MembershipType>();
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<Venue> Venues { get; set; } = new List<Venue>();
    public ICollection<CustomFieldDefinition> CustomFields { get; set; } = new List<CustomFieldDefinition>();
    public ICollection<CommunicationTemplate> CommunicationTemplates { get; set; } = new List<CommunicationTemplate>();
    public ClubSettings? Settings { get; set; }
}
