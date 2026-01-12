import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { ApiService } from './api.service';
import { AuthService } from './auth.service';
import { ClubService } from './club.service';
import { tap } from 'rxjs/operators';
import {
  Season,
  SeasonCreateRequest,
  SeasonUpdateRequest,
  Competition,
  CompetitionListItem,
  CompetitionCreateRequest,
  CompetitionUpdateRequest,
  CompetitionFilter,
  CompetitionRound,
  CompetitionRoundCreateRequest,
  CompetitionTeam,
  CompetitionTeamCreateRequest,
  CompetitionTeamUpdateRequest,
  CompetitionParticipant,
  CompetitionParticipantCreateRequest,
  CompetitionParticipantUpdateRequest,
  Match,
  MatchListItem,
  MatchCreateRequest,
  MatchUpdateRequest,
  MatchResultRequest,
  MatchPostponeRequest,
  MatchFilter,
  MatchEvent,
  MatchEventCreateRequest,
  MatchLineup,
  MatchLineupCreateRequest,
  CompetitionStanding,
  GenerateFixturesRequest,
  DrawRequest,
  TopScorer,
  PagedResult
} from '../models';

@Injectable({
  providedIn: 'root'
})
export class CompetitionService {
  private readonly SELECTED_CLUB_KEY = 'selected_club_id';

  constructor(
    private api: ApiService,
    private authService: AuthService,
    private clubService: ClubService
  ) {}

  private getBaseEndpoint(): string {
    // First try user's clubId (for club managers)
    let clubId = this.authService.currentUser?.clubId;

    // For SuperAdmins, use selected club from localStorage
    if (!clubId && this.authService.isSuperAdmin) {
      clubId = localStorage.getItem(this.SELECTED_CLUB_KEY) || undefined;
    }

    if (!clubId) {
      throw new Error('No club context available. Please select a club first.');
    }
    return `clubs/${clubId}/competition`;
  }

  // Get or set the selected club for SuperAdmins
  getSelectedClubId(): string | null {
    return this.authService.currentUser?.clubId || localStorage.getItem(this.SELECTED_CLUB_KEY);
  }

  setSelectedClubId(clubId: string): void {
    localStorage.setItem(this.SELECTED_CLUB_KEY, clubId);
  }

  clearSelectedClubId(): void {
    localStorage.removeItem(this.SELECTED_CLUB_KEY);
  }

  // ============== Seasons ==============

  getSeasons(): Observable<Season[]> {
    return this.api.get<Season[]>(`${this.getBaseEndpoint()}/seasons`);
  }

  getSeason(id: string): Observable<Season> {
    return this.api.get<Season>(`${this.getBaseEndpoint()}/seasons/${id}`);
  }

  createSeason(data: SeasonCreateRequest): Observable<Season> {
    return this.api.post<Season>(`${this.getBaseEndpoint()}/seasons`, data);
  }

  updateSeason(id: string, data: SeasonUpdateRequest): Observable<Season> {
    return this.api.put<Season>(`${this.getBaseEndpoint()}/seasons/${id}`, data);
  }

  deleteSeason(id: string): Observable<void> {
    return this.api.delete<void>(`${this.getBaseEndpoint()}/seasons/${id}`);
  }

  // ============== Competitions ==============

  getCompetitions(filter?: CompetitionFilter): Observable<PagedResult<CompetitionListItem>> {
    return this.api.get<PagedResult<CompetitionListItem>>(this.getBaseEndpoint(), filter);
  }

  getCompetition(id: string): Observable<Competition> {
    return this.api.get<Competition>(`${this.getBaseEndpoint()}/${id}`);
  }

  createCompetition(data: CompetitionCreateRequest): Observable<Competition> {
    return this.api.post<Competition>(this.getBaseEndpoint(), data);
  }

  updateCompetition(id: string, data: CompetitionUpdateRequest): Observable<Competition> {
    return this.api.put<Competition>(`${this.getBaseEndpoint()}/${id}`, data);
  }

  deleteCompetition(id: string): Observable<void> {
    return this.api.delete<void>(`${this.getBaseEndpoint()}/${id}`);
  }

