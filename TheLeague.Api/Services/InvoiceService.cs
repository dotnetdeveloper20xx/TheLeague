using Microsoft.EntityFrameworkCore;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Entities;
using TheLeague.Core.Enums;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Services;

public class InvoiceService : IInvoiceService
{
    private readonly ApplicationDbContext _context;

    public InvoiceService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<InvoiceListDto>> GetInvoicesAsync(Guid clubId, InvoiceFilterRequest filter)
    {
        var query = _context.Invoices.IgnoreQueryFilters()
            .Where(i => i.ClubId == clubId);

        // Apply filters
        if (filter.MemberId.HasValue)
            query = query.Where(i => i.MemberId == filter.MemberId.Value);

        if (filter.Status.HasValue)
            query = query.Where(i => i.Status == filter.Status.Value);

        if (filter.CollectionStatus.HasValue)
            query = query.Where(i => i.CollectionStatus == filter.CollectionStatus.Value);

        if (filter.IsOverdue.HasValue)
            query = query.Where(i => i.IsOverdue == filter.IsOverdue.Value);

        if (filter.DateFrom.HasValue)
            query = query.Where(i => i.InvoiceDate >= filter.DateFrom.Value);

        if (filter.DateTo.HasValue)
            query = query.Where(i => i.InvoiceDate <= filter.DateTo.Value);

        if (filter.DueDateFrom.HasValue)
            query = query.Where(i => i.DueDate >= filter.DueDateFrom.Value);

        if (filter.DueDateTo.HasValue)
            query = query.Where(i => i.DueDate <= filter.DueDateTo.Value);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.ToLower();
            query = query.Where(i =>
                i.InvoiceNumber.ToLower().Contains(search) ||
                i.Member.FirstName.ToLower().Contains(search) ||
                i.Member.LastName.ToLower().Contains(search) ||
                i.Member.Email.ToLower().Contains(search));
        }

        var totalCount = await query.CountAsync();

        var invoices = await query
            .Include(i => i.Member)
            .OrderByDescending(i => i.InvoiceDate)
            .ThenByDescending(i => i.InvoiceNumber)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(i => new InvoiceListDto(
                i.Id,
                i.InvoiceNumber,
                i.MemberId,
                $"{i.Member.FirstName} {i.Member.LastName}",
                i.InvoiceDate,
                i.DueDate,
                i.TotalAmount,
                i.PaidAmount,
                i.BalanceDue,
                i.Currency,
                i.Status,
                i.IsOverdue,
                i.DaysOverdue
            ))
            .ToListAsync();

