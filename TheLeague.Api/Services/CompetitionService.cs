using Microsoft.EntityFrameworkCore;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Entities;
using TheLeague.Core.Enums;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Services;

public class CompetitionService : ICompetitionService
{
    private readonly ApplicationDbContext _context;

    public CompetitionService(ApplicationDbContext context)
    {
        _context = context;
    }

    #region Seasons

    public async Task<IEnumerable<SeasonDto>> GetSeasonsAsync(Guid clubId)
    {
        return await _context.Seasons.IgnoreQueryFilters()
            .Where(s => s.ClubId == clubId)
            .OrderByDescending(s => s.StartDate)
            .Select(s => new SeasonDto(
                s.Id,
                s.Name,
                s.StartDate,
                s.EndDate,
                s.IsCurrent,
                s.IsCompleted,
                s.Competitions.Count
            ))
            .ToListAsync();
    }

    public async Task<SeasonDto?> GetSeasonByIdAsync(Guid clubId, Guid id)
    {
        var season = await _context.Seasons.IgnoreQueryFilters()
            .Include(s => s.Competitions)
            .FirstOrDefaultAsync(s => s.ClubId == clubId && s.Id == id);

        return season == null ? null : new SeasonDto(
            season.Id,
            season.Name,
            season.StartDate,
            season.EndDate,
            season.IsCurrent,
            season.IsCompleted,
            season.Competitions.Count
        );
    }

    public async Task<SeasonDto> CreateSeasonAsync(Guid clubId, SeasonCreateRequest request)
    {
        // If this is set as current, unset other current seasons
        if (request.IsCurrent)
        {
            var currentSeasons = await _context.Seasons.IgnoreQueryFilters()
                .Where(s => s.ClubId == clubId && s.IsCurrent)
                .ToListAsync();
            foreach (var s in currentSeasons)
                s.IsCurrent = false;
        }

        var season = new Season
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Name = request.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsCurrent = request.IsCurrent
        };

        _context.Seasons.Add(season);
        await _context.SaveChangesAsync();

