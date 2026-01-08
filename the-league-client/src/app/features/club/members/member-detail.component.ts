import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { MemberService } from '../../../core/services/member.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent, StatusBadgeComponent, ConfirmDialogComponent } from '../../../shared/components';
import { DateFormatPipe, CurrencyFormatPipe } from '../../../shared/pipes';
import { Member } from '../../../core/models';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, LoadingSpinnerComponent, StatusBadgeComponent, ConfirmDialogComponent, DateFormatPipe, CurrencyFormatPipe],
  template: `
    @if (isLoading()) {
      <div class="flex justify-center py-12">
        <app-loading-spinner size="lg" message="Loading member details..."></app-loading-spinner>
      </div>
    } @else if (member()) {
      <div class="space-y-6">
        <!-- Header -->
        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div class="flex items-center gap-4">
            <a routerLink="/club/members" class="p-2 text-gray-500 hover:text-gray-700 rounded-lg hover:bg-gray-100">
              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
              </svg>
            </a>
            <div class="w-16 h-16 rounded-full bg-primary-100 flex items-center justify-center text-primary-600 text-2xl font-bold">
              {{ member()!.firstName[0] }}{{ member()!.lastName[0] }}
            </div>
            <div>
              <h1 class="text-2xl font-bold text-gray-900">{{ member()!.fullName }}</h1>
              <div class="flex items-center gap-2 mt-1">
                <app-status-badge [status]="member()!.status" type="member"></app-status-badge>
                @if (member()!.isFamilyAccount) {
                  <span class="badge badge-info">Family Account</span>
                }
              </div>
            </div>
          </div>
          <div class="flex gap-3">
            <a [routerLink]="['/club/members', member()!.id, 'edit']" class="btn-secondary">Edit</a>
            <button (click)="showDeleteDialog.set(true)" class="btn-danger">Delete</button>
          </div>
        </div>

        <!-- Info Cards -->
        <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
          <!-- Contact Info -->
          <div class="card">
            <h3 class="card-header">Contact Information</h3>
            <dl class="space-y-3">
              <div>
                <dt class="text-sm text-gray-500">Email</dt>
                <dd class="text-gray-900">
                  <a [href]="'mailto:' + member()!.email" class="text-primary-600 hover:underline">{{ member()!.email }}</a>
                  @if (member()!.emailVerified) {
                    <span class="ml-2 text-green-600" title="Verified">&#10003;</span>
                  }
                </dd>
              </div>
              <div>
                <dt class="text-sm text-gray-500">Phone</dt>
                <dd class="text-gray-900">{{ member()!.phone || '-' }}</dd>
              </div>
              <div>
                <dt class="text-sm text-gray-500">Address</dt>
                <dd class="text-gray-900">
                  @if (member()!.address) {
                    {{ member()!.address }}<br />
                    {{ member()!.city }} {{ member()!.postCode }}
                  } @else {
                    -
                  }
                </dd>
              </div>
            </dl>
          </div>

          <!-- Membership Info -->
          <div class="card">
            <h3 class="card-header">Membership</h3>
            @if (member()!.currentMembership) {
              <dl class="space-y-3">
                <div>
                  <dt class="text-sm text-gray-500">Type</dt>
                  <dd class="text-gray-900 font-medium">{{ member()!.currentMembership!.membershipType }}</dd>
                </div>
                <div>
                  <dt class="text-sm text-gray-500">Status</dt>
                  <dd><app-status-badge [status]="member()!.currentMembership!.status" type="membership"></app-status-badge></dd>
                </div>
                <div>
                  <dt class="text-sm text-gray-500">Valid Until</dt>
                  <dd class="text-gray-900">{{ member()!.currentMembership!.endDate | dateFormat }}</dd>
                </div>
                <div>
                  <dt class="text-sm text-gray-500">Amount Due</dt>
                  <dd class="text-gray-900">{{ member()!.currentMembership!.amountDue | currencyFormat }}</dd>
                </div>
              </dl>
            } @else {
              <p class="text-gray-500 py-4">No active membership</p>
              <button class="btn-primary w-full mt-2">Assign Membership</button>
            }
          </div>

          <!-- Emergency Contact -->
          <div class="card">
            <h3 class="card-header">Emergency Contact</h3>
            @if (member()!.emergencyContactName) {
              <dl class="space-y-3">
                <div>
                  <dt class="text-sm text-gray-500">Name</dt>
                  <dd class="text-gray-900">{{ member()!.emergencyContactName }}</dd>
                </div>
                <div>
                  <dt class="text-sm text-gray-500">Phone</dt>
                  <dd class="text-gray-900">{{ member()!.emergencyContactPhone || '-' }}</dd>
                </div>
                <div>
                  <dt class="text-sm text-gray-500">Relationship</dt>
                  <dd class="text-gray-900">{{ member()!.emergencyContactRelation || '-' }}</dd>
                </div>
              </dl>
            } @else {
              <p class="text-gray-500 py-4">No emergency contact on file</p>
            }
          </div>
        </div>

        <!-- Family Members -->
        @if (member()!.isFamilyAccount && member()!.familyMembers?.length) {
          <div class="card">
            <h3 class="card-header">Family Members</h3>
            <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
              @for (fm of member()!.familyMembers; track fm.id) {
                <div class="p-4 bg-gray-50 rounded-lg">
                  <div class="flex items-center justify-between">
                    <div>
                      <p class="font-medium text-gray-900">{{ fm.fullName }}</p>
                      <p class="text-sm text-gray-500">{{ fm.relation }}</p>
                    </div>
                    <span class="badge" [class]="fm.isActive ? 'badge-success' : 'badge-gray'">
                      {{ fm.isActive ? 'Active' : 'Inactive' }}
                    </span>
                  </div>
                  @if (fm.dateOfBirth) {
                    <p class="text-sm text-gray-500 mt-2">DOB: {{ fm.dateOfBirth | dateFormat:'short' }}</p>
                  }
                </div>
              }
            </div>
          </div>
        }

        <!-- Medical Info -->
        @if (member()!.medicalConditions || member()!.allergies) {
          <div class="card">
            <h3 class="card-header">Medical Information</h3>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <h4 class="text-sm font-medium text-gray-500 mb-2">Medical Conditions</h4>
                <p class="text-gray-900">{{ member()!.medicalConditions || 'None reported' }}</p>
              </div>
              <div>
                <h4 class="text-sm font-medium text-gray-500 mb-2">Allergies</h4>
                <p class="text-gray-900">{{ member()!.allergies || 'None reported' }}</p>
              </div>
            </div>
          </div>
        }

        <!-- Activity -->
        <div class="card">
          <h3 class="card-header">Activity</h3>
          <dl class="flex gap-8">
            <div>
              <dt class="text-sm text-gray-500">Joined</dt>
              <dd class="text-gray-900 font-medium">{{ member()!.joinedDate | dateFormat }}</dd>
            </div>
            <div>
              <dt class="text-sm text-gray-500">Status</dt>
              <dd><app-status-badge [status]="member()!.status" type="member"></app-status-badge></dd>
            </div>
          </dl>
        </div>
      </div>
    }

    <!-- Delete Confirmation -->
    <app-confirm-dialog
      [isOpen]="showDeleteDialog()"
      title="Delete Member"
      [message]="'Are you sure you want to delete ' + (member()?.fullName || '') + '? This action cannot be undone.'"
      confirmText="Delete"
      type="danger"
      (confirm)="deleteMember()"
      (cancel)="showDeleteDialog.set(false)"
    ></app-confirm-dialog>
  `
})
export class MemberDetailComponent implements OnInit {
  private memberService = inject(MemberService);
  private notificationService = inject(NotificationService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  isLoading = signal(true);
  member = signal<Member | null>(null);
  showDeleteDialog = signal(false);

  ngOnInit(): void {
    const memberId = this.route.snapshot.params['id'];
    if (memberId) {
      this.loadMember(memberId);
    }
  }

  private loadMember(id: string): void {
    this.memberService.getMember(id).subscribe({
      next: (member) => {
        this.member.set(member);
        this.isLoading.set(false);
      },
      error: () => {
        this.notificationService.error('Failed to load member');
        this.router.navigate(['/club/members']);
      }
    });
  }

  deleteMember(): void {
    const member = this.member();
    if (!member) return;

    this.memberService.deleteMember(member.id).subscribe({
      next: () => {
        this.notificationService.success(`${member.fullName} has been deleted`);
        this.router.navigate(['/club/members']);
      },
      error: () => {
        this.notificationService.error('Failed to delete member');
      }
    });
  }
}
