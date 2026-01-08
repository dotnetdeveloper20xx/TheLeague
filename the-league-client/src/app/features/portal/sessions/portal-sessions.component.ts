import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PortalService } from '../../../core/services/portal.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent, EmptyStateComponent, ConfirmDialogComponent } from '../../../shared/components';
import { DateFormatPipe, CurrencyFormatPipe } from '../../../shared/pipes';
import { Session } from '../../../core/models';

@Component({
  selector: 'app-portal-sessions',
  standalone: true,
  imports: [CommonModule, LoadingSpinnerComponent, EmptyStateComponent, ConfirmDialogComponent, DateFormatPipe, CurrencyFormatPipe],
  template: `
    <div class="space-y-6">
      <div class="flex items-center justify-between">
        <h1 class="text-2xl font-bold text-gray-900">Sessions</h1>
        <div class="flex gap-2">
          <button
            (click)="viewMode = 'available'"
            [class]="viewMode === 'available' ? 'btn-primary' : 'btn-secondary'"
            class="text-sm"
          >
            Available
          </button>
          <button
            (click)="viewMode = 'booked'"
            [class]="viewMode === 'booked' ? 'btn-primary' : 'btn-secondary'"
            class="text-sm"
          >
            My Bookings
          </button>
        </div>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg"></app-loading-spinner>
        </div>
      } @else {
        @if (viewMode === 'available') {
          @if (availableSessions().length === 0) {
            <app-empty-state
              icon="calendar"
              title="No sessions available"
              message="There are no upcoming sessions to book at the moment."
            ></app-empty-state>
          } @else {
            <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              @for (session of availableSessions(); track session.id) {
                <div class="card hover:shadow-lg transition-shadow">
                  <div class="flex items-center justify-between mb-3">
                    <span class="badge badge-info">{{ session.category }}</span>
                    @if (session.sessionFee) {
                      <span class="text-primary-600 font-medium">{{ session.sessionFee | currencyFormat }}</span>
                    } @else {
                      <span class="text-green-600 font-medium">Free</span>
                    }
                  </div>
                  <h3 class="text-lg font-semibold text-gray-900 mb-2">{{ session.title }}</h3>
                  <div class="space-y-2 text-sm text-gray-600 mb-4">
                    <div class="flex items-center">
                      <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                      </svg>
                      {{ session.startTime | dateFormat:'datetime' }}
                    </div>
                    <div class="flex items-center">
                      <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0z" />
                      </svg>
                      {{ session.availableSpots }} spots left
                    </div>
                  </div>
                  <button
                    (click)="bookSession(session)"
                    [disabled]="session.availableSpots === 0"
                    class="btn-primary w-full"
                    [class.opacity-50]="session.availableSpots === 0"
                  >
                    {{ session.availableSpots === 0 ? 'Fully Booked' : 'Book Session' }}
                  </button>
                </div>
              }
            </div>
          }
        } @else {
          @if (bookedSessions().length === 0) {
            <app-empty-state
              icon="calendar"
              title="No bookings"
              message="You haven't booked any sessions yet."
            ></app-empty-state>
          } @else {
            <div class="space-y-4">
              @for (session of bookedSessions(); track session.id) {
                <div class="card flex flex-col sm:flex-row sm:items-center justify-between gap-4">
                  <div>
                    <h3 class="font-semibold text-gray-900">{{ session.title }}</h3>
                    <p class="text-sm text-gray-500">{{ session.startTime | dateFormat:'datetime' }}</p>
                  </div>
                  <div class="flex items-center gap-3">
                    <span class="badge badge-success">Booked</span>
                    <button (click)="confirmCancel(session)" class="text-red-600 hover:text-red-700 text-sm font-medium">
                      Cancel
                    </button>
                  </div>
                </div>
              }
            </div>
          }
        }
      }
    </div>

    <app-confirm-dialog
      [isOpen]="showCancelDialog()"
      title="Cancel Booking"
      message="Are you sure you want to cancel this booking?"
      confirmText="Cancel Booking"
      type="warning"
      (confirm)="cancelBooking()"
      (cancel)="showCancelDialog.set(false)"
    ></app-confirm-dialog>
  `
})
export class PortalSessionsComponent implements OnInit {
  private portalService = inject(PortalService);
  private notificationService = inject(NotificationService);

  isLoading = signal(true);
  availableSessions = signal<Session[]>([]);
  bookedSessions = signal<Session[]>([]);
  viewMode: 'available' | 'booked' = 'available';

  showCancelDialog = signal(false);
  selectedSession = signal<Session | null>(null);

  ngOnInit(): void {
    this.loadSessions();
  }

  loadSessions(): void {
    this.isLoading.set(true);
    this.portalService.getUpcomingSessions().subscribe({
      next: (sessions) => {
        this.availableSessions.set(sessions);
      }
    });
    this.portalService.getMyBookings().subscribe({
      next: (sessions) => {
        this.bookedSessions.set(sessions);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }

  bookSession(session: Session): void {
    this.portalService.bookSession(session.id).subscribe({
      next: () => {
        this.notificationService.success('Session booked successfully!');
        this.loadSessions();
      },
      error: () => {
        this.notificationService.error('Failed to book session');
      }
    });
  }

  confirmCancel(session: Session): void {
    this.selectedSession.set(session);
    this.showCancelDialog.set(true);
  }

  cancelBooking(): void {
    const session = this.selectedSession();
    if (!session) return;

    this.portalService.cancelBooking(session.id).subscribe({
      next: () => {
        this.notificationService.success('Booking cancelled');
        this.showCancelDialog.set(false);
        this.loadSessions();
      },
      error: () => {
        this.notificationService.error('Failed to cancel booking');
      }
    });
  }
}
