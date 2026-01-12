using TheLeague.Api.DTOs;

namespace TheLeague.Api.Services.Interfaces;

public interface IFeeService
{
    Task<PagedResult<FeeListDto>> GetFeesAsync(Guid clubId, FeeFilterRequest filter);
    Task<IEnumerable<FeeListDto>> GetAllFeesAsync(Guid clubId, bool includeInactive = false);
    Task<FeeDto?> GetFeeByIdAsync(Guid clubId, Guid id);
    Task<FeeDto> CreateFeeAsync(Guid clubId, FeeCreateRequest request, string? createdBy = null);
    Task<FeeDto?> UpdateFeeAsync(Guid clubId, Guid id, FeeUpdateRequest request, string? updatedBy = null);
    Task<bool> DeleteFeeAsync(Guid clubId, Guid id);
    Task<bool> ToggleActiveAsync(Guid clubId, Guid id);
    Task<IEnumerable<FeeListDto>> GetFeesByTypeAsync(Guid clubId, Core.Enums.FeeType type);
}
