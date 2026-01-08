import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PaymentService } from '../../../core/services/payment.service';
import { LoadingSpinnerComponent, PaginationComponent, StatusBadgeComponent, EmptyStateComponent } from '../../../shared/components';
import { DateFormatPipe, CurrencyFormatPipe } from '../../../shared/pipes';
import { Payment, PaymentStatus, PagedResult } from '../../../core/models';

@Component({
  selector: 'app-payments-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, LoadingSpinnerComponent, PaginationComponent, StatusBadgeComponent, EmptyStateComponent, DateFormatPipe, CurrencyFormatPipe],
  template: `
    <div class="space-y-6">
      <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 class="text-2xl font-bold text-gray-900">Payments</h1>
          <p class="text-gray-500 mt-1">View and manage all payments</p>
        </div>
        <button (click)="showManualPayment = true" class="btn-primary inline-flex items-center">
          <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
          </svg>
          Record Payment
        </button>
      </div>

      <!-- Stats -->
      <div class="grid grid-cols-1 sm:grid-cols-3 gap-4">
        <div class="card bg-green-50">
          <p class="text-sm text-green-600">Completed</p>
          <p class="text-2xl font-bold text-green-700">{{ stats().completed | currencyFormat }}</p>
        </div>
        <div class="card bg-yellow-50">
          <p class="text-sm text-yellow-600">Pending</p>
          <p class="text-2xl font-bold text-yellow-700">{{ stats().pending | currencyFormat }}</p>
        </div>
        <div class="card bg-red-50">
          <p class="text-sm text-red-600">Failed/Refunded</p>
          <p class="text-2xl font-bold text-red-700">{{ stats().failed | currencyFormat }}</p>
        </div>
      </div>

      <!-- Filters -->
      <div class="card">
        <div class="flex flex-wrap gap-4">
          <select [(ngModel)]="statusFilter" (ngModelChange)="loadPayments()" class="form-input w-40">
            <option value="">All Status</option>
            @for (status of paymentStatuses; track status) {
              <option [value]="status">{{ status }}</option>
            }
          </select>
          <input type="date" [(ngModel)]="dateFrom" (ngModelChange)="loadPayments()" class="form-input" placeholder="From" />
          <input type="date" [(ngModel)]="dateTo" (ngModelChange)="loadPayments()" class="form-input" placeholder="To" />
        </div>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg" message="Loading payments..."></app-loading-spinner>
        </div>
      } @else if (payments().length === 0) {
        <app-empty-state
          icon="credit-card"
          title="No payments found"
          message="Payments will appear here once members make transactions."
        ></app-empty-state>
      } @else {
        <div class="card overflow-hidden p-0">
          <table class="table">
            <thead>
              <tr>
                <th>Member</th>
                <th>Type</th>
                <th>Amount</th>
                <th>Method</th>
                <th>Status</th>
                <th>Date</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              @for (payment of payments(); track payment.id) {
                <tr>
                  <td class="font-medium text-gray-900">{{ payment.memberName }}</td>
                  <td>{{ payment.type }}</td>
                  <td class="font-medium">{{ payment.amount | currencyFormat }}</td>
                  <td>{{ payment.method }}</td>
                  <td><app-status-badge [status]="payment.status" type="payment"></app-status-badge></td>
                  <td>{{ payment.paymentDate | dateFormat }}</td>
                  <td>
                    <a [routerLink]="['/club/payments', payment.id]" class="text-primary-600 hover:text-primary-700 text-sm">
                      View
                    </a>
                  </td>
                </tr>
              }
            </tbody>
          </table>
        </div>

        @if (totalCount() > pageSize) {
          <app-pagination
            [currentPage]="currentPage()"
            [pageSize]="pageSize"
            [totalCount]="totalCount()"
            (pageChange)="onPageChange($event)"
          ></app-pagination>
        }
      }
    </div>
  `
})
export class PaymentsListComponent implements OnInit {
  private paymentService = inject(PaymentService);

  isLoading = signal(true);
  payments = signal<Payment[]>([]);
  totalCount = signal(0);
  currentPage = signal(1);
  pageSize = 10;

  statusFilter = '';
  dateFrom = '';
  dateTo = '';
  showManualPayment = false;

  paymentStatuses = Object.values(PaymentStatus);

  stats = signal({ completed: 0, pending: 0, failed: 0 });

  ngOnInit(): void {
    this.loadPayments();
  }

  loadPayments(): void {
    this.isLoading.set(true);
    this.paymentService.getPayments({
      status: this.statusFilter ? this.statusFilter as PaymentStatus : undefined,
      dateFrom: this.dateFrom ? new Date(this.dateFrom) : undefined,
      dateTo: this.dateTo ? new Date(this.dateTo) : undefined,
      page: this.currentPage(),
      pageSize: this.pageSize
    }).subscribe({
      next: (result: PagedResult<Payment>) => {
        this.payments.set(result.items);
        this.totalCount.set(result.totalCount);
        this.calculateStats(result.items);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }

  private calculateStats(payments: Payment[]): void {
    let completed = 0, pending = 0, failed = 0;
    payments.forEach(p => {
      if (p.status === 'Completed') completed += p.amount;
      else if (p.status === 'Pending' || p.status === 'Processing') pending += p.amount;
      else failed += p.amount;
    });
    this.stats.set({ completed, pending, failed });
  }

  onPageChange(page: number): void {
    this.currentPage.set(page);
    this.loadPayments();
  }
}
