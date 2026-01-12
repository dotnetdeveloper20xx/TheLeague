import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { FeeService } from '../../../core/services/fee.service';
import { LoadingSpinnerComponent, EmptyStateComponent } from '../../../shared/components';
import { FeeListItem, FeeType, FeeFrequency, FeeTypeLabels, FeeFrequencyLabels, PagedResult } from '../../../core/models';
import { NotificationService } from '../../../core/services';

@Component({
  selector: 'app-fees-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, LoadingSpinnerComponent, EmptyStateComponent],
  template: `
    <div class="space-y-6">
      <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 class="text-2xl font-bold text-gray-900">Fee Management</h1>
          <p class="text-gray-500 mt-1">Define and manage fees for your club</p>
        </div>
        <a routerLink="new" class="btn-primary inline-flex items-center">
          <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
          </svg>
          Add Fee
        </a>
      </div>

      <!-- Filters -->
      <div class="card">
        <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Search</label>
            <input
              type="text"
              [(ngModel)]="searchTerm"
              (ngModelChange)="onSearch()"
              placeholder="Search fees..."
              class="input"
            />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Fee Type</label>
            <select [(ngModel)]="selectedType" (ngModelChange)="loadFees()" class="input">
              <option [ngValue]="null">All Types</option>
              @for (type of feeTypes; track type) {
                <option [value]="type">{{ getFeeTypeLabel(type) }}</option>
              }
            </select>
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Frequency</label>
            <select [(ngModel)]="selectedFrequency" (ngModelChange)="loadFees()" class="input">
              <option [ngValue]="null">All Frequencies</option>
              @for (freq of frequencies; track freq) {
                <option [value]="freq">{{ getFrequencyLabel(freq) }}</option>
              }
            </select>
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Status</label>
            <select [(ngModel)]="showInactive" (ngModelChange)="loadFees()" class="input">
              <option [ngValue]="false">Active Only</option>
              <option [ngValue]="true">Include Inactive</option>
            </select>
          </div>
        </div>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg"></app-loading-spinner>
        </div>
      } @else if (fees().length === 0) {
        <app-empty-state
          icon="fee"
          title="No fees defined"
          message="Create your first fee to start managing club charges."
        ></app-empty-state>
      } @else {
        <div class="card overflow-hidden">
          <table class="min-w-full divide-y divide-gray-200">
            <thead class="bg-gray-50">
              <tr>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Name</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Type</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Amount</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Frequency</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Payments</th>
                <th class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              @for (fee of fees(); track fee.id) {
                <tr [class.opacity-60]="!fee.isActive">
                  <td class="px-6 py-4 whitespace-nowrap">
                    <div class="flex items-center">
                      <div>
                        <div class="text-sm font-medium text-gray-900">{{ fee.name }}</div>
                        @if (fee.code) {
                          <div class="text-sm text-gray-500">{{ fee.code }}</div>
                        }
                      </div>
                      @if (fee.isRequired) {
                        <span class="ml-2 badge badge-warning">Required</span>
                      }
                    </div>
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap">
                    <span class="badge badge-info">{{ getFeeTypeLabel(fee.type) }}</span>
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {{ fee.currency }} {{ fee.amount | number:'1.2-2' }}
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {{ getFrequencyLabel(fee.frequency) }}
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap">
                    <button
                      (click)="toggleActive(fee)"
                      [class]="fee.isActive ? 'badge badge-success cursor-pointer' : 'badge badge-gray cursor-pointer'"
                    >
                      {{ fee.isActive ? 'Active' : 'Inactive' }}
                    </button>
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {{ fee.paymentCount }}
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                    <a [routerLink]="[fee.id]" class="text-blue-600 hover:text-blue-900 mr-4">Edit</a>
                    <button (click)="deleteFee(fee)" class="text-red-600 hover:text-red-900">Delete</button>
                  </td>
                </tr>
              }
            </tbody>
          </table>
        </div>

        <!-- Pagination -->
        @if (totalPages() > 1) {
          <div class="flex items-center justify-between">
            <p class="text-sm text-gray-700">
              Showing {{ (currentPage() - 1) * pageSize + 1 }} to {{ Math.min(currentPage() * pageSize, totalCount()) }} of {{ totalCount() }} fees
            </p>
            <div class="flex gap-2">
              <button
                (click)="goToPage(currentPage() - 1)"
                [disabled]="currentPage() === 1"
                class="btn-secondary"
                [class.opacity-50]="currentPage() === 1"
              >
                Previous
              </button>
              <button
                (click)="goToPage(currentPage() + 1)"
                [disabled]="currentPage() === totalPages()"
                class="btn-secondary"
                [class.opacity-50]="currentPage() === totalPages()"
              >
                Next
              </button>
            </div>
          </div>
        }
      }
    </div>
  `
})
export class FeesListComponent implements OnInit {
  private feeService = inject(FeeService);
  private notificationService = inject(NotificationService);

  isLoading = signal(true);
  fees = signal<FeeListItem[]>([]);
  currentPage = signal(1);
  totalPages = signal(1);
  totalCount = signal(0);
  pageSize = 20;

  searchTerm = '';
  selectedType: FeeType | null = null;
  selectedFrequency: FeeFrequency | null = null;
  showInactive = false;

  feeTypes = Object.values(FeeType);
  frequencies = Object.values(FeeFrequency);

  Math = Math;

  ngOnInit(): void {
    this.loadFees();
  }

  loadFees(): void {
    this.isLoading.set(true);
    this.feeService.getFees({
      search: this.searchTerm || undefined,
      type: this.selectedType || undefined,
      frequency: this.selectedFrequency || undefined,
      isActive: this.showInactive ? undefined : true,
      page: this.currentPage(),
      pageSize: this.pageSize
    }).subscribe({
      next: (result) => {
        this.fees.set(result.items);
        this.totalCount.set(result.totalCount);
        this.totalPages.set(result.totalPages);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
        this.notificationService.error('Failed to load fees');
      }
    });
  }

  onSearch(): void {
    this.currentPage.set(1);
    this.loadFees();
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages()) {
      this.currentPage.set(page);
      this.loadFees();
    }
  }

  toggleActive(fee: FeeListItem): void {
    this.feeService.toggleActive(fee.id).subscribe({
      next: () => {
        this.notificationService.success(`Fee ${fee.isActive ? 'deactivated' : 'activated'} successfully`);
        this.loadFees();
      },
      error: () => {
        this.notificationService.error('Failed to update fee status');
      }
    });
  }

  deleteFee(fee: FeeListItem): void {
    if (confirm(`Are you sure you want to delete "${fee.name}"?`)) {
      this.feeService.deleteFee(fee.id).subscribe({
        next: () => {
          this.notificationService.success('Fee deleted successfully');
          this.loadFees();
        },
        error: () => {
          this.notificationService.error('Failed to delete fee');
        }
      });
    }
  }

  getFeeTypeLabel(type: FeeType): string {
    return FeeTypeLabels[type] || type;
  }

  getFrequencyLabel(frequency: FeeFrequency): string {
    return FeeFrequencyLabels[frequency] || frequency;
  }
}
