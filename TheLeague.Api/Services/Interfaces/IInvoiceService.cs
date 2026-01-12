using TheLeague.Api.DTOs;

namespace TheLeague.Api.Services.Interfaces;

public interface IInvoiceService
{
    // Invoice CRUD
    Task<PagedResult<InvoiceListDto>> GetInvoicesAsync(Guid clubId, InvoiceFilterRequest filter);
    Task<InvoiceDto?> GetInvoiceByIdAsync(Guid clubId, Guid id);
    Task<InvoiceDto?> GetInvoiceByNumberAsync(Guid clubId, string invoiceNumber);
    Task<InvoiceDto> CreateInvoiceAsync(Guid clubId, InvoiceCreateRequest request, string? createdBy = null);
    Task<InvoiceDto?> UpdateInvoiceAsync(Guid clubId, Guid id, InvoiceUpdateRequest request, string? updatedBy = null);
    Task<bool> DeleteInvoiceAsync(Guid clubId, Guid id);

    // Line Items
    Task<InvoiceDto?> AddLineItemAsync(Guid clubId, Guid invoiceId, InvoiceLineItemCreateRequest request);
    Task<InvoiceDto?> UpdateLineItemAsync(Guid clubId, Guid invoiceId, Guid lineItemId, InvoiceLineItemCreateRequest request);
    Task<InvoiceDto?> RemoveLineItemAsync(Guid clubId, Guid invoiceId, Guid lineItemId);

    // Status Operations
    Task<InvoiceDto?> SendInvoiceAsync(Guid clubId, Guid id, InvoiceSendRequest request, string? sentBy = null);
    Task<InvoiceDto?> MarkAsPaidAsync(Guid clubId, Guid id, string? paidBy = null);
    Task<InvoiceDto?> VoidInvoiceAsync(Guid clubId, Guid id, InvoiceVoidRequest request, string? voidedBy = null);
    Task<InvoiceDto?> SendReminderAsync(Guid clubId, Guid id);

    // Payment Recording
    Task<InvoiceDto?> RecordPaymentAsync(Guid clubId, Guid id, RecordPaymentRequest request, string? recordedBy = null);

    // Member Invoices
    Task<IEnumerable<InvoiceListDto>> GetMemberInvoicesAsync(Guid clubId, Guid memberId);
    Task<IEnumerable<InvoiceListDto>> GetOverdueInvoicesAsync(Guid clubId);

    // Summary & Reports
    Task<InvoiceSummaryDto> GetInvoiceSummaryAsync(Guid clubId);

    // Utilities
    Task<string> GenerateInvoiceNumberAsync(Guid clubId);
    Task RecalculateInvoiceTotalsAsync(Guid clubId, Guid invoiceId);
    Task UpdateOverdueStatusAsync(Guid clubId);
}
