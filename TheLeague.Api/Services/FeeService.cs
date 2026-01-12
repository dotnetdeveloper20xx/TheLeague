using Microsoft.EntityFrameworkCore;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Entities;
using TheLeague.Core.Enums;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Services;

public class FeeService : IFeeService
{
    private readonly ApplicationDbContext _context;

    public FeeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<FeeListDto>> GetFeesAsync(Guid clubId, FeeFilterRequest filter)
    {
        var query = _context.Fees.IgnoreQueryFilters()
            .Where(f => f.ClubId == clubId);

        // Apply filters
        if (filter.Type.HasValue)
            query = query.Where(f => f.Type == filter.Type.Value);

        if (filter.Frequency.HasValue)
            query = query.Where(f => f.Frequency == filter.Frequency.Value);

        if (filter.IsActive.HasValue)
            query = query.Where(f => f.IsActive == filter.IsActive.Value);

        if (filter.IsRequired.HasValue)
            query = query.Where(f => f.IsRequired == filter.IsRequired.Value);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.ToLower();
            query = query.Where(f =>
                f.Name.ToLower().Contains(search) ||
                (f.Code != null && f.Code.ToLower().Contains(search)) ||
                (f.Description != null && f.Description.ToLower().Contains(search)));
        }

        var totalCount = await query.CountAsync();

        var fees = await query
            .Include(f => f.Payments)
            .OrderBy(f => f.SortOrder)
            .ThenBy(f => f.Name)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(f => new FeeListDto(
                f.Id,
                f.Name,
                f.Code,
                f.Type,
                f.Amount,
                f.Currency,
                f.Frequency,
                f.IsActive,
                f.IsRequired,
                f.SortOrder,
                f.Payments.Count
            ))
            .ToListAsync();

