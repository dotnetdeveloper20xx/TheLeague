using Microsoft.EntityFrameworkCore;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Entities;
using TheLeague.Core.Enums;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Services;

public class PaymentService : IPaymentService
{
    private readonly ApplicationDbContext _context;
    private readonly Dictionary<string, MockPaymentIntent> _paymentIntents = new();
    private readonly Dictionary<string, MockPayPalOrder> _paypalOrders = new();

    public PaymentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<PaymentListDto>> GetPaymentsAsync(Guid clubId, PaymentFilterRequest filter)
    {
        var query = _context.Payments.IgnoreQueryFilters()
            .Where(p => p.ClubId == clubId)
            .Include(p => p.Member)
            .AsQueryable();

        if (filter.MemberId.HasValue)
            query = query.Where(p => p.MemberId == filter.MemberId.Value);
        if (filter.Status.HasValue)
            query = query.Where(p => p.Status == filter.Status.Value);
        if (filter.Method.HasValue)
            query = query.Where(p => p.Method == filter.Method.Value);
        if (filter.Type.HasValue)
            query = query.Where(p => p.Type == filter.Type.Value);
        if (filter.DateFrom.HasValue)
            query = query.Where(p => p.PaymentDate >= filter.DateFrom.Value);
        if (filter.DateTo.HasValue)
            query = query.Where(p => p.PaymentDate <= filter.DateTo.Value);

        query = filter.SortDescending
            ? query.OrderByDescending(p => p.PaymentDate)
            : query.OrderBy(p => p.PaymentDate);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(p => new PaymentListDto(
                p.Id,
                p.Member.FirstName + " " + p.Member.LastName,
                p.Amount,
                p.Currency,
                p.Status,
                p.Method,
                p.Type,
                p.PaymentDate,
                p.ReceiptNumber
            ))
            .ToListAsync();

        return new PagedResult<PaymentListDto>(
            items,
            totalCount,
            filter.Page,
            filter.PageSize,
            (int)Math.Ceiling(totalCount / (double)filter.PageSize)
        );
    }

    public async Task<PaymentDto?> GetPaymentByIdAsync(Guid clubId, Guid id)
    {
        var payment = await _context.Payments.IgnoreQueryFilters()
            .Include(p => p.Member)
            .FirstOrDefaultAsync(p => p.ClubId == clubId && p.Id == id);

        return payment == null ? null : MapToDto(payment);
    }

    public async Task<IEnumerable<PaymentDto>> GetMemberPaymentsAsync(Guid clubId, Guid memberId)
    {
        var payments = await _context.Payments.IgnoreQueryFilters()
            .Where(p => p.ClubId == clubId && p.MemberId == memberId)
            .Include(p => p.Member)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();

        return payments.Select(MapToDto);
    }

    public async Task<PaymentDto> RecordManualPaymentAsync(Guid clubId, ManualPaymentRequest request, string recordedBy)
    {
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            MemberId = request.MemberId,
            MembershipId = request.MembershipId,
            Amount = request.Amount,
            Currency = request.Currency,
            Status = PaymentStatus.Completed,
            Method = request.Method,
            Type = request.Type,
            Description = request.Description,
            PaymentDate = DateTime.UtcNow,
            ProcessedDate = DateTime.UtcNow,
            ManualPaymentReference = request.ManualPaymentReference,
            RecordedBy = recordedBy,
            ReceiptNumber = GenerateReceiptNumber()
        };

        _context.Payments.Add(payment);

        // Update membership if applicable
        if (request.MembershipId.HasValue)
        {
            var membership = await _context.Memberships.FindAsync(request.MembershipId.Value);
            if (membership != null)
            {
                membership.AmountPaid += request.Amount;
                membership.AmountDue = Math.Max(0, membership.AmountDue - request.Amount);
                membership.LastPaymentDate = DateTime.UtcNow;

                if (membership.AmountDue <= 0)
                {
                    membership.Status = MembershipStatus.Active;
                    var member = await _context.Members.FindAsync(membership.MemberId);
                    if (member != null) member.Status = MemberStatus.Active;
                }
            }
        }

        await _context.SaveChangesAsync();

        payment = await _context.Payments
            .Include(p => p.Member)
            .FirstAsync(p => p.Id == payment.Id);

