import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { ClubService } from '../../../core/services/club.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent, ConfirmDialogComponent } from '../../../shared/components';
import { Club } from '../../../core/models';

@Component({
  selector: 'app-club-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, LoadingSpinnerComponent, ConfirmDialogComponent],
  template: `
    @if (isLoading()) {
      <div class="flex justify-center py-12">
        <app-loading-spinner size="lg" message="Loading club details..."></app-loading-spinner>
      </div>
    } @else if (club()) {
      <div class="space-y-6">
        <!-- Header -->
        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div class="flex items-center gap-4">
            <a routerLink="/admin/clubs" class="p-2 text-gray-500 hover:text-gray-700 rounded-lg hover:bg-gray-100">
              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
              </svg>
            </a>
            @if (club()!.logoUrl) {
              <img [src]="club()!.logoUrl" [alt]="club()!.name" class="w-16 h-16 rounded-lg object-cover" />
            } @else {
              <div
                class="w-16 h-16 rounded-lg flex items-center justify-center text-white text-2xl font-bold"
                [style.backgroundColor]="club()!.primaryColor"
              >
                {{ club()!.name[0] }}
              </div>
            }
            <div>
              <h1 class="text-2xl font-bold text-gray-900">{{ club()!.name }}</h1>
              <p class="text-gray-500">{{ club()!.clubType }}</p>
            </div>
          </div>
          <div class="flex gap-3">
            <a [routerLink]="['/admin/clubs', club()!.id, 'edit']" class="btn-secondary">
              Edit Club
            </a>
            <button (click)="showDeleteDialog.set(true)" class="btn-danger">
              Delete
            </button>
          </div>
        </div>

        <!-- Stats -->
        <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
          <div class="card">
            <p class="text-sm text-gray-500">Total Members</p>
            <p class="text-2xl font-bold text-gray-900 mt-1">{{ club()!.memberCount || 0 }}</p>
          </div>
          <div class="card">
            <p class="text-sm text-gray-500">Active Members</p>
            <p class="text-2xl font-bold text-gray-900 mt-1">{{ club()!.activeMembers || 0 }}</p>
          </div>
          <div class="card">
            <p class="text-sm text-gray-500">Status</p>
            <span class="badge mt-2" [class]="club()!.isActive ? 'badge-success' : 'badge-gray'">
              {{ club()!.isActive ? 'Active' : 'Inactive' }}
            </span>
          </div>
          <div class="card">
            <p class="text-sm text-gray-500">Created</p>
            <p class="text-lg font-medium text-gray-900 mt-1">{{ club()!.createdAt | date:'mediumDate' }}</p>
          </div>
        </div>

        <!-- Details -->
        <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
          <!-- Club Info -->
          <div class="card">
            <h3 class="card-header">Club Information</h3>
            <dl class="space-y-4">
              <div>
                <dt class="text-sm text-gray-500">Description</dt>
                <dd class="text-gray-900 mt-1">{{ club()!.description || 'No description' }}</dd>
              </div>
              <div>
                <dt class="text-sm text-gray-500">URL Slug</dt>
                <dd class="text-gray-900 mt-1">{{ club()!.slug }}</dd>
              </div>
              <div>
                <dt class="text-sm text-gray-500">Branding</dt>
                <dd class="flex items-center gap-3 mt-1">
                  <div class="flex items-center gap-2">
                    <div class="w-6 h-6 rounded" [style.backgroundColor]="club()!.primaryColor"></div>
                    <span class="text-sm text-gray-600">Primary</span>
                  </div>
                  <div class="flex items-center gap-2">
                    <div class="w-6 h-6 rounded" [style.backgroundColor]="club()!.secondaryColor"></div>
                    <span class="text-sm text-gray-600">Secondary</span>
                  </div>
                </dd>
              </div>
            </dl>
          </div>

          <!-- Contact Info -->
          <div class="card">
            <h3 class="card-header">Contact Information</h3>
            <dl class="space-y-4">
              <div>
                <dt class="text-sm text-gray-500">Email</dt>
                <dd class="text-gray-900 mt-1">
                  @if (club()!.contactEmail) {
                    <a [href]="'mailto:' + club()!.contactEmail" class="text-primary-600 hover:underline">
                      {{ club()!.contactEmail }}
                    </a>
                  } @else {
                    <span class="text-gray-400">Not provided</span>
                  }
                </dd>
              </div>
              <div>
                <dt class="text-sm text-gray-500">Phone</dt>
                <dd class="text-gray-900 mt-1">{{ club()!.contactPhone || 'Not provided' }}</dd>
              </div>
              <div>
                <dt class="text-sm text-gray-500">Address</dt>
                <dd class="text-gray-900 mt-1">{{ club()!.address || 'Not provided' }}</dd>
              </div>
              <div>
                <dt class="text-sm text-gray-500">Website</dt>
                <dd class="mt-1">
                  @if (club()!.website) {
                    <a [href]="club()!.website" target="_blank" class="text-primary-600 hover:underline">
                      {{ club()!.website }}
                    </a>
                  } @else {
                    <span class="text-gray-400">Not provided</span>
                  }
                </dd>
              </div>
            </dl>
          </div>
        </div>
      </div>
    }

    <!-- Delete Confirmation -->
    <app-confirm-dialog
      [isOpen]="showDeleteDialog()"
      title="Delete Club"
      [message]="'Are you sure you want to delete ' + (club()?.name || '') + '? This will remove all associated data and cannot be undone.'"
      confirmText="Delete"
      type="danger"
      (confirm)="deleteClub()"
      (cancel)="showDeleteDialog.set(false)"
    ></app-confirm-dialog>
  `
})
export class ClubDetailComponent implements OnInit {
  private clubService = inject(ClubService);
  private notificationService = inject(NotificationService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  isLoading = signal(true);
  club = signal<Club | null>(null);
  showDeleteDialog = signal(false);

  ngOnInit(): void {
    const clubId = this.route.snapshot.params['id'];
    if (clubId) {
      this.loadClub(clubId);
    }
  }

  private loadClub(id: string): void {
    this.clubService.getClub(id).subscribe({
      next: (club) => {
        this.club.set(club);
        this.isLoading.set(false);
      },
      error: () => {
        this.notificationService.error('Failed to load club');
        this.router.navigate(['/admin/clubs']);
      }
    });
  }

  deleteClub(): void {
    const club = this.club();
    if (!club) return;

    this.clubService.deleteClub(club.id).subscribe({
      next: () => {
        this.notificationService.success(`${club.name} has been deleted`);
        this.router.navigate(['/admin/clubs']);
      },
      error: () => {
        this.notificationService.error('Failed to delete club');
      }
    });
  }
}