        return new PagedResult<InvoiceListDto>(
            invoices,
            totalCount,
            filter.Page,
            filter.PageSize,
            (int)Math.Ceiling(totalCount / (double)filter.PageSize)
        );
    }

    public async Task<InvoiceDto?> GetInvoiceByIdAsync(Guid clubId, Guid id)
    {
        var invoice = await _context.Invoices.IgnoreQueryFilters()
            .Include(i => i.Member)
            .Include(i => i.LineItems.OrderBy(li => li.SortOrder))
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.ClubId == clubId && i.Id == id);

        return invoice == null ? null : MapToDto(invoice);
    }

    public async Task<InvoiceDto?> GetInvoiceByNumberAsync(Guid clubId, string invoiceNumber)
    {
        var invoice = await _context.Invoices.IgnoreQueryFilters()
            .Include(i => i.Member)
            .Include(i => i.LineItems.OrderBy(li => li.SortOrder))
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.ClubId == clubId && i.InvoiceNumber == invoiceNumber);

        return invoice == null ? null : MapToDto(invoice);
    }

    public async Task<InvoiceDto> CreateInvoiceAsync(Guid clubId, InvoiceCreateRequest request, string? createdBy = null)
    {
        var invoiceNumber = await GenerateInvoiceNumberAsync(clubId);

        var member = await _context.Members.IgnoreQueryFilters()
            .FirstOrDefaultAsync(m => m.ClubId == clubId && m.Id == request.MemberId);

        var invoice = new Invoice
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            MemberId = request.MemberId,
            InvoiceNumber = invoiceNumber,
            InvoiceDate = request.InvoiceDate ?? DateTime.UtcNow,
            DueDate = request.DueDate,
            PurchaseOrderNumber = request.PurchaseOrderNumber,
            Currency = request.Currency,
            BillingName = request.BillingName ?? (member != null ? $"{member.FirstName} {member.LastName}" : null),
            BillingAddress = request.BillingAddress ?? member?.Address,
            BillingCity = request.BillingCity ?? member?.City,
            BillingPostcode = request.BillingPostcode ?? member?.PostCode,
            BillingCountry = request.BillingCountry ?? member?.Country,
            BillingEmail = request.BillingEmail ?? member?.Email,
            PaymentTermsDays = request.PaymentTermsDays,
            AllowPartialPayment = request.AllowPartialPayment,
            AllowOnlinePayment = request.AllowOnlinePayment,
            InternalNotes = request.InternalNotes ?? request.Notes,
            Status = InvoiceStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };

        _context.Invoices.Add(invoice);

        // Add line items
        if (request.LineItems != null)
        {
            var sortOrder = 0;
            foreach (var item in request.LineItems)
            {
                var lineItem = CreateLineItem(invoice.Id, item, sortOrder++);
                _context.InvoiceLineItems.Add(lineItem);
            }
        }

        await _context.SaveChangesAsync();

        // Recalculate totals
        await RecalculateInvoiceTotalsAsync(clubId, invoice.Id);

        return (await GetInvoiceByIdAsync(clubId, invoice.Id))!;
    }

    public async Task<InvoiceDto?> UpdateInvoiceAsync(Guid clubId, Guid id, InvoiceUpdateRequest request, string? updatedBy = null)
    {
        var invoice = await _context.Invoices.IgnoreQueryFilters()
            .FirstOrDefaultAsync(i => i.ClubId == clubId && i.Id == id);

        if (invoice == null) return null;

        // Only allow updates on draft invoices
        if (invoice.Status != InvoiceStatus.Draft && invoice.Status != InvoiceStatus.Sent)
        {
            throw new InvalidOperationException("Cannot update invoice in current status");
        }

        if (request.DueDate.HasValue) invoice.DueDate = request.DueDate.Value;
        if (request.PurchaseOrderNumber != null) invoice.PurchaseOrderNumber = request.PurchaseOrderNumber;
        if (request.BillingName != null) invoice.BillingName = request.BillingName;
        if (request.BillingAddress != null) invoice.BillingAddress = request.BillingAddress;
        if (request.BillingCity != null) invoice.BillingCity = request.BillingCity;
        if (request.BillingPostcode != null) invoice.BillingPostcode = request.BillingPostcode;
        if (request.BillingCountry != null) invoice.BillingCountry = request.BillingCountry;
        if (request.BillingEmail != null) invoice.BillingEmail = request.BillingEmail;
        if (request.PaymentTermsDays.HasValue) invoice.PaymentTermsDays = request.PaymentTermsDays.Value;
        if (request.AllowPartialPayment.HasValue) invoice.AllowPartialPayment = request.AllowPartialPayment.Value;
        if (request.AllowOnlinePayment.HasValue) invoice.AllowOnlinePayment = request.AllowOnlinePayment.Value;
        if (request.Notes != null) invoice.Notes = request.Notes;
        if (request.InternalNotes != null) invoice.InternalNotes = request.InternalNotes;

        invoice.UpdatedAt = DateTime.UtcNow;
        invoice.UpdatedBy = updatedBy;

        await _context.SaveChangesAsync();
        return await GetInvoiceByIdAsync(clubId, id);
    }

    public async Task<bool> DeleteInvoiceAsync(Guid clubId, Guid id)
    {
        var invoice = await _context.Invoices.IgnoreQueryFilters()
            .Include(i => i.LineItems)
            .FirstOrDefaultAsync(i => i.ClubId == clubId && i.Id == id);

        if (invoice == null) return false;

        // Only allow deletion of draft invoices
        if (invoice.Status != InvoiceStatus.Draft)
        {
            throw new InvalidOperationException("Only draft invoices can be deleted");
        }

        _context.InvoiceLineItems.RemoveRange(invoice.LineItems);
        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<InvoiceDto?> AddLineItemAsync(Guid clubId, Guid invoiceId, InvoiceLineItemCreateRequest request)
    {
        var invoice = await _context.Invoices.IgnoreQueryFilters()
            .Include(i => i.LineItems)
            .FirstOrDefaultAsync(i => i.ClubId == clubId && i.Id == invoiceId);

        if (invoice == null) return null;

        var maxOrder = invoice.LineItems.Any() ? invoice.LineItems.Max(li => li.SortOrder) : -1;
        var lineItem = CreateLineItem(invoiceId, request, maxOrder + 1);
        _context.InvoiceLineItems.Add(lineItem);
        await _context.SaveChangesAsync();

        await RecalculateInvoiceTotalsAsync(clubId, invoiceId);
        return await GetInvoiceByIdAsync(clubId, invoiceId);
    }

    public async Task<InvoiceDto?> UpdateLineItemAsync(Guid clubId, Guid invoiceId, Guid lineItemId, InvoiceLineItemCreateRequest request)
    {
        var lineItem = await _context.InvoiceLineItems
            .Include(li => li.Invoice)
            .FirstOrDefaultAsync(li => li.Id == lineItemId && li.InvoiceId == invoiceId && li.Invoice.ClubId == clubId);

        if (lineItem == null) return null;

        lineItem.Description = request.Description;
        lineItem.FeeId = request.FeeId;
        lineItem.FeeType = request.FeeType;
        lineItem.Quantity = request.Quantity;
        lineItem.UnitPrice = request.UnitPrice;
        lineItem.DiscountAmount = request.DiscountAmount;
        lineItem.DiscountPercent = request.DiscountPercent;
        lineItem.TaxRate = request.TaxRate;
        lineItem.ServicePeriod = request.ServicePeriod;
        lineItem.ServiceStartDate = request.ServiceStartDate;
        lineItem.ServiceEndDate = request.ServiceEndDate;
        lineItem.GLAccountCode = request.GLAccountCode;
        lineItem.CostCenter = request.CostCenter;

        // Recalculate line item totals
        lineItem.SubTotal = lineItem.Quantity * lineItem.UnitPrice;
        if (lineItem.DiscountAmount.HasValue)
            lineItem.SubTotal -= lineItem.DiscountAmount.Value;
        else if (lineItem.DiscountPercent.HasValue)
            lineItem.SubTotal -= lineItem.SubTotal * (lineItem.DiscountPercent.Value / 100);

        if (lineItem.TaxRate.HasValue)
            lineItem.TaxAmount = lineItem.SubTotal * (lineItem.TaxRate.Value / 100);

        lineItem.Total = lineItem.SubTotal + (lineItem.TaxAmount ?? 0);

        await _context.SaveChangesAsync();
        await RecalculateInvoiceTotalsAsync(clubId, invoiceId);
        return await GetInvoiceByIdAsync(clubId, invoiceId);
    }

    public async Task<InvoiceDto?> RemoveLineItemAsync(Guid clubId, Guid invoiceId, Guid lineItemId)
    {
        var lineItem = await _context.InvoiceLineItems
            .Include(li => li.Invoice)
            .FirstOrDefaultAsync(li => li.Id == lineItemId && li.InvoiceId == invoiceId && li.Invoice.ClubId == clubId);

        if (lineItem == null) return null;

        _context.InvoiceLineItems.Remove(lineItem);
        await _context.SaveChangesAsync();

        await RecalculateInvoiceTotalsAsync(clubId, invoiceId);
        return await GetInvoiceByIdAsync(clubId, invoiceId);
    }

    public async Task<InvoiceDto?> SendInvoiceAsync(Guid clubId, Guid id, InvoiceSendRequest request, string? sentBy = null)
    {
        var invoice = await _context.Invoices.IgnoreQueryFilters()
            .FirstOrDefaultAsync(i => i.ClubId == clubId && i.Id == id);

        if (invoice == null) return null;

        invoice.Status = InvoiceStatus.Sent;
        invoice.SentDate = DateTime.UtcNow;
        invoice.UpdatedAt = DateTime.UtcNow;
        invoice.UpdatedBy = sentBy;

        await _context.SaveChangesAsync();

        // TODO: Send email notification if request.SendEmail is true

        return await GetInvoiceByIdAsync(clubId, id);
    }

    public async Task<InvoiceDto?> MarkAsPaidAsync(Guid clubId, Guid id, string? paidBy = null)
    {
        var invoice = await _context.Invoices.IgnoreQueryFilters()
            .FirstOrDefaultAsync(i => i.ClubId == clubId && i.Id == id);

        if (invoice == null) return null;

        invoice.Status = InvoiceStatus.Paid;
        invoice.PaidDate = DateTime.UtcNow;
        invoice.PaidAmount = invoice.TotalAmount;
        invoice.BalanceDue = 0;
        invoice.IsOverdue = false;
        invoice.UpdatedAt = DateTime.UtcNow;
        invoice.UpdatedBy = paidBy;

        await _context.SaveChangesAsync();
        return await GetInvoiceByIdAsync(clubId, id);
    }

    public async Task<InvoiceDto?> VoidInvoiceAsync(Guid clubId, Guid id, InvoiceVoidRequest request, string? voidedBy = null)
    {
        var invoice = await _context.Invoices.IgnoreQueryFilters()
            .FirstOrDefaultAsync(i => i.ClubId == clubId && i.Id == id);

        if (invoice == null) return null;

        invoice.Status = InvoiceStatus.Voided;
        invoice.VoidedDate = DateTime.UtcNow;
        invoice.VoidReason = request.Reason;
        invoice.VoidedBy = voidedBy;
        invoice.UpdatedAt = DateTime.UtcNow;
        invoice.UpdatedBy = voidedBy;

        await _context.SaveChangesAsync();
        return await GetInvoiceByIdAsync(clubId, id);
    }

    public async Task<InvoiceDto?> SendReminderAsync(Guid clubId, Guid id)
    {
        var invoice = await _context.Invoices.IgnoreQueryFilters()
            .FirstOrDefaultAsync(i => i.ClubId == clubId && i.Id == id);

        if (invoice == null) return null;

        invoice.RemindersSent++;
        invoice.LastReminderDate = DateTime.UtcNow;
        invoice.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // TODO: Send reminder email

        return await GetInvoiceByIdAsync(clubId, id);
    }

    public async Task<InvoiceDto?> RecordPaymentAsync(Guid clubId, Guid id, RecordPaymentRequest request, string? recordedBy = null)
    {
        var invoice = await _context.Invoices.IgnoreQueryFilters()
            .FirstOrDefaultAsync(i => i.ClubId == clubId && i.Id == id);

        if (invoice == null) return null;

        // Create payment record
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            MemberId = invoice.MemberId,
            InvoiceId = invoice.Id,
            Amount = request.Amount,
            Currency = invoice.Currency,
            Status = PaymentStatus.Completed,
            Method = request.Method,
            Type = PaymentType.Other,
            PaymentDate = request.PaymentDate ?? DateTime.UtcNow,
            ProcessedDate = DateTime.UtcNow,
            Description = $"Payment for Invoice {invoice.InvoiceNumber}",
            ManualPaymentReference = request.Reference,
            RecordedBy = recordedBy,
            InternalNotes = request.Notes
        };

        _context.Payments.Add(payment);

        // Update invoice
        invoice.PaidAmount += request.Amount;
        invoice.BalanceDue = invoice.TotalAmount - invoice.PaidAmount;

        if (invoice.BalanceDue <= 0)
        {
            invoice.Status = InvoiceStatus.Paid;
            invoice.PaidDate = DateTime.UtcNow;
            invoice.IsOverdue = false;
        }
        else if (invoice.Status == InvoiceStatus.Draft)
        {
            invoice.Status = InvoiceStatus.PartiallyPaid;
        }

        invoice.UpdatedAt = DateTime.UtcNow;
        invoice.UpdatedBy = recordedBy;

        await _context.SaveChangesAsync();
        return await GetInvoiceByIdAsync(clubId, id);
    }

    public async Task<IEnumerable<InvoiceListDto>> GetMemberInvoicesAsync(Guid clubId, Guid memberId)
    {
        return await _context.Invoices.IgnoreQueryFilters()
            .Where(i => i.ClubId == clubId && i.MemberId == memberId)
            .Include(i => i.Member)
            .OrderByDescending(i => i.InvoiceDate)
            .Select(i => new InvoiceListDto(
                i.Id,
                i.InvoiceNumber,
                i.MemberId,
                $"{i.Member.FirstName} {i.Member.LastName}",
                i.InvoiceDate,
                i.DueDate,
                i.TotalAmount,
                i.PaidAmount,
                i.BalanceDue,
                i.Currency,
                i.Status,
                i.IsOverdue,
                i.DaysOverdue
            ))
            .ToListAsync();
    }

    public async Task<IEnumerable<InvoiceListDto>> GetOverdueInvoicesAsync(Guid clubId)
    {
        return await _context.Invoices.IgnoreQueryFilters()
            .Where(i => i.ClubId == clubId && i.IsOverdue && i.Status != InvoiceStatus.Paid && i.Status != InvoiceStatus.Voided)
            .Include(i => i.Member)
            .OrderByDescending(i => i.DaysOverdue)
            .Select(i => new InvoiceListDto(
                i.Id,
                i.InvoiceNumber,
                i.MemberId,
                $"{i.Member.FirstName} {i.Member.LastName}",
                i.InvoiceDate,
                i.DueDate,
                i.TotalAmount,
                i.PaidAmount,
                i.BalanceDue,
                i.Currency,
                i.Status,
                i.IsOverdue,
                i.DaysOverdue
            ))
            .ToListAsync();
    }

    public async Task<InvoiceSummaryDto> GetInvoiceSummaryAsync(Guid clubId)
    {
        var invoices = await _context.Invoices.IgnoreQueryFilters()
            .Where(i => i.ClubId == clubId)
            .ToListAsync();

        var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

        return new InvoiceSummaryDto(
            TotalInvoices: invoices.Count,
            DraftCount: invoices.Count(i => i.Status == InvoiceStatus.Draft),
            SentCount: invoices.Count(i => i.Status == InvoiceStatus.Sent),
            PaidCount: invoices.Count(i => i.Status == InvoiceStatus.Paid),
            OverdueCount: invoices.Count(i => i.IsOverdue && i.Status != InvoiceStatus.Paid && i.Status != InvoiceStatus.Voided),
            TotalOutstanding: invoices.Where(i => i.Status != InvoiceStatus.Paid && i.Status != InvoiceStatus.Voided).Sum(i => i.BalanceDue),
            TotalOverdue: invoices.Where(i => i.IsOverdue && i.Status != InvoiceStatus.Paid && i.Status != InvoiceStatus.Voided).Sum(i => i.BalanceDue),
            TotalPaidThisMonth: invoices.Where(i => i.PaidDate >= startOfMonth).Sum(i => i.PaidAmount)
        );
    }

    public async Task<string> GenerateInvoiceNumberAsync(Guid clubId)
    {
        var year = DateTime.UtcNow.Year;
        var lastInvoice = await _context.Invoices.IgnoreQueryFilters()
            .Where(i => i.ClubId == clubId && i.InvoiceNumber.StartsWith($"INV-{year}"))
            .OrderByDescending(i => i.InvoiceNumber)
            .FirstOrDefaultAsync();

        int nextNumber = 1;
        if (lastInvoice != null)
        {
            var lastNumberStr = lastInvoice.InvoiceNumber.Split('-').Last();
            if (int.TryParse(lastNumberStr, out int lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }

        return $"INV-{year}-{nextNumber:D5}";
    }

    public async Task RecalculateInvoiceTotalsAsync(Guid clubId, Guid invoiceId)
    {
        var invoice = await _context.Invoices.IgnoreQueryFilters()
            .Include(i => i.LineItems)
            .FirstOrDefaultAsync(i => i.ClubId == clubId && i.Id == invoiceId);

        if (invoice == null) return;

        invoice.SubTotal = invoice.LineItems.Sum(li => li.SubTotal);
        invoice.TaxAmount = invoice.LineItems.Sum(li => li.TaxAmount ?? 0);
        invoice.TotalAmount = invoice.SubTotal + (invoice.TaxAmount ?? 0) - (invoice.DiscountAmount ?? 0);
        invoice.BalanceDue = invoice.TotalAmount - invoice.PaidAmount;
        invoice.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task UpdateOverdueStatusAsync(Guid clubId)
    {
        var today = DateTime.UtcNow.Date;
        var invoices = await _context.Invoices.IgnoreQueryFilters()
            .Where(i => i.ClubId == clubId && i.Status != InvoiceStatus.Paid && i.Status != InvoiceStatus.Voided && i.Status != InvoiceStatus.Draft)
            .ToListAsync();

        foreach (var invoice in invoices)
        {
            var isOverdue = invoice.DueDate.Date < today && invoice.BalanceDue > 0;
            var daysOverdue = isOverdue ? (today - invoice.DueDate.Date).Days : 0;

            invoice.IsOverdue = isOverdue;
            invoice.DaysOverdue = daysOverdue;

            // Update collection status
            if (daysOverdue >= 120)
                invoice.CollectionStatus = CollectionStatus.Overdue120Plus;
            else if (daysOverdue >= 90)
                invoice.CollectionStatus = CollectionStatus.Overdue90;
            else if (daysOverdue >= 60)
                invoice.CollectionStatus = CollectionStatus.Overdue60;
            else if (daysOverdue >= 30)
                invoice.CollectionStatus = CollectionStatus.Overdue30;
            else
                invoice.CollectionStatus = CollectionStatus.Current;
        }

        await _context.SaveChangesAsync();
    }

    private static InvoiceLineItem CreateLineItem(Guid invoiceId, InvoiceLineItemCreateRequest request, int sortOrder)
    {
        var subTotal = request.Quantity * request.UnitPrice;
        if (request.DiscountAmount.HasValue)
            subTotal -= request.DiscountAmount.Value;
        else if (request.DiscountPercent.HasValue)
            subTotal -= subTotal * (request.DiscountPercent.Value / 100);

        decimal? taxAmount = null;
        if (request.TaxRate.HasValue)
            taxAmount = subTotal * (request.TaxRate.Value / 100);

        var total = subTotal + (taxAmount ?? 0);

        return new InvoiceLineItem
        {
            Id = Guid.NewGuid(),
            InvoiceId = invoiceId,
            FeeId = request.FeeId,
            Description = request.Description,
            FeeType = request.FeeType,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            DiscountAmount = request.DiscountAmount,
            DiscountPercent = request.DiscountPercent,
            SubTotal = subTotal,
            TaxRate = request.TaxRate,
            TaxAmount = taxAmount,
            Total = total,
            ServicePeriod = request.ServicePeriod,
            ServiceStartDate = request.ServiceStartDate,
            ServiceEndDate = request.ServiceEndDate,
            GLAccountCode = request.GLAccountCode,
            CostCenter = request.CostCenter,
            SortOrder = sortOrder
        };
    }

    private static InvoiceDto MapToDto(Invoice i) => new(
        i.Id,
        i.InvoiceNumber,
        i.MemberId,
        $"{i.Member.FirstName} {i.Member.LastName}",
        i.Member.Email,
        i.InvoiceDate,
        i.DueDate,
        i.PaidDate,
        i.SentDate,
        i.SubTotal,
        i.DiscountAmount,
        i.TaxAmount,
        i.TotalAmount,
        i.PaidAmount,
        i.BalanceDue,
        i.Currency,
        i.Status,
        i.CollectionStatus,
        i.BillingName,
        i.BillingAddress,
        i.BillingEmail,
        i.PaymentTermsDays,
        i.IsOverdue,
        i.DaysOverdue,
        i.RemindersSent,
        i.Notes,
        i.PdfUrl,
        i.CreatedAt,
        i.LineItems.Select(li => new InvoiceLineItemDto(
            li.Id,
            li.Description,
            li.FeeType,
            li.Quantity,
            li.UnitPrice,
            li.DiscountAmount,
            li.SubTotal,
            li.TaxAmount,
            li.Total,
            li.ServicePeriod,
            li.SortOrder
        )),
        i.Payments.Select(p => new InvoicePaymentDto(
            p.Id,
            p.Amount,
            p.PaymentDate,
            p.Method,
            p.Status,
            p.ReceiptNumber
        ))
    );
}
