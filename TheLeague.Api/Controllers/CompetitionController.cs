using Microsoft.AspNetCore.Mvc;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;

namespace TheLeague.Api.Controllers;

[ApiController]
[Route("api/clubs/{clubId}/[controller]")]
public class CompetitionController : ControllerBase
{
    private readonly ICompetitionService _competitionService;

    public CompetitionController(ICompetitionService competitionService)
    {
        _competitionService = competitionService;
    }

    #region Seasons

    [HttpGet("seasons")]
    public async Task<ActionResult<IEnumerable<SeasonDto>>> GetSeasons(Guid clubId)
    {
        var seasons = await _competitionService.GetSeasonsAsync(clubId);
        return Ok(seasons);
    }

    [HttpGet("seasons/{id}")]
    public async Task<ActionResult<SeasonDto>> GetSeason(Guid clubId, Guid id)
    {
        var season = await _competitionService.GetSeasonByIdAsync(clubId, id);
        return season == null ? NotFound() : Ok(season);
    }

    [HttpPost("seasons")]
    public async Task<ActionResult<SeasonDto>> CreateSeason(Guid clubId, SeasonCreateRequest request)
    {
        var season = await _competitionService.CreateSeasonAsync(clubId, request);
        return CreatedAtAction(nameof(GetSeason), new { clubId, id = season.Id }, season);
    }

    [HttpPut("seasons/{id}")]
    public async Task<ActionResult<SeasonDto>> UpdateSeason(Guid clubId, Guid id, SeasonUpdateRequest request)
    {
        var season = await _competitionService.UpdateSeasonAsync(clubId, id, request);
        return season == null ? NotFound() : Ok(season);
    }

    [HttpDelete("seasons/{id}")]
    public async Task<ActionResult> DeleteSeason(Guid clubId, Guid id)
    {
        var result = await _competitionService.DeleteSeasonAsync(clubId, id);
        return result ? NoContent() : NotFound();
    }

    #endregion

    #region Competitions

