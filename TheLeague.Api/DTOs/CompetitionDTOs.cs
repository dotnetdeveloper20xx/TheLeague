using System.ComponentModel.DataAnnotations;
using TheLeague.Core.Enums;

namespace TheLeague.Api.DTOs;

// Season DTOs
public record SeasonDto(
    Guid Id,
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    bool IsCurrent,
    bool IsCompleted,
    int CompetitionCount
);

public record SeasonCreateRequest(
    [Required] string Name,
    [Required] DateTime StartDate,
    [Required] DateTime EndDate,
    bool IsCurrent = false
);

public record SeasonUpdateRequest(
    string? Name,
    DateTime? StartDate,
    DateTime? EndDate,
    bool? IsCurrent,
    bool? IsCompleted
);

// Competition DTOs
public record CompetitionDto(
    Guid Id,
    string Name,
    string? Code,
    string? Description,
    CompetitionType Type,
    CompetitionStatus Status,
    string? Sport,
    string? Category,
    string? Division,
    SkillLevel SkillLevel,
    AgeGroup AgeGroup,
    DateTime? StartDate,
    DateTime? EndDate,
    DateTime? RegistrationOpenDate,
    DateTime? RegistrationCloseDate,
    string? Format,
    int? NumberOfRounds,
    bool IsTeamBased,
    int? MinTeams,
    int? MaxTeams,
    int CurrentTeamCount,
    int? MinPlayersPerTeam,
    int? MaxPlayersPerTeam,
    decimal? EntryFee,
    string Currency,
    bool HasPrizes,
    string? PrizeDescription,
    decimal? TotalPrizeMoney,
    int PointsForWin,
    int PointsForDraw,
    int PointsForLoss,
    string? OrganizerName,
    string? ContactEmail,
    string? ImageUrl,
    bool IsPublished,
    int TotalMatches,
    int CompletedMatches,
    SeasonDto? Season,
    VenueDto? Venue
);

public record CompetitionListDto(
    Guid Id,
    string Name,
    string? Code,
    CompetitionType Type,
    CompetitionStatus Status,
    string? Sport,
    DateTime? StartDate,
    DateTime? EndDate,
    int CurrentTeamCount,
    int? MaxTeams,
    int TotalMatches,
    int CompletedMatches,
    bool IsPublished,
    string? ImageUrl
);

public record CompetitionCreateRequest(
    [Required] string Name,
    string? Code,
    string? Description,
    [Required] CompetitionType Type,
    string? Sport,
    string? Category,
    string? Division,
    SkillLevel SkillLevel = SkillLevel.AllLevels,
    AgeGroup AgeGroup = AgeGroup.AllAges,
    int? MinAge = null,
    int? MaxAge = null,
    Gender? TargetGender = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    DateTime? RegistrationOpenDate = null,
    DateTime? RegistrationCloseDate = null,
    Guid? SeasonId = null,
    Guid? VenueId = null,
    string? Format = null,
    int? NumberOfRounds = null,
    bool HomeAndAway = false,
    bool IsTeamBased = true,
    int? MinTeams = null,
    int? MaxTeams = null,
    int? MinPlayersPerTeam = null,
    int? MaxPlayersPerTeam = null,
    decimal? EntryFee = null,
    decimal? TeamEntryFee = null,
    decimal? PlayerEntryFee = null,
    bool HasPrizes = false,
    string? PrizeDescription = null,
    decimal? TotalPrizeMoney = null,
    int PointsForWin = 3,
    int PointsForDraw = 1,
    int PointsForLoss = 0,
    string? Rules = null,
    string? OrganizerName = null,
    string? ContactEmail = null,
    string? ContactPhone = null,
    string? ImageUrl = null,
    bool IsPublished = false
);

public record CompetitionUpdateRequest(
    string? Name,
    string? Code,
    string? Description,
    CompetitionType? Type,
    CompetitionStatus? Status,
    string? Sport,
    string? Category,
    string? Division,
    SkillLevel? SkillLevel,
    AgeGroup? AgeGroup,
    DateTime? StartDate,
    DateTime? EndDate,
    DateTime? RegistrationOpenDate,
    DateTime? RegistrationCloseDate,
    Guid? SeasonId,
    Guid? VenueId,
    string? Format,
    int? NumberOfRounds,
    bool? HomeAndAway,
    int? MinTeams,
    int? MaxTeams,
    int? MinPlayersPerTeam,
    int? MaxPlayersPerTeam,
    decimal? EntryFee,
    bool? HasPrizes,
    string? PrizeDescription,
    decimal? TotalPrizeMoney,
    int? PointsForWin,
    int? PointsForDraw,
    int? PointsForLoss,
    string? Rules,
    string? OrganizerName,
    string? ContactEmail,
    string? ContactPhone,
    string? ImageUrl,
    bool? IsPublished
);

