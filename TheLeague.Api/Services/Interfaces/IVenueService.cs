using TheLeague.Api.DTOs;

namespace TheLeague.Api.Services.Interfaces;

public interface IVenueService
{
    Task<IEnumerable<VenueDto>> GetVenuesAsync(Guid clubId);
    Task<VenueDto?> GetVenueByIdAsync(Guid clubId, Guid id);
    Task<VenueDto> CreateVenueAsync(Guid clubId, VenueCreateRequest request);
    Task<VenueDto?> UpdateVenueAsync(Guid clubId, Guid id, VenueUpdateRequest request);
    Task<bool> DeleteVenueAsync(Guid clubId, Guid id);
    Task<VenueScheduleDto> GetVenueScheduleAsync(Guid clubId, Guid id, DateTime? fromDate = null, DateTime? toDate = null);
}
