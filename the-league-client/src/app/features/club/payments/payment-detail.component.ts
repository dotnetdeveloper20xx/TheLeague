import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { PaymentService } from '../../../core/services/payment.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent, StatusBadgeComponent, ConfirmDialogComponent } from '../../../shared/components';
import { DateFormatPipe, CurrencyFormatPipe } from '../../../shared/pipes';
import { Payment } from '../../../core/models';

@Component({
  selector: 'app-payment-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, LoadingSpinnerComponent, StatusBadgeComponent, ConfirmDialogComponent, DateFormatPipe, CurrencyFormatPipe],
  template: `
    @if (isLoading()) {
      <div class="flex justify-center py-12">
        <app-loading-spinner size="lg" message="Loading payment..."></app-loading-spinner>
      </div>
    } @else if (payment()) {
      <div class="space-y-6">
        <div class="flex items-center gap-4">
          <a routerLink="/club/payments" class="p-2 text-gray-500 hover:text-gray-700 rounded-lg hover:bg-gray-100">
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
            </svg>
          </a>
          <div class="flex-1">
            <h1 class="text-2xl font-bold text-gray-900">Payment Details</h1>
            <p class="text-gray-500">Receipt #{{ payment()!.receiptNumber || payment()!.id.slice(0, 8) }}</p>
          </div>
          @if (payment()!.status === 'Completed') {
            <button (click)="showRefundDialog.set(true)" class="btn-danger">Refund</button>
          }
        </div>

        <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
          <div class="card">
            <h3 class="card-header">Payment Information</h3>
            <dl class="space-y-4">
              <div class="flex justify-between">
                <dt class="text-gray-500">Amount</dt>
                <dd class="text-2xl font-bold text-gray-900">{{ payment()!.amount | currencyFormat }}</dd>
              </div>
              <div class="flex justify-between">
                <dt class="text-gray-500">Status</dt>
                <dd><app-status-badge [status]="payment()!.status" type="payment"></app-status-badge></dd>
              </div>
              <div class="flex justify-between">
                <dt class="text-gray-500">Type</dt>
                <dd class="text-gray-900">{{ payment()!.type }}</dd>
              </div>
              <div class="flex justify-between">
                <dt class="text-gray-500">Method</dt>
                <dd class="text-gray-900">{{ payment()!.method }}</dd>
              </div>
              <div class="flex justify-between">
                <dt class="text-gray-500">Date</dt>
                <dd class="text-gray-900">{{ payment()!.paymentDate | dateFormat:'datetime' }}</dd>
              </div>
              @if (payment()!.description) {
                <div>
                  <dt class="text-gray-500 mb-1">Description</dt>
                  <dd class="text-gray-900">{{ payment()!.description }}</dd>
                </div>
              }
            </dl>
          </div>

          <div class="card">
            <h3 class="card-header">Member Information</h3>
            <dl class="space-y-4">
              <div>
                <dt class="text-sm text-gray-500">Member Name</dt>
                <dd class="text-gray-900 font-medium">{{ payment()!.memberName }}</dd>
              </div>
              @if (payment()!.manualPaymentReference) {
                <div>
                  <dt class="text-sm text-gray-500">Reference</dt>
                  <dd class="text-gray-900">{{ payment()!.manualPaymentReference }}</dd>
                </div>
              }
              @if (payment()!.recordedBy) {
                <div>
                  <dt class="text-sm text-gray-500">Recorded By</dt>
                  <dd class="text-gray-900">{{ payment()!.recordedBy }}</dd>
                </div>
              }
            </dl>
          </div>
        </div>
      </div>
    }

    <app-confirm-dialog
      [isOpen]="showRefundDialog()"
      title="Refund Payment"
      message="Are you sure you want to refund this payment? This action cannot be undone."
      confirmText="Refund"
      type="warning"
      (confirm)="refundPayment()"
      (cancel)="showRefundDialog.set(false)"
    ></app-confirm-dialog>
  `
})
export class PaymentDetailComponent implements OnInit {
  private paymentService = inject(PaymentService);
  private notificationService = inject(NotificationService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  isLoading = signal(true);
  payment = signal<Payment | null>(null);
  showRefundDialog = signal(false);

  ngOnInit(): void {
    const paymentId = this.route.snapshot.params['id'];
    if (paymentId) {
      this.loadPayment(paymentId);
    }
  }

  private loadPayment(id: string): void {
    this.paymentService.getPayment(id).subscribe({
      next: (payment) => {
        this.payment.set(payment);
        this.isLoading.set(false);
      },
      error: () => {
        this.notificationService.error('Failed to load payment');
        this.router.navigate(['/club/payments']);
      }
    });
  }

  refundPayment(): void {
    const payment = this.payment();
    if (!payment) return;

    this.paymentService.refundPayment(payment.id, { reason: 'Requested refund' }).subscribe({
      next: () => {
        this.notificationService.success('Payment refunded successfully');
        this.showRefundDialog.set(false);
        this.loadPayment(payment.id);
      },
      error: () => {
        this.notificationService.error('Failed to refund payment');
      }
    });
  }
}
