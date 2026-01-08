import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VenueService } from '../../../core/services/venue.service';
import { LoadingSpinnerComponent, EmptyStateComponent } from '../../../shared/components';
import { Venue } from '../../../core/models';

@Component({
  selector: 'app-venues-list',
  standalone: true,
  imports: [CommonModule, LoadingSpinnerComponent, EmptyStateComponent],
  template: `
    <div class="space-y-6">
      <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 class="text-2xl font-bold text-gray-900">Venues</h1>
          <p class="text-gray-500 mt-1">Manage club venues and facilities</p>
        </div>
        <button class="btn-primary inline-flex items-center">
          <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
          </svg>
          Add Venue
        </button>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg"></app-loading-spinner>
        </div>
      } @else if (venues().length === 0) {
        <app-empty-state
          icon="venue"
          title="No venues"
          message="Add your first venue to start scheduling sessions."
        ></app-empty-state>
      } @else {
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          @for (venue of venues(); track venue.id) {
            <div class="card" [class.opacity-60]="!venue.isActive">
              @if (venue.imageUrl) {
                <img [src]="venue.imageUrl" [alt]="venue.name" class="w-full h-40 object-cover rounded-lg mb-4" />
              } @else {
                <div class="w-full h-40 bg-gray-100 rounded-lg mb-4 flex items-center justify-center">
                  <svg class="w-12 h-12 text-gray-300" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                  </svg>
                </div>
              }
              <div class="flex items-start justify-between mb-2">
                <h3 class="text-lg font-semibold text-gray-900">{{ venue.name }}</h3>
                @if (venue.isPrimary) {
                  <span class="badge badge-info">Primary</span>
                }
              </div>
              <p class="text-sm text-gray-500 mb-4">{{ venue.description || 'No description' }}</p>
              @if (venue.address) {
                <p class="text-sm text-gray-600 mb-2">
                  <svg class="w-4 h-4 inline mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                  </svg>
                  {{ venue.address }}
                </p>
              }
              @if (venue.capacity) {
                <p class="text-sm text-gray-600">
                  Capacity: {{ venue.capacity }}
                </p>
              }
            </div>
          }
        </div>
      }
    </div>
  `
})
export class VenuesListComponent implements OnInit {
  private venueService = inject(VenueService);

  isLoading = signal(true);
  venues = signal<Venue[]>([]);

  ngOnInit(): void {
    this.venueService.getVenues({ includeInactive: true }).subscribe({
      next: (venues) => {
        this.venues.set(venues);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }
}
