// Competition-related models for Phase 10: Events & Competitions Enhanced

// Enums
export enum CompetitionType {
  League = 'League',
  Tournament = 'Tournament',
  Cup = 'Cup',
  Friendly = 'Friendly',
  Championship = 'Championship',
  Qualifier = 'Qualifier',
  Playoff = 'Playoff',
  RoundRobin = 'RoundRobin',
  Ladder = 'Ladder',
  TimeTrial = 'TimeTrial',
  Other = 'Other'
}

export enum CompetitionStatus {
  Draft = 'Draft',
  Published = 'Published',
  RegistrationOpen = 'RegistrationOpen',
  RegistrationClosed = 'RegistrationClosed',
  DrawComplete = 'DrawComplete',
  InProgress = 'InProgress',
  Completed = 'Completed',
  Cancelled = 'Cancelled',
  Postponed = 'Postponed',
  Archived = 'Archived'
}

export enum MatchStatus {
  Scheduled = 'Scheduled',
  Confirmed = 'Confirmed',
  InProgress = 'InProgress',
  Completed = 'Completed',
  Postponed = 'Postponed',
  Cancelled = 'Cancelled',
  Walkover = 'Walkover',
  Bye = 'Bye',
  Abandoned = 'Abandoned',
  Disputed = 'Disputed'
}

export enum MatchResult {
  NotPlayed = 'NotPlayed',
  HomeWin = 'HomeWin',
  AwayWin = 'AwayWin',
  Draw = 'Draw',
  HomeWalkover = 'HomeWalkover',
  AwayWalkover = 'AwayWalkover',
  BothWalkover = 'BothWalkover',
  Void = 'Void'
}

export enum TeamStatus {
  Registered = 'Registered',
  Confirmed = 'Confirmed',
  Withdrawn = 'Withdrawn',
  Disqualified = 'Disqualified',
  Eliminated = 'Eliminated',
  Active = 'Active',
  Champion = 'Champion',
  RunnerUp = 'RunnerUp'
}

export enum ParticipantRole {
  Player = 'Player',
  Captain = 'Captain',
  ViceCaptain = 'ViceCaptain',
  Coach = 'Coach',
  Manager = 'Manager',
  Official = 'Official',
  Substitute = 'Substitute',
  Reserve = 'Reserve'
}

export enum SkillLevel {
  Beginner = 'Beginner',
  Elementary = 'Elementary',
  Intermediate = 'Intermediate',
  UpperIntermediate = 'UpperIntermediate',
  Advanced = 'Advanced',
  Expert = 'Expert',
  AllLevels = 'AllLevels'
}

export enum AgeGroup {
  Infant = 'Infant',
  Toddler = 'Toddler',
  Child = 'Child',
  PreTeen = 'PreTeen',
  Teen = 'Teen',
  Adult = 'Adult',
  Senior = 'Senior',
  AllAges = 'AllAges'
}

// Season interfaces
export interface Season {
  id: string;
  name: string;
  startDate: Date;
  endDate: Date;
  isCurrent: boolean;
  isCompleted: boolean;
  competitionCount: number;
}

export interface SeasonCreateRequest {
  name: string;
  startDate: Date;
  endDate: Date;
  isCurrent?: boolean;
}

export interface SeasonUpdateRequest {
  name?: string;
  startDate?: Date;
  endDate?: Date;
  isCurrent?: boolean;
  isCompleted?: boolean;
}

