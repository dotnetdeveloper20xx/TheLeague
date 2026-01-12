using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents a competition (league, tournament, cup, etc.) organized by the club.
/// </summary>
public class Competition
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid? VenueId { get; set; }
    public Guid? SeasonId { get; set; }

    // Basic Info
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public CompetitionType Type { get; set; }
    public CompetitionStatus Status { get; set; } = CompetitionStatus.Draft;

    // Classification
    public string? Sport { get; set; }
    public string? Category { get; set; }
    public string? Division { get; set; }
    public SkillLevel SkillLevel { get; set; } = SkillLevel.AllLevels;
    public AgeGroup AgeGroup { get; set; } = AgeGroup.AllAges;
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public Gender? TargetGender { get; set; }

    // Schedule
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? RegistrationOpenDate { get; set; }
    public DateTime? RegistrationCloseDate { get; set; }
    public DateTime? DrawDate { get; set; }

    // Format
    public string? Format { get; set; } // Round-robin, knockout, etc.
    public int? NumberOfRounds { get; set; }
    public int? MatchesPerRound { get; set; }
    public bool HomeAndAway { get; set; }
    public int? LegsPerMatch { get; set; }

    // Teams/Participants
    public bool IsTeamBased { get; set; } = true;
    public int? MinTeams { get; set; }
    public int? MaxTeams { get; set; }
    public int CurrentTeamCount { get; set; }
    public int? MinPlayersPerTeam { get; set; }
    public int? MaxPlayersPerTeam { get; set; }

    // Registration & Entry
    public bool RequiresRegistration { get; set; } = true;
    public bool RequiresApproval { get; set; }
    public decimal? EntryFee { get; set; }
    public decimal? TeamEntryFee { get; set; }
    public decimal? PlayerEntryFee { get; set; }
    public string Currency { get; set; } = "GBP";
    public Guid? FeeId { get; set; }

    // Prizes
    public bool HasPrizes { get; set; }
    public string? PrizeDescription { get; set; }
    public decimal? TotalPrizeMoney { get; set; }
    public string? Prizes { get; set; } // JSON array of prize breakdown

    // Scoring & Points
    public int PointsForWin { get; set; } = 3;
    public int PointsForDraw { get; set; } = 1;
    public int PointsForLoss { get; set; } = 0;
    public string? BonusPointRules { get; set; } // JSON

    // Tiebreaker Rules
    public string? TiebreakerRules { get; set; } // JSON array of tiebreaker order

    // Rules & Regulations
    public string? Rules { get; set; }
    public string? MatchRules { get; set; }
    public int? MatchDurationMinutes { get; set; }
    public int? HalfTimeDurationMinutes { get; set; }
    public bool AllowSubstitutes { get; set; } = true;
    public int? MaxSubstitutionsPerMatch { get; set; }

    // Officials
    public Guid? OrganizerId { get; set; }
    public string? OrganizerName { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }

    // Sponsorship
    public bool HasSponsors { get; set; }
    public string? Sponsors { get; set; } // JSON array

    // Media
    public string? ImageUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string? LogoUrl { get; set; }

    // Publishing
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? PublishedBy { get; set; }
    public bool ShowOnWebsite { get; set; } = true;

    // Statistics
    public int TotalMatches { get; set; }
    public int CompletedMatches { get; set; }
    public int TotalGoals { get; set; }
    public int TotalPoints { get; set; }

    // Notes
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public Venue? Venue { get; set; }
    public Season? Season { get; set; }
    public Member? Organizer { get; set; }
    public Fee? Fee { get; set; }
    public ICollection<CompetitionTeam> Teams { get; set; } = new List<CompetitionTeam>();
    public ICollection<CompetitionRound> Rounds { get; set; } = new List<CompetitionRound>();
    public ICollection<Match> Matches { get; set; } = new List<Match>();
    public ICollection<CompetitionStanding> Standings { get; set; } = new List<CompetitionStanding>();
}

/// <summary>
/// Represents a season for competitions.
/// </summary>
public class Season
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }

    public string Name { get; set; } = string.Empty; // e.g., "2024/25"
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public bool IsCompleted { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public ICollection<Competition> Competitions { get; set; } = new List<Competition>();
}

/// <summary>
/// Represents a round within a competition.
/// </summary>
public class CompetitionRound
{
    public Guid Id { get; set; }
    public Guid CompetitionId { get; set; }

    public int RoundNumber { get; set; }
    public string Name { get; set; } = string.Empty; // e.g., "Quarter-Finals", "Week 5"
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? Deadline { get; set; }

    public bool IsComplete { get; set; }
    public int MatchCount { get; set; }
    public int CompletedMatchCount { get; set; }

    public string? Notes { get; set; }

