import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { EventService } from '../../../core/services/event.service';
import { LoadingSpinnerComponent, EmptyStateComponent } from '../../../shared/components';
import { DateFormatPipe, CurrencyFormatPipe } from '../../../shared/pipes';
import { Event, PagedResult } from '../../../core/models';

@Component({
  selector: 'app-events-list',
  standalone: true,
  imports: [CommonModule, RouterLink, LoadingSpinnerComponent, EmptyStateComponent, DateFormatPipe, CurrencyFormatPipe],
  template: `
    <div class="space-y-6">
      <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 class="text-2xl font-bold text-gray-900">Events</h1>
          <p class="text-gray-500 mt-1">Manage club events and registrations</p>
        </div>
        <a routerLink="/club/events/new" class="btn-primary inline-flex items-center">
          <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
          </svg>
          New Event
        </a>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg" message="Loading events..."></app-loading-spinner>
        </div>
      } @else if (events().length === 0) {
        <app-empty-state
          icon="calendar"
          title="No events found"
          message="Create your first event to engage members."
          actionText="New Event"
          (action)="navigateToCreate()"
        ></app-empty-state>
      } @else {
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          @for (event of events(); track event.id) {
            <div class="card hover:shadow-lg transition-shadow" [class.opacity-60]="event.isCancelled">
              @if (event.imageUrl) {
                <img [src]="event.imageUrl" [alt]="event.title" class="w-full h-40 object-cover rounded-lg mb-4" />
              } @else {
                <div class="w-full h-40 bg-gradient-to-br from-primary-500 to-primary-700 rounded-lg mb-4 flex items-center justify-center">
                  <svg class="w-16 h-16 text-white/50" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                  </svg>
                </div>
              }

              <div class="flex items-center gap-2 mb-2">
                <span class="badge badge-info">{{ event.type }}</span>
                @if (!event.isPublished) {
                  <span class="badge badge-warning">Draft</span>
                }
                @if (event.isCancelled) {
                  <span class="badge badge-danger">Cancelled</span>
                }
              </div>

              <h3 class="text-lg font-semibold text-gray-900 mb-2">{{ event.title }}</h3>

              <div class="space-y-2 text-sm text-gray-600 mb-4">
                <div class="flex items-center">
                  <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                  </svg>
                  {{ event.startDateTime | dateFormat:'datetime' }}
                </div>
                @if (event.location) {
                  <div class="flex items-center">
                    <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                    </svg>
                    {{ event.location }}
                  </div>
                }
                <div class="flex items-center">
                  <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0z" />
                  </svg>
                  {{ event.currentAttendees }} registered
                </div>
              </div>

              @if (event.isTicketed) {
                <p class="text-lg font-semibold text-primary-600 mb-4">
                  {{ event.memberTicketPrice || event.ticketPrice | currencyFormat }}
                </p>
              }

              <a [routerLink]="['/club/events', event.id]" class="btn-primary w-full text-center text-sm">
                View Details
              </a>
            </div>
          }
        </div>
      }
    </div>
  `
})
export class EventsListComponent implements OnInit {
  private eventService = inject(EventService);

  isLoading = signal(true);
  events = signal<Event[]>([]);

  ngOnInit(): void {
    this.loadEvents();
  }

  loadEvents(): void {
    this.eventService.getEvents({ includeUnpublished: true }).subscribe({
      next: (result: PagedResult<Event>) => {
        this.events.set(result.items);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }

  navigateToCreate(): void {
    window.location.href = '/club/events/new';
  }
}
