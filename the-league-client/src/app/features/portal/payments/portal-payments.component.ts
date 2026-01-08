import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PortalService } from '../../../core/services/portal.service';
import { LoadingSpinnerComponent, StatusBadgeComponent, EmptyStateComponent } from '../../../shared/components';
import { DateFormatPipe, CurrencyFormatPipe } from '../../../shared/pipes';
import { Payment } from '../../../core/models';

@Component({
  selector: 'app-portal-payments',
  standalone: true,
  imports: [CommonModule, LoadingSpinnerComponent, StatusBadgeComponent, EmptyStateComponent, DateFormatPipe, CurrencyFormatPipe],
  template: `
    <div class="space-y-6">
      <h1 class="text-2xl font-bold text-gray-900">Payment History</h1>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg"></app-loading-spinner>
        </div>
      } @else if (payments().length === 0) {
        <app-empty-state
          icon="credit-card"
          title="No payments"
          message="You haven't made any payments yet."
        ></app-empty-state>
      } @else {
        <div class="card overflow-hidden p-0">
          <table class="table">
            <thead>
              <tr>
                <th>Date</th>
                <th>Description</th>
                <th>Amount</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              @for (payment of payments(); track payment.id) {
                <tr>
                  <td>{{ payment.paymentDate | dateFormat }}</td>
                  <td>
                    <p class="font-medium text-gray-900">{{ payment.type }}</p>
                    @if (payment.description) {
                      <p class="text-sm text-gray-500">{{ payment.description }}</p>
                    }
                  </td>
                  <td class="font-medium">{{ payment.amount | currencyFormat }}</td>
                  <td><app-status-badge [status]="payment.status" type="payment"></app-status-badge></td>
                </tr>
              }
            </tbody>
          </table>
        </div>
      }
    </div>
  `
})
export class PortalPaymentsComponent implements OnInit {
  private portalService = inject(PortalService);

  isLoading = signal(true);
  payments = signal<Payment[]>([]);

  ngOnInit(): void {
    this.portalService.getPaymentHistory().subscribe({
      next: (payments) => {
        this.payments.set(payments);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }
}
