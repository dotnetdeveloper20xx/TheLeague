import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { ClubService } from '../../../core/services/club.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent } from '../../../shared/components';
import { ClubType } from '../../../core/models';

@Component({
  selector: 'app-club-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, LoadingSpinnerComponent],
  template: `
    <div class="space-y-6">
      <!-- Header -->
      <div class="flex items-center gap-4">
        <a routerLink="/admin/clubs" class="p-2 text-gray-500 hover:text-gray-700 rounded-lg hover:bg-gray-100">
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
          </svg>
        </a>
        <div>
          <h1 class="text-2xl font-bold text-gray-900">{{ isEditMode ? 'Edit Club' : 'Add New Club' }}</h1>
          <p class="text-gray-500 mt-1">{{ isEditMode ? 'Update club information' : 'Create a new club' }}</p>
        </div>
      </div>

      <form [formGroup]="clubForm" (ngSubmit)="onSubmit()" class="card max-w-2xl">
        <div class="space-y-6">
          <!-- Basic Info -->
          <div>
            <h3 class="text-lg font-medium text-gray-900 mb-4">Basic Information</h3>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div class="md:col-span-2">
                <label class="form-label">Club Name *</label>
                <input type="text" formControlName="name" class="form-input" placeholder="Enter club name" />
                @if (clubForm.get('name')?.invalid && clubForm.get('name')?.touched) {
                  <p class="form-error">Club name is required</p>
                }
              </div>

              <div>
                <label class="form-label">URL Slug *</label>
                <input type="text" formControlName="slug" class="form-input" placeholder="e.g., riverside-tennis" />
                @if (clubForm.get('slug')?.invalid && clubForm.get('slug')?.touched) {
                  <p class="form-error">Slug is required and must be lowercase with hyphens only</p>
                }
              </div>

              <div>
                <label class="form-label">Club Type *</label>
                <select formControlName="clubType" class="form-input">
                  <option value="">Select type</option>
                  @for (type of clubTypes; track type) {
                    <option [value]="type">{{ type }}</option>
                  }
                </select>
                @if (clubForm.get('clubType')?.invalid && clubForm.get('clubType')?.touched) {
                  <p class="form-error">Club type is required</p>
                }
              </div>

              <div class="md:col-span-2">
                <label class="form-label">Description</label>
                <textarea formControlName="description" class="form-input" rows="3" placeholder="Brief description of the club"></textarea>
              </div>
            </div>
          </div>

          <!-- Contact Info -->
          <div>
            <h3 class="text-lg font-medium text-gray-900 mb-4">Contact Information</h3>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="form-label">Contact Email</label>
                <input type="email" formControlName="contactEmail" class="form-input" placeholder="contact@club.com" />
              </div>

              <div>
                <label class="form-label">Contact Phone</label>
                <input type="tel" formControlName="contactPhone" class="form-input" placeholder="+44 1234 567890" />
              </div>

              <div class="md:col-span-2">
                <label class="form-label">Address</label>
                <input type="text" formControlName="address" class="form-input" placeholder="Club address" />
              </div>

              <div class="md:col-span-2">
                <label class="form-label">Website</label>
                <input type="url" formControlName="website" class="form-input" placeholder="https://www.club.com" />
              </div>
            </div>
          </div>

          <!-- Branding -->
          <div>
            <h3 class="text-lg font-medium text-gray-900 mb-4">Branding</h3>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="form-label">Primary Color</label>
                <div class="flex items-center gap-3">
                  <input type="color" formControlName="primaryColor" class="h-10 w-20 rounded border cursor-pointer" />
                  <input type="text" [value]="clubForm.get('primaryColor')?.value" class="form-input flex-1" readonly />
                </div>
              </div>

              <div>
                <label class="form-label">Secondary Color</label>
                <div class="flex items-center gap-3">
                  <input type="color" formControlName="secondaryColor" class="h-10 w-20 rounded border cursor-pointer" />
                  <input type="text" [value]="clubForm.get('secondaryColor')?.value" class="form-input flex-1" readonly />
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Actions -->
        <div class="flex justify-end gap-3 mt-8 pt-6 border-t border-gray-200">
          <a routerLink="/admin/clubs" class="btn-secondary">Cancel</a>
          <button type="submit" [disabled]="clubForm.invalid || isSubmitting" class="btn-primary">
            @if (isSubmitting) {
              <app-loading-spinner size="sm" containerClass="mr-2"></app-loading-spinner>
              {{ isEditMode ? 'Updating...' : 'Creating...' }}
            } @else {
              {{ isEditMode ? 'Update Club' : 'Create Club' }}
            }
          </button>
        </div>
      </form>
    </div>
  `
})
export class ClubFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private clubService = inject(ClubService);
  private notificationService = inject(NotificationService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  clubForm: FormGroup;
  isEditMode = false;
  isSubmitting = false;
  clubId?: string;

  clubTypes = Object.values(ClubType);

  constructor() {
    this.clubForm = this.fb.group({
      name: ['', Validators.required],
      slug: ['', [Validators.required, Validators.pattern(/^[a-z0-9-]+$/)]],
      clubType: ['', Validators.required],
      description: [''],
      contactEmail: ['', Validators.email],
      contactPhone: [''],
      address: [''],
      website: [''],
      primaryColor: ['#3B82F6'],
      secondaryColor: ['#10B981']
    });
  }

  ngOnInit(): void {
    this.clubId = this.route.snapshot.params['id'];
    if (this.clubId) {
      this.isEditMode = true;
      this.loadClub();
    }

    // Auto-generate slug from name
    this.clubForm.get('name')?.valueChanges.subscribe(name => {
      if (!this.isEditMode && name) {
        const slug = name.toLowerCase().replace(/[^a-z0-9]+/g, '-').replace(/(^-|-$)/g, '');
        this.clubForm.patchValue({ slug }, { emitEvent: false });
      }
    });
  }

  private loadClub(): void {
    this.clubService.getClub(this.clubId!).subscribe({
      next: (club) => {
        this.clubForm.patchValue({
          name: club.name,
          slug: club.slug,
          clubType: club.clubType,
          description: club.description,
          contactEmail: club.contactEmail,
          contactPhone: club.contactPhone,
          address: club.address,
          website: club.website,
          primaryColor: club.primaryColor,
          secondaryColor: club.secondaryColor
        });
      },
      error: () => {
        this.notificationService.error('Failed to load club');
        this.router.navigate(['/admin/clubs']);
      }
    });
  }

  onSubmit(): void {
    if (this.clubForm.invalid) return;

    this.isSubmitting = true;
    const formData = this.clubForm.value;

    const request = this.isEditMode
      ? this.clubService.updateClub(this.clubId!, formData)
      : this.clubService.createClub(formData);

    request.subscribe({
      next: () => {
        this.notificationService.success(
          this.isEditMode ? 'Club updated successfully' : 'Club created successfully'
        );
        this.router.navigate(['/admin/clubs']);
      },
      error: () => {
        this.isSubmitting = false;
        this.notificationService.error(
          this.isEditMode ? 'Failed to update club' : 'Failed to create club'
        );
      }
    });
  }
}
