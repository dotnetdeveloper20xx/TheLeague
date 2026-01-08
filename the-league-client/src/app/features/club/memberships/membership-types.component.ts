import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MembershipService } from '../../../core/services/membership.service';
import { LoadingSpinnerComponent } from '../../../shared/components';
import { CurrencyFormatPipe } from '../../../shared/pipes';
import { MembershipType } from '../../../core/models';

@Component({
  selector: 'app-membership-types',
  standalone: true,
  imports: [CommonModule, RouterLink, LoadingSpinnerComponent, CurrencyFormatPipe],
  template: `
    <div class="space-y-6">
      <div class="flex items-center gap-4">
        <a routerLink="/club/memberships" class="p-2 text-gray-500 hover:text-gray-700 rounded-lg hover:bg-gray-100">
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
          </svg>
        </a>
        <div>
          <h1 class="text-2xl font-bold text-gray-900">Membership Types</h1>
          <p class="text-gray-500 mt-1">Configure your club's membership options</p>
        </div>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg"></app-loading-spinner>
        </div>
      } @else {
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          @for (type of membershipTypes(); track type.id) {
            <div class="card" [class.opacity-60]="!type.isActive">
              <div class="flex items-start justify-between mb-4">
                <h3 class="text-lg font-semibold text-gray-900">{{ type.name }}</h3>
                <span class="badge" [class]="type.isActive ? 'badge-success' : 'badge-gray'">
                  {{ type.isActive ? 'Active' : 'Inactive' }}
                </span>
              </div>
              <p class="text-sm text-gray-500 mb-4">{{ type.description || 'No description' }}</p>
              <div class="space-y-2 text-sm">
                <div class="flex justify-between">
                  <span class="text-gray-500">Annual Fee</span>
                  <span class="font-medium">{{ type.annualFee | currencyFormat }}</span>
                </div>
                @if (type.monthlyFee) {
                  <div class="flex justify-between">
                    <span class="text-gray-500">Monthly Fee</span>
                    <span class="font-medium">{{ type.monthlyFee | currencyFormat }}</span>
                  </div>
                }
                <div class="flex justify-between">
                  <span class="text-gray-500">Members</span>
                  <span class="font-medium">{{ type.memberCount }}</span>
                </div>
              </div>
            </div>
          }
        </div>
      }
    </div>
  `
})
export class MembershipTypesComponent implements OnInit {
  private membershipService = inject(MembershipService);

  isLoading = signal(true);
  membershipTypes = signal<MembershipType[]>([]);

  ngOnInit(): void {
    this.membershipService.getMembershipTypes().subscribe({
      next: (types) => {
        this.membershipTypes.set(types);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }
}
