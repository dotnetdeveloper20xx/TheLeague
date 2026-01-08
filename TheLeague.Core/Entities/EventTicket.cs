namespace TheLeague.Core.Entities;

public class EventTicket
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid EventId { get; set; }
    public Guid MemberId { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }

    public string TicketCode { get; set; } = string.Empty;
    public bool IsUsed { get; set; }
    public DateTime? UsedAt { get; set; }

    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;
    public Guid? PaymentId { get; set; }

    public Event Event { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public Payment? Payment { get; set; }
}
