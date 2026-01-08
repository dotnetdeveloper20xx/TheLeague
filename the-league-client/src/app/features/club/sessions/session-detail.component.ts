import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { SessionService } from '../../../core/services/session.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent, StatusBadgeComponent, ConfirmDialogComponent } from '../../../shared/components';
import { DateFormatPipe, CurrencyFormatPipe } from '../../../shared/pipes';
import { Session, SessionBooking } from '../../../core/models';

@Component({
  selector: 'app-session-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, LoadingSpinnerComponent, StatusBadgeComponent, ConfirmDialogComponent, DateFormatPipe, CurrencyFormatPipe],
  template: `
    @if (isLoading()) {
      <div class="flex justify-center py-12">
        <app-loading-spinner size="lg" message="Loading session..."></app-loading-spinner>
      </div>
    } @else if (session()) {
      <div class="space-y-6">
        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div class="flex items-center gap-4">
            <a routerLink="/club/sessions" class="p-2 text-gray-500 hover:text-gray-700 rounded-lg hover:bg-gray-100">
              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
              </svg>
            </a>
            <div>
              <h1 class="text-2xl font-bold text-gray-900">{{ session()!.title }}</h1>
              <div class="flex items-center gap-2 mt-1">
                <span class="badge badge-info">{{ session()!.category }}</span>
                @if (session()!.isCancelled) {
                  <span class="badge badge-danger">Cancelled</span>
                }
              </div>
            </div>
          </div>
          @if (!session()!.isCancelled) {
            <div class="flex gap-3">
              <button (click)="showCancelDialog.set(true)" class="btn-danger">Cancel Session</button>
            </div>
          }
        </div>

        <!-- Session Info -->
        <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
          <div class="card lg:col-span-2">
            <h3 class="card-header">Session Details</h3>
            <dl class="grid grid-cols-2 gap-4">
              <div>
                <dt class="text-sm text-gray-500">Date & Time</dt>
                <dd class="text-gray-900 font-medium">{{ session()!.startTime | dateFormat:'datetime' }}</dd>
              </div>
              <div>
                <dt class="text-sm text-gray-500">Duration</dt>
                <dd class="text-gray-900">{{ getDuration() }}</dd>
              </div>
              <div>
                <dt class="text-sm text-gray-500">Capacity</dt>
                <dd class="text-gray-900">{{ session()!.currentBookings }}/{{ session()!.capacity }}</dd>
              </div>
              <div>
                <dt class="text-sm text-gray-500">Fee</dt>
                <dd class="text-gray-900">{{ session()!.sessionFee ? (session()!.sessionFee | currencyFormat) : 'Free' }}</dd>
              </div>
              @if (session()!.venue) {
                <div class="col-span-2">
                  <dt class="text-sm text-gray-500">Venue</dt>
                  <dd class="text-gray-900">{{ session()!.venue!.name }}</dd>
                </div>
              }
              @if (session()!.description) {
                <div class="col-span-2">
                  <dt class="text-sm text-gray-500">Description</dt>
                  <dd class="text-gray-900">{{ session()!.description }}</dd>
                </div>
              }
            </dl>
          </div>

          <div class="card">
            <h3 class="card-header">Availability</h3>
            <div class="text-center py-4">
              <p class="text-4xl font-bold text-gray-900">{{ session()!.availableSpots }}</p>
              <p class="text-gray-500">spots remaining</p>
            </div>
            <div class="h-3 bg-gray-200 rounded-full overflow-hidden">
              <div
                class="h-full bg-primary-600 transition-all"
                [style.width.%]="(session()!.currentBookings / session()!.capacity) * 100"
              ></div>
            </div>
          </div>
        </div>

        <!-- Bookings -->
        <div class="card">
          <h3 class="card-header">Bookings ({{ session()!.bookings?.length || 0 }})</h3>
          @if (session()!.bookings?.length) {
            <div class="overflow-x-auto">
              <table class="table">
                <thead>
                  <tr>
                    <th>Member</th>
                    <th>Booked At</th>
                    <th>Status</th>
                    <th>Attendance</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  @for (booking of session()!.bookings; track booking.id) {
                    <tr>
                      <td>
                        <p class="font-medium">{{ booking.memberName }}</p>
                        @if (booking.familyMemberName) {
                          <p class="text-sm text-gray-500">For: {{ booking.familyMemberName }}</p>
                        }
                      </td>
                      <td>{{ booking.bookedAt | dateFormat:'datetime' }}</td>
                      <td><app-status-badge [status]="booking.status" type="booking"></app-status-badge></td>
                      <td>
                        @if (booking.attended) {
                          <span class="text-green-600">Attended</span>
                        } @else if (booking.status === 'NoShow') {
                          <span class="text-red-600">No Show</span>
                        } @else {
                          <span class="text-gray-400">-</span>
                        }
                      </td>
                      <td>
                        <div class="flex gap-2">
                          @if (!booking.attended && booking.status === 'Confirmed') {
                            <button (click)="checkIn(booking)" class="text-primary-600 hover:text-primary-700 text-sm">
                              Check In
                            </button>
                          }
                        </div>
                      </td>
                    </tr>
                  }
                </tbody>
              </table>
            </div>
          } @else {
            <p class="text-gray-500 text-center py-8">No bookings yet</p>
          }
        </div>
      </div>
    }

    <!-- Cancel Session Dialog -->
    <app-confirm-dialog
      [isOpen]="showCancelDialog()"
      title="Cancel Session"
      message="Are you sure you want to cancel this session? All bookings will be cancelled and members will be notified."
      confirmText="Cancel Session"
      type="danger"
      (confirm)="cancelSession()"
      (cancel)="showCancelDialog.set(false)"
    ></app-confirm-dialog>
  `
})
export class SessionDetailComponent implements OnInit {
  private sessionService = inject(SessionService);
  private notificationService = inject(NotificationService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  isLoading = signal(true);
  session = signal<Session | null>(null);
  showCancelDialog = signal(false);

  ngOnInit(): void {
    const sessionId = this.route.snapshot.params['id'];
    if (sessionId) {
      this.loadSession(sessionId);
    }
  }

  private loadSession(id: string): void {
    this.sessionService.getSession(id).subscribe({
      next: (session) => {
        this.session.set(session);
        this.isLoading.set(false);
      },
      error: () => {
        this.notificationService.error('Failed to load session');
        this.router.navigate(['/club/sessions']);
      }
    });
  }

  getDuration(): string {
    const session = this.session();
    if (!session) return '';
    const start = new Date(session.startTime);
    const end = new Date(session.endTime);
    const diffMs = end.getTime() - start.getTime();
    const diffMins = Math.round(diffMs / 60000);
    const hours = Math.floor(diffMins / 60);
    const mins = diffMins % 60;
    return hours > 0 ? `${hours}h ${mins}m` : `${mins}m`;
  }

  checkIn(booking: SessionBooking): void {
    const session = this.session();
    if (!session) return;

    this.sessionService.checkInBooking(session.id, booking.id).subscribe({
      next: () => {
        this.notificationService.success('Member checked in');
        this.loadSession(session.id);
      },
      error: () => {
        this.notificationService.error('Failed to check in');
      }
    });
  }

  cancelSession(): void {
    const session = this.session();
    if (!session) return;

    this.sessionService.cancelSession(session.id, 'Cancelled by administrator').subscribe({
      next: () => {
        this.notificationService.success('Session cancelled');
        this.showCancelDialog.set(false);
        this.loadSession(session.id);
      },
      error: () => {
        this.notificationService.error('Failed to cancel session');
      }
    });
  }
}