  publishCompetition(id: string): Observable<void> {
    return this.api.post<void>(`${this.getBaseEndpoint()}/${id}/publish`, {});
  }

  // ============== Competition Rounds ==============

  getRounds(competitionId: string): Observable<CompetitionRound[]> {
    return this.api.get<CompetitionRound[]>(`${this.getBaseEndpoint()}/${competitionId}/rounds`);
  }

  createRound(competitionId: string, data: CompetitionRoundCreateRequest): Observable<CompetitionRound> {
    return this.api.post<CompetitionRound>(`${this.getBaseEndpoint()}/${competitionId}/rounds`, data);
  }

  deleteRound(competitionId: string, roundId: string): Observable<void> {
    return this.api.delete<void>(`${this.getBaseEndpoint()}/${competitionId}/rounds/${roundId}`);
  }

  // ============== Competition Teams ==============

  getTeams(competitionId: string): Observable<CompetitionTeam[]> {
    return this.api.get<CompetitionTeam[]>(`${this.getBaseEndpoint()}/${competitionId}/teams`);
  }

  getTeam(competitionId: string, teamId: string): Observable<CompetitionTeam> {
    return this.api.get<CompetitionTeam>(`${this.getBaseEndpoint()}/${competitionId}/teams/${teamId}`);
  }

  registerTeam(competitionId: string, data: CompetitionTeamCreateRequest): Observable<CompetitionTeam> {
    return this.api.post<CompetitionTeam>(`${this.getBaseEndpoint()}/${competitionId}/teams`, data);
  }

  updateTeam(competitionId: string, teamId: string, data: CompetitionTeamUpdateRequest): Observable<CompetitionTeam> {
    return this.api.put<CompetitionTeam>(`${this.getBaseEndpoint()}/${competitionId}/teams/${teamId}`, data);
  }

  withdrawTeam(competitionId: string, teamId: string): Observable<void> {
    return this.api.post<void>(`${this.getBaseEndpoint()}/${competitionId}/teams/${teamId}/withdraw`, {});
  }

  approveTeam(competitionId: string, teamId: string): Observable<void> {
    return this.api.post<void>(`${this.getBaseEndpoint()}/${competitionId}/teams/${teamId}/approve`, {});
  }

  // ============== Team Participants ==============

  getTeamParticipants(competitionId: string, teamId: string): Observable<CompetitionParticipant[]> {
    return this.api.get<CompetitionParticipant[]>(
      `${this.getBaseEndpoint()}/${competitionId}/teams/${teamId}/participants`
    );
  }

  addParticipant(
    competitionId: string,
    teamId: string,
    data: CompetitionParticipantCreateRequest
  ): Observable<CompetitionParticipant> {
    return this.api.post<CompetitionParticipant>(
      `${this.getBaseEndpoint()}/${competitionId}/teams/${teamId}/participants`,
      data
    );
  }

  updateParticipant(
    competitionId: string,
    teamId: string,
    participantId: string,
    data: CompetitionParticipantUpdateRequest
  ): Observable<CompetitionParticipant> {
    return this.api.put<CompetitionParticipant>(
      `${this.getBaseEndpoint()}/${competitionId}/teams/${teamId}/participants/${participantId}`,
      data
    );
  }

  removeParticipant(competitionId: string, teamId: string, participantId: string): Observable<void> {
    return this.api.delete<void>(
      `${this.getBaseEndpoint()}/${competitionId}/teams/${teamId}/participants/${participantId}`
    );
  }

  // ============== Matches ==============

  getMatches(competitionId: string, filter?: MatchFilter): Observable<PagedResult<MatchListItem>> {
    return this.api.get<PagedResult<MatchListItem>>(
      `${this.getBaseEndpoint()}/${competitionId}/matches`,
      filter
    );
  }

  getMatch(competitionId: string, matchId: string): Observable<Match> {
    return this.api.get<Match>(`${this.getBaseEndpoint()}/${competitionId}/matches/${matchId}`);
  }

  createMatch(competitionId: string, data: MatchCreateRequest): Observable<Match> {
    return this.api.post<Match>(`${this.getBaseEndpoint()}/${competitionId}/matches`, data);
  }

