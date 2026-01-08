import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PortalService } from '../../../core/services/portal.service';
import { LoadingSpinnerComponent, EmptyStateComponent } from '../../../shared/components';
import { DateFormatPipe } from '../../../shared/pipes';
import { Member, FamilyMember } from '../../../core/models';

@Component({
  selector: 'app-family-members',
  standalone: true,
  imports: [CommonModule, LoadingSpinnerComponent, EmptyStateComponent, DateFormatPipe],
  template: `
    <div class="space-y-6">
      <div class="flex items-center justify-between">
        <h1 class="text-2xl font-bold text-gray-900">Family Members</h1>
        @if (member()?.isFamilyAccount) {
          <button class="btn-primary">Add Family Member</button>
        }
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg"></app-loading-spinner>
        </div>
      } @else if (!member()?.isFamilyAccount) {
        <div class="card text-center py-8">
          <p class="text-gray-500 mb-4">Your account is not set up as a family account.</p>
          <p class="text-sm text-gray-400">Contact your club administrator to upgrade to a family membership.</p>
        </div>
      } @else if (!member()?.familyMembers?.length) {
        <app-empty-state
          icon="users"
          title="No family members"
          message="You can add family members to your account."
          actionText="Add Family Member"
        ></app-empty-state>
      } @else {
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          @for (fm of member()!.familyMembers; track fm.id) {
            <div class="card">
              <div class="flex items-center gap-4 mb-4">
                <div class="w-12 h-12 rounded-full bg-primary-100 flex items-center justify-center text-primary-600 font-bold">
                  {{ fm.firstName[0] }}{{ fm.lastName[0] }}
                </div>
                <div>
                  <h3 class="font-semibold text-gray-900">{{ fm.fullName }}</h3>
                  <p class="text-sm text-gray-500">{{ fm.relation }}</p>
                </div>
              </div>
              <div class="space-y-2 text-sm">
                @if (fm.dateOfBirth) {
                  <div class="flex justify-between">
                    <span class="text-gray-500">Date of Birth</span>
                    <span class="text-gray-900">{{ fm.dateOfBirth | dateFormat:'short' }}</span>
                  </div>
                }
                <div class="flex justify-between">
                  <span class="text-gray-500">Status</span>
                  <span [class]="fm.isActive ? 'text-green-600' : 'text-gray-400'">
                    {{ fm.isActive ? 'Active' : 'Inactive' }}
                  </span>
                </div>
              </div>
              <div class="mt-4 pt-4 border-t flex gap-2">
                <button class="btn-secondary flex-1 text-sm">Edit</button>
                <button class="btn-danger text-sm px-3">
                  <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                  </svg>
                </button>
              </div>
            </div>
          }
        </div>
      }
    </div>
  `
})
export class FamilyMembersComponent implements OnInit {
  private portalService = inject(PortalService);

  isLoading = signal(true);
  member = signal<Member | null>(null);

  ngOnInit(): void {
    this.portalService.getProfile().subscribe({
      next: (member) => {
        this.member.set(member);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }
}
