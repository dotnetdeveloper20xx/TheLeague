import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CompetitionService } from '../../../core/services/competition.service';
import { ClubService } from '../../../core/services/club.service';
import { AuthService } from '../../../core/services/auth.service';
import { LoadingSpinnerComponent, EmptyStateComponent, PaginationComponent } from '../../../shared/components';
import { DateFormatPipe } from '../../../shared/pipes';
import {
  CompetitionListItem,
  CompetitionType,
  CompetitionStatus,
  CompetitionFilter,
  PagedResult,
  Club
} from '../../../core/models';

@Component({
  selector: 'app-competitions-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    FormsModule,
    LoadingSpinnerComponent,
    EmptyStateComponent,
    PaginationComponent,
    DateFormatPipe
  ],
  template: `
    <div class="space-y-6">
      <!-- Club Selector for SuperAdmin -->
      @if (showClubSelector()) {
        <div class="card bg-blue-50 border-blue-200">
          <div class="flex items-center justify-between">
            <div>
              <p class="text-sm font-medium text-blue-800">Select a club to manage competitions</p>
              <p class="text-xs text-blue-600 mt-1">As a Super Admin, you can manage any club's competitions</p>
            </div>
            <select
              [(ngModel)]="selectedClubId"
              (change)="onClubChange()"
              class="input-field max-w-xs"
            >
              <option value="">Select a club...</option>
              @for (club of clubs(); track club.id) {
                <option [value]="club.id">{{ club.name }}</option>
              }
            </select>
          </div>
        </div>
      }

      <!-- Header -->
      <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 class="text-2xl font-bold text-gray-900">Competitions</h1>
          <p class="text-gray-500 mt-1">Manage leagues, tournaments, and competitions</p>
        </div>
        <a routerLink="/club/competitions/new" class="btn-primary inline-flex items-center" [class.pointer-events-none]="!hasClubContext()" [class.opacity-50]="!hasClubContext()">
          <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
          </svg>
          New Competition
        </a>
      </div>

      <!-- Filters -->
      <div class="card">
        <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Type</label>
            <select
              [(ngModel)]="filter.type"
              (change)="loadCompetitions()"
              class="input-field"
            >
              <option [ngValue]="undefined">All Types</option>
              @for (type of competitionTypes; track type) {
                <option [value]="type">{{ formatType(type) }}</option>
              }
            </select>
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Status</label>
            <select
              [(ngModel)]="filter.status"
              (change)="loadCompetitions()"
              class="input-field"
            >
              <option [ngValue]="undefined">All Statuses</option>
              @for (status of competitionStatuses; track status) {
                <option [value]="status">{{ formatStatus(status) }}</option>
              }
            </select>
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Sport</label>
            <input
              type="text"
              [(ngModel)]="filter.sport"
              (input)="onSearchInput()"
              placeholder="Filter by sport..."
              class="input-field"
            />
          </div>
          <div class="flex items-end">
            <button (click)="clearFilters()" class="btn-secondary w-full">
              Clear Filters
            </button>
          </div>
        </div>
      </div>

      <!-- Loading State -->
      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg" message="Loading competitions..."></app-loading-spinner>
        </div>
      } @else if (competitions().length === 0) {
        <!-- Empty State -->
        <app-empty-state
          icon="trophy"
          title="No competitions found"
          message="Create your first competition to manage leagues and tournaments."
          actionText="New Competition"
          (action)="navigateToCreate()"
        ></app-empty-state>
      } @else {
        <!-- Competition Grid -->
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          @for (competition of competitions(); track competition.id) {
            <div class="card hover:shadow-lg transition-shadow cursor-pointer" [routerLink]="['/club/competitions', competition.id]">
              <!-- Image/Placeholder -->
              @if (competition.imageUrl) {
                <img [src]="competition.imageUrl" [alt]="competition.name" class="w-full h-40 object-cover rounded-lg mb-4" />
              } @else {
                <div class="w-full h-40 bg-gradient-to-br from-emerald-500 to-emerald-700 rounded-lg mb-4 flex items-center justify-center">
                  <svg class="w-16 h-16 text-white/50" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10" />
                  </svg>
                </div>
              }

              <!-- Badges -->
              <div class="flex flex-wrap items-center gap-2 mb-2">
                <span class="badge" [class]="getTypeBadgeClass(competition.type)">
                  {{ formatType(competition.type) }}
                </span>
                <span class="badge" [class]="getStatusBadgeClass(competition.status)">
                  {{ formatStatus(competition.status) }}
                </span>
                @if (!competition.isPublished) {
                  <span class="badge badge-warning">Draft</span>
                }
              </div>

              <!-- Title -->
              <h3 class="text-lg font-semibold text-gray-900 mb-2">{{ competition.name }}</h3>
              @if (competition.code) {
                <p class="text-sm text-gray-500 mb-2">{{ competition.code }}</p>
              }

              <!-- Details -->
              <div class="space-y-2 text-sm text-gray-600 mb-4">
                @if (competition.sport) {
                  <div class="flex items-center">
                    <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z" />
                    </svg>
                    {{ competition.sport }}
                  </div>
                }
                @if (competition.startDate) {
                  <div class="flex items-center">
                    <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                    </svg>
                    {{ competition.startDate | dateFormat }}
                    @if (competition.endDate) {
                      - {{ competition.endDate | dateFormat }}
                    }
                  </div>
                }
                <div class="flex items-center">
                  <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0z" />
                  </svg>
                  {{ competition.currentTeamCount }} teams
                  @if (competition.maxTeams) {
                    / {{ competition.maxTeams }} max
                  }
                </div>
              </div>

              <!-- Progress Bar for Matches -->
              @if (competition.totalMatches > 0) {
                <div class="mb-4">
                  <div class="flex justify-between text-xs text-gray-500 mb-1">
                    <span>Matches Progress</span>
                    <span>{{ competition.completedMatches }}/{{ competition.totalMatches }}</span>
                  </div>
                  <div class="w-full bg-gray-200 rounded-full h-2">
                    <div
                      class="bg-primary-600 h-2 rounded-full"
                      [style.width.%]="(competition.completedMatches / competition.totalMatches) * 100"
                    ></div>
                  </div>
                </div>
              }

              <!-- Action Button -->
              <a [routerLink]="['/club/competitions', competition.id]" class="btn-primary w-full text-center text-sm">
                Manage Competition
              </a>
            </div>
          }
        </div>

        <!-- Pagination -->
        @if (totalPages() > 1) {
          <app-pagination
            [currentPage]="currentPage()"
            [pageSize]="pageSize"
            [totalCount]="totalItems()"
            (pageChange)="onPageChange($event)"
          ></app-pagination>
        }
      }
    </div>
  `
})
export class CompetitionsListComponent implements OnInit {
  private competitionService = inject(CompetitionService);
  private clubService = inject(ClubService);
  private authService = inject(AuthService);

