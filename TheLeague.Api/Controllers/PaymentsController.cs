using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Controllers;

[Authorize]
public class PaymentsController : BaseApiController
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService, ITenantService tenantService)
        : base(tenantService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<PagedResult<PaymentListDto>>> GetAll([FromQuery] PaymentFilterRequest filter)
    {
        var clubId = GetClubId();
        var result = await _paymentService.GetPaymentsAsync(clubId, filter);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentDto>> GetById(Guid id)
    {
        var clubId = GetClubId();
        var payment = await _paymentService.GetPaymentByIdAsync(clubId, id);
        if (payment == null)
            return NotFound();
        return Ok(payment);
    }

    [HttpGet("summary")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<PaymentSummaryDto>> GetSummary()
    {
        var clubId = GetClubId();
        var summary = await _paymentService.GetPaymentSummaryAsync(clubId);
        return Ok(summary);
    }

    [HttpPost("manual")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<PaymentDto>> RecordManual([FromBody] ManualPaymentRequest request)
    {
        var clubId = GetClubId();
        var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
        var payment = await _paymentService.RecordManualPaymentAsync(clubId, request, userName);
        return Ok(payment);
    }

    [HttpPost("{id}/refund")]
    [Authorize(Roles = "ClubManager,SuperAdmin")]
    public async Task<ActionResult<PaymentDto>> Refund(Guid id, [FromBody] RefundRequest request)
    {
        var clubId = GetClubId();
        var payment = await _paymentService.RefundPaymentAsync(clubId, id, request);
        if (payment == null)
            return NotFound();
        return Ok(payment);
    }

    // Stripe
    [HttpPost("stripe/create-intent")]
    public async Task<ActionResult<CreatePaymentIntentResponse>> CreateStripeIntent([FromBody] CreatePaymentIntentRequest request)
    {
        var clubId = GetClubId();
        var result = await _paymentService.CreateStripePaymentIntentAsync(clubId, request);
        return Ok(result);
    }

    [HttpPost("stripe/confirm/{paymentIntentId}")]
    public async Task<ActionResult<PaymentDto>> ConfirmStripe(string paymentIntentId)
    {
        var clubId = GetClubId();
        var payment = await _paymentService.ProcessStripePaymentAsync(clubId, paymentIntentId);
        return Ok(payment);
    }

    // PayPal
    [HttpPost("paypal/create-order")]
    public async Task<ActionResult<PayPalOrderResponse>> CreatePayPalOrder([FromBody] PayPalOrderRequest request)
    {
        var clubId = GetClubId();
        var result = await _paymentService.CreatePayPalOrderAsync(clubId, request);
        return Ok(result);
    }

    [HttpPost("paypal/capture/{orderId}")]
    public async Task<ActionResult<PaymentDto>> CapturePayPal(string orderId)
    {
        var clubId = GetClubId();
        var payment = await _paymentService.CapturePayPalOrderAsync(clubId, orderId);
        return Ok(payment);
    }
}
