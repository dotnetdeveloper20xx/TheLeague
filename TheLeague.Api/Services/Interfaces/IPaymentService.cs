using TheLeague.Api.DTOs;

namespace TheLeague.Api.Services.Interfaces;

public interface IPaymentService
{
    Task<PagedResult<PaymentListDto>> GetPaymentsAsync(Guid clubId, PaymentFilterRequest filter);
    Task<PaymentDto?> GetPaymentByIdAsync(Guid clubId, Guid id);
    Task<IEnumerable<PaymentDto>> GetMemberPaymentsAsync(Guid clubId, Guid memberId);
    Task<PaymentDto> RecordManualPaymentAsync(Guid clubId, ManualPaymentRequest request, string recordedBy);
    Task<PaymentDto?> RefundPaymentAsync(Guid clubId, Guid id, RefundRequest request);
    Task<PaymentSummaryDto> GetPaymentSummaryAsync(Guid clubId);

    // Mock Stripe
    Task<CreatePaymentIntentResponse> CreateStripePaymentIntentAsync(Guid clubId, CreatePaymentIntentRequest request);
    Task<PaymentDto> ProcessStripePaymentAsync(Guid clubId, string paymentIntentId);

    // Mock PayPal
    Task<PayPalOrderResponse> CreatePayPalOrderAsync(Guid clubId, PayPalOrderRequest request);
    Task<PaymentDto> CapturePayPalOrderAsync(Guid clubId, string orderId);
}
