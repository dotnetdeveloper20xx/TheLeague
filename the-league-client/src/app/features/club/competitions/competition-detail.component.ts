import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CompetitionService } from '../../../core/services/competition.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent, ConfirmDialogComponent } from '../../../shared/components';
import { DateFormatPipe, CurrencyFormatPipe } from '../../../shared/pipes';
import {
  Competition,
  CompetitionTeam,
  CompetitionTeamCreateRequest,
  MatchListItem,
  CompetitionStanding,
  CompetitionStatus,
  CompetitionType,
  TeamStatus,
  MatchStatus,
  MatchResult,
  MatchResultRequest,
  MatchEvent,
  MatchEventCreateRequest,
  GenerateFixturesRequest,
  TopScorer
} from '../../../core/models';

// Event type constants
const EVENT_TYPES = {
  GOAL: 'goal',
  YELLOW_CARD: 'yellowcard',
  RED_CARD: 'redcard',
  SUBSTITUTION: 'substitution',
  PENALTY: 'penalty',
  OWN_GOAL: 'owngoal'
} as const;

type TabType = 'overview' | 'teams' | 'matches' | 'standings' | 'scorers';

@Component({
  selector: 'app-competition-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    ReactiveFormsModule,
    LoadingSpinnerComponent,
    ConfirmDialogComponent,
    DateFormatPipe,
    CurrencyFormatPipe
  ],
  template: `
    <div class="space-y-6">
      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg" message="Loading competition..."></app-loading-spinner>
        </div>
      } @else if (competition()) {
        <!-- Header -->
        <div class="flex flex-col md:flex-row md:items-start md:justify-between gap-4">
          <div class="flex items-start gap-4">
            <a routerLink="/club/competitions" class="btn-secondary mt-1">
              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
              </svg>
            </a>
            <div>
              <div class="flex items-center gap-2 mb-1">
                <span class="badge" [class]="getTypeBadgeClass(competition()!.type)">
                  {{ formatType(competition()!.type) }}
                </span>
                <span class="badge" [class]="getStatusBadgeClass(competition()!.status)">
                  {{ formatStatus(competition()!.status) }}
                </span>
                @if (!competition()!.isPublished) {
                  <span class="badge badge-warning">Draft</span>
                }
              </div>
              <h1 class="text-2xl font-bold text-gray-900">{{ competition()!.name }}</h1>
              @if (competition()!.code) {
                <p class="text-gray-500">{{ competition()!.code }}</p>
              }
            </div>
          </div>
          <div class="flex flex-wrap gap-2">
            @if (!competition()!.isPublished) {
              <button (click)="publishCompetition()" class="btn-success">
                <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
                </svg>
                Publish
              </button>
            }
            <a [routerLink]="['/club/competitions', competition()!.id, 'edit']" class="btn-secondary">
              <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
              </svg>
              Edit
            </a>
            <button (click)="showDeleteConfirm.set(true)" class="btn-danger">
              <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
              </svg>
              Delete
            </button>
          </div>
        </div>

        <!-- Stats Cards -->
        <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
          <div class="card text-center">
            <p class="text-3xl font-bold text-primary-600">{{ competition()!.currentTeamCount }}</p>
            <p class="text-sm text-gray-500">Teams</p>
          </div>
          <div class="card text-center">
            <p class="text-3xl font-bold text-primary-600">{{ competition()!.totalMatches }}</p>
            <p class="text-sm text-gray-500">Total Matches</p>
          </div>
          <div class="card text-center">
            <p class="text-3xl font-bold text-green-600">{{ competition()!.completedMatches }}</p>
            <p class="text-sm text-gray-500">Completed</p>
          </div>
          <div class="card text-center">
            <p class="text-3xl font-bold text-orange-600">{{ competition()!.totalMatches - competition()!.completedMatches }}</p>
            <p class="text-sm text-gray-500">Remaining</p>
          </div>
        </div>

        <!-- Tabs -->
        <div class="border-b border-gray-200">
          <nav class="flex space-x-8">
            <button
              (click)="activeTab.set('overview')"
              class="py-4 px-1 border-b-2 font-medium text-sm"
              [class.border-primary-500]="activeTab() === 'overview'"
              [class.text-primary-600]="activeTab() === 'overview'"
              [class.border-transparent]="activeTab() !== 'overview'"
              [class.text-gray-500]="activeTab() !== 'overview'"
            >
              Overview
            </button>
            <button
              (click)="activeTab.set('teams'); loadTeams()"
              class="py-4 px-1 border-b-2 font-medium text-sm"
              [class.border-primary-500]="activeTab() === 'teams'"
              [class.text-primary-600]="activeTab() === 'teams'"
              [class.border-transparent]="activeTab() !== 'teams'"
              [class.text-gray-500]="activeTab() !== 'teams'"
            >
              Teams ({{ teams().length }})
            </button>
            <button
              (click)="activeTab.set('matches'); loadMatches()"
              class="py-4 px-1 border-b-2 font-medium text-sm"
              [class.border-primary-500]="activeTab() === 'matches'"
              [class.text-primary-600]="activeTab() === 'matches'"
              [class.border-transparent]="activeTab() !== 'matches'"
              [class.text-gray-500]="activeTab() !== 'matches'"
            >
              Matches ({{ matches().length }})
            </button>
            <button
              (click)="activeTab.set('standings'); loadStandings()"
              class="py-4 px-1 border-b-2 font-medium text-sm"
              [class.border-primary-500]="activeTab() === 'standings'"
              [class.text-primary-600]="activeTab() === 'standings'"
              [class.border-transparent]="activeTab() !== 'standings'"
              [class.text-gray-500]="activeTab() !== 'standings'"
            >
              Standings
            </button>
            <button
              (click)="activeTab.set('scorers'); loadTopScorers()"
              class="py-4 px-1 border-b-2 font-medium text-sm"
              [class.border-primary-500]="activeTab() === 'scorers'"
              [class.text-primary-600]="activeTab() === 'scorers'"
              [class.border-transparent]="activeTab() !== 'scorers'"
              [class.text-gray-500]="activeTab() !== 'scorers'"
            >
              Top Scorers
            </button>
          </nav>
        </div>

        <!-- Tab Content -->
        @switch (activeTab()) {
          @case ('overview') {
            <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
              <!-- Details Card -->
              <div class="card">
                <h3 class="text-lg font-semibold text-gray-900 mb-4">Competition Details</h3>
                <dl class="space-y-3">
                  @if (competition()!.description) {
                    <div>
                      <dt class="text-sm font-medium text-gray-500">Description</dt>
                      <dd class="text-sm text-gray-900">{{ competition()!.description }}</dd>
                    </div>
                  }
                  @if (competition()!.sport) {
                    <div class="flex justify-between">
                      <dt class="text-sm font-medium text-gray-500">Sport</dt>
                      <dd class="text-sm text-gray-900">{{ competition()!.sport }}</dd>
                    </div>
                  }
                  @if (competition()!.category) {
                    <div class="flex justify-between">
                      <dt class="text-sm font-medium text-gray-500">Category</dt>
                      <dd class="text-sm text-gray-900">{{ competition()!.category }}</dd>
                    </div>
                  }
                  @if (competition()!.division) {
                    <div class="flex justify-between">
                      <dt class="text-sm font-medium text-gray-500">Division</dt>
                      <dd class="text-sm text-gray-900">{{ competition()!.division }}</dd>
                    </div>
                  }
                  @if (competition()!.season) {
                    <div class="flex justify-between">
                      <dt class="text-sm font-medium text-gray-500">Season</dt>
                      <dd class="text-sm text-gray-900">{{ competition()!.season!.name }}</dd>
                    </div>
                  }
                  @if (competition()!.format) {
                    <div class="flex justify-between">
                      <dt class="text-sm font-medium text-gray-500">Format</dt>
                      <dd class="text-sm text-gray-900">{{ competition()!.format }}</dd>
                    </div>
                  }
                </dl>
              </div>

              <!-- Dates Card -->
              <div class="card">
                <h3 class="text-lg font-semibold text-gray-900 mb-4">Dates</h3>
                <dl class="space-y-3">
                  @if (competition()!.startDate) {
                    <div class="flex justify-between">
                      <dt class="text-sm font-medium text-gray-500">Start Date</dt>
                      <dd class="text-sm text-gray-900">{{ competition()!.startDate | dateFormat }}</dd>
                    </div>
                  }
                  @if (competition()!.endDate) {
                    <div class="flex justify-between">
                      <dt class="text-sm font-medium text-gray-500">End Date</dt>
                      <dd class="text-sm text-gray-900">{{ competition()!.endDate | dateFormat }}</dd>
                    </div>
                  }
                  @if (competition()!.registrationOpenDate) {
                    <div class="flex justify-between">
                      <dt class="text-sm font-medium text-gray-500">Registration Opens</dt>
                      <dd class="text-sm text-gray-900">{{ competition()!.registrationOpenDate | dateFormat }}</dd>
                    </div>
                  }
                  @if (competition()!.registrationCloseDate) {
                    <div class="flex justify-between">
                      <dt class="text-sm font-medium text-gray-500">Registration Closes</dt>
                      <dd class="text-sm text-gray-900">{{ competition()!.registrationCloseDate | dateFormat }}</dd>
                    </div>
                  }
                </dl>
              </div>

              <!-- Points System Card -->
              <div class="card">
                <h3 class="text-lg font-semibold text-gray-900 mb-4">Points System</h3>
                <dl class="space-y-3">
                  <div class="flex justify-between">
                    <dt class="text-sm font-medium text-gray-500">Win</dt>
                    <dd class="text-sm font-semibold text-green-600">{{ competition()!.pointsForWin }} pts</dd>
                  </div>
                  <div class="flex justify-between">
                    <dt class="text-sm font-medium text-gray-500">Draw</dt>
                    <dd class="text-sm font-semibold text-yellow-600">{{ competition()!.pointsForDraw }} pts</dd>
                  </div>
                  <div class="flex justify-between">
                    <dt class="text-sm font-medium text-gray-500">Loss</dt>
                    <dd class="text-sm font-semibold text-red-600">{{ competition()!.pointsForLoss }} pts</dd>
                  </div>
                </dl>
              </div>

              <!-- Entry & Prizes Card -->
              <div class="card">
                <h3 class="text-lg font-semibold text-gray-900 mb-4">Entry & Prizes</h3>
                <dl class="space-y-3">
                  @if (competition()!.entryFee) {
                    <div class="flex justify-between">
                      <dt class="text-sm font-medium text-gray-500">Entry Fee</dt>
                      <dd class="text-sm text-gray-900">{{ competition()!.entryFee | currencyFormat:competition()!.currency }}</dd>
                    </div>
                  }
                  <div class="flex justify-between">
                    <dt class="text-sm font-medium text-gray-500">Has Prizes</dt>
                    <dd class="text-sm text-gray-900">{{ competition()!.hasPrizes ? 'Yes' : 'No' }}</dd>
                  </div>
                  @if (competition()!.totalPrizeMoney) {
                    <div class="flex justify-between">
                      <dt class="text-sm font-medium text-gray-500">Total Prize Money</dt>
                      <dd class="text-sm text-gray-900">{{ competition()!.totalPrizeMoney | currencyFormat:competition()!.currency }}</dd>
                    </div>
                  }
                  @if (competition()!.prizeDescription) {
                    <div>
                      <dt class="text-sm font-medium text-gray-500">Prize Description</dt>
                      <dd class="text-sm text-gray-900">{{ competition()!.prizeDescription }}</dd>
                    </div>
                  }
                </dl>
              </div>
            </div>
          }

          @case ('teams') {
            <div class="space-y-4">
              <div class="flex justify-between items-center">
                <h3 class="text-lg font-semibold text-gray-900">Registered Teams</h3>
                <button (click)="showAddTeamModal.set(true)" class="btn-primary">
                  <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
                  </svg>
                  Add Team
                </button>
              </div>

              @if (isLoadingTeams()) {
                <div class="flex justify-center py-8">
                  <app-loading-spinner size="md" message="Loading teams..."></app-loading-spinner>
                </div>
              } @else if (teams().length === 0) {
                <div class="text-center py-8 text-gray-500">
                  <p>No teams registered yet</p>
                </div>
              } @else {
                <div class="overflow-x-auto">
                  <table class="min-w-full divide-y divide-gray-200">
                    <thead class="bg-gray-50">
                      <tr>
                        <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Team</th>
                        <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Status</th>
                        <th class="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase">P</th>
                        <th class="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase">W</th>
                        <th class="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase">D</th>
                        <th class="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase">L</th>
                        <th class="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase">GD</th>
                        <th class="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase">Pts</th>
                        <th class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Actions</th>
                      </tr>
                    </thead>
                    <tbody class="bg-white divide-y divide-gray-200">
                      @for (team of teams(); track team.id) {
                        <tr>
                          <td class="px-6 py-4 whitespace-nowrap">
                            <div class="flex items-center">
                              @if (team.logoUrl) {
                                <img [src]="team.logoUrl" [alt]="team.name" class="w-8 h-8 rounded-full mr-3" />
                              } @else {
                                <div class="w-8 h-8 rounded-full bg-gray-200 flex items-center justify-center mr-3">
                                  <span class="text-xs font-medium text-gray-500">{{ team.name.charAt(0) }}</span>
                                </div>
                              }
                              <div>
                                <div class="text-sm font-medium text-gray-900">{{ team.name }}</div>
                                @if (team.code) {
                                  <div class="text-sm text-gray-500">{{ team.code }}</div>
                                }
                              </div>
                            </div>
                          </td>
                          <td class="px-6 py-4 whitespace-nowrap">
                            <span class="badge" [class]="getTeamStatusBadgeClass(team.status)">
                              {{ team.status }}
                            </span>
                          </td>
                          <td class="px-6 py-4 whitespace-nowrap text-center text-sm">{{ team.played }}</td>
                          <td class="px-6 py-4 whitespace-nowrap text-center text-sm text-green-600">{{ team.won }}</td>
                          <td class="px-6 py-4 whitespace-nowrap text-center text-sm text-yellow-600">{{ team.drawn }}</td>
                          <td class="px-6 py-4 whitespace-nowrap text-center text-sm text-red-600">{{ team.lost }}</td>
                          <td class="px-6 py-4 whitespace-nowrap text-center text-sm">{{ team.goalDifference }}</td>
                          <td class="px-6 py-4 whitespace-nowrap text-center text-sm font-semibold">{{ team.points }}</td>
                          <td class="px-6 py-4 whitespace-nowrap text-right text-sm">
                            @if (!team.isApproved) {
                              <button (click)="approveTeam(team)" class="text-green-600 hover:text-green-900 mr-3">Approve</button>
                            }
                            <button (click)="withdrawTeam(team)" class="text-red-600 hover:text-red-900">Withdraw</button>
                          </td>
                        </tr>
                      }
                    </tbody>
                  </table>
                </div>
              }
            </div>
          }

          @case ('matches') {
            <div class="space-y-4">
              <div class="flex justify-between items-center">
                <h3 class="text-lg font-semibold text-gray-900">Fixtures & Results</h3>
                <div class="flex gap-2">
                  @if (teams().length >= 2 && matches().length === 0) {
                    <button (click)="generateFixtures()" class="btn-success" [disabled]="isGeneratingFixtures()">
                      @if (isGeneratingFixtures()) {
                        <svg class="animate-spin -ml-1 mr-2 h-4 w-4" fill="none" viewBox="0 0 24 24">
                          <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                          <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z"></path>
                        </svg>
                      }
                      Generate Fixtures
                    </button>
                  }
                  <button (click)="recalculateStandings()" class="btn-secondary">
                    <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
                    </svg>
                    Recalculate
                  </button>
                </div>
              </div>

              @if (isLoadingMatches()) {
                <div class="flex justify-center py-8">
                  <app-loading-spinner size="md" message="Loading matches..."></app-loading-spinner>
                </div>
              } @else if (matches().length === 0) {
                <div class="text-center py-8 text-gray-500">
                  <p>No matches scheduled yet</p>
                  @if (teams().length >= 2) {
                    <p class="mt-2">Click "Generate Fixtures" to create the match schedule</p>
                  } @else {
                    <p class="mt-2">You need at least 2 teams to generate fixtures</p>
                  }
                </div>
              } @else {
                <div class="space-y-3">
                  @for (match of matches(); track match.id) {
                    <div class="card p-4 hover:shadow-md transition-shadow cursor-pointer" (click)="openMatchDetail(match)">
                      <div class="flex items-center justify-between">
                        <div class="flex-1">
                          @if (match.roundName) {
                            <p class="text-xs text-gray-500 mb-1">{{ match.roundName }}</p>
                          }
                          <div class="flex items-center gap-4">
                            <span class="flex-1 text-right font-medium" [class.text-green-600]="match.result === 'HomeWin'">
                              {{ match.homeTeamName || 'TBD' }}
                            </span>
                            <div class="px-4 py-2 bg-gray-100 rounded-lg min-w-[80px] text-center">
                              @if (match.status === 'Completed') {
                                <span class="font-bold">{{ match.homeScore }} - {{ match.awayScore }}</span>
                              } @else {
                                <span class="text-sm text-gray-500">vs</span>
                              }
                            </div>
                            <span class="flex-1 font-medium" [class.text-green-600]="match.result === 'AwayWin'">
                              {{ match.awayTeamName || 'TBD' }}
                            </span>
                          </div>
                        </div>
                        <div class="ml-4 text-right">
                          <span class="badge" [class]="getMatchStatusBadgeClass(match.status)">
                            {{ match.status }}
                          </span>
                          @if (match.scheduledDateTime) {
                            <p class="text-xs text-gray-500 mt-1">{{ match.scheduledDateTime | dateFormat:'datetime' }}</p>
                          }
                        </div>
                      </div>
                    </div>
                  }
                </div>
              }
            </div>
          }

          @case ('standings') {
            <div class="space-y-4">
              <div class="flex justify-between items-center">
                <h3 class="text-lg font-semibold text-gray-900">League Table</h3>
                <button (click)="recalculateStandings()" class="btn-secondary">
                  <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
                  </svg>
                  Recalculate
                </button>
              </div>

              @if (isLoadingStandings()) {
                <div class="flex justify-center py-8">
                  <app-loading-spinner size="md" message="Loading standings..."></app-loading-spinner>
                </div>
              } @else if (standings().length === 0) {
                <div class="text-center py-8 text-gray-500">
                  <p>No standings available yet</p>
                </div>
              } @else {
                <div class="overflow-x-auto">
                  <table class="min-w-full divide-y divide-gray-200">
                    <thead class="bg-gray-50">
                      <tr>
                        <th class="px-4 py-3 text-center text-xs font-medium text-gray-500 uppercase w-12">#</th>
                        <th class="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">Team</th>
                        <th class="px-4 py-3 text-center text-xs font-medium text-gray-500 uppercase">P</th>
                        <th class="px-4 py-3 text-center text-xs font-medium text-gray-500 uppercase">W</th>
                        <th class="px-4 py-3 text-center text-xs font-medium text-gray-500 uppercase">D</th>
                        <th class="px-4 py-3 text-center text-xs font-medium text-gray-500 uppercase">L</th>
                        <th class="px-4 py-3 text-center text-xs font-medium text-gray-500 uppercase">GF</th>
                        <th class="px-4 py-3 text-center text-xs font-medium text-gray-500 uppercase">GA</th>
                        <th class="px-4 py-3 text-center text-xs font-medium text-gray-500 uppercase">GD</th>
                        <th class="px-4 py-3 text-center text-xs font-medium text-gray-500 uppercase">Pts</th>
                        <th class="px-4 py-3 text-center text-xs font-medium text-gray-500 uppercase">Form</th>
                      </tr>
                    </thead>
                    <tbody class="bg-white divide-y divide-gray-200">
                      @for (standing of standings(); track standing.id) {
                        <tr [class.bg-green-50]="standing.isPromoted" [class.bg-red-50]="standing.isRelegated">
                          <td class="px-4 py-3 text-center">
                            <span class="font-semibold" [class.text-green-600]="standing.position === 1">
                              {{ standing.position }}
                            </span>
                            @if (standing.previousPosition && standing.previousPosition !== standing.position) {
                              @if (standing.previousPosition > standing.position) {
                                <span class="text-green-500 ml-1">&#9650;</span>
                              } @else {
                                <span class="text-red-500 ml-1">&#9660;</span>
                              }
                            }
                          </td>
                          <td class="px-4 py-3">
                            <div class="flex items-center">
                              @if (standing.teamLogo) {
                                <img [src]="standing.teamLogo" [alt]="standing.teamName" class="w-6 h-6 rounded-full mr-2" />
                              }
                              <span class="font-medium">{{ standing.teamName }}</span>
                            </div>
                          </td>
                          <td class="px-4 py-3 text-center">{{ standing.played }}</td>
                          <td class="px-4 py-3 text-center text-green-600">{{ standing.won }}</td>
                          <td class="px-4 py-3 text-center text-yellow-600">{{ standing.drawn }}</td>
                          <td class="px-4 py-3 text-center text-red-600">{{ standing.lost }}</td>
                          <td class="px-4 py-3 text-center">{{ standing.goalsFor }}</td>
                          <td class="px-4 py-3 text-center">{{ standing.goalsAgainst }}</td>
                          <td class="px-4 py-3 text-center" [class.text-green-600]="standing.goalDifference > 0" [class.text-red-600]="standing.goalDifference < 0">
                            {{ standing.goalDifference > 0 ? '+' : '' }}{{ standing.goalDifference }}
                          </td>
                          <td class="px-4 py-3 text-center font-bold">{{ standing.totalPoints }}</td>
                          <td class="px-4 py-3 text-center">
                            @if (standing.form) {
                              <div class="flex justify-center gap-1">
                                @for (result of standing.form.split(''); track $index) {
                                  <span
                                    class="w-5 h-5 rounded-full text-xs flex items-center justify-center text-white"
                                    [class.bg-green-500]="result === 'W'"
                                    [class.bg-yellow-500]="result === 'D'"
                                    [class.bg-red-500]="result === 'L'"
                                  >
                                    {{ result }}
                                  </span>
                                }
                              </div>
                            }
                          </td>
                        </tr>
                      }
                    </tbody>
                  </table>
                </div>
              }
            </div>
          }

          @case ('scorers') {
            <div class="space-y-4">
              <h3 class="text-lg font-semibold text-gray-900">Top Scorers</h3>

              @if (isLoadingScorers()) {
                <div class="flex justify-center py-8">
                  <app-loading-spinner size="md" message="Loading top scorers..."></app-loading-spinner>
                </div>
              } @else if (topScorers().length === 0) {
                <div class="text-center py-8 text-gray-500">
                  <p>No goals scored yet</p>
                  <p class="text-sm mt-2">Record match results with goal events to see the leaderboard</p>
                </div>
              } @else {
                <div class="overflow-x-auto">
                  <table class="min-w-full divide-y divide-gray-200">
                    <thead class="bg-gray-50">
                      <tr>
                        <th class="px-4 py-3 text-center text-xs font-medium text-gray-500 uppercase w-12">#</th>
                        <th class="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">Player</th>
                        <th class="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">Team</th>
                        <th class="px-4 py-3 text-center text-xs font-medium text-gray-500 uppercase">Goals</th>
                        <th class="px-4 py-3 text-center text-xs font-medium text-gray-500 uppercase">Assists</th>
                        <th class="px-4 py-3 text-center text-xs font-medium text-gray-500 uppercase">Penalties</th>
                        <th class="px-4 py-3 text-center text-xs font-medium text-gray-500 uppercase">Apps</th>
                      </tr>
                    </thead>
                    <tbody class="bg-white divide-y divide-gray-200">
                      @for (scorer of topScorers(); track scorer.rank) {
                        <tr [class.bg-yellow-50]="scorer.rank <= 3">
                          <td class="px-4 py-3 text-center">
                            @if (scorer.rank === 1) {
                              <span class="text-2xl">🥇</span>
                            } @else if (scorer.rank === 2) {
                              <span class="text-2xl">🥈</span>
                            } @else if (scorer.rank === 3) {
                              <span class="text-2xl">🥉</span>
                            } @else {
                              <span class="font-semibold text-gray-600">{{ scorer.rank }}</span>
                            }
                          </td>
                          <td class="px-4 py-3">
                            <span class="font-medium text-gray-900">{{ scorer.playerName }}</span>
                          </td>
                          <td class="px-4 py-3">
                            <div class="flex items-center">
                              @if (scorer.teamLogo) {
                                <img [src]="scorer.teamLogo" [alt]="scorer.teamName" class="w-6 h-6 rounded-full mr-2" />
                              }
                              <span class="text-gray-600">{{ scorer.teamName || 'Unknown' }}</span>
                            </div>
                          </td>
                          <td class="px-4 py-3 text-center">
                            <span class="font-bold text-lg text-primary-600">{{ scorer.goals }}</span>
                          </td>
                          <td class="px-4 py-3 text-center text-gray-600">{{ scorer.assists }}</td>
                          <td class="px-4 py-3 text-center text-gray-600">{{ scorer.penalties }}</td>
                          <td class="px-4 py-3 text-center text-gray-600">{{ scorer.appearances }}</td>
                        </tr>
                      }
                    </tbody>
                  </table>
                </div>
              }
            </div>
          }
        }
      }
    </div>

    <!-- Delete Confirmation Dialog -->
    @if (showDeleteConfirm()) {
      <app-confirm-dialog
        title="Delete Competition"
        message="Are you sure you want to delete this competition? This action cannot be undone and will remove all associated teams, matches, and standings."
        confirmText="Delete"
        confirmClass="btn-danger"
        (confirm)="deleteCompetition()"
        (cancel)="showDeleteConfirm.set(false)"
      ></app-confirm-dialog>
    }

    <!-- Add Team Modal -->
    @if (showAddTeamModal()) {
      <div class="fixed inset-0 z-50 overflow-y-auto">
        <div class="flex items-center justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
          <div class="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" (click)="closeAddTeamModal()"></div>
          <span class="hidden sm:inline-block sm:align-middle sm:h-screen">&#8203;</span>
          <div class="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full">
            <form [formGroup]="teamForm" (ngSubmit)="registerTeam()">
              <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
                <div class="flex items-center justify-between mb-4">
                  <h3 class="text-lg font-semibold text-gray-900">Register New Team</h3>
                  <button type="button" (click)="closeAddTeamModal()" class="text-gray-400 hover:text-gray-500">
                    <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                    </svg>
                  </button>
                </div>
                <div class="space-y-4">
                  <div>
                    <label class="block text-sm font-medium text-gray-700 mb-1">Team Name *</label>
                    <input type="text" formControlName="name" class="input-field" placeholder="Enter team name" />
                    @if (teamForm.get('name')?.touched && teamForm.get('name')?.errors?.['required']) {
                      <p class="text-red-500 text-xs mt-1">Team name is required</p>
                    }
                  </div>
                  <div class="grid grid-cols-2 gap-4">
                    <div>
                      <label class="block text-sm font-medium text-gray-700 mb-1">Short Name</label>
                      <input type="text" formControlName="shortName" class="input-field" placeholder="e.g., FCB" maxlength="10" />
                    </div>
                    <div>
                      <label class="block text-sm font-medium text-gray-700 mb-1">Code</label>
                      <input type="text" formControlName="code" class="input-field" placeholder="e.g., TEAM001" />
                    </div>
                  </div>
                  <div>
                    <label class="block text-sm font-medium text-gray-700 mb-1">Captain Name</label>
                    <input type="text" formControlName="captainName" class="input-field" placeholder="Team captain's name" />
                  </div>
                  <div class="grid grid-cols-2 gap-4">
                    <div>
                      <label class="block text-sm font-medium text-gray-700 mb-1">Contact Email</label>
                      <input type="email" formControlName="contactEmail" class="input-field" placeholder="team@example.com" />
                      @if (teamForm.get('contactEmail')?.touched && teamForm.get('contactEmail')?.errors?.['email']) {
                        <p class="text-red-500 text-xs mt-1">Invalid email format</p>
                      }
                    </div>
                    <div>
                      <label class="block text-sm font-medium text-gray-700 mb-1">Contact Phone</label>
                      <input type="tel" formControlName="contactPhone" class="input-field" placeholder="+1 234 567 8900" />
                    </div>
                  </div>
                  <div>
                    <label class="block text-sm font-medium text-gray-700 mb-1">Group</label>
                    <input type="text" formControlName="group" class="input-field" placeholder="e.g., Group A" />
                  </div>
                  <div>
                    <label class="block text-sm font-medium text-gray-700 mb-1">Logo URL</label>
                    <input type="url" formControlName="logoUrl" class="input-field" placeholder="https://example.com/logo.png" />
                  </div>
                </div>
              </div>
              <div class="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse gap-2">
                <button
                  type="submit"
                  [disabled]="teamForm.invalid || isRegisteringTeam()"
                  class="btn-primary"
                  [class.opacity-50]="teamForm.invalid || isRegisteringTeam()"
                >
                  @if (isRegisteringTeam()) {
                    <svg class="animate-spin -ml-1 mr-2 h-4 w-4" fill="none" viewBox="0 0 24 24">
                      <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                      <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z"></path>
                    </svg>
                    Registering...
                  } @else {
                    Register Team
                  }
                </button>
                <button type="button" (click)="closeAddTeamModal()" class="btn-secondary">
                  Cancel
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    }

    <!-- Match Result Modal -->
    @if (showMatchResultModal() && selectedMatch()) {
      <div class="fixed inset-0 z-50 overflow-y-auto">
        <div class="flex items-center justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
          <div class="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" (click)="closeMatchResultModal()"></div>
          <span class="hidden sm:inline-block sm:align-middle sm:h-screen">&#8203;</span>
          <div class="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-2xl sm:w-full max-h-[90vh] overflow-y-auto">
            <form [formGroup]="matchResultForm" (ngSubmit)="submitMatchResult()">
              <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
                <div class="flex items-center justify-between mb-4">
                  <h3 class="text-lg font-semibold text-gray-900">Record Match Result</h3>
                  <button type="button" (click)="closeMatchResultModal()" class="text-gray-400 hover:text-gray-500">
                    <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                    </svg>
                  </button>
                </div>

                <!-- Match Info -->
                <div class="bg-gray-50 rounded-lg p-4 mb-4">
                  @if (selectedMatch()!.roundName) {
                    <p class="text-xs text-gray-500 text-center mb-2">{{ selectedMatch()!.roundName }}</p>
                  }
                  <div class="flex items-center justify-center gap-4">
                    <span class="flex-1 text-right font-medium text-gray-900">{{ selectedMatch()!.homeTeamName || 'TBD' }}</span>
                    <span class="text-gray-400">vs</span>
                    <span class="flex-1 font-medium text-gray-900">{{ selectedMatch()!.awayTeamName || 'TBD' }}</span>
                  </div>
                  @if (selectedMatch()!.scheduledDateTime) {
                    <p class="text-xs text-gray-500 text-center mt-2">{{ selectedMatch()!.scheduledDateTime | dateFormat:'datetime' }}</p>
                  }
                </div>

                <div class="space-y-4">
                  <!-- Score Entry -->
                  <div>
                    <label class="block text-sm font-medium text-gray-700 mb-2 text-center">Final Score</label>
                    <div class="flex items-center justify-center gap-4">
                      <div class="text-center">
                        <p class="text-xs text-gray-500 mb-1">{{ selectedMatch()!.homeTeamName || 'Home' }}</p>
                        <input
                          type="number"
                          formControlName="homeScore"
                          min="0"
                          class="input-field w-20 text-center text-2xl font-bold"
                        />
                      </div>
                      <span class="text-2xl font-bold text-gray-400 mt-4">-</span>
                      <div class="text-center">
                        <p class="text-xs text-gray-500 mb-1">{{ selectedMatch()!.awayTeamName || 'Away' }}</p>
                        <input
                          type="number"
                          formControlName="awayScore"
                          min="0"
                          class="input-field w-20 text-center text-2xl font-bold"
                        />
                      </div>
                    </div>
                  </div>

                  <!-- Half Time Score (Optional) -->
                  <div>
                    <label class="block text-sm font-medium text-gray-700 mb-2 text-center">Half Time Score (Optional)</label>
                    <div class="flex items-center justify-center gap-4">
                      <input
                        type="number"
                        formControlName="homeHalfTimeScore"
                        min="0"
                        placeholder="HT"
                        class="input-field w-16 text-center"
                      />
                      <span class="text-gray-400">-</span>
                      <input
                        type="number"
                        formControlName="awayHalfTimeScore"
                        min="0"
                        placeholder="HT"
                        class="input-field w-16 text-center"
                      />
                    </div>
                  </div>

                  <!-- Match Report -->
                  <div>
                    <label class="block text-sm font-medium text-gray-700 mb-1">Match Report (Optional)</label>
                    <textarea
                      formControlName="matchReport"
                      rows="2"
                      class="input-field"
                      placeholder="Add any notes about the match..."
                    ></textarea>
                  </div>
                </div>
              </div>

              <!-- Match Events Section -->
              <div class="border-t border-gray-200 px-4 py-4 sm:px-6">
                <h4 class="text-sm font-semibold text-gray-900 mb-3">Match Events (Goals, Cards)</h4>

                <!-- Add Event Form -->
                <div class="bg-gray-50 rounded-lg p-3 mb-3" [formGroup]="eventForm">
                  <div class="grid grid-cols-12 gap-2">
                    <div class="col-span-3">
                      <select formControlName="eventType" class="input-field text-sm py-1.5">
                        @for (et of eventTypes; track et.value) {
                          <option [value]="et.value">{{ et.icon }} {{ et.label }}</option>
                        }
                      </select>
                    </div>
                    <div class="col-span-2">
                      <input
                        type="number"
                        formControlName="minute"
                        min="1"
                        max="120"
                        class="input-field text-sm py-1.5 text-center"
                        placeholder="Min"
                      />
                    </div>
                    <div class="col-span-3">
                      <select formControlName="teamId" class="input-field text-sm py-1.5">
                        <option value="">Team</option>
                        @if (selectedMatch()) {
                          <option [value]="selectedMatch()!.homeTeamId || ''">{{ selectedMatch()!.homeTeamName }}</option>
                          <option [value]="selectedMatch()!.awayTeamId || ''">{{ selectedMatch()!.awayTeamName }}</option>
                        }
                      </select>
                    </div>
                    <div class="col-span-4">
                      <input
                        type="text"
                        formControlName="playerName"
                        class="input-field text-sm py-1.5"
                        placeholder="Player name"
                      />
                    </div>
                  </div>
                  @if (eventForm.value.eventType === 'goal') {
                    <div class="mt-2">
                      <input
                        type="text"
                        formControlName="assistPlayerName"
                        class="input-field text-sm py-1.5"
                        placeholder="Assist by (optional)"
                      />
                    </div>
                  }
                  <div class="mt-2 flex justify-end">
                    <button
                      type="button"
                      (click)="addMatchEvent()"
                      [disabled]="eventForm.invalid || isAddingEvent()"
                      class="btn-primary text-sm py-1.5 px-3"
                      [class.opacity-50]="eventForm.invalid || isAddingEvent()"
                    >
                      @if (isAddingEvent()) {
                        Adding...
                      } @else {
                        + Add Event
                      }
                    </button>
                  </div>
                </div>

                <!-- Events List -->
                @if (isLoadingEvents()) {
                  <div class="text-center py-4">
                    <span class="text-sm text-gray-500">Loading events...</span>
                  </div>
                } @else if (matchEvents().length === 0) {
                  <div class="text-center py-4 text-sm text-gray-500">
                    No events recorded yet
                  </div>
                } @else {
                  <div class="space-y-2 max-h-48 overflow-y-auto">
                    @for (event of matchEvents(); track event.id) {
                      <div class="flex items-center justify-between bg-white border border-gray-200 rounded-lg px-3 py-2">
                        <div class="flex items-center gap-3">
                          <span class="text-lg">{{ getEventIcon(event.eventType) }}</span>
                          <span class="font-mono text-sm text-gray-500 w-10">{{ event.minute }}'</span>
                          <div>
                            <span class="text-sm font-medium">{{ event.description || event.participantName || 'Unknown' }}</span>
                            @if (event.teamName) {
                              <span class="text-xs text-gray-500 ml-1">({{ event.teamName }})</span>
                            }
                          </div>
                        </div>
                        <button
                          type="button"
                          (click)="removeMatchEvent(event.id)"
                          class="text-red-500 hover:text-red-700 p-1"
                          title="Remove event"
                        >
                          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                          </svg>
                        </button>
                      </div>
                    }
                  </div>
                }
              </div>

              <div class="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse gap-2">
                <button
                  type="submit"
                  [disabled]="matchResultForm.invalid || isSubmittingResult()"
                  class="btn-success"
                  [class.opacity-50]="matchResultForm.invalid || isSubmittingResult()"
                >
                  @if (isSubmittingResult()) {
                    <svg class="animate-spin -ml-1 mr-2 h-4 w-4" fill="none" viewBox="0 0 24 24">
                      <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                      <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z"></path>
                    </svg>
                    Saving...
                  } @else {
                    Save Result
                  }
                </button>
                <button type="button" (click)="closeMatchResultModal()" class="btn-secondary">
                  Cancel
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    }
  `
})
export class CompetitionDetailComponent implements OnInit {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private fb = inject(FormBuilder);
  private competitionService = inject(CompetitionService);
  private notificationService = inject(NotificationService);