// Competition Round DTOs
public record CompetitionRoundDto(
    Guid Id,
    int RoundNumber,
    string Name,
    DateTime? StartDate,
    DateTime? EndDate,
    bool IsComplete,
    int MatchCount,
    int CompletedMatchCount
);

public record CompetitionRoundCreateRequest(
    [Required] int RoundNumber,
    [Required] string Name,
    DateTime? StartDate = null,
    DateTime? EndDate = null
);

// Competition Team DTOs
public record CompetitionTeamDto(
    Guid Id,
    Guid CompetitionId,
    string Name,
    string? ShortName,
    string? Code,
    TeamStatus Status,
    int? SeedNumber,
    int? DrawPosition,
    string? Group,
    Guid? CaptainId,
    string? CaptainName,
    string? ContactEmail,
    bool IsApproved,
    bool EntryFeePaid,
    int Played,
    int Won,
    int Drawn,
    int Lost,
    int GoalsFor,
    int GoalsAgainst,
    int GoalDifference,
    int Points,
    int Position,
    string? LogoUrl,
    int ParticipantCount
);

public record CompetitionTeamListDto(
    Guid Id,
    string Name,
    string? ShortName,
    string? Code,
    TeamStatus Status,
    string? Group,
    int Played,
    int Won,
    int Drawn,
    int Lost,
    int GoalDifference,
    int Points,
    int Position,
    string? LogoUrl
);

public record CompetitionTeamCreateRequest(
    [Required] string Name,
    string? ShortName = null,
    string? Code = null,
    int? SeedNumber = null,
    string? Group = null,
    Guid? CaptainId = null,
    string? CaptainName = null,
    string? ContactEmail = null,
    string? ContactPhone = null,
    Guid? HomeVenueId = null,
    string? HomeColors = null,
    string? AwayColors = null,
    string? LogoUrl = null
);

public record CompetitionTeamUpdateRequest(
    string? Name,
    string? ShortName,
    string? Code,
    TeamStatus? Status,
    int? SeedNumber,
    int? DrawPosition,
    string? Group,
    Guid? CaptainId,
    string? CaptainName,
    string? ContactEmail,
    string? ContactPhone,
    bool? IsApproved,
    bool? EntryFeePaid,
    string? HomeColors,
    string? AwayColors,
    string? LogoUrl
);

// Competition Participant DTOs
public record CompetitionParticipantDto(
    Guid Id,
    Guid TeamId,
    Guid MemberId,
    string MemberName,
    ParticipantRole Role,
    bool IsActive,
    int? SquadNumber,
    string? Position,
    bool IsEligible,
    int Appearances,
    int Goals,
    int Assists,
    int YellowCards,
    int RedCards,
    int MinutesPlayed
);

public record CompetitionParticipantCreateRequest(
    [Required] Guid MemberId,
    ParticipantRole Role = ParticipantRole.Player,
    int? SquadNumber = null,
    string? Position = null
);

public record CompetitionParticipantUpdateRequest(
    ParticipantRole? Role,
    bool? IsActive,
    int? SquadNumber,
    string? Position,
    bool? IsEligible,
    string? EligibilityNotes
);

// Match DTOs
public record MatchDto(
    Guid Id,
    Guid CompetitionId,
    string CompetitionName,
    Guid? RoundId,
    string? RoundName,
    string? MatchNumber,
    int? LegNumber,
    MatchStatus Status,
    DateTime? ScheduledDateTime,
    DateTime? ActualStartTime,
    DateTime? ActualEndTime,
    Guid? HomeTeamId,
    string? HomeTeamName,
    string? HomeTeamLogo,
    Guid? AwayTeamId,
    string? AwayTeamName,
    string? AwayTeamLogo,
    MatchResult Result,
    int? HomeScore,
    int? AwayScore,
    int? HomeHalfTimeScore,
    int? AwayHalfTimeScore,
    string? RefereeName,
    int? Attendance,
    bool IsPostponed,
    bool IsCancelled,
    string? HighlightsUrl,
    VenueDto? Venue
);

public record MatchListDto(
    Guid Id,
    string? MatchNumber,
    MatchStatus Status,
    DateTime? ScheduledDateTime,
    Guid? HomeTeamId,
    string? HomeTeamName,
    Guid? AwayTeamId,
    string? AwayTeamName,
    MatchResult Result,
    int? HomeScore,
    int? AwayScore,
    string? RoundName
);