        return new PagedResult<FeeListDto>(
            fees,
            totalCount,
            filter.Page,
            filter.PageSize,
            (int)Math.Ceiling(totalCount / (double)filter.PageSize)
        );
    }

    public async Task<IEnumerable<FeeListDto>> GetAllFeesAsync(Guid clubId, bool includeInactive = false)
    {
        var query = _context.Fees.IgnoreQueryFilters()
            .Where(f => f.ClubId == clubId);

        if (!includeInactive)
            query = query.Where(f => f.IsActive);

        return await query
            .Include(f => f.Payments)
            .OrderBy(f => f.SortOrder)
            .ThenBy(f => f.Name)
            .Select(f => new FeeListDto(
                f.Id,
                f.Name,
                f.Code,
                f.Type,
                f.Amount,
                f.Currency,
                f.Frequency,
                f.IsActive,
                f.IsRequired,
                f.SortOrder,
                f.Payments.Count
            ))
            .ToListAsync();
    }

    public async Task<FeeDto?> GetFeeByIdAsync(Guid clubId, Guid id)
    {
        var fee = await _context.Fees.IgnoreQueryFilters()
            .Include(f => f.Payments)
            .FirstOrDefaultAsync(f => f.ClubId == clubId && f.Id == id);

        return fee == null ? null : MapToDto(fee);
    }

    public async Task<FeeDto> CreateFeeAsync(Guid clubId, FeeCreateRequest request, string? createdBy = null)
    {
        var fee = new Fee
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Name = request.Name,
            Description = request.Description,
            Code = request.Code,
            Type = request.Type,
            Amount = request.Amount,
            MinAmount = request.MinAmount,
            MaxAmount = request.MaxAmount,
            Currency = request.Currency,
            IsTaxable = request.IsTaxable,
            TaxRate = request.TaxRate,
            Frequency = request.Frequency,
            FrequencyInterval = request.FrequencyInterval,
            EffectiveFrom = request.EffectiveFrom,
            EffectiveUntil = request.EffectiveUntil,
            DueDayOfMonth = request.DueDayOfMonth,
            HasLateFee = request.HasLateFee,
            LateFeeAmount = request.LateFeeAmount,
            LateFeePercentage = request.LateFeePercentage,
            GracePeriodDays = request.GracePeriodDays,
            LateFeeStartDay = request.LateFeeStartDay,
            AppliesToAll = request.AppliesToAll,
            ApplicableMembershipTypes = request.ApplicableMembershipTypes,
            ApplicableMemberCategories = request.ApplicableMemberCategories,
            MinAge = request.MinAge,
            MaxAge = request.MaxAge,
            AllowEarlyPaymentDiscount = request.AllowEarlyPaymentDiscount,
            EarlyPaymentDiscountPercent = request.EarlyPaymentDiscountPercent,
            EarlyPaymentDays = request.EarlyPaymentDays,
            AllowPartialPayment = request.AllowPartialPayment,
            AllowPaymentPlan = request.AllowPaymentPlan,
            MaxInstallments = request.MaxInstallments,
            IsRefundable = request.IsRefundable,
            RefundableDays = request.RefundableDays,
            AutoGenerate = request.AutoGenerate,
            AutoGenerateDaysBefore = request.AutoGenerateDaysBefore,
            AutoRemind = request.AutoRemind,
            ReminderSchedule = request.ReminderSchedule,
            IsActive = request.IsActive,
            IsRequired = request.IsRequired,
            IsHidden = request.IsHidden,
            SortOrder = request.SortOrder,
            GLAccountCode = request.GLAccountCode,
            CostCenter = request.CostCenter,
            TaxCode = request.TaxCode,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };

        _context.Fees.Add(fee);
        await _context.SaveChangesAsync();

        return MapToDto(fee);
    }

    public async Task<FeeDto?> UpdateFeeAsync(Guid clubId, Guid id, FeeUpdateRequest request, string? updatedBy = null)
    {
        var fee = await _context.Fees.IgnoreQueryFilters()
            .Include(f => f.Payments)
            .FirstOrDefaultAsync(f => f.ClubId == clubId && f.Id == id);

        if (fee == null) return null;

        if (request.Name != null) fee.Name = request.Name;
        if (request.Description != null) fee.Description = request.Description;
        if (request.Code != null) fee.Code = request.Code;
        if (request.Type.HasValue) fee.Type = request.Type.Value;
        if (request.Amount.HasValue) fee.Amount = request.Amount.Value;
        if (request.MinAmount.HasValue) fee.MinAmount = request.MinAmount;
        if (request.MaxAmount.HasValue) fee.MaxAmount = request.MaxAmount;
        if (request.Currency != null) fee.Currency = request.Currency;
        if (request.IsTaxable.HasValue) fee.IsTaxable = request.IsTaxable.Value;
        if (request.TaxRate.HasValue) fee.TaxRate = request.TaxRate;
        if (request.Frequency.HasValue) fee.Frequency = request.Frequency.Value;
        if (request.FrequencyInterval.HasValue) fee.FrequencyInterval = request.FrequencyInterval;
        if (request.EffectiveFrom.HasValue) fee.EffectiveFrom = request.EffectiveFrom;
        if (request.EffectiveUntil.HasValue) fee.EffectiveUntil = request.EffectiveUntil;
        if (request.DueDayOfMonth.HasValue) fee.DueDayOfMonth = request.DueDayOfMonth;
        if (request.HasLateFee.HasValue) fee.HasLateFee = request.HasLateFee.Value;
        if (request.LateFeeAmount.HasValue) fee.LateFeeAmount = request.LateFeeAmount;
        if (request.LateFeePercentage.HasValue) fee.LateFeePercentage = request.LateFeePercentage;
        if (request.GracePeriodDays.HasValue) fee.GracePeriodDays = request.GracePeriodDays;
        if (request.LateFeeStartDay.HasValue) fee.LateFeeStartDay = request.LateFeeStartDay;
        if (request.AppliesToAll.HasValue) fee.AppliesToAll = request.AppliesToAll.Value;
        if (request.ApplicableMembershipTypes != null) fee.ApplicableMembershipTypes = request.ApplicableMembershipTypes;
        if (request.ApplicableMemberCategories != null) fee.ApplicableMemberCategories = request.ApplicableMemberCategories;
        if (request.MinAge.HasValue) fee.MinAge = request.MinAge;
        if (request.MaxAge.HasValue) fee.MaxAge = request.MaxAge;
        if (request.AllowEarlyPaymentDiscount.HasValue) fee.AllowEarlyPaymentDiscount = request.AllowEarlyPaymentDiscount.Value;
        if (request.EarlyPaymentDiscountPercent.HasValue) fee.EarlyPaymentDiscountPercent = request.EarlyPaymentDiscountPercent;
        if (request.EarlyPaymentDays.HasValue) fee.EarlyPaymentDays = request.EarlyPaymentDays;
        if (request.AllowPartialPayment.HasValue) fee.AllowPartialPayment = request.AllowPartialPayment.Value;
        if (request.AllowPaymentPlan.HasValue) fee.AllowPaymentPlan = request.AllowPaymentPlan.Value;
        if (request.MaxInstallments.HasValue) fee.MaxInstallments = request.MaxInstallments;
        if (request.IsRefundable.HasValue) fee.IsRefundable = request.IsRefundable.Value;
        if (request.RefundableDays.HasValue) fee.RefundableDays = request.RefundableDays;
        if (request.AutoGenerate.HasValue) fee.AutoGenerate = request.AutoGenerate.Value;
        if (request.AutoGenerateDaysBefore.HasValue) fee.AutoGenerateDaysBefore = request.AutoGenerateDaysBefore;
        if (request.AutoRemind.HasValue) fee.AutoRemind = request.AutoRemind.Value;
        if (request.ReminderSchedule != null) fee.ReminderSchedule = request.ReminderSchedule;
        if (request.IsActive.HasValue) fee.IsActive = request.IsActive.Value;
        if (request.IsRequired.HasValue) fee.IsRequired = request.IsRequired.Value;
        if (request.IsHidden.HasValue) fee.IsHidden = request.IsHidden.Value;
        if (request.SortOrder.HasValue) fee.SortOrder = request.SortOrder.Value;
        if (request.GLAccountCode != null) fee.GLAccountCode = request.GLAccountCode;
        if (request.CostCenter != null) fee.CostCenter = request.CostCenter;
        if (request.TaxCode != null) fee.TaxCode = request.TaxCode;

        fee.UpdatedAt = DateTime.UtcNow;
        fee.UpdatedBy = updatedBy;

        await _context.SaveChangesAsync();
        return MapToDto(fee);
    }

    public async Task<bool> DeleteFeeAsync(Guid clubId, Guid id)
    {
        var fee = await _context.Fees.IgnoreQueryFilters()
            .FirstOrDefaultAsync(f => f.ClubId == clubId && f.Id == id);

        if (fee == null) return false;

        // Soft delete by setting inactive
        fee.IsActive = false;
        fee.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleActiveAsync(Guid clubId, Guid id)
    {
        var fee = await _context.Fees.IgnoreQueryFilters()
            .FirstOrDefaultAsync(f => f.ClubId == clubId && f.Id == id);

        if (fee == null) return false;

        fee.IsActive = !fee.IsActive;
        fee.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<FeeListDto>> GetFeesByTypeAsync(Guid clubId, FeeType type)
    {
        return await _context.Fees.IgnoreQueryFilters()
            .Where(f => f.ClubId == clubId && f.Type == type && f.IsActive)
            .Include(f => f.Payments)
            .OrderBy(f => f.SortOrder)
            .ThenBy(f => f.Name)
            .Select(f => new FeeListDto(
                f.Id,
                f.Name,
                f.Code,
                f.Type,
                f.Amount,
                f.Currency,
                f.Frequency,
                f.IsActive,
                f.IsRequired,
                f.SortOrder,
                f.Payments.Count
            ))
            .ToListAsync();
    }

    private static FeeDto MapToDto(Fee f) => new(
        f.Id,
        f.Name,
        f.Description,
        f.Code,
        f.Type,
        f.Amount,
        f.MinAmount,
        f.MaxAmount,
        f.Currency,
        f.IsTaxable,
        f.TaxRate,
        f.Frequency,
        f.FrequencyInterval,
        f.EffectiveFrom,
        f.EffectiveUntil,
        f.DueDayOfMonth,
        f.HasLateFee,
        f.LateFeeAmount,
        f.LateFeePercentage,
        f.GracePeriodDays,
        f.AppliesToAll,
        f.ApplicableMembershipTypes,
        f.MinAge,
        f.MaxAge,
        f.AllowEarlyPaymentDiscount,
        f.EarlyPaymentDiscountPercent,
        f.EarlyPaymentDays,
        f.AllowPartialPayment,
        f.AllowPaymentPlan,
        f.MaxInstallments,
        f.IsRefundable,
        f.RefundableDays,
        f.AutoGenerate,
        f.AutoRemind,
        f.IsActive,
        f.IsRequired,
        f.IsHidden,
        f.SortOrder,
        f.GLAccountCode,
        f.CostCenter,
        f.CreatedAt,
        f.Payments?.Count ?? 0
    );
}
