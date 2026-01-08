import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { SessionService } from '../../../core/services/session.service';
import { VenueService } from '../../../core/services/venue.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent } from '../../../shared/components';
import { SessionCategory, Venue } from '../../../core/models';

@Component({
  selector: 'app-session-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, LoadingSpinnerComponent],
  template: `
    <div class="space-y-6">
      <div class="flex items-center gap-4">
        <a routerLink="/club/sessions" class="p-2 text-gray-500 hover:text-gray-700 rounded-lg hover:bg-gray-100">
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
          </svg>
        </a>
        <div>
          <h1 class="text-2xl font-bold text-gray-900">Create New Session</h1>
          <p class="text-gray-500 mt-1">Schedule a new session for members</p>
        </div>
      </div>

      <form [formGroup]="sessionForm" (ngSubmit)="onSubmit()" class="card max-w-2xl">
        <div class="space-y-6">
          <div>
            <label class="form-label">Session Title *</label>
            <input type="text" formControlName="title" class="form-input" placeholder="e.g., Morning Tennis Practice" />
          </div>

          <div>
            <label class="form-label">Description</label>
            <textarea formControlName="description" class="form-input" rows="3" placeholder="Describe the session..."></textarea>
          </div>

          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label class="form-label">Category *</label>
              <select formControlName="category" class="form-input">
                @for (cat of categories; track cat) {
                  <option [value]="cat">{{ cat }}</option>
                }
              </select>
            </div>
            <div>
              <label class="form-label">Venue</label>
              <select formControlName="venueId" class="form-input">
                <option value="">Select venue</option>
                @for (venue of venues; track venue.id) {
                  <option [value]="venue.id">{{ venue.name }}</option>
                }
              </select>
            </div>
          </div>

          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label class="form-label">Start Date & Time *</label>
              <input type="datetime-local" formControlName="startTime" class="form-input" />
            </div>
            <div>
              <label class="form-label">End Date & Time *</label>
              <input type="datetime-local" formControlName="endTime" class="form-input" />
            </div>
          </div>

          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label class="form-label">Capacity *</label>
              <input type="number" formControlName="capacity" class="form-input" min="1" />
            </div>
            <div>
              <label class="form-label">Session Fee</label>
              <input type="number" formControlName="sessionFee" class="form-input" min="0" step="0.01" placeholder="0.00" />
            </div>
          </div>
        </div>

        <div class="flex justify-end gap-3 mt-8 pt-6 border-t border-gray-200">
          <a routerLink="/club/sessions" class="btn-secondary">Cancel</a>
          <button type="submit" [disabled]="sessionForm.invalid || isSubmitting" class="btn-primary">
            @if (isSubmitting) {
              <app-loading-spinner size="sm" containerClass="mr-2"></app-loading-spinner>
              Creating...
            } @else {
              Create Session
            }
          </button>
        </div>
      </form>
    </div>
  `
})
export class SessionFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private sessionService = inject(SessionService);
  private venueService = inject(VenueService);
  private notificationService = inject(NotificationService);
  private router = inject(Router);

  sessionForm: FormGroup;
  isSubmitting = false;
  venues: Venue[] = [];
  categories = Object.values(SessionCategory);

  constructor() {
    this.sessionForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      category: [SessionCategory.AllAges, Validators.required],
      startTime: ['', Validators.required],
      endTime: ['', Validators.required],
      capacity: [20, [Validators.required, Validators.min(1)]],
      sessionFee: [null],
      venueId: ['']
    });
  }

  ngOnInit(): void {
    this.venueService.getVenues().subscribe({
      next: (venues) => {
        this.venues = venues.filter(v => v.isActive);
      }
    });
  }

  onSubmit(): void {
    if (this.sessionForm.invalid) return;

    this.isSubmitting = true;
    const formData = this.sessionForm.value;

    this.sessionService.createSession({
      ...formData,
      startTime: new Date(formData.startTime),
      endTime: new Date(formData.endTime),
      venueId: formData.venueId || undefined
    }).subscribe({
      next: () => {
        this.notificationService.success('Session created successfully');
        this.router.navigate(['/club/sessions']);
      },
      error: () => {
        this.isSubmitting = false;
        this.notificationService.error('Failed to create session');
      }
    });
  }
}