  updateMatch(competitionId: string, matchId: string, data: MatchUpdateRequest): Observable<Match> {
    return this.api.put<Match>(`${this.getBaseEndpoint()}/${competitionId}/matches/${matchId}`, data);
  }

  recordMatchResult(competitionId: string, matchId: string, data: MatchResultRequest): Observable<Match> {
    return this.api.post<Match>(
      `${this.getBaseEndpoint()}/${competitionId}/matches/${matchId}/result`,
      data
    );
  }

  postponeMatch(competitionId: string, matchId: string, data: MatchPostponeRequest): Observable<void> {
    return this.api.post<void>(
      `${this.getBaseEndpoint()}/${competitionId}/matches/${matchId}/postpone`,
      data
    );
  }

  cancelMatch(competitionId: string, matchId: string, reason?: string): Observable<void> {
    return this.api.post<void>(
      `${this.getBaseEndpoint()}/${competitionId}/matches/${matchId}/cancel`,
      reason || null
    );
  }

  deleteMatch(competitionId: string, matchId: string): Observable<void> {
    return this.api.delete<void>(`${this.getBaseEndpoint()}/${competitionId}/matches/${matchId}`);
  }

  // ============== Match Events ==============

  getMatchEvents(competitionId: string, matchId: string): Observable<MatchEvent[]> {
    return this.api.get<MatchEvent[]>(
      `${this.getBaseEndpoint()}/${competitionId}/matches/${matchId}/events`
    );
  }

  addMatchEvent(competitionId: string, matchId: string, data: MatchEventCreateRequest): Observable<MatchEvent> {
    return this.api.post<MatchEvent>(
      `${this.getBaseEndpoint()}/${competitionId}/matches/${matchId}/events`,
      data
    );
  }

  deleteMatchEvent(competitionId: string, matchId: string, eventId: string): Observable<void> {
    return this.api.delete<void>(
      `${this.getBaseEndpoint()}/${competitionId}/matches/${matchId}/events/${eventId}`
    );
  }

  // ============== Match Lineups ==============

  getMatchLineups(competitionId: string, matchId: string): Observable<MatchLineup[]> {
    return this.api.get<MatchLineup[]>(
      `${this.getBaseEndpoint()}/${competitionId}/matches/${matchId}/lineups`
    );
  }

  addLineupPlayer(competitionId: string, matchId: string, data: MatchLineupCreateRequest): Observable<MatchLineup> {
    return this.api.post<MatchLineup>(
      `${this.getBaseEndpoint()}/${competitionId}/matches/${matchId}/lineups`,
      data
    );
  }

  removeLineupPlayer(competitionId: string, matchId: string, lineupId: string): Observable<void> {
    return this.api.delete<void>(
      `${this.getBaseEndpoint()}/${competitionId}/matches/${matchId}/lineups/${lineupId}`
    );
  }

  // ============== Standings ==============

  getStandings(competitionId: string, group?: string): Observable<CompetitionStanding[]> {
    const params = group ? { group } : undefined;
    return this.api.get<CompetitionStanding[]>(
      `${this.getBaseEndpoint()}/${competitionId}/standings`,
      params
    );
  }

  recalculateStandings(competitionId: string): Observable<void> {
    return this.api.post<void>(`${this.getBaseEndpoint()}/${competitionId}/standings/recalculate`, {});
  }

  // ============== Top Scorers ==============

  getTopScorers(competitionId: string, limit: number = 10): Observable<TopScorer[]> {
    return this.api.get<TopScorer[]>(
      `${this.getBaseEndpoint()}/${competitionId}/top-scorers`,
      { limit: limit.toString() }
    );
  }

  // ============== Fixtures & Draw ==============

  generateFixtures(competitionId: string, request: GenerateFixturesRequest): Observable<Match[]> {
    return this.api.post<Match[]>(
      `${this.getBaseEndpoint()}/${competitionId}/fixtures/generate`,
      request
    );
  }

  performDraw(competitionId: string, request: DrawRequest): Observable<void> {
    return this.api.post<void>(`${this.getBaseEndpoint()}/${competitionId}/draw`, request);
  }
}