  isLoading = signal(true);
  competitions = signal<CompetitionListItem[]>([]);
  clubs = signal<Club[]>([]);
  currentPage = signal(1);
  totalPages = signal(1);
  totalItems = signal(0);
  pageSize = 12;
  selectedClubId = '';

  filter: CompetitionFilter = {
    includeUnpublished: true,
    page: 1,
    pageSize: this.pageSize
  };

  private searchTimeout: any;

  competitionTypes = Object.values(CompetitionType);
  competitionStatuses = Object.values(CompetitionStatus);

  ngOnInit(): void {
    // Load clubs for SuperAdmin
    if (this.authService.isSuperAdmin) {
      this.loadClubs();
    }

    // Check if there's a selected club or user has clubId
    const savedClubId = this.competitionService.getSelectedClubId();
    if (savedClubId) {
      this.selectedClubId = savedClubId;
      this.loadCompetitions();
    } else if (!this.authService.isSuperAdmin) {
      // Club manager without clubId - shouldn't happen but handle gracefully
      this.isLoading.set(false);
    } else {
      // SuperAdmin needs to select a club
      this.isLoading.set(false);
    }
  }

  showClubSelector(): boolean {
    return this.authService.isSuperAdmin && !this.authService.currentUser?.clubId;
  }

  hasClubContext(): boolean {
    return !!this.competitionService.getSelectedClubId();
  }

  loadClubs(): void {
    this.clubService.getClubs().subscribe({
      next: (result) => {
        this.clubs.set(result.items);
        // Auto-select first club if none selected
        if (!this.selectedClubId && result.items.length > 0) {
          this.selectedClubId = result.items[0].id;
          this.competitionService.setSelectedClubId(this.selectedClubId);
          this.loadCompetitions();
        }
      },
      error: () => {}
    });
  }

  onClubChange(): void {
    if (this.selectedClubId) {
      this.competitionService.setSelectedClubId(this.selectedClubId);
      this.loadCompetitions();
    }
  }

  loadCompetitions(): void {
    this.isLoading.set(true);
    this.filter.page = this.currentPage();

    this.competitionService.getCompetitions(this.filter).subscribe({
      next: (result: PagedResult<CompetitionListItem>) => {
        this.competitions.set(result.items);
        this.totalPages.set(result.totalPages);
        this.totalItems.set(result.totalCount);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }

  onSearchInput(): void {
    clearTimeout(this.searchTimeout);
    this.searchTimeout = setTimeout(() => {
      this.currentPage.set(1);
      this.loadCompetitions();
    }, 300);
  }

  onPageChange(page: number): void {
    this.currentPage.set(page);
    this.loadCompetitions();
  }

  clearFilters(): void {
    this.filter = {
      includeUnpublished: true,
      page: 1,
      pageSize: this.pageSize
    };
    this.currentPage.set(1);
    this.loadCompetitions();
  }

  navigateToCreate(): void {
    window.location.href = '/club/competitions/new';
  }

  formatType(type: CompetitionType): string {
    const typeLabels: Record<CompetitionType, string> = {
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
    return typeLabels[type] || type;
  }

  formatStatus(status: CompetitionStatus): string {
    const statusLabels: Record<CompetitionStatus, string> = {
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
    return statusLabels[status] || status;
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
}
