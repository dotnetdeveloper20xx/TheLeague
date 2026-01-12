import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { InvoiceService } from '../../../core/services/invoice.service';
import { NotificationService } from '../../../core/services';
import { LoadingSpinnerComponent } from '../../../shared/components';
import { Invoice, InvoiceStatus, InvoiceStatusLabels, InvoiceStatusColors, PaymentMethod } from '../../../core/models';
import { FeeTypeLabels, FeeType } from '../../../core/models/fee.model';

@Component({
  selector: 'app-invoice-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, LoadingSpinnerComponent],
  template: `
    <div class="space-y-6">
      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg"></app-loading-spinner>
        </div>
      } @else if (invoice()) {
        <!-- Header -->
        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div class="flex items-center gap-4">
            <a routerLink="/club/invoices" class="text-gray-500 hover:text-gray-700">
              <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
              </svg>
            </a>
            <div>
              <h1 class="text-2xl font-bold text-gray-900">{{ invoice()!.invoiceNumber }}</h1>
              <p class="text-gray-500">Invoice for {{ invoice()!.memberName }}</p>
            </div>
          </div>
          <div class="flex items-center gap-2">
            <span [class]="'badge ' + getStatusColor(invoice()!.status)">{{ getStatusLabel(invoice()!.status) }}</span>
            @if (invoice()!.isOverdue) {
              <span class="badge badge-danger">{{ invoice()!.daysOverdue }} days overdue</span>
            }
          </div>
        </div>

        <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
          <!-- Main Content -->
          <div class="lg:col-span-2 space-y-6">
            <!-- Invoice Details Card -->
            <div class="card">
              <div class="flex justify-between items-start mb-6">
                <div>
                  <h2 class="text-lg font-semibold">Invoice Details</h2>
                  <p class="text-sm text-gray-500">Created {{ invoice()!.createdAt | date:'medium' }}</p>
                </div>
                <div class="text-right">
                  <p class="text-sm text-gray-500">Invoice Date</p>
                  <p class="font-semibold">{{ invoice()!.invoiceDate | date:'mediumDate' }}</p>
                  <p class="text-sm text-gray-500 mt-2">Due Date</p>
                  <p class="font-semibold" [class.text-red-600]="invoice()!.isOverdue">{{ invoice()!.dueDate | date:'mediumDate' }}</p>
                </div>
              </div>

              <!-- Billing Address -->
              @if (invoice()!.billingName || invoice()!.billingAddress) {
                <div class="border-t pt-4 mb-6">
                  <h3 class="text-sm font-medium text-gray-700 mb-2">Bill To</h3>
                  <p class="text-gray-900">{{ invoice()!.billingName }}</p>
                  @if (invoice()!.billingAddress) {
                    <p class="text-gray-600">{{ invoice()!.billingAddress }}</p>
                  }
                  @if (invoice()!.billingEmail) {
                    <p class="text-gray-600">{{ invoice()!.billingEmail }}</p>
                  }
                </div>
              }

              <!-- Line Items -->
              <div class="border-t pt-4">
                <h3 class="text-sm font-medium text-gray-700 mb-4">Line Items</h3>
                <table class="min-w-full">
                  <thead>
                    <tr class="text-left text-xs text-gray-500 uppercase">
                      <th class="pb-2">Description</th>
                      <th class="pb-2 text-center">Qty</th>
                      <th class="pb-2 text-right">Unit Price</th>
                      <th class="pb-2 text-right">Total</th>
                    </tr>
                  </thead>
                  <tbody class="divide-y">
                    @for (item of invoice()!.lineItems; track item.id) {
                      <tr>
                        <td class="py-3">
                          <p class="font-medium">{{ item.description }}</p>
                          @if (item.servicePeriod) {
                            <p class="text-sm text-gray-500">{{ item.servicePeriod }}</p>
                          }
                          @if (item.feeType) {
                            <span class="text-xs text-gray-400">{{ getFeeTypeLabel(item.feeType) }}</span>
                          }
                        </td>
                        <td class="py-3 text-center">{{ item.quantity }}</td>
                        <td class="py-3 text-right">{{ invoice()!.currency }} {{ item.unitPrice | number:'1.2-2' }}</td>
                        <td class="py-3 text-right font-medium">{{ invoice()!.currency }} {{ item.total | number:'1.2-2' }}</td>
                      </tr>
                    }
                  </tbody>
                </table>

                <!-- Totals -->
                <div class="border-t mt-4 pt-4 space-y-2">
                  <div class="flex justify-between text-sm">
                    <span class="text-gray-600">Subtotal</span>
                    <span>{{ invoice()!.currency }} {{ invoice()!.subTotal | number:'1.2-2' }}</span>
                  </div>
                  @if (invoice()!.discountAmount) {
                    <div class="flex justify-between text-sm text-green-600">
                      <span>Discount</span>
                      <span>-{{ invoice()!.currency }} {{ invoice()!.discountAmount | number:'1.2-2' }}</span>
                    </div>
                  }
                  @if (invoice()!.taxAmount) {
                    <div class="flex justify-between text-sm">
                      <span class="text-gray-600">Tax</span>
                      <span>{{ invoice()!.currency }} {{ invoice()!.taxAmount | number:'1.2-2' }}</span>
                    </div>
                  }
                  <div class="flex justify-between text-lg font-bold border-t pt-2">
                    <span>Total</span>
                    <span>{{ invoice()!.currency }} {{ invoice()!.totalAmount | number:'1.2-2' }}</span>
                  </div>
                  <div class="flex justify-between text-sm">
                    <span class="text-gray-600">Paid</span>
                    <span class="text-green-600">{{ invoice()!.currency }} {{ invoice()!.paidAmount | number:'1.2-2' }}</span>
                  </div>
                  <div class="flex justify-between text-lg font-bold" [class.text-red-600]="invoice()!.balanceDue > 0">
                    <span>Balance Due</span>
                    <span>{{ invoice()!.currency }} {{ invoice()!.balanceDue | number:'1.2-2' }}</span>
                  </div>
                </div>
              </div>

              @if (invoice()!.notes) {
                <div class="border-t mt-4 pt-4">
                  <h3 class="text-sm font-medium text-gray-700 mb-2">Notes</h3>
                  <p class="text-gray-600 text-sm whitespace-pre-wrap">{{ invoice()!.notes }}</p>
                </div>
              }
            </div>

            <!-- Payments History -->
            @if (invoice()!.payments.length > 0) {
              <div class="card">
                <h2 class="text-lg font-semibold mb-4">Payment History</h2>
                <div class="space-y-3">
                  @for (payment of invoice()!.payments; track payment.id) {
                    <div class="flex justify-between items-center p-3 bg-gray-50 rounded-lg">
                      <div>
                        <p class="font-medium">{{ invoice()!.currency }} {{ payment.amount | number:'1.2-2' }}</p>
                        <p class="text-sm text-gray-500">{{ payment.paymentDate | date:'medium' }} - {{ payment.method }}</p>
                      </div>
                      @if (payment.receiptNumber) {
                        <span class="text-sm text-gray-500">{{ payment.receiptNumber }}</span>
                      }
                    </div>
                  }
                </div>
              </div>
            }
          </div>

          <!-- Sidebar -->
          <div class="space-y-6">
            <!-- Actions Card -->
            <div class="card">
              <h2 class="text-lg font-semibold mb-4">Actions</h2>
              <div class="space-y-3">
                @if (invoice()!.status === 'Draft') {
                  <button (click)="sendInvoice()" class="btn-primary w-full">
                    <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
                    </svg>
                    Send Invoice
                  </button>
                  <a [routerLink]="['edit']" class="btn-secondary w-full inline-flex items-center justify-center">
                    <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                    </svg>
                    Edit Invoice
                  </a>
                }
                @if (invoice()!.status !== 'Paid' && invoice()!.status !== 'Voided') {
                  <button (click)="recordPayment()" class="btn-secondary w-full">
                    <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 9V7a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2m2 4h10a2 2 0 002-2v-6a2 2 0 00-2-2H9a2 2 0 00-2 2v6a2 2 0 002 2zm7-5a2 2 0 11-4 0 2 2 0 014 0z" />
                    </svg>
                    Record Payment
                  </button>
                  <button (click)="markAsPaid()" class="btn-secondary w-full text-green-600 border-green-300 hover:bg-green-50">
                    Mark as Paid
                  </button>
                  @if (invoice()!.status === 'Sent' || invoice()!.isOverdue) {
                    <button (click)="sendReminder()" class="btn-secondary w-full">
                      Send Reminder
                    </button>
                  }
                }
                @if (invoice()!.status !== 'Voided' && invoice()!.status !== 'Paid') {
                  <button (click)="voidInvoice()" class="btn-secondary w-full text-red-600 border-red-300 hover:bg-red-50">
                    Void Invoice
                  </button>
                }
              </div>
            </div>

            <!-- Member Info -->
            <div class="card">
              <h2 class="text-lg font-semibold mb-4">Member</h2>
              <p class="font-medium">{{ invoice()!.memberName }}</p>
              @if (invoice()!.memberEmail) {
                <p class="text-sm text-gray-500">{{ invoice()!.memberEmail }}</p>
              }
              <a [routerLink]="['/club/members', invoice()!.memberId]" class="text-blue-600 text-sm hover:underline mt-2 inline-block">
                View Member Profile
              </a>
            </div>

            <!-- Invoice Info -->
            <div class="card">
              <h2 class="text-lg font-semibold mb-4">Information</h2>
              <div class="space-y-2 text-sm">
                <div class="flex justify-between">
                  <span class="text-gray-500">Payment Terms</span>
                  <span>{{ invoice()!.paymentTermsDays }} days</span>
                </div>
                @if (invoice()!.sentDate) {
                  <div class="flex justify-between">
                    <span class="text-gray-500">Sent Date</span>
                    <span>{{ invoice()!.sentDate | date:'mediumDate' }}</span>
                  </div>
                }
                @if (invoice()!.paidDate) {
                  <div class="flex justify-between">
                    <span class="text-gray-500">Paid Date</span>
                    <span>{{ invoice()!.paidDate | date:'mediumDate' }}</span>
                  </div>
                }
                <div class="flex justify-between">
                  <span class="text-gray-500">Reminders Sent</span>
                  <span>{{ invoice()!.remindersSent }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      }
    </div>
  `
})
export class InvoiceDetailComponent implements OnInit {
  private invoiceService = inject(InvoiceService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private notificationService = inject(NotificationService);

  isLoading = signal(true);
  invoice = signal<Invoice | null>(null);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadInvoice(id);
    }
  }

  loadInvoice(id: string): void {
    this.isLoading.set(true);
    this.invoiceService.getInvoice(id).subscribe({
      next: (invoice) => {
        this.invoice.set(invoice);
        this.isLoading.set(false);
      },
      error: () => {
        this.notificationService.error('Invoice not found');
        this.router.navigate(['/club/invoices']);
      }
    });
  }

  sendInvoice(): void {
    if (confirm('Send this invoice to the member?')) {
      this.invoiceService.sendInvoice(this.invoice()!.id).subscribe({
        next: (invoice) => {
          this.invoice.set(invoice);
          this.notificationService.success('Invoice sent successfully');
        },
        error: () => this.notificationService.error('Failed to send invoice')
      });
    }
  }

  recordPayment(): void {
    const inv = this.invoice()!;
    const amount = prompt(`Enter payment amount (Balance: ${inv.currency} ${inv.balanceDue.toFixed(2)}):`, inv.balanceDue.toString());
    if (amount && !isNaN(parseFloat(amount))) {
      this.invoiceService.recordPayment(inv.id, {
        amount: parseFloat(amount),
        method: PaymentMethod.Cash
      }).subscribe({
        next: (invoice) => {
          this.invoice.set(invoice);
          this.notificationService.success('Payment recorded successfully');
        },
        error: () => this.notificationService.error('Failed to record payment')
      });
    }
  }

  markAsPaid(): void {
    if (confirm('Mark this invoice as fully paid?')) {
      this.invoiceService.markAsPaid(this.invoice()!.id).subscribe({
        next: (invoice) => {
          this.invoice.set(invoice);
          this.notificationService.success('Invoice marked as paid');
        },
        error: () => this.notificationService.error('Failed to update invoice')
      });
    }
  }

  sendReminder(): void {
    if (confirm('Send a payment reminder to the member?')) {
      this.invoiceService.sendReminder(this.invoice()!.id).subscribe({
        next: (invoice) => {
          this.invoice.set(invoice);
          this.notificationService.success('Reminder sent');
        },
        error: () => this.notificationService.error('Failed to send reminder')
      });
    }
  }

  voidInvoice(): void {
    const reason = prompt('Enter void reason:');
    if (reason) {
      this.invoiceService.voidInvoice(this.invoice()!.id, reason).subscribe({
        next: (invoice) => {
          this.invoice.set(invoice);
          this.notificationService.success('Invoice voided');
        },
        error: () => this.notificationService.error('Failed to void invoice')
      });
    }
  }

  getStatusLabel(status: InvoiceStatus): string {
    return InvoiceStatusLabels[status] || status;
  }

  getStatusColor(status: InvoiceStatus): string {
    return InvoiceStatusColors[status] || 'badge-gray';
  }

  getFeeTypeLabel(type: FeeType): string {
    return FeeTypeLabels[type] || type;
  }
}