    // Navigation
    public Competition Competition { get; set; } = null!;
    public ICollection<Match> Matches { get; set; } = new List<Match>();
}

/// <summary>
/// Represents a team registered for a competition.
/// </summary>
public class CompetitionTeam
{
    public Guid Id { get; set; }
    public Guid CompetitionId { get; set; }
    public Guid ClubId { get; set; }

    // Team Info
    public string Name { get; set; } = string.Empty;
    public string? ShortName { get; set; }
    public string? Code { get; set; }
    public TeamStatus Status { get; set; } = TeamStatus.Registered;

    // Seed/Draw
    public int? SeedNumber { get; set; }
    public int? DrawPosition { get; set; }
    public string? Group { get; set; }

    // Captain/Contact
    public Guid? CaptainId { get; set; }
    public string? CaptainName { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }

    // Entry
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public bool IsApproved { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }

    // Payment
    public bool EntryFeePaid { get; set; }
    public decimal? EntryFeeAmount { get; set; }
    public Guid? PaymentId { get; set; }

    // Statistics
    public int Played { get; set; }
    public int Won { get; set; }
    public int Drawn { get; set; }
    public int Lost { get; set; }
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }
    public int GoalDifference { get; set; }
    public int Points { get; set; }
    public int BonusPoints { get; set; }
    public int Position { get; set; }

    // Home Venue
    public Guid? HomeVenueId { get; set; }
    public string? HomeColors { get; set; }
    public string? AwayColors { get; set; }

    // Media
    public string? LogoUrl { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }

    // Navigation
    public Competition Competition { get; set; } = null!;
    public Club Club { get; set; } = null!;
    public Member? Captain { get; set; }
    public Venue? HomeVenue { get; set; }
    public Payment? Payment { get; set; }
    public ICollection<CompetitionParticipant> Participants { get; set; } = new List<CompetitionParticipant>();
    public ICollection<Match> HomeMatches { get; set; } = new List<Match>();
    public ICollection<Match> AwayMatches { get; set; } = new List<Match>();
}

/// <summary>
/// Represents a player/participant in a competition team.
/// </summary>
public class CompetitionParticipant
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public Guid MemberId { get; set; }

    // Role & Status
    public ParticipantRole Role { get; set; } = ParticipantRole.Player;
    public bool IsActive { get; set; } = true;
    public int? SquadNumber { get; set; }
    public string? Position { get; set; }

    // Registration
    public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;
    public bool IsEligible { get; set; } = true;
    public string? EligibilityNotes { get; set; }

    // Statistics
    public int Appearances { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }
    public int YellowCards { get; set; }
    public int RedCards { get; set; }
    public int MinutesPlayed { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }

    // Navigation
    public CompetitionTeam Team { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public ICollection<MatchEvent> MatchEvents { get; set; } = new List<MatchEvent>();
}

/// <summary>
/// Represents a match/game in a competition.
/// </summary>
public class Match
{
    public Guid Id { get; set; }
    public Guid CompetitionId { get; set; }
    public Guid? RoundId { get; set; }
    public Guid? VenueId { get; set; }
    public Guid? FacilityId { get; set; }

    // Teams
    public Guid? HomeTeamId { get; set; }
    public Guid? AwayTeamId { get; set; }

    // Match Details
    public string? MatchNumber { get; set; }
    public int? LegNumber { get; set; }
    public MatchStatus Status { get; set; } = MatchStatus.Scheduled;

    // Schedule
    public DateTime? ScheduledDateTime { get; set; }
    public DateTime? ActualStartTime { get; set; }
    public DateTime? ActualEndTime { get; set; }
    public int? DurationMinutes { get; set; }

