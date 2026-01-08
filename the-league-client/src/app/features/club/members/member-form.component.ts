import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { MemberService } from '../../../core/services/member.service';
import { MembershipService } from '../../../core/services/membership.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent } from '../../../shared/components';
import { FamilyMemberRelation, MembershipType } from '../../../core/models';

@Component({
  selector: 'app-member-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, LoadingSpinnerComponent],
  template: `
    <div class="space-y-6">
      <!-- Header -->
      <div class="flex items-center gap-4">
        <a routerLink="/club/members" class="p-2 text-gray-500 hover:text-gray-700 rounded-lg hover:bg-gray-100">
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
          </svg>
        </a>
        <div>
          <h1 class="text-2xl font-bold text-gray-900">{{ isEditMode ? 'Edit Member' : 'Add New Member' }}</h1>
          <p class="text-gray-500 mt-1">{{ isEditMode ? 'Update member details' : 'Register a new club member' }}</p>
        </div>
      </div>

      <form [formGroup]="memberForm" (ngSubmit)="onSubmit()" class="space-y-6 max-w-3xl">
        <!-- Personal Information -->
        <div class="card">
          <h3 class="text-lg font-medium text-gray-900 mb-4">Personal Information</h3>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label class="form-label">First Name *</label>
              <input type="text" formControlName="firstName" class="form-input" />
            </div>
            <div>
              <label class="form-label">Last Name *</label>
              <input type="text" formControlName="lastName" class="form-input" />
            </div>
            <div>
              <label class="form-label">Email *</label>
              <input type="email" formControlName="email" class="form-input" />
            </div>
            <div>
              <label class="form-label">Phone</label>
              <input type="tel" formControlName="phone" class="form-input" />
            </div>
            <div>
              <label class="form-label">Date of Birth</label>
              <input type="date" formControlName="dateOfBirth" class="form-input" />
            </div>
            <div>
              <label class="form-label">Membership Type</label>
              <select formControlName="membershipTypeId" class="form-input">
                <option value="">Select membership</option>
                @for (type of membershipTypes; track type.id) {
                  <option [value]="type.id">{{ type.name }} - {{ type.annualFee | currency:'GBP' }}/year</option>
                }
              </select>
            </div>
          </div>
        </div>

        <!-- Address -->
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

        <!-- Emergency Contact -->
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

        <!-- Medical Information -->
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

        <!-- Family Account Toggle -->
        <div class="card">
          <label class="flex items-center gap-3">
            <input type="checkbox" formControlName="isFamilyAccount" class="rounded border-gray-300 text-primary-600 focus:ring-primary-500" />
            <span class="font-medium text-gray-900">This is a family account</span>
          </label>
          <p class="text-sm text-gray-500 mt-1 ml-7">Enable to add family members to this account</p>

          @if (memberForm.get('isFamilyAccount')?.value) {
            <div class="mt-6 space-y-4">
              <div class="flex items-center justify-between">
                <h4 class="font-medium text-gray-900">Family Members</h4>
                <button type="button" (click)="addFamilyMember()" class="btn-secondary text-sm">
                  Add Family Member
                </button>
              </div>

              @for (family of familyMembers.controls; track $index; let i = $index) {
                <div class="p-4 bg-gray-50 rounded-lg" [formGroupName]="i">
                  <div class="flex justify-between items-start mb-4">
                    <h5 class="font-medium text-gray-700">Family Member {{ i + 1 }}</h5>
                    <button type="button" (click)="removeFamilyMember(i)" class="text-red-600 hover:text-red-700">
                      <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                      </svg>
                    </button>
                  </div>
                  <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
                    <div>
                      <label class="form-label text-sm">First Name</label>
                      <input type="text" formControlName="firstName" class="form-input" />
                    </div>
                    <div>
                      <label class="form-label text-sm">Last Name</label>
                      <input type="text" formControlName="lastName" class="form-input" />
                    </div>
                    <div>
                      <label class="form-label text-sm">Date of Birth</label>
                      <input type="date" formControlName="dateOfBirth" class="form-input" />
                    </div>
                    <div>
                      <label class="form-label text-sm">Relation</label>
                      <select formControlName="relation" class="form-input">
                        @for (rel of relations; track rel) {
                          <option [value]="rel">{{ rel }}</option>
                        }
                      </select>
                    </div>
                  </div>
                </div>
              }
            </div>
          }
        </div>

        <!-- Actions -->
        <div class="flex justify-end gap-3">
          <a routerLink="/club/members" class="btn-secondary">Cancel</a>
          <button type="submit" [disabled]="memberForm.invalid || isSubmitting" class="btn-primary">
            @if (isSubmitting) {
              <app-loading-spinner size="sm" containerClass="mr-2"></app-loading-spinner>
              {{ isEditMode ? 'Updating...' : 'Creating...' }}
            } @else {
              {{ isEditMode ? 'Update Member' : 'Create Member' }}
            }
          </button>
        </div>
      </form>
    </div>
  `
})
export class MemberFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private memberService = inject(MemberService);
  private membershipService = inject(MembershipService);
  private notificationService = inject(NotificationService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  memberForm: FormGroup;
  isEditMode = false;
  isSubmitting = false;
  memberId?: string;
  membershipTypes: MembershipType[] = [];
  relations = Object.values(FamilyMemberRelation);

  constructor() {
    this.memberForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: [''],
      dateOfBirth: [''],
      address: [''],
      city: [''],
      postCode: [''],
      emergencyContactName: [''],
      emergencyContactPhone: [''],
      emergencyContactRelation: [''],
      medicalConditions: [''],
      allergies: [''],
      isFamilyAccount: [false],
      membershipTypeId: [''],
      familyMembers: this.fb.array([])
    });
  }

  get familyMembers(): FormArray {
    return this.memberForm.get('familyMembers') as FormArray;
  }

  ngOnInit(): void {
    this.loadMembershipTypes();

    this.memberId = this.route.snapshot.params['id'];
    if (this.memberId) {
      this.isEditMode = true;
      this.loadMember();
    }
  }

  private loadMembershipTypes(): void {
    this.membershipService.getMembershipTypes().subscribe({
      next: (types) => {
        this.membershipTypes = types.filter(t => t.isActive);
      }
    });
  }

  private loadMember(): void {
    this.memberService.getMember(this.memberId!).subscribe({
      next: (member) => {
        this.memberForm.patchValue({
          firstName: member.firstName,
          lastName: member.lastName,
          email: member.email,
          phone: member.phone,
          dateOfBirth: member.dateOfBirth,
          address: member.address,
          city: member.city,
          postCode: member.postCode,
          emergencyContactName: member.emergencyContactName,
          emergencyContactPhone: member.emergencyContactPhone,
          emergencyContactRelation: member.emergencyContactRelation,
          medicalConditions: member.medicalConditions,
          allergies: member.allergies,
          isFamilyAccount: member.isFamilyAccount
        });

        if (member.familyMembers) {
          member.familyMembers.forEach(fm => {
            this.familyMembers.push(this.fb.group({
              firstName: [fm.firstName, Validators.required],
              lastName: [fm.lastName, Validators.required],
              dateOfBirth: [fm.dateOfBirth],
              relation: [fm.relation, Validators.required]
            }));
          });
        }
      },
      error: () => {
        this.notificationService.error('Failed to load member');
        this.router.navigate(['/club/members']);
      }
    });
  }

  addFamilyMember(): void {
    this.familyMembers.push(this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      dateOfBirth: [''],
      relation: [FamilyMemberRelation.Child, Validators.required]
    }));
  }

  removeFamilyMember(index: number): void {
    this.familyMembers.removeAt(index);
  }

  onSubmit(): void {
    if (this.memberForm.invalid) return;

    this.isSubmitting = true;
    const formData = this.memberForm.value;

    const request = this.isEditMode
      ? this.memberService.updateMember(this.memberId!, formData)
      : this.memberService.createMember(formData);

    request.subscribe({
      next: () => {
        this.notificationService.success(
          this.isEditMode ? 'Member updated successfully' : 'Member created successfully'
        );
        this.router.navigate(['/club/members']);
      },
      error: () => {
        this.isSubmitting = false;
        this.notificationService.error(
          this.isEditMode ? 'Failed to update member' : 'Failed to create member'
        );
      }
    });
  }
}