        return new SeasonDto(season.Id, season.Name, season.StartDate, season.EndDate, season.IsCurrent, season.IsCompleted, 0);
    }

    public async Task<SeasonDto?> UpdateSeasonAsync(Guid clubId, Guid id, SeasonUpdateRequest request)
    {
        var season = await _context.Seasons.IgnoreQueryFilters()
            .Include(s => s.Competitions)
            .FirstOrDefaultAsync(s => s.ClubId == clubId && s.Id == id);

        if (season == null) return null;

        if (request.Name != null) season.Name = request.Name;
        if (request.StartDate.HasValue) season.StartDate = request.StartDate.Value;
        if (request.EndDate.HasValue) season.EndDate = request.EndDate.Value;
        if (request.IsCurrent.HasValue)
        {
            if (request.IsCurrent.Value)
            {
                var currentSeasons = await _context.Seasons.IgnoreQueryFilters()
                    .Where(s => s.ClubId == clubId && s.IsCurrent && s.Id != id)
                    .ToListAsync();
                foreach (var s in currentSeasons)
                    s.IsCurrent = false;
            }
            season.IsCurrent = request.IsCurrent.Value;
        }
        if (request.IsCompleted.HasValue) season.IsCompleted = request.IsCompleted.Value;

        await _context.SaveChangesAsync();

        return new SeasonDto(season.Id, season.Name, season.StartDate, season.EndDate, season.IsCurrent, season.IsCompleted, season.Competitions.Count);
    }

    public async Task<bool> DeleteSeasonAsync(Guid clubId, Guid id)
    {
        var season = await _context.Seasons.IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.ClubId == clubId && s.Id == id);

        if (season == null) return false;

        _context.Seasons.Remove(season);
        await _context.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Competitions

    public async Task<PagedResult<CompetitionListDto>> GetCompetitionsAsync(Guid clubId, CompetitionFilterRequest filter)
    {
        var query = _context.Competitions.IgnoreQueryFilters()
            .Where(c => c.ClubId == clubId)
            .AsQueryable();

        if (filter.Type.HasValue)
            query = query.Where(c => c.Type == filter.Type.Value);
        if (filter.Status.HasValue)
            query = query.Where(c => c.Status == filter.Status.Value);
        if (!string.IsNullOrEmpty(filter.Sport))
            query = query.Where(c => c.Sport == filter.Sport);
        if (filter.SeasonId.HasValue)
            query = query.Where(c => c.SeasonId == filter.SeasonId.Value);
        if (!filter.IncludeUnpublished.GetValueOrDefault())
            query = query.Where(c => c.IsPublished);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(c => c.StartDate)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(c => new CompetitionListDto(
                c.Id,
                c.Name,
                c.Code,
                c.Type,
                c.Status,
                c.Sport,
                c.StartDate,
                c.EndDate,
                c.Teams.Count,
                c.MaxTeams,
                c.Matches.Count,
                c.Matches.Count(m => m.Status == MatchStatus.Completed),
                c.IsPublished,
                c.ImageUrl
            ))
            .ToListAsync();

        return new PagedResult<CompetitionListDto>(
            items,
            totalCount,
            filter.Page,
            filter.PageSize,
            (int)Math.Ceiling(totalCount / (double)filter.PageSize)
        );
    }

    public async Task<CompetitionDto?> GetCompetitionByIdAsync(Guid clubId, Guid id)
    {
        var comp = await _context.Competitions.IgnoreQueryFilters()
            .Include(c => c.Season)
            .Include(c => c.Venue)
            .Include(c => c.Teams)
            .Include(c => c.Matches)
            .FirstOrDefaultAsync(c => c.ClubId == clubId && c.Id == id);

        return comp == null ? null : MapToCompetitionDto(comp);
    }

    public async Task<CompetitionDto> CreateCompetitionAsync(Guid clubId, CompetitionCreateRequest request)
    {
        var comp = new Competition
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Name = request.Name,
            Code = request.Code,
            Description = request.Description,
            Type = request.Type,
            Status = CompetitionStatus.Draft,
            Sport = request.Sport,
            Category = request.Category,
            Division = request.Division,
            SkillLevel = request.SkillLevel,
            AgeGroup = request.AgeGroup,
            MinAge = request.MinAge,
            MaxAge = request.MaxAge,
            TargetGender = request.TargetGender,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            RegistrationOpenDate = request.RegistrationOpenDate,
            RegistrationCloseDate = request.RegistrationCloseDate,
            SeasonId = request.SeasonId,
            VenueId = request.VenueId,
            Format = request.Format,
            NumberOfRounds = request.NumberOfRounds,
            HomeAndAway = request.HomeAndAway,
            IsTeamBased = request.IsTeamBased,
            MinTeams = request.MinTeams,
            MaxTeams = request.MaxTeams,
            MinPlayersPerTeam = request.MinPlayersPerTeam,
            MaxPlayersPerTeam = request.MaxPlayersPerTeam,
            EntryFee = request.EntryFee,
            TeamEntryFee = request.TeamEntryFee,
            PlayerEntryFee = request.PlayerEntryFee,
            HasPrizes = request.HasPrizes,
            PrizeDescription = request.PrizeDescription,
            TotalPrizeMoney = request.TotalPrizeMoney,
            PointsForWin = request.PointsForWin,
            PointsForDraw = request.PointsForDraw,
            PointsForLoss = request.PointsForLoss,
            Rules = request.Rules,
            OrganizerName = request.OrganizerName,
            ContactEmail = request.ContactEmail,
            ContactPhone = request.ContactPhone,
            ImageUrl = request.ImageUrl,
            IsPublished = request.IsPublished
        };

        _context.Competitions.Add(comp);
        await _context.SaveChangesAsync();

        return MapToCompetitionDto(comp);
    }

    public async Task<CompetitionDto?> UpdateCompetitionAsync(Guid clubId, Guid id, CompetitionUpdateRequest request)
    {
        var comp = await _context.Competitions.IgnoreQueryFilters()
            .Include(c => c.Season)
            .Include(c => c.Venue)
            .Include(c => c.Teams)
            .Include(c => c.Matches)
            .FirstOrDefaultAsync(c => c.ClubId == clubId && c.Id == id);

        if (comp == null) return null;

        if (request.Name != null) comp.Name = request.Name;
        if (request.Code != null) comp.Code = request.Code;
        if (request.Description != null) comp.Description = request.Description;
        if (request.Type.HasValue) comp.Type = request.Type.Value;
        if (request.Status.HasValue) comp.Status = request.Status.Value;
        if (request.Sport != null) comp.Sport = request.Sport;
        if (request.Category != null) comp.Category = request.Category;
        if (request.Division != null) comp.Division = request.Division;
        if (request.SkillLevel.HasValue) comp.SkillLevel = request.SkillLevel.Value;
        if (request.AgeGroup.HasValue) comp.AgeGroup = request.AgeGroup.Value;
        if (request.StartDate.HasValue) comp.StartDate = request.StartDate;
        if (request.EndDate.HasValue) comp.EndDate = request.EndDate;
        if (request.RegistrationOpenDate.HasValue) comp.RegistrationOpenDate = request.RegistrationOpenDate;
        if (request.RegistrationCloseDate.HasValue) comp.RegistrationCloseDate = request.RegistrationCloseDate;
        if (request.SeasonId.HasValue) comp.SeasonId = request.SeasonId;
        if (request.VenueId.HasValue) comp.VenueId = request.VenueId;
        if (request.Format != null) comp.Format = request.Format;
        if (request.NumberOfRounds.HasValue) comp.NumberOfRounds = request.NumberOfRounds;
        if (request.HomeAndAway.HasValue) comp.HomeAndAway = request.HomeAndAway.Value;
        if (request.MinTeams.HasValue) comp.MinTeams = request.MinTeams;
        if (request.MaxTeams.HasValue) comp.MaxTeams = request.MaxTeams;
        if (request.MinPlayersPerTeam.HasValue) comp.MinPlayersPerTeam = request.MinPlayersPerTeam;
        if (request.MaxPlayersPerTeam.HasValue) comp.MaxPlayersPerTeam = request.MaxPlayersPerTeam;
        if (request.EntryFee.HasValue) comp.EntryFee = request.EntryFee;
        if (request.HasPrizes.HasValue) comp.HasPrizes = request.HasPrizes.Value;
        if (request.PrizeDescription != null) comp.PrizeDescription = request.PrizeDescription;
        if (request.TotalPrizeMoney.HasValue) comp.TotalPrizeMoney = request.TotalPrizeMoney;
        if (request.PointsForWin.HasValue) comp.PointsForWin = request.PointsForWin.Value;
        if (request.PointsForDraw.HasValue) comp.PointsForDraw = request.PointsForDraw.Value;
        if (request.PointsForLoss.HasValue) comp.PointsForLoss = request.PointsForLoss.Value;
        if (request.Rules != null) comp.Rules = request.Rules;
        if (request.OrganizerName != null) comp.OrganizerName = request.OrganizerName;
        if (request.ContactEmail != null) comp.ContactEmail = request.ContactEmail;
        if (request.ContactPhone != null) comp.ContactPhone = request.ContactPhone;
        if (request.ImageUrl != null) comp.ImageUrl = request.ImageUrl;
        if (request.IsPublished.HasValue) comp.IsPublished = request.IsPublished.Value;

        await _context.SaveChangesAsync();
        return MapToCompetitionDto(comp);
    }

    public async Task<bool> DeleteCompetitionAsync(Guid clubId, Guid id)
    {
        var comp = await _context.Competitions.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.ClubId == clubId && c.Id == id);

        if (comp == null) return false;

        _context.Competitions.Remove(comp);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> PublishCompetitionAsync(Guid clubId, Guid id)
    {
        var comp = await _context.Competitions.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.ClubId == clubId && c.Id == id);

        if (comp == null) return false;

        comp.IsPublished = true;
        comp.Status = CompetitionStatus.Published;
        await _context.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Competition Rounds

    public async Task<IEnumerable<CompetitionRoundDto>> GetRoundsAsync(Guid clubId, Guid competitionId)
    {
        // Verify competition belongs to club
        var competitionExists = await _context.Competitions.IgnoreQueryFilters()
            .AnyAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (!competitionExists) return Enumerable.Empty<CompetitionRoundDto>();

        return await _context.CompetitionRounds.IgnoreQueryFilters()
            .Where(r => r.CompetitionId == competitionId)
            .OrderBy(r => r.RoundNumber)
            .Select(r => new CompetitionRoundDto(
                r.Id,
                r.RoundNumber,
                r.Name,
                r.StartDate,
                r.EndDate,
                r.IsComplete,
                r.Matches.Count,
                r.Matches.Count(m => m.Status == MatchStatus.Completed)
            ))
            .ToListAsync();
    }

    public async Task<CompetitionRoundDto> CreateRoundAsync(Guid clubId, Guid competitionId, CompetitionRoundCreateRequest request)
    {
        // Verify competition belongs to club
        var competition = await _context.Competitions.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (competition == null) throw new InvalidOperationException("Competition not found");

        var round = new CompetitionRound
        {
            Id = Guid.NewGuid(),
            CompetitionId = competitionId,
            RoundNumber = request.RoundNumber,
            Name = request.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        _context.CompetitionRounds.Add(round);
        await _context.SaveChangesAsync();

        return new CompetitionRoundDto(round.Id, round.RoundNumber, round.Name, round.StartDate, round.EndDate, round.IsComplete, 0, 0);
    }

    public async Task<bool> DeleteRoundAsync(Guid clubId, Guid competitionId, Guid roundId)
    {
        // Verify competition belongs to club
        var competitionExists = await _context.Competitions.IgnoreQueryFilters()
            .AnyAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (!competitionExists) return false;

        var round = await _context.CompetitionRounds.IgnoreQueryFilters()
            .FirstOrDefaultAsync(r => r.CompetitionId == competitionId && r.Id == roundId);

        if (round == null) return false;

        _context.CompetitionRounds.Remove(round);
        await _context.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Competition Teams

    public async Task<IEnumerable<CompetitionTeamDto>> GetTeamsAsync(Guid clubId, Guid competitionId)
    {
        return await _context.CompetitionTeams.IgnoreQueryFilters()
            .Where(t => t.ClubId == clubId && t.CompetitionId == competitionId)
            .Include(t => t.Participants)
            .OrderBy(t => t.Position)
            .ThenBy(t => t.Name)
            .Select(t => MapToTeamDto(t))
            .ToListAsync();
    }

    public async Task<CompetitionTeamDto?> GetTeamByIdAsync(Guid clubId, Guid competitionId, Guid teamId)
    {
        var team = await _context.CompetitionTeams.IgnoreQueryFilters()
            .Include(t => t.Participants)
            .FirstOrDefaultAsync(t => t.ClubId == clubId && t.CompetitionId == competitionId && t.Id == teamId);

        return team == null ? null : MapToTeamDto(team);
    }

    public async Task<CompetitionTeamDto> RegisterTeamAsync(Guid clubId, Guid competitionId, CompetitionTeamCreateRequest request)
    {
        var team = new CompetitionTeam
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            CompetitionId = competitionId,
            Name = request.Name,
            ShortName = request.ShortName,
            Code = request.Code,
            SeedNumber = request.SeedNumber,
            Group = request.Group,
            CaptainId = request.CaptainId,
            CaptainName = request.CaptainName,
            ContactEmail = request.ContactEmail,
            ContactPhone = request.ContactPhone,
            HomeVenueId = request.HomeVenueId,
            HomeColors = request.HomeColors,
            AwayColors = request.AwayColors,
            LogoUrl = request.LogoUrl,
            Status = TeamStatus.Registered
        };

        _context.CompetitionTeams.Add(team);
        await _context.SaveChangesAsync();

        return MapToTeamDto(team);
    }

    public async Task<CompetitionTeamDto?> UpdateTeamAsync(Guid clubId, Guid competitionId, Guid teamId, CompetitionTeamUpdateRequest request)
    {
        var team = await _context.CompetitionTeams.IgnoreQueryFilters()
            .Include(t => t.Participants)
            .FirstOrDefaultAsync(t => t.ClubId == clubId && t.CompetitionId == competitionId && t.Id == teamId);

        if (team == null) return null;

        if (request.Name != null) team.Name = request.Name;
        if (request.ShortName != null) team.ShortName = request.ShortName;
        if (request.Code != null) team.Code = request.Code;
        if (request.Status.HasValue) team.Status = request.Status.Value;
        if (request.SeedNumber.HasValue) team.SeedNumber = request.SeedNumber;
        if (request.DrawPosition.HasValue) team.DrawPosition = request.DrawPosition;
        if (request.Group != null) team.Group = request.Group;
        if (request.CaptainId.HasValue) team.CaptainId = request.CaptainId;
        if (request.CaptainName != null) team.CaptainName = request.CaptainName;
        if (request.ContactEmail != null) team.ContactEmail = request.ContactEmail;
        if (request.ContactPhone != null) team.ContactPhone = request.ContactPhone;
        if (request.IsApproved.HasValue) team.IsApproved = request.IsApproved.Value;
        if (request.EntryFeePaid.HasValue) team.EntryFeePaid = request.EntryFeePaid.Value;
        if (request.HomeColors != null) team.HomeColors = request.HomeColors;
        if (request.AwayColors != null) team.AwayColors = request.AwayColors;
        if (request.LogoUrl != null) team.LogoUrl = request.LogoUrl;

        await _context.SaveChangesAsync();
        return MapToTeamDto(team);
    }

    public async Task<bool> WithdrawTeamAsync(Guid clubId, Guid competitionId, Guid teamId)
    {
        var team = await _context.CompetitionTeams.IgnoreQueryFilters()
            .FirstOrDefaultAsync(t => t.ClubId == clubId && t.CompetitionId == competitionId && t.Id == teamId);

        if (team == null) return false;

        team.Status = TeamStatus.Withdrawn;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ApproveTeamAsync(Guid clubId, Guid competitionId, Guid teamId)
    {
        var team = await _context.CompetitionTeams.IgnoreQueryFilters()
            .FirstOrDefaultAsync(t => t.ClubId == clubId && t.CompetitionId == competitionId && t.Id == teamId);

        if (team == null) return false;

        team.IsApproved = true;
        team.Status = TeamStatus.Confirmed;
        await _context.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Team Participants

    public async Task<IEnumerable<CompetitionParticipantDto>> GetTeamParticipantsAsync(Guid clubId, Guid competitionId, Guid teamId)
    {
        // Verify team belongs to club and competition
        var teamExists = await _context.CompetitionTeams.IgnoreQueryFilters()
            .AnyAsync(t => t.ClubId == clubId && t.CompetitionId == competitionId && t.Id == teamId);
        if (!teamExists) return Enumerable.Empty<CompetitionParticipantDto>();

        return await _context.CompetitionParticipants.IgnoreQueryFilters()
            .Where(p => p.TeamId == teamId)
            .Include(p => p.Member)
            .OrderBy(p => p.SquadNumber)
            .ThenBy(p => p.Member!.LastName)
            .Select(p => new CompetitionParticipantDto(
                p.Id,
                p.TeamId,
                p.MemberId,
                p.Member != null ? p.Member.FullName : "Unknown",
                p.Role,
                p.IsActive,
                p.SquadNumber,
                p.Position,
                p.IsEligible,
                p.Appearances,
                p.Goals,
                p.Assists,
                p.YellowCards,
                p.RedCards,
                p.MinutesPlayed
            ))
            .ToListAsync();
    }

    public async Task<CompetitionParticipantDto> AddParticipantAsync(Guid clubId, Guid competitionId, Guid teamId, CompetitionParticipantCreateRequest request)
    {
        // Verify team belongs to club and competition
        var team = await _context.CompetitionTeams.IgnoreQueryFilters()
            .FirstOrDefaultAsync(t => t.ClubId == clubId && t.CompetitionId == competitionId && t.Id == teamId);
        if (team == null) throw new InvalidOperationException("Team not found");

        var member = await _context.Members.FindAsync(request.MemberId);

        var participant = new CompetitionParticipant
        {
            Id = Guid.NewGuid(),
            TeamId = teamId,
            MemberId = request.MemberId,
            Role = request.Role,
            SquadNumber = request.SquadNumber,
            Position = request.Position,
            IsActive = true,
            IsEligible = true
        };

        _context.CompetitionParticipants.Add(participant);
        await _context.SaveChangesAsync();

        return new CompetitionParticipantDto(
            participant.Id,
            participant.TeamId,
            participant.MemberId,
            member?.FullName ?? "Unknown",
            participant.Role,
            participant.IsActive,
            participant.SquadNumber,
            participant.Position,
            participant.IsEligible,
            0, 0, 0, 0, 0, 0
        );
    }

    public async Task<CompetitionParticipantDto?> UpdateParticipantAsync(Guid clubId, Guid competitionId, Guid teamId, Guid participantId, CompetitionParticipantUpdateRequest request)
    {
        // Verify team belongs to club and competition
        var teamExists = await _context.CompetitionTeams.IgnoreQueryFilters()
            .AnyAsync(t => t.ClubId == clubId && t.CompetitionId == competitionId && t.Id == teamId);
        if (!teamExists) return null;

        var participant = await _context.CompetitionParticipants.IgnoreQueryFilters()
            .Include(p => p.Member)
            .FirstOrDefaultAsync(p => p.TeamId == teamId && p.Id == participantId);

        if (participant == null) return null;

        if (request.Role.HasValue) participant.Role = request.Role.Value;
        if (request.IsActive.HasValue) participant.IsActive = request.IsActive.Value;
        if (request.SquadNumber.HasValue) participant.SquadNumber = request.SquadNumber;
        if (request.Position != null) participant.Position = request.Position;
        if (request.IsEligible.HasValue) participant.IsEligible = request.IsEligible.Value;
        if (request.EligibilityNotes != null) participant.EligibilityNotes = request.EligibilityNotes;

        await _context.SaveChangesAsync();

        return new CompetitionParticipantDto(
            participant.Id,
            participant.TeamId,
            participant.MemberId,
            participant.Member?.FullName ?? "Unknown",
            participant.Role,
            participant.IsActive,
            participant.SquadNumber,
            participant.Position,
            participant.IsEligible,
            participant.Appearances,
            participant.Goals,
            participant.Assists,
            participant.YellowCards,
            participant.RedCards,
            participant.MinutesPlayed
        );
    }

    public async Task<bool> RemoveParticipantAsync(Guid clubId, Guid competitionId, Guid teamId, Guid participantId)
    {
        // Verify team belongs to club and competition
        var teamExists = await _context.CompetitionTeams.IgnoreQueryFilters()
            .AnyAsync(t => t.ClubId == clubId && t.CompetitionId == competitionId && t.Id == teamId);
        if (!teamExists) return false;

        var participant = await _context.CompetitionParticipants.IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.TeamId == teamId && p.Id == participantId);

        if (participant == null) return false;

        _context.CompetitionParticipants.Remove(participant);
        await _context.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Matches

    public async Task<PagedResult<MatchListDto>> GetMatchesAsync(Guid clubId, Guid competitionId, MatchFilterRequest filter)
    {
        // Verify competition belongs to club
        var competitionExists = await _context.Competitions.IgnoreQueryFilters()
            .AnyAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (!competitionExists) return new PagedResult<MatchListDto>(new List<MatchListDto>(), 0, 1, filter.PageSize, 0);

        var query = _context.Matches.IgnoreQueryFilters()
            .Where(m => m.CompetitionId == competitionId)
            .AsQueryable();

        if (filter.RoundId.HasValue)
            query = query.Where(m => m.RoundId == filter.RoundId.Value);
        if (filter.TeamId.HasValue)
            query = query.Where(m => m.HomeTeamId == filter.TeamId.Value || m.AwayTeamId == filter.TeamId.Value);
        if (filter.Status.HasValue)
            query = query.Where(m => m.Status == filter.Status.Value);
        if (filter.DateFrom.HasValue)
            query = query.Where(m => m.ScheduledDateTime >= filter.DateFrom.Value);
        if (filter.DateTo.HasValue)
            query = query.Where(m => m.ScheduledDateTime <= filter.DateTo.Value);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(m => m.ScheduledDateTime)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Round)
            .Select(m => new MatchListDto(
                m.Id,
                m.MatchNumber,
                m.Status,
                m.ScheduledDateTime,
                m.HomeTeamId,
                m.HomeTeam != null ? m.HomeTeam.Name : null,
                m.AwayTeamId,
                m.AwayTeam != null ? m.AwayTeam.Name : null,
                m.Result,
                m.HomeScore,
                m.AwayScore,
                m.Round != null ? m.Round.Name : null
            ))
            .ToListAsync();

        return new PagedResult<MatchListDto>(
            items,
            totalCount,
            filter.Page,
            filter.PageSize,
            (int)Math.Ceiling(totalCount / (double)filter.PageSize)
        );
    }

    public async Task<MatchDto?> GetMatchByIdAsync(Guid clubId, Guid competitionId, Guid matchId)
    {
        // Verify competition belongs to club
        var competitionExists = await _context.Competitions.IgnoreQueryFilters()
            .AnyAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (!competitionExists) return null;

        var match = await _context.Matches.IgnoreQueryFilters()
            .Include(m => m.Competition)
            .Include(m => m.Round)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Venue)
            .FirstOrDefaultAsync(m => m.CompetitionId == competitionId && m.Id == matchId);

        return match == null ? null : MapToMatchDto(match);
    }

    public async Task<MatchDto> CreateMatchAsync(Guid clubId, Guid competitionId, MatchCreateRequest request)
    {
        // Verify competition belongs to club
        var competition = await _context.Competitions.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (competition == null) throw new InvalidOperationException("Competition not found");

        var match = new Match
        {
            Id = Guid.NewGuid(),
            CompetitionId = competitionId,
            RoundId = request.RoundId,
            VenueId = request.VenueId,
            FacilityId = request.FacilityId,
            HomeTeamId = request.HomeTeamId,
            AwayTeamId = request.AwayTeamId,
            MatchNumber = request.MatchNumber,
            LegNumber = request.LegNumber,
            ScheduledDateTime = request.ScheduledDateTime,
            RefereeName = request.RefereeName,
            RefereeId = request.RefereeId,
            Status = MatchStatus.Scheduled,
            Result = MatchResult.NotPlayed
        };

        _context.Matches.Add(match);
        await _context.SaveChangesAsync();

        // Reload with relationships
        match = await _context.Matches
            .Include(m => m.Competition)
            .Include(m => m.Round)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Venue)
            .FirstAsync(m => m.Id == match.Id);

        return MapToMatchDto(match);
    }

    public async Task<MatchDto?> UpdateMatchAsync(Guid clubId, Guid competitionId, Guid matchId, MatchUpdateRequest request)
    {
        // Verify competition belongs to club
        var competitionExists = await _context.Competitions.IgnoreQueryFilters()
            .AnyAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (!competitionExists) return null;

        var match = await _context.Matches.IgnoreQueryFilters()
            .Include(m => m.Competition)
            .Include(m => m.Round)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Venue)
            .FirstOrDefaultAsync(m => m.CompetitionId == competitionId && m.Id == matchId);

        if (match == null) return null;

        if (request.RoundId.HasValue) match.RoundId = request.RoundId;
        if (request.VenueId.HasValue) match.VenueId = request.VenueId;
        if (request.FacilityId.HasValue) match.FacilityId = request.FacilityId;
        if (request.HomeTeamId.HasValue) match.HomeTeamId = request.HomeTeamId;
        if (request.AwayTeamId.HasValue) match.AwayTeamId = request.AwayTeamId;
        if (request.MatchNumber != null) match.MatchNumber = request.MatchNumber;
        if (request.Status.HasValue) match.Status = request.Status.Value;
        if (request.ScheduledDateTime.HasValue) match.ScheduledDateTime = request.ScheduledDateTime;
        if (request.ActualStartTime.HasValue) match.ActualStartTime = request.ActualStartTime;
        if (request.ActualEndTime.HasValue) match.ActualEndTime = request.ActualEndTime;
        if (request.RefereeName != null) match.RefereeName = request.RefereeName;
        if (request.RefereeId.HasValue) match.RefereeId = request.RefereeId;
        if (request.Attendance.HasValue) match.Attendance = request.Attendance;
        if (request.Weather != null) match.Weather = request.Weather;
        if (request.PitchCondition != null) match.PitchCondition = request.PitchCondition;
        if (request.Notes != null) match.Notes = request.Notes;

        await _context.SaveChangesAsync();
        return MapToMatchDto(match);
    }

    public async Task<MatchDto?> RecordMatchResultAsync(Guid clubId, Guid competitionId, Guid matchId, MatchResultRequest request)
    {
        // Verify competition belongs to club
        var competition = await _context.Competitions.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (competition == null) return null;

        var match = await _context.Matches.IgnoreQueryFilters()
            .Include(m => m.Competition)
            .Include(m => m.Round)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Venue)
            .FirstOrDefaultAsync(m => m.CompetitionId == competitionId && m.Id == matchId);

        if (match == null) return null;

        match.HomeScore = request.HomeScore;
        match.AwayScore = request.AwayScore;
        match.HomeHalfTimeScore = request.HomeHalfTimeScore;
        match.AwayHalfTimeScore = request.AwayHalfTimeScore;
        match.HomeExtraTimeScore = request.HomeExtraTimeScore;
        match.AwayExtraTimeScore = request.AwayExtraTimeScore;
        match.HomePenaltyScore = request.HomePenaltyScore;
        match.AwayPenaltyScore = request.AwayPenaltyScore;
        match.MatchReport = request.MatchReport;
        match.Status = MatchStatus.Completed;

        // Determine result
        if (request.HomeScore > request.AwayScore)
            match.Result = MatchResult.HomeWin;
        else if (request.AwayScore > request.HomeScore)
            match.Result = MatchResult.AwayWin;
        else
            match.Result = MatchResult.Draw;

        // Update team standings
        await UpdateTeamStatsAsync(match, competition);

        await _context.SaveChangesAsync();
        return MapToMatchDto(match);
    }

    public async Task<bool> PostponeMatchAsync(Guid clubId, Guid competitionId, Guid matchId, MatchPostponeRequest request)
    {
        // Verify competition belongs to club
        var competitionExists = await _context.Competitions.IgnoreQueryFilters()
            .AnyAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (!competitionExists) return false;

        var match = await _context.Matches.IgnoreQueryFilters()
            .FirstOrDefaultAsync(m => m.CompetitionId == competitionId && m.Id == matchId);

        if (match == null) return false;

        match.IsPostponed = true;
        match.Status = MatchStatus.Postponed;
        match.PostponementReason = request.Reason;
        if (request.NewDateTime.HasValue)
            match.ScheduledDateTime = request.NewDateTime;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelMatchAsync(Guid clubId, Guid competitionId, Guid matchId, string? reason)
    {
        // Verify competition belongs to club
        var competitionExists = await _context.Competitions.IgnoreQueryFilters()
            .AnyAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (!competitionExists) return false;

        var match = await _context.Matches.IgnoreQueryFilters()
            .FirstOrDefaultAsync(m => m.CompetitionId == competitionId && m.Id == matchId);

        if (match == null) return false;

        match.IsCancelled = true;
        match.Status = MatchStatus.Cancelled;
        match.CancellationReason = reason;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteMatchAsync(Guid clubId, Guid competitionId, Guid matchId)
    {
        // Verify competition belongs to club
        var competitionExists = await _context.Competitions.IgnoreQueryFilters()
            .AnyAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (!competitionExists) return false;

        var match = await _context.Matches.IgnoreQueryFilters()
            .FirstOrDefaultAsync(m => m.CompetitionId == competitionId && m.Id == matchId);

        if (match == null) return false;

        _context.Matches.Remove(match);
        await _context.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Match Events

    public async Task<IEnumerable<MatchEventDto>> GetMatchEventsAsync(Guid clubId, Guid competitionId, Guid matchId)
    {
        // Verify competition belongs to club
        var competitionExists = await _context.Competitions.IgnoreQueryFilters()
            .AnyAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (!competitionExists) return Enumerable.Empty<MatchEventDto>();

        return await _context.MatchEvents.IgnoreQueryFilters()
            .Where(e => e.MatchId == matchId)
            .Include(e => e.Participant)
            .ThenInclude(p => p!.Member)
            .Include(e => e.Team)
            .Include(e => e.AssistByParticipant)
            .ThenInclude(a => a!.Member)
            .OrderBy(e => e.Minute)
            .ThenBy(e => e.AdditionalMinutes)
            .Select(e => new MatchEventDto(
                e.Id,
                e.MatchId,
                e.EventType,
                e.Minute,
                e.AdditionalMinutes,
                e.Period,
                e.ParticipantId,
                e.Participant != null && e.Participant.Member != null ? e.Participant.Member.FullName : null,
                e.Participant != null ? e.Participant.SquadNumber : null,
                e.TeamId,
                e.Team != null ? e.Team.Name : null,
                e.AssistByParticipantId,
                e.AssistByParticipant != null && e.AssistByParticipant.Member != null ? e.AssistByParticipant.Member.FullName : null,
                e.Description
            ))
            .ToListAsync();
    }

    public async Task<MatchEventDto> AddMatchEventAsync(Guid clubId, Guid competitionId, Guid matchId, MatchEventCreateRequest request)
    {
        // Verify competition belongs to club
        var competition = await _context.Competitions.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (competition == null) throw new InvalidOperationException("Competition not found");

        var matchEvent = new MatchEvent
        {
            Id = Guid.NewGuid(),
            MatchId = matchId,
            EventType = request.EventType,
            Minute = request.Minute,
            AdditionalMinutes = request.AdditionalMinutes,
            Period = request.Period,
            ParticipantId = request.ParticipantId,
            TeamId = request.TeamId,
            AssistByParticipantId = request.AssistByParticipantId,
            SubstitutedForParticipantId = request.SubstitutedForParticipantId,
            Description = request.Description
        };

        _context.MatchEvents.Add(matchEvent);

        // Update participant stats based on event type
        if (request.ParticipantId.HasValue)
        {
            var participant = await _context.CompetitionParticipants.FindAsync(request.ParticipantId.Value);
            if (participant != null)
            {
                switch (request.EventType.ToLower())
                {
                    case "goal":
                        participant.Goals++;
                        break;
                    case "yellowcard":
                        participant.YellowCards++;
                        break;
                    case "redcard":
                        participant.RedCards++;
                        break;
                }
            }
        }

        if (request.AssistByParticipantId.HasValue && request.EventType.ToLower() == "goal")
        {
            var assister = await _context.CompetitionParticipants.FindAsync(request.AssistByParticipantId.Value);
            if (assister != null)
                assister.Assists++;
        }

        await _context.SaveChangesAsync();

        // Reload with relationships
        matchEvent = await _context.MatchEvents
            .Include(e => e.Participant)
            .ThenInclude(p => p!.Member)
            .Include(e => e.Team)
            .Include(e => e.AssistByParticipant)
            .ThenInclude(a => a!.Member)
            .FirstAsync(e => e.Id == matchEvent.Id);

        return new MatchEventDto(
            matchEvent.Id,
            matchEvent.MatchId,
            matchEvent.EventType,
            matchEvent.Minute,
            matchEvent.AdditionalMinutes,
            matchEvent.Period,
            matchEvent.ParticipantId,
            matchEvent.Participant?.Member?.FullName,
            matchEvent.Participant?.SquadNumber,
            matchEvent.TeamId,
            matchEvent.Team?.Name,
            matchEvent.AssistByParticipantId,
            matchEvent.AssistByParticipant?.Member?.FullName,
            matchEvent.Description
        );
    }

    public async Task<bool> DeleteMatchEventAsync(Guid clubId, Guid competitionId, Guid matchId, Guid eventId)
    {
        // Verify competition belongs to club
        var competitionExists = await _context.Competitions.IgnoreQueryFilters()
            .AnyAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (!competitionExists) return false;

        var matchEvent = await _context.MatchEvents.IgnoreQueryFilters()
            .FirstOrDefaultAsync(e => e.MatchId == matchId && e.Id == eventId);

        if (matchEvent == null) return false;

        _context.MatchEvents.Remove(matchEvent);
        await _context.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Match Lineups

    public async Task<IEnumerable<MatchLineupDto>> GetMatchLineupsAsync(Guid clubId, Guid competitionId, Guid matchId)
    {
        // Verify competition belongs to club
        var competitionExists = await _context.Competitions.IgnoreQueryFilters()
            .AnyAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (!competitionExists) return Enumerable.Empty<MatchLineupDto>();

        return await _context.MatchLineups.IgnoreQueryFilters()
            .Where(l => l.MatchId == matchId)
            .Include(l => l.Team)
            .Include(l => l.Participant)
            .ThenInclude(p => p!.Member)
            .OrderBy(l => l.TeamId)
            .ThenByDescending(l => l.IsStarting)
            .ThenBy(l => l.PositionOrder)
            .Select(l => new MatchLineupDto(
                l.Id,
                l.MatchId,
                l.TeamId,
                l.Team != null ? l.Team.Name : "Unknown",
                l.ParticipantId,
                l.Participant != null && l.Participant.Member != null ? l.Participant.Member.FullName : "Unknown",
                l.IsStarting,
                l.ShirtNumber,
                l.Position,
                l.MinutesPlayed,
                l.SubbedOnMinute,
                l.SubbedOffMinute,
                l.Rating,
                l.IsManOfTheMatch
            ))
            .ToListAsync();
    }

    public async Task<MatchLineupDto> AddLineupPlayerAsync(Guid clubId, Guid competitionId, Guid matchId, MatchLineupCreateRequest request)
    {
        // Verify competition belongs to club
        var competition = await _context.Competitions.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (competition == null) throw new InvalidOperationException("Competition not found");

        var lineup = new MatchLineup
        {
            Id = Guid.NewGuid(),
            MatchId = matchId,
            TeamId = request.TeamId,
            ParticipantId = request.ParticipantId,
            IsStarting = request.IsStarting,
            ShirtNumber = request.ShirtNumber,
            Position = request.Position,
            PositionOrder = request.PositionOrder
        };

        _context.MatchLineups.Add(lineup);
        await _context.SaveChangesAsync();

        // Reload with relationships
        lineup = await _context.MatchLineups
            .Include(l => l.Team)
            .Include(l => l.Participant)
            .ThenInclude(p => p!.Member)
            .FirstAsync(l => l.Id == lineup.Id);

        return new MatchLineupDto(
            lineup.Id,
            lineup.MatchId,
            lineup.TeamId,
            lineup.Team?.Name ?? "Unknown",
            lineup.ParticipantId,
            lineup.Participant?.Member?.FullName ?? "Unknown",
            lineup.IsStarting,
            lineup.ShirtNumber,
            lineup.Position,
            lineup.MinutesPlayed,
            lineup.SubbedOnMinute,
            lineup.SubbedOffMinute,
            lineup.Rating,
            lineup.IsManOfTheMatch
        );
    }

    public async Task<bool> RemoveLineupPlayerAsync(Guid clubId, Guid competitionId, Guid matchId, Guid lineupId)
    {
        // Verify competition belongs to club
        var competitionExists = await _context.Competitions.IgnoreQueryFilters()
            .AnyAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (!competitionExists) return false;

        var lineup = await _context.MatchLineups.IgnoreQueryFilters()
            .FirstOrDefaultAsync(l => l.MatchId == matchId && l.Id == lineupId);

        if (lineup == null) return false;

        _context.MatchLineups.Remove(lineup);
        await _context.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Standings

    public async Task<IEnumerable<CompetitionStandingDto>> GetStandingsAsync(Guid clubId, Guid competitionId, string? group = null)
    {
        // Verify competition belongs to club
        var competitionExists = await _context.Competitions.IgnoreQueryFilters()
            .AnyAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (!competitionExists) return Enumerable.Empty<CompetitionStandingDto>();

        var query = _context.CompetitionStandings.IgnoreQueryFilters()
            .Where(s => s.CompetitionId == competitionId);

        if (!string.IsNullOrEmpty(group))
            query = query.Where(s => s.Group == group);

        return await query
            .Include(s => s.Team)
            .OrderBy(s => s.Group)
            .ThenBy(s => s.Position)
            .Select(s => new CompetitionStandingDto(
                s.Id,
                s.CompetitionId,
                s.TeamId,
                s.Team != null ? s.Team.Name : "Unknown",
                s.Team != null ? s.Team.LogoUrl : null,
                s.Position,
                s.PreviousPosition,
                s.Group,
                s.Played,
                s.Won,
                s.Drawn,
                s.Lost,
                s.GoalsFor,
                s.GoalsAgainst,
                s.GoalDifference,
                s.Points,
                s.BonusPoints,
                s.Points + s.BonusPoints,
                s.Form,
                s.Zone,
                s.IsPromoted,
                s.IsRelegated
            ))
            .ToListAsync();
    }

    public async Task RecalculateStandingsAsync(Guid clubId, Guid competitionId)
    {
        var competition = await _context.Competitions.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.ClubId == clubId && c.Id == competitionId);

        if (competition == null) return;

        // Get all teams
        var teams = await _context.CompetitionTeams.IgnoreQueryFilters()
            .Where(t => t.ClubId == clubId && t.CompetitionId == competitionId)
            .ToListAsync();

        // Get all completed matches
        var matches = await _context.Matches.IgnoreQueryFilters()
            .Where(m => m.CompetitionId == competitionId && m.Status == MatchStatus.Completed)
            .ToListAsync();

        // Delete existing standings
        var existingStandings = await _context.CompetitionStandings.IgnoreQueryFilters()
            .Where(s => s.CompetitionId == competitionId)
            .ToListAsync();
        _context.CompetitionStandings.RemoveRange(existingStandings);

        // Calculate new standings per group
        var teamsByGroup = teams.GroupBy(t => t.Group ?? "");

        foreach (var group in teamsByGroup)
        {
            var groupTeams = group.ToList();
            var standings = new List<CompetitionStanding>();

            foreach (var team in groupTeams)
            {
                var teamMatches = matches.Where(m => m.HomeTeamId == team.Id || m.AwayTeamId == team.Id).ToList();

                var standing = new CompetitionStanding
                {
                    Id = Guid.NewGuid(),
                    CompetitionId = competitionId,
                    TeamId = team.Id,
                    Group = team.Group,
                    Played = teamMatches.Count,
                    Won = teamMatches.Count(m =>
                        (m.HomeTeamId == team.Id && m.Result == MatchResult.HomeWin) ||
                        (m.AwayTeamId == team.Id && m.Result == MatchResult.AwayWin)),
                    Drawn = teamMatches.Count(m => m.Result == MatchResult.Draw),
                    Lost = teamMatches.Count(m =>
                        (m.HomeTeamId == team.Id && m.Result == MatchResult.AwayWin) ||
                        (m.AwayTeamId == team.Id && m.Result == MatchResult.HomeWin)),
                    GoalsFor = teamMatches.Sum(m =>
                        m.HomeTeamId == team.Id ? (m.HomeScore ?? 0) : (m.AwayScore ?? 0)),
                    GoalsAgainst = teamMatches.Sum(m =>
                        m.HomeTeamId == team.Id ? (m.AwayScore ?? 0) : (m.HomeScore ?? 0))
                };

                standing.GoalDifference = standing.GoalsFor - standing.GoalsAgainst;
                standing.Points = (standing.Won * competition.PointsForWin) +
                                 (standing.Drawn * competition.PointsForDraw) +
                                 (standing.Lost * competition.PointsForLoss);

                // Calculate form (last 5 matches)
                var recentMatches = teamMatches.OrderByDescending(m => m.ScheduledDateTime).Take(5);
                standing.Form = string.Join("", recentMatches.Select(m =>
                {
                    if ((m.HomeTeamId == team.Id && m.Result == MatchResult.HomeWin) ||
                        (m.AwayTeamId == team.Id && m.Result == MatchResult.AwayWin))
                        return "W";
                    if (m.Result == MatchResult.Draw)
                        return "D";
                    return "L";
                }));

                standings.Add(standing);

                // Update team stats
                team.Played = standing.Played;
                team.Won = standing.Won;
                team.Drawn = standing.Drawn;
                team.Lost = standing.Lost;
                team.GoalsFor = standing.GoalsFor;
                team.GoalsAgainst = standing.GoalsAgainst;
                team.GoalDifference = standing.GoalDifference;
                team.Points = standing.Points;
            }

            // Sort and assign positions
            var sortedStandings = standings
                .OrderByDescending(s => s.Points)
                .ThenByDescending(s => s.GoalDifference)
                .ThenByDescending(s => s.GoalsFor)
                .ThenBy(s => s.Team?.Name)
                .ToList();

            for (int i = 0; i < sortedStandings.Count; i++)
            {
                sortedStandings[i].Position = i + 1;
                var team = groupTeams.First(t => t.Id == sortedStandings[i].TeamId);
                team.Position = i + 1;
            }

            _context.CompetitionStandings.AddRange(sortedStandings);
        }

        await _context.SaveChangesAsync();
    }

    #endregion

    #region Top Scorers

    public async Task<IEnumerable<TopScorerDto>> GetTopScorersAsync(Guid clubId, Guid competitionId, int limit = 10)
    {
        // Get all goal events for matches in this competition
        var goalEvents = await _context.MatchEvents.IgnoreQueryFilters()
            .Include(e => e.Match)
            .Include(e => e.Team)
            .Where(e => e.Match != null &&
                   e.Match.CompetitionId == competitionId &&
                   (e.EventType.ToLower() == "goal" || e.EventType.ToLower() == "penalty"))
            .ToListAsync();

        // Group by player name (from description field) and team
        var scorerStats = goalEvents
            .GroupBy(e => new {
                PlayerName = ExtractPlayerName(e.Description ?? "Unknown"),
                e.TeamId,
                TeamName = e.Team?.Name,
                TeamLogo = e.Team?.LogoUrl
            })
            .Select(g => new {
                g.Key.PlayerName,
                g.Key.TeamId,
                g.Key.TeamName,
                g.Key.TeamLogo,
                Goals = g.Count(e => e.EventType.ToLower() == "goal"),
                Penalties = g.Count(e => e.EventType.ToLower() == "penalty"),
                // Count assists from description containing "assist:"
                Assists = 0, // Assists are tracked differently - from assist mentions
                Appearances = g.Select(e => e.MatchId).Distinct().Count()
            })
            .OrderByDescending(s => s.Goals + s.Penalties)
            .ThenByDescending(s => s.Appearances)
            .Take(limit)
            .ToList();

        // Also count assists from events where this player assisted
        var assistCounts = goalEvents
            .Where(e => !string.IsNullOrEmpty(e.Description) && e.Description.Contains("assist:"))
            .Select(e => ExtractAssistName(e.Description!))
            .Where(name => !string.IsNullOrEmpty(name))
            .GroupBy(name => name)
            .ToDictionary(g => g.Key!, g => g.Count());

        var rank = 1;
        return scorerStats.Select(s => new TopScorerDto(
            rank++,
            s.PlayerName,
            s.TeamId,
            s.TeamName,
            s.TeamLogo,
            s.Goals,
            assistCounts.GetValueOrDefault(s.PlayerName, 0),
            s.Penalties,
            s.Appearances
        )).ToList();
    }

    private static string ExtractPlayerName(string description)
    {
        // Extract player name from description like "John Smith" or "John Smith (assist: Mike)"
        if (string.IsNullOrEmpty(description)) return "Unknown";
        var parenIndex = description.IndexOf('(');
        if (parenIndex > 0)
            return description.Substring(0, parenIndex).Trim();
        return description.Trim();
    }

    private static string? ExtractAssistName(string description)
    {
        // Extract assist name from "Player (assist: AssistPlayer)"
        var match = System.Text.RegularExpressions.Regex.Match(description, @"assist:\s*([^)]+)");
        return match.Success ? match.Groups[1].Value.Trim() : null;
    }

    #endregion

    #region Fixtures Generation

    public async Task<IEnumerable<MatchDto>> GenerateFixturesAsync(Guid clubId, Guid competitionId, GenerateFixturesRequest request)
    {
        var competition = await _context.Competitions.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.ClubId == clubId && c.Id == competitionId);
        if (competition == null) throw new InvalidOperationException("Competition not found");

        var teams = await _context.CompetitionTeams.IgnoreQueryFilters()
            .Where(t => t.ClubId == clubId && t.CompetitionId == competitionId && t.Status == TeamStatus.Confirmed)
            .ToListAsync();

        if (teams.Count < 2)
            throw new InvalidOperationException("At least 2 teams are required to generate fixtures");

        if (request.RandomizeOrder)
            teams = teams.OrderBy(_ => Guid.NewGuid()).ToList();

        // Add a bye team if odd number
        var teamIds = teams.Select(t => t.Id).ToList();
        if (teamIds.Count % 2 != 0)
            teamIds.Add(Guid.Empty); // Bye

        var n = teamIds.Count;
        var totalRounds = n - 1;
        var matchesPerRound = n / 2;
        var matches = new List<Match>();
        var startDate = request.StartDate ?? DateTime.UtcNow.Date;
        var roundNumber = 0;

        // Round-robin algorithm
        for (int round = 0; round < totalRounds; round++)
        {
            roundNumber++;
            var roundDate = startDate.AddDays(round * request.DaysBetweenRounds);

            // Create round
            var competitionRound = new CompetitionRound
            {
                Id = Guid.NewGuid(),
                CompetitionId = competitionId,
                RoundNumber = roundNumber,
                Name = $"Round {roundNumber}",
                StartDate = roundDate
            };
            _context.CompetitionRounds.Add(competitionRound);

            for (int match = 0; match < matchesPerRound; match++)
            {
                var home = teamIds[match];
                var away = teamIds[n - 1 - match];

                // Skip bye matches
                if (home == Guid.Empty || away == Guid.Empty)
                    continue;

                var newMatch = new Match
                {
                    Id = Guid.NewGuid(),
                    CompetitionId = competitionId,
                    RoundId = competitionRound.Id,
                    HomeTeamId = home,
                    AwayTeamId = away,
                    MatchNumber = $"R{roundNumber}M{match + 1}",
                    ScheduledDateTime = roundDate,
                    Status = MatchStatus.Scheduled,
                    Result = MatchResult.NotPlayed
                };
                matches.Add(newMatch);
            }

            // Rotate teams (keep first team fixed)
            var last = teamIds[n - 1];
            for (int i = n - 1; i > 1; i--)
                teamIds[i] = teamIds[i - 1];
            teamIds[1] = last;
        }

        // Generate return fixtures if home and away
        if (request.HomeAndAway)
        {
            var returnMatches = new List<Match>();
            foreach (var match in matches)
            {
                roundNumber++;
                var returnRoundDate = startDate.AddDays((totalRounds + matches.IndexOf(match) / matchesPerRound) * request.DaysBetweenRounds);

                var returnMatch = new Match
                {
                    Id = Guid.NewGuid(),
                    CompetitionId = competitionId,
                    HomeTeamId = match.AwayTeamId,
                    AwayTeamId = match.HomeTeamId,
                    MatchNumber = $"R{totalRounds + 1 + matches.IndexOf(match) / matchesPerRound}M{matches.IndexOf(match) % matchesPerRound + 1}",
                    ScheduledDateTime = returnRoundDate,
                    Status = MatchStatus.Scheduled,
                    Result = MatchResult.NotPlayed,
                    LegNumber = 2
                };
                returnMatches.Add(returnMatch);
            }
            matches.AddRange(returnMatches);
        }

        _context.Matches.AddRange(matches);
        await _context.SaveChangesAsync();

        // Reload with relationships
        var matchIds = matches.Select(m => m.Id).ToList();
        var loadedMatches = await _context.Matches
            .Include(m => m.Competition)
            .Include(m => m.Round)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Venue)
            .Where(m => matchIds.Contains(m.Id))
            .ToListAsync();

        return loadedMatches.Select(MapToMatchDto);
    }

    public async Task<bool> PerformDrawAsync(Guid clubId, Guid competitionId, DrawRequest request)
    {
        var teams = await _context.CompetitionTeams.IgnoreQueryFilters()
            .Where(t => t.ClubId == clubId && t.CompetitionId == competitionId && t.Status == TeamStatus.Confirmed)
            .ToListAsync();

        if (teams.Count < 2)
            return false;

        if (request.RandomDraw)
        {
            teams = teams.OrderBy(_ => Guid.NewGuid()).ToList();
        }
        else if (request.SeedTeams)
        {
            teams = teams.OrderBy(t => t.SeedNumber ?? int.MaxValue).ToList();
        }

        // Assign draw positions
        for (int i = 0; i < teams.Count; i++)
        {
            teams[i].DrawPosition = i + 1;
        }

        // Update competition status
        var competition = await _context.Competitions.FindAsync(competitionId);
        if (competition != null)
        {
            competition.Status = CompetitionStatus.DrawComplete;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    #endregion

    #region Private Helpers

    private static CompetitionDto MapToCompetitionDto(Competition c) => new(
        c.Id,
        c.Name,
        c.Code,
        c.Description,
        c.Type,
        c.Status,
        c.Sport,
        c.Category,
        c.Division,
        c.SkillLevel,
        c.AgeGroup,
        c.StartDate,
        c.EndDate,
        c.RegistrationOpenDate,
        c.RegistrationCloseDate,
        c.Format,
        c.NumberOfRounds,
        c.IsTeamBased,
        c.MinTeams,
        c.MaxTeams,
        c.Teams?.Count ?? 0,
        c.MinPlayersPerTeam,
        c.MaxPlayersPerTeam,
        c.EntryFee,
        c.Currency ?? "USD",
        c.HasPrizes,
        c.PrizeDescription,
        c.TotalPrizeMoney,
        c.PointsForWin,
        c.PointsForDraw,
        c.PointsForLoss,
        c.OrganizerName,
        c.ContactEmail,
        c.ImageUrl,
        c.IsPublished,
        c.Matches?.Count ?? 0,
        c.Matches?.Count(m => m.Status == MatchStatus.Completed) ?? 0,
        c.Season == null ? null : new SeasonDto(c.Season.Id, c.Season.Name, c.Season.StartDate, c.Season.EndDate, c.Season.IsCurrent, c.Season.IsCompleted, 0),
        c.Venue == null ? null : new VenueDto(c.Venue.Id, c.Venue.Name, c.Venue.Description, c.Venue.AddressLine1, c.Venue.PostCode, c.Venue.Latitude, c.Venue.Longitude, c.Venue.TotalCapacity, c.Venue.AdditionalAmenities, c.Venue.ImageUrl, c.Venue.IsActive, c.Venue.IsPrimary, 0, 0)
    );

    private static CompetitionTeamDto MapToTeamDto(CompetitionTeam t) => new(
        t.Id,
        t.CompetitionId,
        t.Name,
        t.ShortName,
        t.Code,
        t.Status,
        t.SeedNumber,
        t.DrawPosition,
        t.Group,
        t.CaptainId,
        t.CaptainName,
        t.ContactEmail,
        t.IsApproved,
        t.EntryFeePaid,
        t.Played,
        t.Won,
        t.Drawn,
        t.Lost,
        t.GoalsFor,
        t.GoalsAgainst,
        t.GoalDifference,
        t.Points,
        t.Position,
        t.LogoUrl,
        t.Participants?.Count ?? 0
    );

    private static MatchDto MapToMatchDto(Match m) => new(
        m.Id,
        m.CompetitionId,
        m.Competition?.Name ?? "Unknown",
        m.RoundId,
        m.Round?.Name,
        m.MatchNumber,
        m.LegNumber,
        m.Status,
        m.ScheduledDateTime,
        m.ActualStartTime,
        m.ActualEndTime,
        m.HomeTeamId,
        m.HomeTeam?.Name,
        m.HomeTeam?.LogoUrl,
        m.AwayTeamId,
        m.AwayTeam?.Name,
        m.AwayTeam?.LogoUrl,
        m.Result,
        m.HomeScore,
        m.AwayScore,
        m.HomeHalfTimeScore,
        m.AwayHalfTimeScore,
        m.RefereeName,
        m.Attendance,
        m.IsPostponed,
        m.IsCancelled,
        m.HighlightsUrl,
        m.Venue == null ? null : new VenueDto(m.Venue.Id, m.Venue.Name, m.Venue.Description, m.Venue.AddressLine1, m.Venue.PostCode, m.Venue.Latitude, m.Venue.Longitude, m.Venue.TotalCapacity, m.Venue.AdditionalAmenities, m.Venue.ImageUrl, m.Venue.IsActive, m.Venue.IsPrimary, 0, 0)
    );

    private async Task UpdateTeamStatsAsync(Match match, Competition competition)
    {
        if (match.HomeTeamId.HasValue)
        {
            var homeTeam = await _context.CompetitionTeams.FindAsync(match.HomeTeamId.Value);
            if (homeTeam != null)
            {
                homeTeam.Played++;
                homeTeam.GoalsFor += match.HomeScore ?? 0;
                homeTeam.GoalsAgainst += match.AwayScore ?? 0;
                homeTeam.GoalDifference = homeTeam.GoalsFor - homeTeam.GoalsAgainst;

                if (match.Result == MatchResult.HomeWin)
                {
                    homeTeam.Won++;
                    homeTeam.Points += competition.PointsForWin;
                }
                else if (match.Result == MatchResult.Draw)
                {
                    homeTeam.Drawn++;
                    homeTeam.Points += competition.PointsForDraw;
                }
                else if (match.Result == MatchResult.AwayWin)
                {
                    homeTeam.Lost++;
                    homeTeam.Points += competition.PointsForLoss;
                }
            }
        }

        if (match.AwayTeamId.HasValue)
        {
            var awayTeam = await _context.CompetitionTeams.FindAsync(match.AwayTeamId.Value);
            if (awayTeam != null)
            {
                awayTeam.Played++;
                awayTeam.GoalsFor += match.AwayScore ?? 0;
                awayTeam.GoalsAgainst += match.HomeScore ?? 0;
                awayTeam.GoalDifference = awayTeam.GoalsFor - awayTeam.GoalsAgainst;

                if (match.Result == MatchResult.AwayWin)
                {
                    awayTeam.Won++;
                    awayTeam.Points += competition.PointsForWin;
                }
                else if (match.Result == MatchResult.Draw)
                {
                    awayTeam.Drawn++;
                    awayTeam.Points += competition.PointsForDraw;
                }
                else if (match.Result == MatchResult.HomeWin)
                {
                    awayTeam.Lost++;
                    awayTeam.Points += competition.PointsForLoss;
                }
            }
        }
    }

    #endregion
}