  isLoading = signal(true);
  isLoadingTeams = signal(false);
  isLoadingMatches = signal(false);
  isLoadingStandings = signal(false);
  isLoadingScorers = signal(false);
  isGeneratingFixtures = signal(false);
  isRegisteringTeam = signal(false);
  isSubmittingResult = signal(false);
  showDeleteConfirm = signal(false);
  showAddTeamModal = signal(false);
  showMatchResultModal = signal(false);

  selectedMatch = signal<MatchListItem | null>(null);

  teamForm: FormGroup = this.fb.group({
    name: ['', Validators.required],
    shortName: [''],
    code: [''],
    captainName: [''],
    contactEmail: ['', Validators.email],
    contactPhone: [''],
    group: [''],
    logoUrl: ['']
  });

  matchResultForm: FormGroup = this.fb.group({
    homeScore: [0, [Validators.required, Validators.min(0)]],
    awayScore: [0, [Validators.required, Validators.min(0)]],
    homeHalfTimeScore: [null],
    awayHalfTimeScore: [null],
    matchReport: ['']
  });

  eventForm: FormGroup = this.fb.group({
    eventType: ['goal', Validators.required],
    minute: [1, [Validators.required, Validators.min(1), Validators.max(120)]],
    teamId: ['', Validators.required],
    playerName: ['', Validators.required],
    assistPlayerName: [''],
    description: ['']
  });