// Competition interfaces
export interface Competition {
  id: string;
  name: string;
  code?: string;
  description?: string;
  type: CompetitionType;
  status: CompetitionStatus;
  sport?: string;
  category?: string;
  division?: string;
  skillLevel: SkillLevel;
  ageGroup: AgeGroup;
  startDate?: Date;
  endDate?: Date;
  registrationOpenDate?: Date;
  registrationCloseDate?: Date;
  format?: string;
  numberOfRounds?: number;
  isTeamBased: boolean;
  minTeams?: number;
  maxTeams?: number;
  currentTeamCount: number;
  minPlayersPerTeam?: number;
  maxPlayersPerTeam?: number;
  entryFee?: number;
  currency: string;
  hasPrizes: boolean;
  prizeDescription?: string;
  totalPrizeMoney?: number;
  pointsForWin: number;
  pointsForDraw: number;
  pointsForLoss: number;
  organizerName?: string;
  contactEmail?: string;
  imageUrl?: string;
  isPublished: boolean;
  totalMatches: number;
  completedMatches: number;
  season?: Season;
  venue?: CompetitionVenue;
}

export interface CompetitionListItem {
  id: string;
  name: string;
  code?: string;
  type: CompetitionType;
  status: CompetitionStatus;
  sport?: string;
  startDate?: Date;
  endDate?: Date;
  currentTeamCount: number;
  maxTeams?: number;
  totalMatches: number;
  completedMatches: number;
  isPublished: boolean;
  imageUrl?: string;
}

export interface CompetitionCreateRequest {
  name: string;
  code?: string;
  description?: string;
  type: CompetitionType;
  sport?: string;
  category?: string;
  division?: string;
  skillLevel?: SkillLevel;
  ageGroup?: AgeGroup;
  minAge?: number;
  maxAge?: number;
  targetGender?: string;
  startDate?: Date;
  endDate?: Date;
  registrationOpenDate?: Date;
  registrationCloseDate?: Date;
  seasonId?: string;
  venueId?: string;
  format?: string;
  numberOfRounds?: number;
  homeAndAway?: boolean;
  isTeamBased?: boolean;
  minTeams?: number;
  maxTeams?: number;
  minPlayersPerTeam?: number;
  maxPlayersPerTeam?: number;
  entryFee?: number;
  teamEntryFee?: number;
  playerEntryFee?: number;
  hasPrizes?: boolean;
  prizeDescription?: string;
  totalPrizeMoney?: number;
  pointsForWin?: number;
  pointsForDraw?: number;
  pointsForLoss?: number;
  rules?: string;
  organizerName?: string;
  contactEmail?: string;
  contactPhone?: string;
  imageUrl?: string;
  isPublished?: boolean;
}

export interface CompetitionUpdateRequest {
  name?: string;
  code?: string;
  description?: string;
  type?: CompetitionType;
  status?: CompetitionStatus;
  sport?: string;
  category?: string;
  division?: string;
  skillLevel?: SkillLevel;
  ageGroup?: AgeGroup;
  startDate?: Date;
  endDate?: Date;
  registrationOpenDate?: Date;
  registrationCloseDate?: Date;
  seasonId?: string;
  venueId?: string;
  format?: string;
  numberOfRounds?: number;
  homeAndAway?: boolean;
  minTeams?: number;
  maxTeams?: number;
  minPlayersPerTeam?: number;
  maxPlayersPerTeam?: number;
  entryFee?: number;
  hasPrizes?: boolean;
  prizeDescription?: string;
  totalPrizeMoney?: number;
  pointsForWin?: number;
  pointsForDraw?: number;
  pointsForLoss?: number;
  rules?: string;
  organizerName?: string;
  contactEmail?: string;
  contactPhone?: string;
  imageUrl?: string;
  isPublished?: boolean;
}

export interface CompetitionVenue {
  id: string;
  name: string;
  address?: string;
}

// Competition Round interfaces
export interface CompetitionRound {
  id: string;
  roundNumber: number;
  name: string;
  startDate?: Date;
  endDate?: Date;
  isComplete: boolean;
  matchCount: number;
  completedMatchCount: number;
}

export interface CompetitionRoundCreateRequest {
  roundNumber: number;
  name: string;
  startDate?: Date;
  endDate?: Date;
}

