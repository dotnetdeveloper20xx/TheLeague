using TheLeague.Api.DTOs;

namespace TheLeague.Api.Services.Interfaces;

public interface IClubService
{
    Task<IEnumerable<ClubDto>> GetAllClubsAsync();
    Task<ClubDto?> GetClubByIdAsync(Guid id);
    Task<ClubDto?> GetClubBySlugAsync(string slug);
    Task<ClubDto> CreateClubAsync(ClubCreateRequest request);
    Task<ClubDto?> UpdateClubAsync(Guid id, ClubUpdateRequest request);
    Task<bool> DeleteClubAsync(Guid id);
    Task<ClubSettingsDto?> GetClubSettingsAsync(Guid clubId);
    Task<ClubSettingsDto?> UpdateClubSettingsAsync(Guid clubId, ClubSettingsUpdateRequest request);
    Task<ClubDashboardDto> GetClubDashboardAsync(Guid clubId);
}
