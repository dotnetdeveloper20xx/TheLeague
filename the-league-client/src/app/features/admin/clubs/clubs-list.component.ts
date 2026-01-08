import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ClubService } from '../../../core/services/club.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent, PaginationComponent, ConfirmDialogComponent, EmptyStateComponent } from '../../../shared/components';
import { Club, PagedResult } from '../../../core/models';

@Component({
  selector: 'app-clubs-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, LoadingSpinnerComponent, PaginationComponent, ConfirmDialogComponent, EmptyStateComponent],
  template: `
    <div class="space-y-6">
      <!-- Header -->
      <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 class="text-2xl font-bold text-gray-900">Clubs</h1>
          <p class="text-gray-500 mt-1">Manage registered clubs</p>
        </div>
        <a routerLink="/admin/clubs/new" class="btn-primary inline-flex items-center">
          <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
          </svg>
          Add Club
        </a>
      </div>

      <!-- Search -->
      <div class="card">
        <div class="flex flex-col sm:flex-row gap-4">
          <div class="flex-1">
            <input
              type="text"
              [(ngModel)]="searchTerm"
              (ngModelChange)="onSearch()"
              placeholder="Search clubs..."
              class="form-input"
            />
          </div>
          <select [(ngModel)]="statusFilter" (ngModelChange)="onSearch()" class="form-input w-full sm:w-40">
            <option value="">All Status</option>
            <option value="active">Active</option>
            <option value="inactive">Inactive</option>
          </select>
        </div>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg" message="Loading clubs..."></app-loading-spinner>
        </div>
      } @else if (clubs().length === 0) {
        <app-empty-state
          icon="clubs"
          title="No clubs found"
          message="Get started by creating a new club."
          actionText="Add Club"
          (action)="navigateToCreate()"
        ></app-empty-state>
      } @else {
        <!-- Clubs Grid -->
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          @for (club of clubs(); track club.id) {
            <div class="card hover:shadow-lg transition-shadow">
              <div class="flex items-start justify-between mb-4">
                @if (club.logoUrl) {
                  <img [src]="club.logoUrl" [alt]="club.name" class="w-16 h-16 rounded-lg object-cover" />
                } @else {
                  <div
                    class="w-16 h-16 rounded-lg flex items-center justify-center text-white text-2xl font-bold"
                    [style.backgroundColor]="club.primaryColor || '#3B82F6'"
                  >
                    {{ club.name[0] }}
                  </div>
                }
                <span class="badge" [class]="club.isActive ? 'badge-success' : 'badge-gray'">
                  {{ club.isActive ? 'Active' : 'Inactive' }}
                </span>
              </div>

              <h3 class="text-lg font-semibold text-gray-900 mb-1">{{ club.name }}</h3>
              <p class="text-sm text-gray-500 mb-4">{{ club.clubType }}</p>

              <div class="grid grid-cols-2 gap-4 mb-4">
                <div>
                  <p class="text-sm text-gray-500">Members</p>
                  <p class="text-lg font-semibold text-gray-900">{{ club.memberCount || 0 }}</p>
                </div>
                <div>
                  <p class="text-sm text-gray-500">Active</p>
                  <p class="text-lg font-semibold text-gray-900">{{ club.activeMembers || 0 }}</p>
                </div>
              </div>

              <div class="flex gap-2">
                <a [routerLink]="['/admin/clubs', club.id]" class="flex-1 btn-primary text-center text-sm py-2">
                  View Details
                </a>
                <button (click)="confirmDelete(club)" class="btn-danger text-sm py-2 px-3">
                  <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                  </svg>
                </button>
              </div>
            </div>
          }
        </div>

        <!-- Pagination -->
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

    <!-- Delete Confirmation -->
    <app-confirm-dialog
      [isOpen]="showDeleteDialog()"
      title="Delete Club"
      [message]="'Are you sure you want to delete ' + (selectedClub()?.name || '') + '? This action cannot be undone.'"
      confirmText="Delete"
      type="danger"
      (confirm)="deleteClub()"
      (cancel)="showDeleteDialog.set(false)"
    ></app-confirm-dialog>
  `
})
export class ClubsListComponent implements OnInit {
  private clubService = inject(ClubService);
  private notificationService = inject(NotificationService);

  isLoading = signal(true);
  clubs = signal<Club[]>([]);
  totalCount = signal(0);
  currentPage = signal(1);
  pageSize = 9;

  searchTerm = '';
  statusFilter = '';

  showDeleteDialog = signal(false);
  selectedClub = signal<Club | null>(null);

  ngOnInit(): void {
    this.loadClubs();
  }

  loadClubs(): void {
    this.isLoading.set(true);
    this.clubService.getClubs({
      page: this.currentPage(),
      pageSize: this.pageSize,
      searchTerm: this.searchTerm || undefined
    }).subscribe({
      next: (result: PagedResult<Club>) => {
        let filteredItems = result.items;
        if (this.statusFilter) {
          filteredItems = result.items.filter(c =>
            this.statusFilter === 'active' ? c.isActive : !c.isActive
          );
        }
        this.clubs.set(filteredItems);
        this.totalCount.set(result.totalCount);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }

  onSearch(): void {
    this.currentPage.set(1);
    this.loadClubs();
  }

  onPageChange(page: number): void {
    this.currentPage.set(page);
    this.loadClubs();
  }

  confirmDelete(club: Club): void {
    this.selectedClub.set(club);
    this.showDeleteDialog.set(true);
  }

  deleteClub(): void {
    const club = this.selectedClub();
    if (!club) return;

    this.clubService.deleteClub(club.id).subscribe({
      next: () => {
        this.notificationService.success(`${club.name} has been deleted`);
        this.showDeleteDialog.set(false);
        this.loadClubs();
      },
      error: () => {
        this.notificationService.error('Failed to delete club');
      }
    });
  }

  navigateToCreate(): void {
    window.location.href = '/admin/clubs/new';
  }
}