    // Result
    public MatchResult Result { get; set; } = MatchResult.NotPlayed;
    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }
    public int? HomeHalfTimeScore { get; set; }
    public int? AwayHalfTimeScore { get; set; }
    public int? HomeExtraTimeScore { get; set; }
    public int? AwayExtraTimeScore { get; set; }
    public int? HomePenaltyScore { get; set; }
    public int? AwayPenaltyScore { get; set; }

    // Aggregate (for two-leg ties)
    public int? HomeAggregateScore { get; set; }
    public int? AwayAggregateScore { get; set; }
    public Guid? FirstLegMatchId { get; set; }

    // Points Awarded
    public int? HomePointsAwarded { get; set; }
    public int? AwayPointsAwarded { get; set; }
    public int? HomeBonusPoints { get; set; }
    public int? AwayBonusPoints { get; set; }

    // Officials
    public string? RefereeName { get; set; }
    public Guid? RefereeId { get; set; }
    public string? AssistantReferees { get; set; } // JSON array
    public string? FourthOfficial { get; set; }

    // Attendance
    public int? Attendance { get; set; }

    // Weather/Conditions
    public string? Weather { get; set; }
    public string? PitchCondition { get; set; }

    // Postponement/Cancellation
    public bool IsPostponed { get; set; }
    public DateTime? OriginalDateTime { get; set; }
    public string? PostponementReason { get; set; }
    public bool IsCancelled { get; set; }
    public string? CancellationReason { get; set; }

    // Match Report
    public string? MatchReport { get; set; }
    public string? HighlightsUrl { get; set; }
    public string? PhotosUrl { get; set; }

    // Confirmation
    public bool HomeResultConfirmed { get; set; }
    public DateTime? HomeConfirmedAt { get; set; }
    public string? HomeConfirmedBy { get; set; }
    public bool AwayResultConfirmed { get; set; }
    public DateTime? AwayConfirmedAt { get; set; }
    public string? AwayConfirmedBy { get; set; }

    // Disputes
    public bool IsDisputed { get; set; }
    public string? DisputeReason { get; set; }
    public string? DisputeResolution { get; set; }

    // Notes
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Competition Competition { get; set; } = null!;
    public CompetitionRound? Round { get; set; }
    public Venue? Venue { get; set; }
    public Facility? Facility { get; set; }
    public CompetitionTeam? HomeTeam { get; set; }
    public CompetitionTeam? AwayTeam { get; set; }
    public Member? Referee { get; set; }
    public Match? FirstLegMatch { get; set; }
    public ICollection<MatchEvent> Events { get; set; } = new List<MatchEvent>();
    public ICollection<MatchLineup> Lineups { get; set; } = new List<MatchLineup>();
}

/// <summary>
/// Represents an event during a match (goal, card, substitution, etc.).
/// </summary>
public class MatchEvent
{
    public Guid Id { get; set; }
    public Guid MatchId { get; set; }
    public Guid? ParticipantId { get; set; }
    public Guid? TeamId { get; set; }

    // Event Details
    public string EventType { get; set; } = string.Empty; // Goal, OwnGoal, YellowCard, RedCard, Substitution, Injury, etc.
    public int Minute { get; set; }
    public int? AdditionalMinutes { get; set; }
    public string? Period { get; set; } // FirstHalf, SecondHalf, ExtraTime, Penalties

    // Additional Info
    public Guid? AssistByParticipantId { get; set; }
    public Guid? SubstitutedForParticipantId { get; set; }
    public string? Description { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }

    // Navigation
    public Match Match { get; set; } = null!;
    public CompetitionParticipant? Participant { get; set; }
    public CompetitionTeam? Team { get; set; }
    public CompetitionParticipant? AssistByParticipant { get; set; }
    public CompetitionParticipant? SubstitutedForParticipant { get; set; }
}

/// <summary>
/// Represents a player's lineup in a match.
/// </summary>
public class MatchLineup
{
    public Guid Id { get; set; }
    public Guid MatchId { get; set; }
    public Guid TeamId { get; set; }
    public Guid ParticipantId { get; set; }

    // Lineup Info
    public bool IsStarting { get; set; }
    public int? ShirtNumber { get; set; }
    public string? Position { get; set; }
    public int? PositionOrder { get; set; }

    // Playing Time
    public int? MinutesPlayed { get; set; }
    public int? SubbedOnMinute { get; set; }
    public int? SubbedOffMinute { get; set; }

    // Performance
    public int? Rating { get; set; }
    public bool IsManOfTheMatch { get; set; }

    // Navigation
    public Match Match { get; set; } = null!;
    public CompetitionTeam Team { get; set; } = null!;
    public CompetitionParticipant Participant { get; set; } = null!;
}

/// <summary>
/// Represents team standings in a competition.
/// </summary>
public class CompetitionStanding
{
    public Guid Id { get; set; }
    public Guid CompetitionId { get; set; }
    public Guid TeamId { get; set; }

    // Position
    public int Position { get; set; }
    public int? PreviousPosition { get; set; }
    public string? Group { get; set; }

    // Match Stats
    public int Played { get; set; }
    public int Won { get; set; }
    public int Drawn { get; set; }
    public int Lost { get; set; }

    // Goals/Points
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }
    public int GoalDifference { get; set; }
    public int Points { get; set; }
    public int BonusPoints { get; set; }
    public int TotalPoints { get; set; }

    // Form
    public string? Form { get; set; } // e.g., "WWDLW"
    public string? RecentResults { get; set; } // JSON array of recent match results

    // Qualifiers
    public string? Zone { get; set; } // Champions, Promotion, Relegation, etc.
    public bool IsPromoted { get; set; }
    public bool IsRelegated { get; set; }
    public bool QualifiedForNext { get; set; }

    // Metadata
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    // Navigation
    public Competition Competition { get; set; } = null!;
    public CompetitionTeam Team { get; set; } = null!;
}
