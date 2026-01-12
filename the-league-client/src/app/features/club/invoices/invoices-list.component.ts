import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { InvoiceService } from '../../../core/services/invoice.service';
import { NotificationService } from '../../../core/services';
import { LoadingSpinnerComponent, EmptyStateComponent } from '../../../shared/components';
import {
  InvoiceListItem,
  InvoiceStatus,
  InvoiceStatusLabels,
  InvoiceStatusColors,
  InvoiceSummary,
  PagedResult
} from '../../../core/models';

@Component({
  selector: 'app-invoices-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, LoadingSpinnerComponent, EmptyStateComponent],
  template: `
    <div class="space-y-6">
      <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 class="text-2xl font-bold text-gray-900">Invoices</h1>
          <p class="text-gray-500 mt-1">Manage and track member invoices</p>
        </div>
        <a routerLink="new" class="btn-primary inline-flex items-center">
          <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
          </svg>
          Create Invoice
        </a>
      </div>

      <!-- Summary Cards -->
      @if (summary()) {
        <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
          <div class="card bg-gradient-to-br from-blue-50 to-blue-100">
            <p class="text-sm text-blue-600 font-medium">Total Outstanding</p>
            <p class="text-2xl font-bold text-blue-900">{{ summary()!.totalOutstanding | currency:'GBP' }}</p>
            <p class="text-xs text-blue-600">{{ summary()!.totalInvoices }} invoices</p>
          </div>
          <div class="card bg-gradient-to-br from-red-50 to-red-100">
            <p class="text-sm text-red-600 font-medium">Overdue</p>
            <p class="text-2xl font-bold text-red-900">{{ summary()!.totalOverdue | currency:'GBP' }}</p>
            <p class="text-xs text-red-600">{{ summary()!.overdueCount }} invoices</p>
          </div>
          <div class="card bg-gradient-to-br from-green-50 to-green-100">
            <p class="text-sm text-green-600 font-medium">Paid This Month</p>
            <p class="text-2xl font-bold text-green-900">{{ summary()!.totalPaidThisMonth | currency:'GBP' }}</p>
            <p class="text-xs text-green-600">{{ summary()!.paidCount }} paid</p>
          </div>
          <div class="card bg-gradient-to-br from-gray-50 to-gray-100">
            <p class="text-sm text-gray-600 font-medium">Draft</p>
            <p class="text-2xl font-bold text-gray-900">{{ summary()!.draftCount }}</p>
            <p class="text-xs text-gray-600">pending to send</p>
          </div>
        </div>
      }

      <!-- Filters -->
      <div class="card">
        <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Search</label>
            <input
              type="text"
              [(ngModel)]="searchTerm"
              (ngModelChange)="onSearch()"
              placeholder="Invoice # or member name..."
              class="input"
            />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Status</label>
            <select [(ngModel)]="selectedStatus" (ngModelChange)="loadInvoices()" class="input">
              <option [ngValue]="null">All Statuses</option>
              @for (status of statuses; track status) {
                <option [value]="status">{{ getStatusLabel(status) }}</option>
              }
            </select>
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Show Overdue</label>
            <select [(ngModel)]="showOverdueOnly" (ngModelChange)="loadInvoices()" class="input">
              <option [ngValue]="null">All Invoices</option>
              <option [ngValue]="true">Overdue Only</option>
              <option [ngValue]="false">Not Overdue</option>
            </select>
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Date Range</label>
            <input
              type="date"
              [(ngModel)]="dateFrom"
              (ngModelChange)="loadInvoices()"
              class="input"
            />
          </div>
        </div>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg"></app-loading-spinner>
        </div>
      } @else if (invoices().length === 0) {
        <app-empty-state
          icon="invoice"
          title="No invoices found"
          message="Create your first invoice to start billing members."
        ></app-empty-state>
      } @else {
        <div class="card overflow-hidden">
          <table class="min-w-full divide-y divide-gray-200">
            <thead class="bg-gray-50">
              <tr>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Invoice #</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Member</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Date</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Due Date</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Amount</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Balance</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                <th class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              @for (invoice of invoices(); track invoice.id) {
                <tr [class.bg-red-50]="invoice.isOverdue">
                  <td class="px-6 py-4 whitespace-nowrap">
                    <a [routerLink]="[invoice.id]" class="text-blue-600 hover:text-blue-900 font-medium">
                      {{ invoice.invoiceNumber }}
                    </a>
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {{ invoice.memberName }}
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {{ invoice.invoiceDate | date:'mediumDate' }}
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm" [class.text-red-600]="invoice.isOverdue" [class.text-gray-500]="!invoice.isOverdue">
                    {{ invoice.dueDate | date:'mediumDate' }}
                    @if (invoice.isOverdue) {
                      <span class="text-xs">({{ invoice.daysOverdue }} days)</span>
                    }
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {{ invoice.currency }} {{ invoice.totalAmount | number:'1.2-2' }}
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm" [class.text-red-600]="invoice.balanceDue > 0" [class.text-green-600]="invoice.balanceDue === 0">
                    {{ invoice.currency }} {{ invoice.balanceDue | number:'1.2-2' }}
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap">
                    <span [class]="'badge ' + getStatusColor(invoice.status)">
                      {{ getStatusLabel(invoice.status) }}
                    </span>
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                    <a [routerLink]="[invoice.id]" class="text-blue-600 hover:text-blue-900 mr-3">View</a>
                    @if (invoice.status === 'Draft') {
                      <button (click)="sendInvoice(invoice)" class="text-green-600 hover:text-green-900 mr-3">Send</button>
                    }
                    @if (invoice.status !== 'Paid' && invoice.status !== 'Voided') {
                      <button (click)="recordPayment(invoice)" class="text-purple-600 hover:text-purple-900">Payment</button>
                    }
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
              Showing {{ (currentPage() - 1) * pageSize + 1 }} to {{ Math.min(currentPage() * pageSize, totalCount()) }} of {{ totalCount() }} invoices
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
export class InvoicesListComponent implements OnInit {
  private invoiceService = inject(InvoiceService);
  private notificationService = inject(NotificationService);

  isLoading = signal(true);
  invoices = signal<InvoiceListItem[]>([]);
  summary = signal<InvoiceSummary | null>(null);
  currentPage = signal(1);
  totalPages = signal(1);
  totalCount = signal(0);
  pageSize = 20;

  searchTerm = '';
  selectedStatus: InvoiceStatus | null = null;
  showOverdueOnly: boolean | null = null;
  dateFrom: string | null = null;

  statuses = Object.values(InvoiceStatus);
  Math = Math;

  ngOnInit(): void {
    this.loadSummary();
    this.loadInvoices();
  }

  loadSummary(): void {
    this.invoiceService.getSummary().subscribe({
      next: (summary) => this.summary.set(summary),
      error: () => {}
    });
  }

  loadInvoices(): void {
    this.isLoading.set(true);
    this.invoiceService.getInvoices({
      search: this.searchTerm || undefined,
      status: this.selectedStatus || undefined,
      isOverdue: this.showOverdueOnly ?? undefined,
      dateFrom: this.dateFrom ? new Date(this.dateFrom) : undefined,
      page: this.currentPage(),
      pageSize: this.pageSize
    }).subscribe({
      next: (result) => {
        this.invoices.set(result.items);
        this.totalCount.set(result.totalCount);
        this.totalPages.set(result.totalPages);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
        this.notificationService.error('Failed to load invoices');
      }
    });
  }

  onSearch(): void {
    this.currentPage.set(1);
    this.loadInvoices();
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages()) {
      this.currentPage.set(page);
      this.loadInvoices();
    }
  }

  sendInvoice(invoice: InvoiceListItem): void {
    if (confirm(`Send invoice ${invoice.invoiceNumber} to ${invoice.memberName}?`)) {
      this.invoiceService.sendInvoice(invoice.id).subscribe({
        next: () => {
          this.notificationService.success('Invoice sent successfully');
          this.loadInvoices();
          this.loadSummary();
        },
        error: () => this.notificationService.error('Failed to send invoice')
      });
    }
  }

  recordPayment(invoice: InvoiceListItem): void {
    const amount = prompt(`Enter payment amount for ${invoice.invoiceNumber} (Balance: ${invoice.currency} ${invoice.balanceDue.toFixed(2)}):`, invoice.balanceDue.toString());
    if (amount && !isNaN(parseFloat(amount))) {
      this.invoiceService.recordPayment(invoice.id, {
        amount: parseFloat(amount),
        method: 'Cash' as any
      }).subscribe({
        next: () => {
          this.notificationService.success('Payment recorded successfully');
          this.loadInvoices();
          this.loadSummary();
        },
        error: () => this.notificationService.error('Failed to record payment')
      });
    }
  }

  getStatusLabel(status: InvoiceStatus): string {
    return InvoiceStatusLabels[status] || status;
  }

  getStatusColor(status: InvoiceStatus): string {
    return InvoiceStatusColors[status] || 'badge-gray';
  }
}