public record MatchCreateRequest(
    Guid? RoundId = null,
    Guid? VenueId = null,
    Guid? FacilityId = null,
    Guid? HomeTeamId = null,
    Guid? AwayTeamId = null,
    string? MatchNumber = null,
    int? LegNumber = null,
    DateTime? ScheduledDateTime = null,
    string? RefereeName = null,
    Guid? RefereeId = null
);

public record MatchUpdateRequest(
    Guid? RoundId,
    Guid? VenueId,
    Guid? FacilityId,
    Guid? HomeTeamId,
    Guid? AwayTeamId,
    string? MatchNumber,
    MatchStatus? Status,
    DateTime? ScheduledDateTime,
    DateTime? ActualStartTime,
    DateTime? ActualEndTime,
    string? RefereeName,
    Guid? RefereeId,
    int? Attendance,
    string? Weather,
    string? PitchCondition,
    string? Notes
);

public record MatchResultRequest(
    [Required] int HomeScore,
    [Required] int AwayScore,
    int? HomeHalfTimeScore = null,
    int? AwayHalfTimeScore = null,
    int? HomeExtraTimeScore = null,
    int? AwayExtraTimeScore = null,
    int? HomePenaltyScore = null,
    int? AwayPenaltyScore = null,
    string? MatchReport = null
);

public record MatchPostponeRequest(
    [Required] string Reason,
    DateTime? NewDateTime = null
);

// Match Event DTOs
public record MatchEventDto(
    Guid Id,
    Guid MatchId,
    string EventType,
    int Minute,
    int? AdditionalMinutes,
    string? Period,
    Guid? ParticipantId,
    string? ParticipantName,
    int? ParticipantNumber,
    Guid? TeamId,
    string? TeamName,
    Guid? AssistByParticipantId,
    string? AssistByName,
    string? Description
);

public record MatchEventCreateRequest(
    [Required] string EventType,
    [Required] int Minute,
    int? AdditionalMinutes = null,
    string? Period = null,
    Guid? ParticipantId = null,
    Guid? TeamId = null,
    Guid? AssistByParticipantId = null,
    Guid? SubstitutedForParticipantId = null,
    string? Description = null
);

// Match Lineup DTOs
public record MatchLineupDto(
    Guid Id,
    Guid MatchId,
    Guid TeamId,
    string TeamName,
    Guid ParticipantId,
    string ParticipantName,
    bool IsStarting,
    int? ShirtNumber,
    string? Position,
    int? MinutesPlayed,
    int? SubbedOnMinute,
    int? SubbedOffMinute,
    int? Rating,
    bool IsManOfTheMatch
);

public record MatchLineupCreateRequest(
    [Required] Guid TeamId,
    [Required] Guid ParticipantId,
    bool IsStarting = true,
    int? ShirtNumber = null,
    string? Position = null,
    int? PositionOrder = null
);

// Competition Standing DTOs
public record CompetitionStandingDto(
    Guid Id,
    Guid CompetitionId,
    Guid TeamId,
    string TeamName,
    string? TeamLogo,
    int Position,
    int? PreviousPosition,
    string? Group,
    int Played,
    int Won,
    int Drawn,
    int Lost,
    int GoalsFor,
    int GoalsAgainst,
    int GoalDifference,
    int Points,
    int BonusPoints,
    int TotalPoints,
    string? Form,
    string? Zone,
    bool IsPromoted,
    bool IsRelegated
);

// Filter requests
public record CompetitionFilterRequest(
    CompetitionType? Type = null,
    CompetitionStatus? Status = null,
    string? Sport = null,
    Guid? SeasonId = null,
    bool? IncludeUnpublished = null,
    int Page = 1,
    int PageSize = 20
);

public record MatchFilterRequest(
    Guid? RoundId = null,
    Guid? TeamId = null,
    MatchStatus? Status = null,
    DateTime? DateFrom = null,
    DateTime? DateTo = null,
    int Page = 1,
    int PageSize = 20
);

// Fixture generation
public record GenerateFixturesRequest(
    bool HomeAndAway = true,
    bool RandomizeOrder = true,
    DateTime? StartDate = null,
    int DaysBetweenRounds = 7
);

// Draw/Bracket
public record DrawRequest(
    bool RandomDraw = true,
    bool SeedTeams = false
);

// Top Scorers
public record TopScorerDto(
    int Rank,
    string PlayerName,
    Guid? TeamId,
    string? TeamName,
    string? TeamLogo,
    int Goals,
    int Assists,
    int Penalties,
    int Appearances
);