        return MapToDto(payment);
    }

    public async Task<PaymentDto?> RefundPaymentAsync(Guid clubId, Guid id, RefundRequest request)
    {
        var payment = await _context.Payments.IgnoreQueryFilters()
            .Include(p => p.Member)
            .FirstOrDefaultAsync(p => p.ClubId == clubId && p.Id == id);

        if (payment == null) return null;

        var refundAmount = request.Amount ?? payment.Amount;

        // Create refund record
        var refund = new Payment
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            MemberId = payment.MemberId,
            MembershipId = payment.MembershipId,
            Amount = -refundAmount,
            Currency = payment.Currency,
            Status = PaymentStatus.Refunded,
            Method = payment.Method,
            Type = payment.Type,
            Description = $"Refund: {request.Reason ?? "No reason provided"}",
            PaymentDate = DateTime.UtcNow,
            ProcessedDate = DateTime.UtcNow,
            ReceiptNumber = GenerateReceiptNumber()
        };

        payment.Status = PaymentStatus.Refunded;
        _context.Payments.Add(refund);
        await _context.SaveChangesAsync();

        return MapToDto(refund);
    }

    public async Task<PaymentSummaryDto> GetPaymentSummaryAsync(Guid clubId)
    {
        var payments = await _context.Payments.IgnoreQueryFilters()
            .Where(p => p.ClubId == clubId && p.Status == PaymentStatus.Completed)
            .ToListAsync();

        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        var startOfYear = new DateTime(now.Year, 1, 1);

        var totalRevenue = payments.Sum(p => p.Amount);
        var totalRevenueThisMonth = payments.Where(p => p.PaymentDate >= startOfMonth).Sum(p => p.Amount);
        var totalRevenueThisYear = payments.Where(p => p.PaymentDate >= startOfYear).Sum(p => p.Amount);

        var outstandingPayments = await _context.Memberships.IgnoreQueryFilters()
            .Where(m => m.ClubId == clubId)
            .SumAsync(m => m.AmountDue);

        var byMethod = payments.GroupBy(p => p.Method)
            .Select(g => new PaymentByMethodDto(g.Key, g.Count(), g.Sum(p => p.Amount)));

        var byType = payments.GroupBy(p => p.Type)
            .Select(g => new PaymentByTypeDto(g.Key, g.Count(), g.Sum(p => p.Amount)));

        return new PaymentSummaryDto(
            totalRevenue,
            totalRevenueThisMonth,
            totalRevenueThisYear,
            outstandingPayments,
            payments.Count,
            payments.Count(p => p.PaymentDate >= startOfMonth),
            byMethod,
            byType
        );
    }

    // Mock Stripe Payment
    public Task<CreatePaymentIntentResponse> CreateStripePaymentIntentAsync(Guid clubId, CreatePaymentIntentRequest request)
    {
        var intentId = $"pi_{Guid.NewGuid():N}";
        var clientSecret = $"cs_{Guid.NewGuid():N}";

        _paymentIntents[intentId] = new MockPaymentIntent
        {
            ClubId = clubId,
            MemberId = request.MemberId,
            MembershipId = request.MembershipId,
            Amount = request.Amount,
            Currency = request.Currency,
            Type = request.Type,
            Description = request.Description
        };

        return Task.FromResult(new CreatePaymentIntentResponse(
            intentId,
            clientSecret,
            request.Amount,
            request.Currency,
            "requires_payment_method"
        ));
    }

    public async Task<PaymentDto> ProcessStripePaymentAsync(Guid clubId, string paymentIntentId)
    {
        if (!_paymentIntents.TryGetValue(paymentIntentId, out var intent))
            throw new InvalidOperationException("Payment intent not found");

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            MemberId = intent.MemberId,
            MembershipId = intent.MembershipId,
            Amount = intent.Amount,
            Currency = intent.Currency,
            Status = PaymentStatus.Completed,
            Method = PaymentMethod.Stripe,
            Type = intent.Type,
            Description = intent.Description,
            PaymentDate = DateTime.UtcNow,
            ProcessedDate = DateTime.UtcNow,
            StripePaymentIntentId = paymentIntentId,
            ReceiptNumber = GenerateReceiptNumber()
        };

        _context.Payments.Add(payment);

        if (intent.MembershipId.HasValue)
        {
            var membership = await _context.Memberships.FindAsync(intent.MembershipId.Value);
            if (membership != null)
            {
                membership.AmountPaid += intent.Amount;
                membership.AmountDue = Math.Max(0, membership.AmountDue - intent.Amount);
                membership.LastPaymentDate = DateTime.UtcNow;
                if (membership.AmountDue <= 0)
                {
                    membership.Status = MembershipStatus.Active;
                    var member = await _context.Members.FindAsync(membership.MemberId);
                    if (member != null) member.Status = MemberStatus.Active;
                }
            }
        }

        await _context.SaveChangesAsync();
        _paymentIntents.Remove(paymentIntentId);

        payment = await _context.Payments.Include(p => p.Member).FirstAsync(p => p.Id == payment.Id);
        return MapToDto(payment);
    }

    // Mock PayPal
    public Task<PayPalOrderResponse> CreatePayPalOrderAsync(Guid clubId, PayPalOrderRequest request)
    {
        var orderId = $"PP_{Guid.NewGuid():N}";

        _paypalOrders[orderId] = new MockPayPalOrder
        {
            ClubId = clubId,
            MemberId = request.MemberId,
            MembershipId = request.MembershipId,
            Amount = request.Amount,
            Currency = request.Currency,
            Type = request.Type,
            Description = request.Description
        };

        return Task.FromResult(new PayPalOrderResponse(
            orderId,
            $"{request.ReturnUrl}?orderId={orderId}",
            request.Amount,
            request.Currency,
            "CREATED"
        ));
    }

    public async Task<PaymentDto> CapturePayPalOrderAsync(Guid clubId, string orderId)
    {
        if (!_paypalOrders.TryGetValue(orderId, out var order))
            throw new InvalidOperationException("PayPal order not found");

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            MemberId = order.MemberId,
            MembershipId = order.MembershipId,
            Amount = order.Amount,
            Currency = order.Currency,
            Status = PaymentStatus.Completed,
            Method = PaymentMethod.PayPal,
            Type = order.Type,
            Description = order.Description,
            PaymentDate = DateTime.UtcNow,
            ProcessedDate = DateTime.UtcNow,
            PayPalTransactionId = orderId,
            ReceiptNumber = GenerateReceiptNumber()
        };

        _context.Payments.Add(payment);

        if (order.MembershipId.HasValue)
        {
            var membership = await _context.Memberships.FindAsync(order.MembershipId.Value);
            if (membership != null)
            {
                membership.AmountPaid += order.Amount;
                membership.AmountDue = Math.Max(0, membership.AmountDue - order.Amount);
                membership.LastPaymentDate = DateTime.UtcNow;
                if (membership.AmountDue <= 0)
                {
                    membership.Status = MembershipStatus.Active;
                    var member = await _context.Members.FindAsync(membership.MemberId);
                    if (member != null) member.Status = MemberStatus.Active;
                }
            }
        }

        await _context.SaveChangesAsync();
        _paypalOrders.Remove(orderId);

        payment = await _context.Payments.Include(p => p.Member).FirstAsync(p => p.Id == payment.Id);
        return MapToDto(payment);
    }

    private static string GenerateReceiptNumber() => $"RCP-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

    private static PaymentDto MapToDto(Payment p) => new(
        p.Id,
        p.MemberId,
        p.Member?.FullName ?? "Unknown",
        p.MembershipId,
        p.EventTicketId,
        p.Amount,
        p.Currency,
        p.Status,
        p.Method,
        p.Type,
        p.Description,
        p.PaymentDate,
        p.ProcessedDate,
        p.ReceiptNumber,
        p.ReceiptUrl,
        p.ManualPaymentReference,
        p.RecordedBy
    );

    private class MockPaymentIntent
    {
        public Guid ClubId { get; set; }
        public Guid MemberId { get; set; }
        public Guid? MembershipId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "GBP";
        public PaymentType Type { get; set; }
        public string? Description { get; set; }
    }

    private class MockPayPalOrder
    {
        public Guid ClubId { get; set; }
        public Guid MemberId { get; set; }
        public Guid? MembershipId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "GBP";
        public PaymentType Type { get; set; }
        public string? Description { get; set; }
    }
}
