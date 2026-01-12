using TheLeague.Api.DTOs;

namespace TheLeague.Api.Services.Interfaces;

public interface ICompetitionService
{
    // Seasons
    Task<IEnumerable<SeasonDto>> GetSeasonsAsync(Guid clubId);
    Task<SeasonDto?> GetSeasonByIdAsync(Guid clubId, Guid id);
    Task<SeasonDto> CreateSeasonAsync(Guid clubId, SeasonCreateRequest request);
    Task<SeasonDto?> UpdateSeasonAsync(Guid clubId, Guid id, SeasonUpdateRequest request);
    Task<bool> DeleteSeasonAsync(Guid clubId, Guid id);

    // Competitions
    Task<PagedResult<CompetitionListDto>> GetCompetitionsAsync(Guid clubId, CompetitionFilterRequest filter);
    Task<CompetitionDto?> GetCompetitionByIdAsync(Guid clubId, Guid id);
    Task<CompetitionDto> CreateCompetitionAsync(Guid clubId, CompetitionCreateRequest request);
    Task<CompetitionDto?> UpdateCompetitionAsync(Guid clubId, Guid id, CompetitionUpdateRequest request);
    Task<bool> DeleteCompetitionAsync(Guid clubId, Guid id);
    Task<bool> PublishCompetitionAsync(Guid clubId, Guid id);

    // Competition Rounds
    Task<IEnumerable<CompetitionRoundDto>> GetRoundsAsync(Guid clubId, Guid competitionId);
    Task<CompetitionRoundDto> CreateRoundAsync(Guid clubId, Guid competitionId, CompetitionRoundCreateRequest request);
    Task<bool> DeleteRoundAsync(Guid clubId, Guid competitionId, Guid roundId);

    // Competition Teams
    Task<IEnumerable<CompetitionTeamDto>> GetTeamsAsync(Guid clubId, Guid competitionId);
    Task<CompetitionTeamDto?> GetTeamByIdAsync(Guid clubId, Guid competitionId, Guid teamId);
    Task<CompetitionTeamDto> RegisterTeamAsync(Guid clubId, Guid competitionId, CompetitionTeamCreateRequest request);
    Task<CompetitionTeamDto?> UpdateTeamAsync(Guid clubId, Guid competitionId, Guid teamId, CompetitionTeamUpdateRequest request);
    Task<bool> WithdrawTeamAsync(Guid clubId, Guid competitionId, Guid teamId);
    Task<bool> ApproveTeamAsync(Guid clubId, Guid competitionId, Guid teamId);

    // Team Participants
    Task<IEnumerable<CompetitionParticipantDto>> GetTeamParticipantsAsync(Guid clubId, Guid competitionId, Guid teamId);
    Task<CompetitionParticipantDto> AddParticipantAsync(Guid clubId, Guid competitionId, Guid teamId, CompetitionParticipantCreateRequest request);
    Task<CompetitionParticipantDto?> UpdateParticipantAsync(Guid clubId, Guid competitionId, Guid teamId, Guid participantId, CompetitionParticipantUpdateRequest request);
    Task<bool> RemoveParticipantAsync(Guid clubId, Guid competitionId, Guid teamId, Guid participantId);

    // Matches
    Task<PagedResult<MatchListDto>> GetMatchesAsync(Guid clubId, Guid competitionId, MatchFilterRequest filter);
    Task<MatchDto?> GetMatchByIdAsync(Guid clubId, Guid competitionId, Guid matchId);
    Task<MatchDto> CreateMatchAsync(Guid clubId, Guid competitionId, MatchCreateRequest request);
    Task<MatchDto?> UpdateMatchAsync(Guid clubId, Guid competitionId, Guid matchId, MatchUpdateRequest request);
    Task<MatchDto?> RecordMatchResultAsync(Guid clubId, Guid competitionId, Guid matchId, MatchResultRequest request);
    Task<bool> PostponeMatchAsync(Guid clubId, Guid competitionId, Guid matchId, MatchPostponeRequest request);
    Task<bool> CancelMatchAsync(Guid clubId, Guid competitionId, Guid matchId, string? reason);
    Task<bool> DeleteMatchAsync(Guid clubId, Guid competitionId, Guid matchId);

    // Match Events
    Task<IEnumerable<MatchEventDto>> GetMatchEventsAsync(Guid clubId, Guid competitionId, Guid matchId);
    Task<MatchEventDto> AddMatchEventAsync(Guid clubId, Guid competitionId, Guid matchId, MatchEventCreateRequest request);
    Task<bool> DeleteMatchEventAsync(Guid clubId, Guid competitionId, Guid matchId, Guid eventId);

    // Match Lineups
    Task<IEnumerable<MatchLineupDto>> GetMatchLineupsAsync(Guid clubId, Guid competitionId, Guid matchId);
    Task<MatchLineupDto> AddLineupPlayerAsync(Guid clubId, Guid competitionId, Guid matchId, MatchLineupCreateRequest request);
    Task<bool> RemoveLineupPlayerAsync(Guid clubId, Guid competitionId, Guid matchId, Guid lineupId);

    // Standings
    Task<IEnumerable<CompetitionStandingDto>> GetStandingsAsync(Guid clubId, Guid competitionId, string? group = null);
    Task RecalculateStandingsAsync(Guid clubId, Guid competitionId);

    // Top Scorers
    Task<IEnumerable<TopScorerDto>> GetTopScorersAsync(Guid clubId, Guid competitionId, int limit = 10);

    // Fixtures Generation
    Task<IEnumerable<MatchDto>> GenerateFixturesAsync(Guid clubId, Guid competitionId, GenerateFixturesRequest request);
    Task<bool> PerformDrawAsync(Guid clubId, Guid competitionId, DrawRequest request);
}