// Competition Team interfaces
export interface CompetitionTeam {
  id: string;
  competitionId: string;
  name: string;
  shortName?: string;
  code?: string;
  status: TeamStatus;
  seedNumber?: number;
  drawPosition?: number;
  group?: string;
  captainId?: string;
  captainName?: string;
  contactEmail?: string;
  isApproved: boolean;
  entryFeePaid: boolean;
  played: number;
  won: number;
  drawn: number;
  lost: number;
  goalsFor: number;
  goalsAgainst: number;
  goalDifference: number;
  points: number;
  position: number;
  logoUrl?: string;
  participantCount: number;
}

export interface CompetitionTeamListItem {
  id: string;
  name: string;
  shortName?: string;
  code?: string;
  status: TeamStatus;
  group?: string;
  played: number;
  won: number;
  drawn: number;
  lost: number;
  goalDifference: number;
  points: number;
  position: number;
  logoUrl?: string;
}

export interface CompetitionTeamCreateRequest {
  name: string;
  shortName?: string;
  code?: string;
  seedNumber?: number;
  group?: string;
  captainId?: string;
  captainName?: string;
  contactEmail?: string;
  contactPhone?: string;
  homeVenueId?: string;
  homeColors?: string;
  awayColors?: string;
  logoUrl?: string;
}

export interface CompetitionTeamUpdateRequest {
  name?: string;
  shortName?: string;
  code?: string;
  status?: TeamStatus;
  seedNumber?: number;
  drawPosition?: number;
  group?: string;
  captainId?: string;
  captainName?: string;
  contactEmail?: string;
  contactPhone?: string;
  isApproved?: boolean;
  entryFeePaid?: boolean;
  homeColors?: string;
  awayColors?: string;
  logoUrl?: string;
}

// Competition Participant interfaces
export interface CompetitionParticipant {
  id: string;
  teamId: string;
  memberId: string;
  memberName: string;
  role: ParticipantRole;
  isActive: boolean;
  squadNumber?: number;
  position?: string;
  isEligible: boolean;
  appearances: number;
  goals: number;
  assists: number;
  yellowCards: number;
  redCards: number;
  minutesPlayed: number;
}

export interface CompetitionParticipantCreateRequest {
  memberId: string;
  role?: ParticipantRole;
  squadNumber?: number;
  position?: string;
}

export interface CompetitionParticipantUpdateRequest {
  role?: ParticipantRole;
  isActive?: boolean;
  squadNumber?: number;
  position?: string;
  isEligible?: boolean;
  eligibilityNotes?: string;
}

// Match interfaces
export interface Match {
  id: string;
  competitionId: string;
  competitionName: string;
  roundId?: string;
  roundName?: string;
  matchNumber?: string;
  legNumber?: number;
  status: MatchStatus;
  scheduledDateTime?: Date;
  actualStartTime?: Date;
  actualEndTime?: Date;
  homeTeamId?: string;
  homeTeamName?: string;
  homeTeamLogo?: string;
  awayTeamId?: string;
  awayTeamName?: string;
  awayTeamLogo?: string;
  result: MatchResult;
  homeScore?: number;
  awayScore?: number;
  homeHalfTimeScore?: number;
  awayHalfTimeScore?: number;
  refereeName?: string;
  attendance?: number;
  isPostponed: boolean;
  isCancelled: boolean;
  highlightsUrl?: string;
  venue?: CompetitionVenue;
}

export interface MatchListItem {
  id: string;
  matchNumber?: string;
  status: MatchStatus;
  scheduledDateTime?: Date;
  homeTeamId?: string;
  homeTeamName?: string;
  awayTeamId?: string;
  awayTeamName?: string;
  result: MatchResult;
  homeScore?: number;
  awayScore?: number;
  roundName?: string;
}

export interface MatchCreateRequest {
  roundId?: string;
  venueId?: string;
  facilityId?: string;
  homeTeamId?: string;
  awayTeamId?: string;
  matchNumber?: string;
  legNumber?: number;
  scheduledDateTime?: Date;
  refereeName?: string;
  refereeId?: string;
}

