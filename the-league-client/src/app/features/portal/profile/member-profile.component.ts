import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { PortalService } from '../../../core/services/portal.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent } from '../../../shared/components';
import { Member } from '../../../core/models';

@Component({
  selector: 'app-member-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, LoadingSpinnerComponent],
  template: `
    <div class="space-y-6">
      <h1 class="text-2xl font-bold text-gray-900">My Profile</h1>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg"></app-loading-spinner>
        </div>
      } @else if (member()) {
        <form [formGroup]="profileForm" (ngSubmit)="onSubmit()" class="space-y-6 max-w-2xl">
          <div class="card">
            <h3 class="text-lg font-medium text-gray-900 mb-4">Personal Information</h3>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="form-label">First Name</label>
                <input type="text" [value]="member()!.firstName" disabled class="form-input bg-gray-50" />
              </div>
              <div>
                <label class="form-label">Last Name</label>
                <input type="text" [value]="member()!.lastName" disabled class="form-input bg-gray-50" />
              </div>
              <div>
                <label class="form-label">Email</label>
                <input type="email" [value]="member()!.email" disabled class="form-input bg-gray-50" />
              </div>
              <div>
                <label class="form-label">Phone</label>
                <input type="tel" formControlName="phone" class="form-input" />
              </div>
            </div>
          </div>

          <div class="card">
            <h3 class="text-lg font-medium text-gray-900 mb-4">Address</h3>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div class="md:col-span-2">
                <label class="form-label">Address</label>
                <input type="text" formControlName="address" class="form-input" />
              </div>
              <div>
                <label class="form-label">City</label>
                <input type="text" formControlName="city" class="form-input" />
              </div>
              <div>
                <label class="form-label">Post Code</label>
                <input type="text" formControlName="postCode" class="form-input" />
              </div>
            </div>
          </div>

          <div class="card">
            <h3 class="text-lg font-medium text-gray-900 mb-4">Emergency Contact</h3>
            <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
              <div>
                <label class="form-label">Contact Name</label>
                <input type="text" formControlName="emergencyContactName" class="form-input" />
              </div>
              <div>
                <label class="form-label">Contact Phone</label>
                <input type="tel" formControlName="emergencyContactPhone" class="form-input" />
              </div>
              <div>
                <label class="form-label">Relationship</label>
                <input type="text" formControlName="emergencyContactRelation" class="form-input" />
              </div>
            </div>
          </div>

          <div class="card">
            <h3 class="text-lg font-medium text-gray-900 mb-4">Medical Information</h3>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="form-label">Medical Conditions</label>
                <textarea formControlName="medicalConditions" class="form-input" rows="3"></textarea>
              </div>
              <div>
                <label class="form-label">Allergies</label>
                <textarea formControlName="allergies" class="form-input" rows="3"></textarea>
              </div>
            </div>
          </div>

          <div class="flex justify-end">
            <button type="submit" [disabled]="isSaving" class="btn-primary">
              @if (isSaving) {
                <app-loading-spinner size="sm" containerClass="mr-2"></app-loading-spinner>
                Saving...
              } @else {
                Save Changes
              }
            </button>
          </div>
        </form>
      }
    </div>
  `
})
export class MemberProfileComponent implements OnInit {
  private fb = inject(FormBuilder);
  private portalService = inject(PortalService);
  private notificationService = inject(NotificationService);

  isLoading = signal(true);
  member = signal<Member | null>(null);
  isSaving = false;
  profileForm: FormGroup;

  constructor() {
    this.profileForm = this.fb.group({
      phone: [''],
      address: [''],
      city: [''],
      postCode: [''],
      emergencyContactName: [''],
      emergencyContactPhone: [''],
      emergencyContactRelation: [''],
      medicalConditions: [''],
      allergies: ['']
    });
  }

  ngOnInit(): void {
    this.portalService.getProfile().subscribe({
      next: (member) => {
        this.member.set(member);
        this.profileForm.patchValue({
          phone: member.phone,
          address: member.address,
          city: member.city,
          postCode: member.postCode,
          emergencyContactName: member.emergencyContactName,
          emergencyContactPhone: member.emergencyContactPhone,
          emergencyContactRelation: member.emergencyContactRelation,
          medicalConditions: member.medicalConditions,
          allergies: member.allergies
        });
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }

  onSubmit(): void {
    this.isSaving = true;
    this.portalService.updateProfile(this.profileForm.value).subscribe({
      next: () => {
        this.isSaving = false;
        this.notificationService.success('Profile updated successfully');
      },
      error: () => {
        this.isSaving = false;
        this.notificationService.error('Failed to update profile');
      }
    });
  }
}