  matchEvents = signal<MatchEvent[]>([]);
  isLoadingEvents = signal(false);
  isAddingEvent = signal(false);

  eventTypes = [
    { value: 'goal', label: 'Goal', icon: '⚽' },
    { value: 'yellowcard', label: 'Yellow Card', icon: '🟨' },
    { value: 'redcard', label: 'Red Card', icon: '🟥' },
    { value: 'owngoal', label: 'Own Goal', icon: '⚽🔴' },
    { value: 'penalty', label: 'Penalty', icon: '🎯' }
  ];

  competition = signal<Competition | null>(null);
  teams = signal<CompetitionTeam[]>([]);
  matches = signal<MatchListItem[]>([]);
  standings = signal<CompetitionStanding[]>([]);
  topScorers = signal<TopScorer[]>([]);
  activeTab = signal<TabType>('overview');

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadCompetition(id);
    }
  }

  loadCompetition(id: string): void {
    this.competitionService.getCompetition(id).subscribe({
      next: (competition) => {
        this.competition.set(competition);
        this.isLoading.set(false);
        this.loadTeams();
      },
      error: () => {
        this.notificationService.error('Failed to load competition');
        this.router.navigate(['/club/competitions']);
      }
    });
  }

  loadTeams(): void {
    const comp = this.competition();
    if (!comp) return;

    this.isLoadingTeams.set(true);
    this.competitionService.getTeams(comp.id).subscribe({
      next: (teams) => {
        this.teams.set(teams);
        this.isLoadingTeams.set(false);
      },
      error: () => {
        this.isLoadingTeams.set(false);
      }
    });
  }

  loadMatches(): void {
    const comp = this.competition();
    if (!comp) return;

    this.isLoadingMatches.set(true);
    this.competitionService.getMatches(comp.id).subscribe({
      next: (result) => {
        this.matches.set(result.items);
        this.isLoadingMatches.set(false);
      },
      error: () => {
        this.isLoadingMatches.set(false);
      }
    });
  }

  loadStandings(): void {
    const comp = this.competition();
    if (!comp) return;

    this.isLoadingStandings.set(true);
    this.competitionService.getStandings(comp.id).subscribe({
      next: (standings) => {
        this.standings.set(standings);
        this.isLoadingStandings.set(false);
      },
      error: () => {
        this.isLoadingStandings.set(false);
      }
    });
  }

  loadTopScorers(): void {
    const comp = this.competition();
    if (!comp) return;

    this.isLoadingScorers.set(true);
    this.competitionService.getTopScorers(comp.id).subscribe({
      next: (scorers) => {
        this.topScorers.set(scorers);
        this.isLoadingScorers.set(false);
      },
      error: () => {
        this.topScorers.set([]);
        this.isLoadingScorers.set(false);
      }
    });
  }

  publishCompetition(): void {
    const comp = this.competition();
    if (!comp) return;

    this.competitionService.publishCompetition(comp.id).subscribe({
      next: () => {
        this.notificationService.success('Competition published successfully');
        this.loadCompetition(comp.id);
      },
      error: () => {
        this.notificationService.error('Failed to publish competition');
      }
    });
  }

  deleteCompetition(): void {
    const comp = this.competition();
    if (!comp) return;

    this.competitionService.deleteCompetition(comp.id).subscribe({
      next: () => {
        this.notificationService.success('Competition deleted successfully');
        this.router.navigate(['/club/competitions']);
      },
      error: () => {
        this.notificationService.error('Failed to delete competition');
        this.showDeleteConfirm.set(false);
      }
    });
  }

  approveTeam(team: CompetitionTeam): void {
    const comp = this.competition();
    if (!comp) return;

    this.competitionService.approveTeam(comp.id, team.id).subscribe({
      next: () => {
        this.notificationService.success(`${team.name} approved`);
        this.loadTeams();
      },
      error: () => {
        this.notificationService.error('Failed to approve team');
      }
    });
  }

  withdrawTeam(team: CompetitionTeam): void {
    const comp = this.competition();
    if (!comp) return;

    this.competitionService.withdrawTeam(comp.id, team.id).subscribe({
      next: () => {
        this.notificationService.success(`${team.name} withdrawn`);
        this.loadTeams();
      },
      error: () => {
        this.notificationService.error('Failed to withdraw team');
      }
    });
  }

  generateFixtures(): void {
    const comp = this.competition();
    if (!comp) return;

    this.isGeneratingFixtures.set(true);
    const request: GenerateFixturesRequest = {
      homeAndAway: true,
      randomizeOrder: true
    };

    this.competitionService.generateFixtures(comp.id, request).subscribe({
      next: (matches) => {
        this.notificationService.success(`${matches.length} fixtures generated`);
        this.loadMatches();
        this.loadCompetition(comp.id);
        this.isGeneratingFixtures.set(false);
      },
      error: () => {
        this.notificationService.error('Failed to generate fixtures');
        this.isGeneratingFixtures.set(false);
      }
    });
  }

  recalculateStandings(): void {
    const comp = this.competition();
    if (!comp) return;

    this.competitionService.recalculateStandings(comp.id).subscribe({
      next: () => {
        this.notificationService.success('Standings recalculated');
        this.loadStandings();
        this.loadTeams();
      },
      error: () => {
        this.notificationService.error('Failed to recalculate standings');
      }
    });
  }

  openMatchDetail(match: MatchListItem): void {
    this.selectedMatch.set(match);
    // Pre-fill the form with existing scores if available
    this.matchResultForm.patchValue({
      homeScore: match.homeScore ?? 0,
      awayScore: match.awayScore ?? 0,
      homeHalfTimeScore: null,
      awayHalfTimeScore: null,
      matchReport: ''
    });
    // Reset event form with home team as default
    this.eventForm.patchValue({
      eventType: 'goal',
      minute: 1,
      teamId: '',
      playerName: '',
      assistPlayerName: '',
      description: ''
    });
    this.showMatchResultModal.set(true);
    this.loadMatchEvents(match.id);
  }

  loadMatchEvents(matchId: string): void {
    const comp = this.competition();
    if (!comp) return;

    this.isLoadingEvents.set(true);
    this.competitionService.getMatchEvents(comp.id, matchId).subscribe({
      next: (events) => {
        this.matchEvents.set(events);
        this.isLoadingEvents.set(false);
      },
      error: () => {
        this.matchEvents.set([]);
        this.isLoadingEvents.set(false);
      }
    });
  }

  closeMatchResultModal(): void {
    this.showMatchResultModal.set(false);
    this.selectedMatch.set(null);
    this.matchEvents.set([]);
    this.matchResultForm.reset({
      homeScore: 0,
      awayScore: 0,
      homeHalfTimeScore: null,
      awayHalfTimeScore: null,
      matchReport: ''
    });
    this.eventForm.reset({
      eventType: 'goal',
      minute: 1,
      teamId: '',
      playerName: '',
      assistPlayerName: '',
      description: ''
    });
  }

  addMatchEvent(): void {
    if (this.eventForm.invalid) return;

    const comp = this.competition();
    const match = this.selectedMatch();
    if (!comp || !match) return;

    this.isAddingEvent.set(true);
    const formValue = this.eventForm.value;

    const request: MatchEventCreateRequest = {
      eventType: formValue.eventType,
      minute: formValue.minute,
      teamId: formValue.teamId || undefined,
      description: formValue.playerName + (formValue.assistPlayerName ? ` (assist: ${formValue.assistPlayerName})` : '')
    };

    this.competitionService.addMatchEvent(comp.id, match.id, request).subscribe({
      next: (event) => {
        this.matchEvents.update(events => [...events, event].sort((a, b) => a.minute - b.minute));
        this.eventForm.patchValue({
          minute: formValue.minute,
          playerName: '',
          assistPlayerName: '',
          description: ''
        });
        this.isAddingEvent.set(false);
        this.notificationService.success('Event added');
      },
      error: () => {
        this.notificationService.error('Failed to add event');
        this.isAddingEvent.set(false);
      }
    });
  }

  removeMatchEvent(eventId: string): void {
    const comp = this.competition();
    const match = this.selectedMatch();
    if (!comp || !match) return;

    this.competitionService.deleteMatchEvent(comp.id, match.id, eventId).subscribe({
      next: () => {
        this.matchEvents.update(events => events.filter(e => e.id !== eventId));
        this.notificationService.success('Event removed');
      },
      error: () => {
        this.notificationService.error('Failed to remove event');
      }
    });
  }

  getEventIcon(eventType: string): string {
    const icons: Record<string, string> = {
      'goal': '⚽',
      'yellowcard': '🟨',
      'redcard': '🟥',
      'owngoal': '⚽🔴',
      'penalty': '🎯',
      'substitution': '🔄'
    };
    return icons[eventType.toLowerCase()] || '📋';
  }

  getEventLabel(eventType: string): string {
    const labels: Record<string, string> = {
      'goal': 'Goal',
      'yellowcard': 'Yellow Card',
      'redcard': 'Red Card',
      'owngoal': 'Own Goal',
      'penalty': 'Penalty',
      'substitution': 'Substitution'
    };
    return labels[eventType.toLowerCase()] || eventType;
  }

  submitMatchResult(): void {
    if (this.matchResultForm.invalid) return;

    const comp = this.competition();
    const match = this.selectedMatch();
    if (!comp || !match) return;

    this.isSubmittingResult.set(true);
    const request: MatchResultRequest = {
      homeScore: this.matchResultForm.value.homeScore,
      awayScore: this.matchResultForm.value.awayScore,
      homeHalfTimeScore: this.matchResultForm.value.homeHalfTimeScore || undefined,
      awayHalfTimeScore: this.matchResultForm.value.awayHalfTimeScore || undefined,
      matchReport: this.matchResultForm.value.matchReport || undefined
    };

    this.competitionService.recordMatchResult(comp.id, match.id, request).subscribe({
      next: () => {
        this.notificationService.success('Match result recorded successfully');
        this.closeMatchResultModal();
        this.loadMatches();
        this.loadStandings();
        this.loadTeams();
        this.isSubmittingResult.set(false);
      },
      error: () => {
        this.notificationService.error('Failed to record match result');
        this.isSubmittingResult.set(false);
      }
    });
  }

  formatType(type: CompetitionType): string {
    const labels: Record<CompetitionType, string> = {
      [CompetitionType.League]: 'League',
      [CompetitionType.Tournament]: 'Tournament',
      [CompetitionType.Cup]: 'Cup',
      [CompetitionType.Friendly]: 'Friendly',
      [CompetitionType.Championship]: 'Championship',
      [CompetitionType.Qualifier]: 'Qualifier',
      [CompetitionType.Playoff]: 'Playoff',
      [CompetitionType.RoundRobin]: 'Round Robin',
      [CompetitionType.Ladder]: 'Ladder',
      [CompetitionType.TimeTrial]: 'Time Trial',
      [CompetitionType.Other]: 'Other'
    };
    return labels[type] || type;
  }

  formatStatus(status: CompetitionStatus): string {
    const labels: Record<CompetitionStatus, string> = {
      [CompetitionStatus.Draft]: 'Draft',
      [CompetitionStatus.Published]: 'Published',
      [CompetitionStatus.RegistrationOpen]: 'Registration Open',
      [CompetitionStatus.RegistrationClosed]: 'Registration Closed',
      [CompetitionStatus.DrawComplete]: 'Draw Complete',
      [CompetitionStatus.InProgress]: 'In Progress',
      [CompetitionStatus.Completed]: 'Completed',
      [CompetitionStatus.Cancelled]: 'Cancelled',
      [CompetitionStatus.Postponed]: 'Postponed',
      [CompetitionStatus.Archived]: 'Archived'
    };
    return labels[status] || status;
  }

  getTypeBadgeClass(type: CompetitionType): string {
    const classes: Record<CompetitionType, string> = {
      [CompetitionType.League]: 'badge-primary',
      [CompetitionType.Tournament]: 'badge-info',
      [CompetitionType.Cup]: 'badge-warning',
      [CompetitionType.Championship]: 'badge-success',
      [CompetitionType.Friendly]: 'badge-secondary',
      [CompetitionType.Qualifier]: 'badge-info',
      [CompetitionType.Playoff]: 'badge-warning',
      [CompetitionType.RoundRobin]: 'badge-primary',
      [CompetitionType.Ladder]: 'badge-secondary',
      [CompetitionType.TimeTrial]: 'badge-info',
      [CompetitionType.Other]: 'badge-secondary'
    };
    return classes[type] || 'badge-secondary';
  }

  getStatusBadgeClass(status: CompetitionStatus): string {
    const classes: Record<CompetitionStatus, string> = {
      [CompetitionStatus.Draft]: 'badge-secondary',
      [CompetitionStatus.Published]: 'badge-info',
      [CompetitionStatus.RegistrationOpen]: 'badge-success',
      [CompetitionStatus.RegistrationClosed]: 'badge-warning',
      [CompetitionStatus.DrawComplete]: 'badge-info',
      [CompetitionStatus.InProgress]: 'badge-primary',
      [CompetitionStatus.Completed]: 'badge-success',
      [CompetitionStatus.Cancelled]: 'badge-danger',
      [CompetitionStatus.Postponed]: 'badge-warning',
      [CompetitionStatus.Archived]: 'badge-secondary'
    };
    return classes[status] || 'badge-secondary';
  }

  getTeamStatusBadgeClass(status: TeamStatus): string {
    const classes: Record<TeamStatus, string> = {
      [TeamStatus.Registered]: 'badge-info',
      [TeamStatus.Confirmed]: 'badge-success',
      [TeamStatus.Withdrawn]: 'badge-danger',
      [TeamStatus.Disqualified]: 'badge-danger',
      [TeamStatus.Eliminated]: 'badge-warning',
      [TeamStatus.Active]: 'badge-success',
      [TeamStatus.Champion]: 'badge-success',
      [TeamStatus.RunnerUp]: 'badge-info'
    };
    return classes[status] || 'badge-secondary';
  }

  getMatchStatusBadgeClass(status: MatchStatus): string {
    const classes: Record<MatchStatus, string> = {
      [MatchStatus.Scheduled]: 'badge-info',
      [MatchStatus.Confirmed]: 'badge-primary',
      [MatchStatus.InProgress]: 'badge-warning',
      [MatchStatus.Completed]: 'badge-success',
      [MatchStatus.Postponed]: 'badge-warning',
      [MatchStatus.Cancelled]: 'badge-danger',
      [MatchStatus.Walkover]: 'badge-secondary',
      [MatchStatus.Bye]: 'badge-secondary',
      [MatchStatus.Abandoned]: 'badge-danger',
      [MatchStatus.Disputed]: 'badge-warning'
    };
    return classes[status] || 'badge-secondary';
  }

  closeAddTeamModal(): void {
    this.showAddTeamModal.set(false);
    this.teamForm.reset();
  }

  registerTeam(): void {
    if (this.teamForm.invalid) return;

    const comp = this.competition();
    if (!comp) return;

    this.isRegisteringTeam.set(true);
    const request: CompetitionTeamCreateRequest = {
      name: this.teamForm.value.name,
      shortName: this.teamForm.value.shortName || undefined,
      code: this.teamForm.value.code || undefined,
      captainName: this.teamForm.value.captainName || undefined,
      contactEmail: this.teamForm.value.contactEmail || undefined,
      contactPhone: this.teamForm.value.contactPhone || undefined,
      group: this.teamForm.value.group || undefined,
      logoUrl: this.teamForm.value.logoUrl || undefined
    };

    this.competitionService.registerTeam(comp.id, request).subscribe({
      next: () => {
        this.notificationService.success(`Team "${request.name}" registered successfully`);
        this.closeAddTeamModal();
        this.loadTeams();
        this.loadCompetition(comp.id);
        this.isRegisteringTeam.set(false);
      },
      error: () => {
        this.notificationService.error('Failed to register team');
        this.isRegisteringTeam.set(false);
      }
    });
  }
}