    [HttpGet]
    public async Task<ActionResult<PagedResult<CompetitionListDto>>> GetCompetitions(
        Guid clubId,
        [FromQuery] CompetitionFilterRequest filter)
    {
        var competitions = await _competitionService.GetCompetitionsAsync(clubId, filter);
        return Ok(competitions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CompetitionDto>> GetCompetition(Guid clubId, Guid id)
    {
        var competition = await _competitionService.GetCompetitionByIdAsync(clubId, id);
        return competition == null ? NotFound() : Ok(competition);
    }

    [HttpPost]
    public async Task<ActionResult<CompetitionDto>> CreateCompetition(Guid clubId, CompetitionCreateRequest request)
    {
        var competition = await _competitionService.CreateCompetitionAsync(clubId, request);
        return CreatedAtAction(nameof(GetCompetition), new { clubId, id = competition.Id }, competition);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CompetitionDto>> UpdateCompetition(Guid clubId, Guid id, CompetitionUpdateRequest request)
    {
        var competition = await _competitionService.UpdateCompetitionAsync(clubId, id, request);
        return competition == null ? NotFound() : Ok(competition);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCompetition(Guid clubId, Guid id)
    {
        var result = await _competitionService.DeleteCompetitionAsync(clubId, id);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("{id}/publish")]
    public async Task<ActionResult> PublishCompetition(Guid clubId, Guid id)
    {
        var result = await _competitionService.PublishCompetitionAsync(clubId, id);
        return result ? Ok() : NotFound();
    }

    #endregion

    #region Competition Rounds

    [HttpGet("{competitionId}/rounds")]
    public async Task<ActionResult<IEnumerable<CompetitionRoundDto>>> GetRounds(Guid clubId, Guid competitionId)
    {
        var rounds = await _competitionService.GetRoundsAsync(clubId, competitionId);
        return Ok(rounds);
    }

    [HttpPost("{competitionId}/rounds")]
    public async Task<ActionResult<CompetitionRoundDto>> CreateRound(
        Guid clubId,
        Guid competitionId,
        CompetitionRoundCreateRequest request)
    {
        try
        {
            var round = await _competitionService.CreateRoundAsync(clubId, competitionId, request);
            return Created($"api/clubs/{clubId}/competition/{competitionId}/rounds/{round.Id}", round);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{competitionId}/rounds/{roundId}")]
    public async Task<ActionResult> DeleteRound(Guid clubId, Guid competitionId, Guid roundId)
    {
        var result = await _competitionService.DeleteRoundAsync(clubId, competitionId, roundId);
        return result ? NoContent() : NotFound();
    }

    #endregion

    #region Competition Teams

    [HttpGet("{competitionId}/teams")]
    public async Task<ActionResult<IEnumerable<CompetitionTeamDto>>> GetTeams(Guid clubId, Guid competitionId)
    {
        var teams = await _competitionService.GetTeamsAsync(clubId, competitionId);
        return Ok(teams);
    }

    [HttpGet("{competitionId}/teams/{teamId}")]
    public async Task<ActionResult<CompetitionTeamDto>> GetTeam(Guid clubId, Guid competitionId, Guid teamId)
    {
        var team = await _competitionService.GetTeamByIdAsync(clubId, competitionId, teamId);
        return team == null ? NotFound() : Ok(team);
    }

    [HttpPost("{competitionId}/teams")]
    public async Task<ActionResult<CompetitionTeamDto>> RegisterTeam(
        Guid clubId,
        Guid competitionId,
        CompetitionTeamCreateRequest request)
    {
        var team = await _competitionService.RegisterTeamAsync(clubId, competitionId, request);
        return CreatedAtAction(nameof(GetTeam), new { clubId, competitionId, teamId = team.Id }, team);
    }

    [HttpPut("{competitionId}/teams/{teamId}")]
    public async Task<ActionResult<CompetitionTeamDto>> UpdateTeam(
        Guid clubId,
        Guid competitionId,
        Guid teamId,
        CompetitionTeamUpdateRequest request)
    {
        var team = await _competitionService.UpdateTeamAsync(clubId, competitionId, teamId, request);
        return team == null ? NotFound() : Ok(team);
    }

    [HttpPost("{competitionId}/teams/{teamId}/withdraw")]
    public async Task<ActionResult> WithdrawTeam(Guid clubId, Guid competitionId, Guid teamId)
    {
        var result = await _competitionService.WithdrawTeamAsync(clubId, competitionId, teamId);
        return result ? Ok() : NotFound();
    }

    [HttpPost("{competitionId}/teams/{teamId}/approve")]
    public async Task<ActionResult> ApproveTeam(Guid clubId, Guid competitionId, Guid teamId)
    {
        var result = await _competitionService.ApproveTeamAsync(clubId, competitionId, teamId);
        return result ? Ok() : NotFound();
    }

    #endregion

    #region Team Participants

    [HttpGet("{competitionId}/teams/{teamId}/participants")]
    public async Task<ActionResult<IEnumerable<CompetitionParticipantDto>>> GetTeamParticipants(
        Guid clubId,
        Guid competitionId,
        Guid teamId)
    {
        var participants = await _competitionService.GetTeamParticipantsAsync(clubId, competitionId, teamId);
        return Ok(participants);
    }

    [HttpPost("{competitionId}/teams/{teamId}/participants")]
    public async Task<ActionResult<CompetitionParticipantDto>> AddParticipant(
        Guid clubId,
        Guid competitionId,
        Guid teamId,
        CompetitionParticipantCreateRequest request)
    {
        try
        {
            var participant = await _competitionService.AddParticipantAsync(clubId, competitionId, teamId, request);
            return Created($"api/clubs/{clubId}/competition/{competitionId}/teams/{teamId}/participants/{participant.Id}", participant);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{competitionId}/teams/{teamId}/participants/{participantId}")]
    public async Task<ActionResult<CompetitionParticipantDto>> UpdateParticipant(
        Guid clubId,
        Guid competitionId,
        Guid teamId,
        Guid participantId,
        CompetitionParticipantUpdateRequest request)
    {
        var participant = await _competitionService.UpdateParticipantAsync(clubId, competitionId, teamId, participantId, request);
        return participant == null ? NotFound() : Ok(participant);
    }

    [HttpDelete("{competitionId}/teams/{teamId}/participants/{participantId}")]
    public async Task<ActionResult> RemoveParticipant(Guid clubId, Guid competitionId, Guid teamId, Guid participantId)
    {
        var result = await _competitionService.RemoveParticipantAsync(clubId, competitionId, teamId, participantId);
        return result ? NoContent() : NotFound();
    }

    #endregion

    #region Matches

    [HttpGet("{competitionId}/matches")]
    public async Task<ActionResult<PagedResult<MatchListDto>>> GetMatches(
        Guid clubId,
        Guid competitionId,
        [FromQuery] MatchFilterRequest filter)
    {
        var matches = await _competitionService.GetMatchesAsync(clubId, competitionId, filter);
        return Ok(matches);
    }

    [HttpGet("{competitionId}/matches/{matchId}")]
    public async Task<ActionResult<MatchDto>> GetMatch(Guid clubId, Guid competitionId, Guid matchId)
    {
        var match = await _competitionService.GetMatchByIdAsync(clubId, competitionId, matchId);
        return match == null ? NotFound() : Ok(match);
    }

    [HttpPost("{competitionId}/matches")]
    public async Task<ActionResult<MatchDto>> CreateMatch(
        Guid clubId,
        Guid competitionId,
        MatchCreateRequest request)
    {
        try
        {
            var match = await _competitionService.CreateMatchAsync(clubId, competitionId, request);
            return CreatedAtAction(nameof(GetMatch), new { clubId, competitionId, matchId = match.Id }, match);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{competitionId}/matches/{matchId}")]
    public async Task<ActionResult<MatchDto>> UpdateMatch(
        Guid clubId,
        Guid competitionId,
        Guid matchId,
        MatchUpdateRequest request)
    {
        var match = await _competitionService.UpdateMatchAsync(clubId, competitionId, matchId, request);
        return match == null ? NotFound() : Ok(match);
    }

    [HttpPost("{competitionId}/matches/{matchId}/result")]
    public async Task<ActionResult<MatchDto>> RecordMatchResult(
        Guid clubId,
        Guid competitionId,
        Guid matchId,
        MatchResultRequest request)
    {
        var match = await _competitionService.RecordMatchResultAsync(clubId, competitionId, matchId, request);
        return match == null ? NotFound() : Ok(match);
    }

    [HttpPost("{competitionId}/matches/{matchId}/postpone")]
    public async Task<ActionResult> PostponeMatch(
        Guid clubId,
        Guid competitionId,
        Guid matchId,
        MatchPostponeRequest request)
    {
        var result = await _competitionService.PostponeMatchAsync(clubId, competitionId, matchId, request);
        return result ? Ok() : NotFound();
    }

    [HttpPost("{competitionId}/matches/{matchId}/cancel")]
    public async Task<ActionResult> CancelMatch(
        Guid clubId,
        Guid competitionId,
        Guid matchId,
        [FromBody] string? reason = null)
    {
        var result = await _competitionService.CancelMatchAsync(clubId, competitionId, matchId, reason);
        return result ? Ok() : NotFound();
    }

    [HttpDelete("{competitionId}/matches/{matchId}")]
    public async Task<ActionResult> DeleteMatch(Guid clubId, Guid competitionId, Guid matchId)
    {
        var result = await _competitionService.DeleteMatchAsync(clubId, competitionId, matchId);
        return result ? NoContent() : NotFound();
    }

    #endregion

    #region Match Events

    [HttpGet("{competitionId}/matches/{matchId}/events")]
    public async Task<ActionResult<IEnumerable<MatchEventDto>>> GetMatchEvents(
        Guid clubId,
        Guid competitionId,
        Guid matchId)
    {
        var events = await _competitionService.GetMatchEventsAsync(clubId, competitionId, matchId);
        return Ok(events);
    }

    [HttpPost("{competitionId}/matches/{matchId}/events")]
    public async Task<ActionResult<MatchEventDto>> AddMatchEvent(
        Guid clubId,
        Guid competitionId,
        Guid matchId,
        MatchEventCreateRequest request)
    {
        try
        {
            var matchEvent = await _competitionService.AddMatchEventAsync(clubId, competitionId, matchId, request);
            return Created($"api/clubs/{clubId}/competition/{competitionId}/matches/{matchId}/events/{matchEvent.Id}", matchEvent);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{competitionId}/matches/{matchId}/events/{eventId}")]
    public async Task<ActionResult> DeleteMatchEvent(Guid clubId, Guid competitionId, Guid matchId, Guid eventId)
    {
        var result = await _competitionService.DeleteMatchEventAsync(clubId, competitionId, matchId, eventId);
        return result ? NoContent() : NotFound();
    }

    #endregion

    #region Match Lineups

    [HttpGet("{competitionId}/matches/{matchId}/lineups")]
    public async Task<ActionResult<IEnumerable<MatchLineupDto>>> GetMatchLineups(
        Guid clubId,
        Guid competitionId,
        Guid matchId)
    {
        var lineups = await _competitionService.GetMatchLineupsAsync(clubId, competitionId, matchId);
        return Ok(lineups);
    }

    [HttpPost("{competitionId}/matches/{matchId}/lineups")]
    public async Task<ActionResult<MatchLineupDto>> AddLineupPlayer(
        Guid clubId,
        Guid competitionId,
        Guid matchId,
        MatchLineupCreateRequest request)
    {
        try
        {
            var lineup = await _competitionService.AddLineupPlayerAsync(clubId, competitionId, matchId, request);
            return Created($"api/clubs/{clubId}/competition/{competitionId}/matches/{matchId}/lineups/{lineup.Id}", lineup);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{competitionId}/matches/{matchId}/lineups/{lineupId}")]
    public async Task<ActionResult> RemoveLineupPlayer(Guid clubId, Guid competitionId, Guid matchId, Guid lineupId)
    {
        var result = await _competitionService.RemoveLineupPlayerAsync(clubId, competitionId, matchId, lineupId);
        return result ? NoContent() : NotFound();
    }

    #endregion

    #region Standings

    [HttpGet("{competitionId}/standings")]
    public async Task<ActionResult<IEnumerable<CompetitionStandingDto>>> GetStandings(
        Guid clubId,
        Guid competitionId,
        [FromQuery] string? group = null)
    {
        var standings = await _competitionService.GetStandingsAsync(clubId, competitionId, group);
        return Ok(standings);
    }

    [HttpPost("{competitionId}/standings/recalculate")]
    public async Task<ActionResult> RecalculateStandings(Guid clubId, Guid competitionId)
    {
        await _competitionService.RecalculateStandingsAsync(clubId, competitionId);
        return Ok();
    }

    #endregion

    #region Top Scorers

    [HttpGet("{competitionId}/top-scorers")]
    public async Task<ActionResult<IEnumerable<TopScorerDto>>> GetTopScorers(
        Guid clubId,
        Guid competitionId,
        [FromQuery] int limit = 10)
    {
        var scorers = await _competitionService.GetTopScorersAsync(clubId, competitionId, limit);
        return Ok(scorers);
    }

    #endregion

    #region Fixtures & Draw

    [HttpPost("{competitionId}/fixtures/generate")]
    public async Task<ActionResult<IEnumerable<MatchDto>>> GenerateFixtures(
        Guid clubId,
        Guid competitionId,
        GenerateFixturesRequest request)
    {
        try
        {
            var matches = await _competitionService.GenerateFixturesAsync(clubId, competitionId, request);
            return Ok(matches);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{competitionId}/draw")]
    public async Task<ActionResult> PerformDraw(Guid clubId, Guid competitionId, DrawRequest request)
    {
        var result = await _competitionService.PerformDrawAsync(clubId, competitionId, request);
        return result ? Ok() : BadRequest("Draw could not be performed. Ensure there are at least 2 confirmed teams.");
    }

    #endregion
}
