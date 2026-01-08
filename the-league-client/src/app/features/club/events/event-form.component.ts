import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { EventService } from '../../../core/services/event.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent } from '../../../shared/components';
import { EventType } from '../../../core/models';

@Component({
  selector: 'app-event-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, LoadingSpinnerComponent],
  template: `
    <div class="space-y-6">
      <div class="flex items-center gap-4">
        <a routerLink="/club/events" class="p-2 text-gray-500 hover:text-gray-700 rounded-lg hover:bg-gray-100">
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
          </svg>
        </a>
        <div>
          <h1 class="text-2xl font-bold text-gray-900">Create New Event</h1>
          <p class="text-gray-500 mt-1">Schedule a new event for your club</p>
        </div>
      </div>

      <form [formGroup]="eventForm" (ngSubmit)="onSubmit()" class="card max-w-2xl">
        <div class="space-y-6">
          <div>
            <label class="form-label">Event Title *</label>
            <input type="text" formControlName="title" class="form-input" placeholder="e.g., Annual Club Tournament" />
          </div>

          <div>
            <label class="form-label">Description</label>
            <textarea formControlName="description" class="form-input" rows="3"></textarea>
          </div>

          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label class="form-label">Event Type *</label>
              <select formControlName="type" class="form-input">
                @for (type of eventTypes; track type) {
                  <option [value]="type">{{ type }}</option>
                }
              </select>
            </div>
            <div>
              <label class="form-label">Location</label>
              <input type="text" formControlName="location" class="form-input" />
            </div>
          </div>

          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label class="form-label">Start Date & Time *</label>
              <input type="datetime-local" formControlName="startDateTime" class="form-input" />
            </div>
            <div>
              <label class="form-label">End Date & Time *</label>
              <input type="datetime-local" formControlName="endDateTime" class="form-input" />
            </div>
          </div>

          <div>
            <label class="form-label">Capacity (leave empty for unlimited)</label>
            <input type="number" formControlName="capacity" class="form-input" min="1" />
          </div>

          <div class="border-t pt-6">
            <label class="flex items-center gap-3 mb-4">
              <input type="checkbox" formControlName="isTicketed" class="rounded border-gray-300 text-primary-600" />
              <span class="font-medium text-gray-900">This is a ticketed event</span>
            </label>

            @if (eventForm.get('isTicketed')?.value) {
              <div class="grid grid-cols-1 md:grid-cols-2 gap-4 ml-7">
                <div>
                  <label class="form-label">Ticket Price</label>
                  <input type="number" formControlName="ticketPrice" class="form-input" min="0" step="0.01" />
                </div>
                <div>
                  <label class="form-label">Member Price</label>
                  <input type="number" formControlName="memberTicketPrice" class="form-input" min="0" step="0.01" />
                </div>
              </div>
            }
          </div>

          <div class="border-t pt-6">
            <label class="flex items-center gap-3 mb-4">
              <input type="checkbox" formControlName="requiresRSVP" class="rounded border-gray-300 text-primary-600" />
              <span class="font-medium text-gray-900">Requires RSVP</span>
            </label>

            @if (eventForm.get('requiresRSVP')?.value) {
              <div class="ml-7">
                <label class="form-label">RSVP Deadline</label>
                <input type="datetime-local" formControlName="rsvpDeadline" class="form-input" />
              </div>
            }
          </div>
        </div>

        <div class="flex justify-end gap-3 mt-8 pt-6 border-t border-gray-200">
          <a routerLink="/club/events" class="btn-secondary">Cancel</a>
          <button type="submit" [disabled]="eventForm.invalid || isSubmitting" class="btn-primary">
            @if (isSubmitting) {
              <app-loading-spinner size="sm" containerClass="mr-2"></app-loading-spinner>
              Creating...
            } @else {
              Create Event
            }
          </button>
        </div>
      </form>
    </div>
  `
})
export class EventFormComponent {
  private fb = inject(FormBuilder);
  private eventService = inject(EventService);
  private notificationService = inject(NotificationService);
  private router = inject(Router);

  eventForm: FormGroup;
  isSubmitting = false;
  eventTypes = Object.values(EventType);

  constructor() {
    this.eventForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      type: [EventType.Social, Validators.required],
      startDateTime: ['', Validators.required],
      endDateTime: ['', Validators.required],
      location: [''],
      capacity: [null],
      isTicketed: [false],
      ticketPrice: [null],
      memberTicketPrice: [null],
      requiresRSVP: [false],
      rsvpDeadline: [null]
    });
  }

  onSubmit(): void {
    if (this.eventForm.invalid) return;

    this.isSubmitting = true;
    const formData = this.eventForm.value;

    this.eventService.createEvent({
      ...formData,
      startDateTime: new Date(formData.startDateTime),
      endDateTime: new Date(formData.endDateTime),
      rsvpDeadline: formData.rsvpDeadline ? new Date(formData.rsvpDeadline) : undefined
    }).subscribe({
      next: () => {
        this.notificationService.success('Event created successfully');
        this.router.navigate(['/club/events']);
      },
      error: () => {
        this.isSubmitting = false;
        this.notificationService.error('Failed to create event');
      }
    });
  }
}
