using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Controllers;

[Authorize]
public class InvoicesController : BaseApiController
{
    private readonly IInvoiceService _invoiceService;

    public InvoicesController(IInvoiceService invoiceService, ITenantService tenantService)
        : base(tenantService)
    {
        _invoiceService = invoiceService;
    }

    private new string? GetUserId() => User.FindFirst("sub")?.Value ?? User.FindFirst("userId")?.Value;

    /// <summary>
    /// Get all invoices with filtering and pagination
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<InvoiceListDto>>> GetAll([FromQuery] InvoiceFilterRequest filter)
    {
        var clubId = GetClubId();
        var invoices = await _invoiceService.GetInvoicesAsync(clubId, filter);
        return Ok(invoices);
    }

    /// <summary>
    /// Get invoice summary statistics
    /// </summary>
    [HttpGet("summary")]
    public async Task<ActionResult<InvoiceSummaryDto>> GetSummary()
    {
        var clubId = GetClubId();
        var summary = await _invoiceService.GetInvoiceSummaryAsync(clubId);
        return Ok(summary);
    }

    /// <summary>
    /// Get overdue invoices
    /// </summary>
    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<InvoiceListDto>>> GetOverdue()
    {
        var clubId = GetClubId();
        var invoices = await _invoiceService.GetOverdueInvoicesAsync(clubId);
        return Ok(invoices);
    }

    /// <summary>
    /// Get a specific invoice by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceDto>> GetById(Guid id)
    {
        var clubId = GetClubId();
        var invoice = await _invoiceService.GetInvoiceByIdAsync(clubId, id);
        if (invoice == null)
            return NotFound();
        return Ok(invoice);
    }

    /// <summary>
    /// Get invoice by invoice number
    /// </summary>
    [HttpGet("by-number/{invoiceNumber}")]
    public async Task<ActionResult<InvoiceDto>> GetByNumber(string invoiceNumber)
    {
        var clubId = GetClubId();
        var invoice = await _invoiceService.GetInvoiceByNumberAsync(clubId, invoiceNumber);
        if (invoice == null)
            return NotFound();
        return Ok(invoice);
    }

    /// <summary>
    /// Get invoices for a specific member
    /// </summary>
    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<InvoiceListDto>>> GetMemberInvoices(Guid memberId)
    {
        var clubId = GetClubId();
        var invoices = await _invoiceService.GetMemberInvoicesAsync(clubId, memberId);
        return Ok(invoices);
    }

    /// <summary>
    /// Create a new invoice
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<InvoiceDto>> Create([FromBody] InvoiceCreateRequest request)
    {
        var clubId = GetClubId();
        var userId = GetUserId();
        var invoice = await _invoiceService.CreateInvoiceAsync(clubId, request, userId);
        return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoice);
    }

    /// <summary>
    /// Update an existing invoice
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<InvoiceDto>> Update(Guid id, [FromBody] InvoiceUpdateRequest request)
    {
        try
        {
            var clubId = GetClubId();
            var userId = GetUserId();
            var invoice = await _invoiceService.UpdateInvoiceAsync(clubId, id, request, userId);
            if (invoice == null)
                return NotFound();
            return Ok(invoice);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Delete a draft invoice
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            var clubId = GetClubId();
            var result = await _invoiceService.DeleteInvoiceAsync(clubId, id);
            if (!result)
                return NotFound();
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Add a line item to an invoice
    /// </summary>
    [HttpPost("{id}/line-items")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<InvoiceDto>> AddLineItem(Guid id, [FromBody] InvoiceLineItemCreateRequest request)
    {
        var clubId = GetClubId();
        var invoice = await _invoiceService.AddLineItemAsync(clubId, id, request);
        if (invoice == null)
            return NotFound();
        return Ok(invoice);
    }

    /// <summary>
    /// Update a line item on an invoice
    /// </summary>
    [HttpPut("{id}/line-items/{lineItemId}")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<InvoiceDto>> UpdateLineItem(Guid id, Guid lineItemId, [FromBody] InvoiceLineItemCreateRequest request)
    {
        var clubId = GetClubId();
        var invoice = await _invoiceService.UpdateLineItemAsync(clubId, id, lineItemId, request);
        if (invoice == null)
            return NotFound();
        return Ok(invoice);
    }

    /// <summary>
    /// Remove a line item from an invoice
    /// </summary>
    [HttpDelete("{id}/line-items/{lineItemId}")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<InvoiceDto>> RemoveLineItem(Guid id, Guid lineItemId)
    {
        var clubId = GetClubId();
        var invoice = await _invoiceService.RemoveLineItemAsync(clubId, id, lineItemId);
        if (invoice == null)
            return NotFound();
        return Ok(invoice);
    }

    /// <summary>
    /// Send an invoice to the member
    /// </summary>
    [HttpPost("{id}/send")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<InvoiceDto>> Send(Guid id, [FromBody] InvoiceSendRequest request)
    {
        var clubId = GetClubId();
        var userId = GetUserId();
        var invoice = await _invoiceService.SendInvoiceAsync(clubId, id, request, userId);
        if (invoice == null)
            return NotFound();
        return Ok(invoice);
    }

    /// <summary>
    /// Mark an invoice as paid
    /// </summary>
    [HttpPost("{id}/mark-paid")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<InvoiceDto>> MarkAsPaid(Guid id)
    {
        var clubId = GetClubId();
        var userId = GetUserId();
        var invoice = await _invoiceService.MarkAsPaidAsync(clubId, id, userId);
        if (invoice == null)
            return NotFound();
        return Ok(invoice);
    }

    /// <summary>
    /// Void an invoice
    /// </summary>
    [HttpPost("{id}/void")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<InvoiceDto>> Void(Guid id, [FromBody] InvoiceVoidRequest request)
    {
        var clubId = GetClubId();
        var userId = GetUserId();
        var invoice = await _invoiceService.VoidInvoiceAsync(clubId, id, request, userId);
        if (invoice == null)
            return NotFound();
        return Ok(invoice);
    }

    /// <summary>
    /// Send a payment reminder
    /// </summary>
    [HttpPost("{id}/send-reminder")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<InvoiceDto>> SendReminder(Guid id)
    {
        var clubId = GetClubId();
        var invoice = await _invoiceService.SendReminderAsync(clubId, id);
        if (invoice == null)
            return NotFound();
        return Ok(invoice);
    }

    /// <summary>
    /// Record a payment against an invoice
    /// </summary>
    [HttpPost("{id}/payments")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<InvoiceDto>> RecordPayment(Guid id, [FromBody] RecordPaymentRequest request)
    {
        var clubId = GetClubId();
        var userId = GetUserId();
        var invoice = await _invoiceService.RecordPaymentAsync(clubId, id, request, userId);
        if (invoice == null)
            return NotFound();
        return Ok(invoice);
    }

    /// <summary>
    /// Update overdue status for all invoices (admin utility)
    /// </summary>
    [HttpPost("update-overdue-status")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult> UpdateOverdueStatus()
    {
        var clubId = GetClubId();
        await _invoiceService.UpdateOverdueStatusAsync(clubId);
        return Ok();
    }
}