export interface MatchUpdateRequest {
  roundId?: string;
  venueId?: string;
  facilityId?: string;
  homeTeamId?: string;
  awayTeamId?: string;
  matchNumber?: string;
  status?: MatchStatus;
  scheduledDateTime?: Date;
  actualStartTime?: Date;
  actualEndTime?: Date;
  refereeName?: string;
  refereeId?: string;
  attendance?: number;
  weather?: string;
  pitchCondition?: string;
  notes?: string;
}

export interface MatchResultRequest {
  homeScore: number;
  awayScore: number;
  homeHalfTimeScore?: number;
  awayHalfTimeScore?: number;
  homeExtraTimeScore?: number;
  awayExtraTimeScore?: number;
  homePenaltyScore?: number;
  awayPenaltyScore?: number;
  matchReport?: string;
}

export interface MatchPostponeRequest {
  reason: string;
  newDateTime?: Date;
}

// Match Event interfaces
export interface MatchEvent {
  id: string;
  matchId: string;
  eventType: string;
  minute: number;
  additionalMinutes?: number;
  period?: string;
  participantId?: string;
  participantName?: string;
  participantNumber?: number;
  teamId?: string;
  teamName?: string;
  assistByParticipantId?: string;
  assistByName?: string;
  description?: string;
}

export interface MatchEventCreateRequest {
  eventType: string;
  minute: number;
  additionalMinutes?: number;
  period?: string;
  participantId?: string;
  teamId?: string;
  assistByParticipantId?: string;
  substitutedForParticipantId?: string;
  description?: string;
}

// Match Lineup interfaces
export interface MatchLineup {
  id: string;
  matchId: string;
  teamId: string;
  teamName: string;
  participantId: string;
  participantName: string;
  isStarting: boolean;
  shirtNumber?: number;
  position?: string;
  minutesPlayed?: number;
  subbedOnMinute?: number;
  subbedOffMinute?: number;
  rating?: number;
  isManOfTheMatch: boolean;
}

export interface MatchLineupCreateRequest {
  teamId: string;
  participantId: string;
  isStarting?: boolean;
  shirtNumber?: number;
  position?: string;
  positionOrder?: number;
}

// Competition Standing interfaces
export interface CompetitionStanding {
  id: string;
  competitionId: string;
  teamId: string;
  teamName: string;
  teamLogo?: string;
  position: number;
  previousPosition?: number;
  group?: string;
  played: number;
  won: number;
  drawn: number;
  lost: number;
  goalsFor: number;
  goalsAgainst: number;
  goalDifference: number;
  points: number;
  bonusPoints: number;
  totalPoints: number;
  form?: string;
  zone?: string;
  isPromoted: boolean;
  isRelegated: boolean;
}

// Filter requests
export interface CompetitionFilter {
  type?: CompetitionType;
  status?: CompetitionStatus;
  sport?: string;
  seasonId?: string;
  includeUnpublished?: boolean;
  page?: number;
  pageSize?: number;
}

export interface MatchFilter {
  roundId?: string;
  teamId?: string;
  status?: MatchStatus;
  dateFrom?: Date;
  dateTo?: Date;
  page?: number;
  pageSize?: number;
}

// Fixture generation
export interface GenerateFixturesRequest {
  homeAndAway?: boolean;
  randomizeOrder?: boolean;
  startDate?: Date;
  daysBetweenRounds?: number;
}

// Draw/Bracket
export interface DrawRequest {
  randomDraw?: boolean;
  seedTeams?: boolean;
}

// Top Scorers
export interface TopScorer {
  rank: number;
  playerName: string;
  teamId?: string;
  teamName?: string;
  teamLogo?: string;
  goals: number;
  assists: number;
  penalties: number;
  appearances: number;
}
