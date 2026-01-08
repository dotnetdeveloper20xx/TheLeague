import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PortalService } from '../../../core/services/portal.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent, EmptyStateComponent } from '../../../shared/components';
import { DateFormatPipe, CurrencyFormatPipe } from '../../../shared/pipes';
import { Event } from '../../../core/models';

@Component({
  selector: 'app-portal-events',
  standalone: true,
  imports: [CommonModule, LoadingSpinnerComponent, EmptyStateComponent, DateFormatPipe, CurrencyFormatPipe],
  template: `
    <div class="space-y-6">
      <h1 class="text-2xl font-bold text-gray-900">Events</h1>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg"></app-loading-spinner>
        </div>
      } @else if (events().length === 0) {
        <app-empty-state
          icon="calendar"
          title="No upcoming events"
          message="There are no upcoming events at the moment."
        ></app-empty-state>
      } @else {
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          @for (event of events(); track event.id) {
            <div class="card hover:shadow-lg transition-shadow">
              @if (event.imageUrl) {
                <img [src]="event.imageUrl" [alt]="event.title" class="w-full h-40 object-cover rounded-lg mb-4" />
              } @else {
                <div class="w-full h-40 bg-gradient-to-br from-purple-500 to-purple-700 rounded-lg mb-4 flex items-center justify-center">
                  <svg class="w-12 h-12 text-white/50" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                  </svg>
                </div>
              }

              <span class="badge badge-info mb-2">{{ event.type }}</span>
              <h3 class="text-lg font-semibold text-gray-900 mb-2">{{ event.title }}</h3>
              <p class="text-sm text-gray-500 mb-4 line-clamp-2">{{ event.description }}</p>

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
              </div>

              @if (event.isTicketed) {
                <p class="text-lg font-semibold text-primary-600 mb-4">
                  {{ event.memberTicketPrice || event.ticketPrice | currencyFormat }}
                </p>
              }

              <button (click)="registerForEvent(event)" class="btn-primary w-full">
                {{ event.requiresRSVP ? 'RSVP' : 'Register' }}
              </button>
            </div>
          }
        </div>
      }
    </div>
  `
})
export class PortalEventsComponent implements OnInit {
  private portalService = inject(PortalService);
  private notificationService = inject(NotificationService);

  isLoading = signal(true);
  events = signal<Event[]>([]);

  ngOnInit(): void {
    this.portalService.getUpcomingEvents().subscribe({
      next: (events) => {
        this.events.set(events);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }

  registerForEvent(event: Event): void {
    this.portalService.registerForEvent(event.id).subscribe({
      next: () => {
        this.notificationService.success('Successfully registered for event!');
      },
      error: () => {
        this.notificationService.error('Failed to register for event');
      }
    });
  }
}
