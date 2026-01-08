import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { EventService } from '../../../core/services/event.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent, ConfirmDialogComponent } from '../../../shared/components';
import { DateFormatPipe, CurrencyFormatPipe } from '../../../shared/pipes';
import { Event } from '../../../core/models';

@Component({
  selector: 'app-event-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, LoadingSpinnerComponent, ConfirmDialogComponent, DateFormatPipe, CurrencyFormatPipe],
  template: `
    @if (isLoading()) {
      <div class="flex justify-center py-12">
        <app-loading-spinner size="lg" message="Loading event..."></app-loading-spinner>
      </div>
    } @else if (event()) {
      <div class="space-y-6">
        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div class="flex items-center gap-4">
            <a routerLink="/club/events" class="p-2 text-gray-500 hover:text-gray-700 rounded-lg hover:bg-gray-100">
              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
              </svg>
            </a>
            <div>
              <h1 class="text-2xl font-bold text-gray-900">{{ event()!.title }}</h1>
              <div class="flex items-center gap-2 mt-1">
                <span class="badge badge-info">{{ event()!.type }}</span>
                @if (!event()!.isPublished) {
                  <span class="badge badge-warning">Draft</span>
                }
                @if (event()!.isCancelled) {
                  <span class="badge badge-danger">Cancelled</span>
                }
              </div>
            </div>
          </div>
          <div class="flex gap-3">
            @if (!event()!.isPublished) {
              <button (click)="publishEvent()" class="btn-success">Publish</button>
            }
            @if (!event()!.isCancelled) {
              <button (click)="showCancelDialog.set(true)" class="btn-danger">Cancel Event</button>
            }
          </div>
        </div>

        <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
          <div class="card lg:col-span-2">
            <h3 class="card-header">Event Details</h3>
            <dl class="grid grid-cols-2 gap-4">
              <div>
                <dt class="text-sm text-gray-500">Start</dt>
                <dd class="text-gray-900 font-medium">{{ event()!.startDateTime | dateFormat:'datetime' }}</dd>
              </div>
              <div>
                <dt class="text-sm text-gray-500">End</dt>
                <dd class="text-gray-900">{{ event()!.endDateTime | dateFormat:'datetime' }}</dd>
              </div>
              @if (event()!.location) {
                <div class="col-span-2">
                  <dt class="text-sm text-gray-500">Location</dt>
                  <dd class="text-gray-900">{{ event()!.location }}</dd>
                </div>
              }
              @if (event()!.description) {
                <div class="col-span-2">
                  <dt class="text-sm text-gray-500">Description</dt>
                  <dd class="text-gray-900">{{ event()!.description }}</dd>
                </div>
              }
              @if (event()!.isTicketed) {
                <div>
                  <dt class="text-sm text-gray-500">Ticket Price</dt>
                  <dd class="text-gray-900">{{ event()!.ticketPrice | currencyFormat }}</dd>
                </div>
                <div>
                  <dt class="text-sm text-gray-500">Member Price</dt>
                  <dd class="text-gray-900">{{ event()!.memberTicketPrice | currencyFormat }}</dd>
                </div>
              }
            </dl>
          </div>

          <div class="card">
            <h3 class="card-header">Registrations</h3>
            <div class="text-center py-4">
              <p class="text-4xl font-bold text-gray-900">{{ event()!.currentAttendees }}</p>
              <p class="text-gray-500">
                @if (event()!.capacity) {
                  of {{ event()!.capacity }} spots
                } @else {
                  registered
                }
              </p>
            </div>
            @if (event()!.capacity) {
              <div class="h-3 bg-gray-200 rounded-full overflow-hidden">
                <div
                  class="h-full bg-primary-600 transition-all"
                  [style.width.%]="(event()!.currentAttendees / event()!.capacity!) * 100"
                ></div>
              </div>
            }
          </div>
        </div>
      </div>
    }

    <app-confirm-dialog
      [isOpen]="showCancelDialog()"
      title="Cancel Event"
      message="Are you sure you want to cancel this event? All registrations will be cancelled."
      confirmText="Cancel Event"
      type="danger"
      (confirm)="cancelEvent()"
      (cancel)="showCancelDialog.set(false)"
    ></app-confirm-dialog>
  `
})
export class EventDetailComponent implements OnInit {
  private eventService = inject(EventService);
  private notificationService = inject(NotificationService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  isLoading = signal(true);
  event = signal<Event | null>(null);
  showCancelDialog = signal(false);

  ngOnInit(): void {
    const eventId = this.route.snapshot.params['id'];
    if (eventId) {
      this.loadEvent(eventId);
    }
  }

  private loadEvent(id: string): void {
    this.eventService.getEvent(id).subscribe({
      next: (event) => {
        this.event.set(event);
        this.isLoading.set(false);
      },
      error: () => {
        this.notificationService.error('Failed to load event');
        this.router.navigate(['/club/events']);
      }
    });
  }

  publishEvent(): void {
    const event = this.event();
    if (!event) return;

    this.eventService.publishEvent(event.id).subscribe({
      next: () => {
        this.notificationService.success('Event published');
        this.loadEvent(event.id);
      },
      error: () => {
        this.notificationService.error('Failed to publish event');
      }
    });
  }

  cancelEvent(): void {
    const event = this.event();
    if (!event) return;

    this.eventService.cancelEvent(event.id, 'Cancelled by administrator').subscribe({
      next: () => {
        this.notificationService.success('Event cancelled');
        this.showCancelDialog.set(false);
        this.loadEvent(event.id);
      },
      error: () => {
        this.notificationService.error('Failed to cancel event');
      }
    });
  }
}
