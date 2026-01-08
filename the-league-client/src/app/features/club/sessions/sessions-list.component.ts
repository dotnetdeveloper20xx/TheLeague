import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SessionService } from '../../../core/services/session.service';
import { LoadingSpinnerComponent, PaginationComponent, StatusBadgeComponent, EmptyStateComponent } from '../../../shared/components';
import { DateFormatPipe, CurrencyFormatPipe } from '../../../shared/pipes';
import { Session, SessionCategory, PagedResult } from '../../../core/models';

@Component({
  selector: 'app-sessions-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, LoadingSpinnerComponent, PaginationComponent, StatusBadgeComponent, EmptyStateComponent, DateFormatPipe, CurrencyFormatPipe],
  template: `
    <div class="space-y-6">
      <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 class="text-2xl font-bold text-gray-900">Sessions</h1>
          <p class="text-gray-500 mt-1">Manage club sessions and bookings</p>
        </div>
        <a routerLink="/club/sessions/new" class="btn-primary inline-flex items-center">
          <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
          </svg>
          New Session
        </a>
      </div>

      <!-- Filters -->
      <div class="card">
        <div class="flex flex-col lg:flex-row gap-4">
          <div class="flex gap-4 flex-1">
            <select [(ngModel)]="categoryFilter" (ngModelChange)="loadSessions()" class="form-input">
              <option value="">All Categories</option>
              @for (cat of categories; track cat) {
                <option [value]="cat">{{ cat }}</option>
              }
            </select>
            <input type="date" [(ngModel)]="dateFrom" (ngModelChange)="loadSessions()" class="form-input" />
            <input type="date" [(ngModel)]="dateTo" (ngModelChange)="loadSessions()" class="form-input" />
          </div>
          <label class="flex items-center gap-2">
            <input type="checkbox" [(ngModel)]="includeCancelled" (ngModelChange)="loadSessions()" class="rounded border-gray-300" />
            <span class="text-sm text-gray-600">Show cancelled</span>
          </label>
        </div>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg" message="Loading sessions..."></app-loading-spinner>
        </div>
      } @else if (sessions().length === 0) {
        <app-empty-state
          icon="calendar"
          title="No sessions found"
          message="Create your first session to get started."
          actionText="New Session"
          (action)="navigateToCreate()"
        ></app-empty-state>
      } @else {
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          @for (session of sessions(); track session.id) {
            <div class="card hover:shadow-lg transition-shadow" [class.opacity-60]="session.isCancelled">
              <div class="flex items-start justify-between mb-3">
                <span class="badge" [class]="getCategoryBadgeClass(session.category)">{{ session.category }}</span>
                @if (session.isCancelled) {
                  <span class="badge badge-danger">Cancelled</span>
                }
              </div>
              <h3 class="text-lg font-semibold text-gray-900 mb-2">{{ session.title }}</h3>
              <p class="text-sm text-gray-500 mb-4">{{ session.description || 'No description' }}</p>

              <div class="space-y-2 text-sm mb-4">
                <div class="flex items-center text-gray-600">
                  <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                  </svg>
                  {{ session.startTime | dateFormat:'datetime' }}
                </div>
                <div class="flex items-center text-gray-600">
                  <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0z" />
                  </svg>
                  {{ session.currentBookings }}/{{ session.capacity }} booked
                </div>
                @if (session.sessionFee) {
                  <div class="flex items-center text-gray-600">
                    <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1" />
                    </svg>
                    {{ session.sessionFee | currencyFormat }}
                  </div>
                }
              </div>

              <!-- Capacity Bar -->
              <div class="mb-4">
                <div class="flex justify-between text-xs text-gray-500 mb-1">
                  <span>Capacity</span>
                  <span>{{ session.availableSpots }} spots left</span>
                </div>
                <div class="h-2 bg-gray-200 rounded-full overflow-hidden">
                  <div
                    class="h-full transition-all"
                    [style.width.%]="(session.currentBookings / session.capacity) * 100"
                    [class]="session.availableSpots === 0 ? 'bg-red-500' : session.availableSpots < 5 ? 'bg-yellow-500' : 'bg-green-500'"
                  ></div>
                </div>
              </div>

              <a [routerLink]="['/club/sessions', session.id]" class="btn-primary w-full text-center text-sm">
                View Details
              </a>
            </div>
          }
        </div>

        @if (totalCount() > pageSize) {
          <app-pagination
            [currentPage]="currentPage()"
            [pageSize]="pageSize"
            [totalCount]="totalCount()"
            (pageChange)="onPageChange($event)"
          ></app-pagination>
        }
      }
    </div>
  `
})
export class SessionsListComponent implements OnInit {
  private sessionService = inject(SessionService);

  isLoading = signal(true);
  sessions = signal<Session[]>([]);
  totalCount = signal(0);
  currentPage = signal(1);
  pageSize = 9;

  categoryFilter = '';
  dateFrom = '';
  dateTo = '';
  includeCancelled = false;

  categories = Object.values(SessionCategory);

  ngOnInit(): void {
    this.loadSessions();
  }

  loadSessions(): void {
    this.isLoading.set(true);
    this.sessionService.getSessions({
      category: this.categoryFilter ? this.categoryFilter as SessionCategory : undefined,
      dateFrom: this.dateFrom ? new Date(this.dateFrom) : undefined,
      dateTo: this.dateTo ? new Date(this.dateTo) : undefined,
      includeCancelled: this.includeCancelled,
      page: this.currentPage(),
      pageSize: this.pageSize
    }).subscribe({
      next: (result: PagedResult<Session>) => {
        this.sessions.set(result.items);
        this.totalCount.set(result.totalCount);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }

  onPageChange(page: number): void {
    this.currentPage.set(page);
    this.loadSessions();
  }

  getCategoryBadgeClass(category: SessionCategory): string {
    const classes: Record<string, string> = {
      'AllAges': 'badge-info',
      'Juniors': 'badge-success',
      'Seniors': 'badge-warning',
      'Beginners': 'badge-gray',
      'Advanced': 'badge-danger'
    };
    return classes[category] || 'badge-gray';
  }

  navigateToCreate(): void {
    window.location.href = '/club/sessions/new';
  }
}
