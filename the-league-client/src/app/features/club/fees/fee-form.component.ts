import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FeeService } from '../../../core/services/fee.service';
import { NotificationService } from '../../../core/services';
import { LoadingSpinnerComponent } from '../../../shared/components';
import { Fee, FeeType, FeeFrequency, FeeTypeLabels, FeeFrequencyLabels } from '../../../core/models';

@Component({
  selector: 'app-fee-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, LoadingSpinnerComponent],
  template: `
    <div class="space-y-6">
      <div class="flex items-center gap-4">
        <a routerLink="/club/fees" class="text-gray-500 hover:text-gray-700">
          <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
          </svg>
        </a>
        <div>
          <h1 class="text-2xl font-bold text-gray-900">{{ isEdit() ? 'Edit Fee' : 'Create Fee' }}</h1>
          <p class="text-gray-500 mt-1">{{ isEdit() ? 'Update fee details' : 'Define a new fee for your club' }}</p>
        </div>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg"></app-loading-spinner>
        </div>
      } @else {
        <form [formGroup]="form" (ngSubmit)="onSubmit()" class="space-y-6">
          <!-- Basic Information -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Basic Information</h2>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Name *</label>
                <input type="text" formControlName="name" class="input" placeholder="e.g., Annual Membership Fee" />
                @if (form.get('name')?.invalid && form.get('name')?.touched) {
                  <p class="text-red-500 text-sm mt-1">Name is required</p>
                }
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Code</label>
                <input type="text" formControlName="code" class="input" placeholder="e.g., MEM-001" />
              </div>
              <div class="md:col-span-2">
                <label class="block text-sm font-medium text-gray-700 mb-1">Description</label>
                <textarea formControlName="description" rows="3" class="input" placeholder="Describe the fee..."></textarea>
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Fee Type *</label>
                <select formControlName="type" class="input">
                  @for (type of feeTypes; track type) {
                    <option [value]="type">{{ getFeeTypeLabel(type) }}</option>
                  }
                </select>
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Frequency *</label>
                <select formControlName="frequency" class="input">
                  @for (freq of frequencies; track freq) {
                    <option [value]="freq">{{ getFrequencyLabel(freq) }}</option>
                  }
                </select>
              </div>
            </div>
          </div>

          <!-- Pricing -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Pricing</h2>
            <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Amount *</label>
                <div class="relative">
                  <span class="absolute left-3 top-2.5 text-gray-500">{{ form.get('currency')?.value }}</span>
                  <input
                    type="number"
                    formControlName="amount"
                    class="input pl-12"
                    step="0.01"
                    min="0"
                    placeholder="0.00"
                  />
                </div>
                @if (form.get('amount')?.invalid && form.get('amount')?.touched) {
                  <p class="text-red-500 text-sm mt-1">Amount is required and must be positive</p>
                }
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Currency</label>
                <select formControlName="currency" class="input">
                  <option value="GBP">GBP - British Pound</option>
                  <option value="USD">USD - US Dollar</option>
                  <option value="EUR">EUR - Euro</option>
                </select>
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Due Day of Month</label>
                <input
                  type="number"
                  formControlName="dueDayOfMonth"
                  class="input"
                  min="1"
                  max="28"
                  placeholder="e.g., 1"
                />
              </div>
              <div class="flex items-center gap-4">
                <label class="flex items-center gap-2">
                  <input type="checkbox" formControlName="isTaxable" class="rounded border-gray-300" />
                  <span class="text-sm text-gray-700">Taxable</span>
                </label>
              </div>
              @if (form.get('isTaxable')?.value) {
                <div>
                  <label class="block text-sm font-medium text-gray-700 mb-1">Tax Rate (%)</label>
                  <input type="number" formControlName="taxRate" class="input" step="0.01" placeholder="e.g., 20" />
                </div>
              }
            </div>
          </div>

          <!-- Late Fee Settings -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Late Fee Settings</h2>
            <div class="space-y-4">
              <label class="flex items-center gap-2">
                <input type="checkbox" formControlName="hasLateFee" class="rounded border-gray-300" />
                <span class="text-sm text-gray-700">Enable late payment fees</span>
              </label>
              @if (form.get('hasLateFee')?.value) {
                <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
                  <div>
                    <label class="block text-sm font-medium text-gray-700 mb-1">Grace Period (days)</label>
                    <input type="number" formControlName="gracePeriodDays" class="input" min="0" placeholder="e.g., 7" />
                  </div>
                  <div>
                    <label class="block text-sm font-medium text-gray-700 mb-1">Late Fee Amount</label>
                    <input type="number" formControlName="lateFeeAmount" class="input" step="0.01" min="0" placeholder="0.00" />
                  </div>
                  <div>
                    <label class="block text-sm font-medium text-gray-700 mb-1">Late Fee (%)</label>
                    <input type="number" formControlName="lateFeePercentage" class="input" step="0.01" min="0" max="100" placeholder="e.g., 5" />
                  </div>
                </div>
              }
            </div>
          </div>

          <!-- Payment Options -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Payment Options</h2>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div class="space-y-3">
                <label class="flex items-center gap-2">
                  <input type="checkbox" formControlName="allowPartialPayment" class="rounded border-gray-300" />
                  <span class="text-sm text-gray-700">Allow partial payments</span>
                </label>
                <label class="flex items-center gap-2">
                  <input type="checkbox" formControlName="allowPaymentPlan" class="rounded border-gray-300" />
                  <span class="text-sm text-gray-700">Allow payment plans</span>
                </label>
                @if (form.get('allowPaymentPlan')?.value) {
                  <div>
                    <label class="block text-sm font-medium text-gray-700 mb-1">Max Installments</label>
                    <input type="number" formControlName="maxInstallments" class="input" min="2" max="24" placeholder="e.g., 12" />
                  </div>
                }
              </div>
              <div class="space-y-3">
                <label class="flex items-center gap-2">
                  <input type="checkbox" formControlName="isRefundable" class="rounded border-gray-300" />
                  <span class="text-sm text-gray-700">Refundable</span>
                </label>
                @if (form.get('isRefundable')?.value) {
                  <div>
                    <label class="block text-sm font-medium text-gray-700 mb-1">Refundable within (days)</label>
                    <input type="number" formControlName="refundableDays" class="input" min="1" placeholder="e.g., 30" />
                  </div>
                }
              </div>
            </div>
          </div>

          <!-- Early Payment Discount -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Early Payment Discount</h2>
            <div class="space-y-4">
              <label class="flex items-center gap-2">
                <input type="checkbox" formControlName="allowEarlyPaymentDiscount" class="rounded border-gray-300" />
                <span class="text-sm text-gray-700">Offer early payment discount</span>
              </label>
              @if (form.get('allowEarlyPaymentDiscount')?.value) {
                <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label class="block text-sm font-medium text-gray-700 mb-1">Discount (%)</label>
                    <input type="number" formControlName="earlyPaymentDiscountPercent" class="input" step="0.01" min="0" max="100" placeholder="e.g., 5" />
                  </div>
                  <div>
                    <label class="block text-sm font-medium text-gray-700 mb-1">Days before due date</label>
                    <input type="number" formControlName="earlyPaymentDays" class="input" min="1" placeholder="e.g., 14" />
                  </div>
                </div>
              }
            </div>
          </div>

          <!-- Automation -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Automation</h2>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <label class="flex items-center gap-2">
                <input type="checkbox" formControlName="autoGenerate" class="rounded border-gray-300" />
                <span class="text-sm text-gray-700">Auto-generate invoices</span>
              </label>
              <label class="flex items-center gap-2">
                <input type="checkbox" formControlName="autoRemind" class="rounded border-gray-300" />
                <span class="text-sm text-gray-700">Send automatic reminders</span>
              </label>
            </div>
          </div>

          <!-- Status & Settings -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Status & Settings</h2>
            <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
              <label class="flex items-center gap-2">
                <input type="checkbox" formControlName="isActive" class="rounded border-gray-300" />
                <span class="text-sm text-gray-700">Active</span>
              </label>
              <label class="flex items-center gap-2">
                <input type="checkbox" formControlName="isRequired" class="rounded border-gray-300" />
                <span class="text-sm text-gray-700">Required fee</span>
              </label>
              <label class="flex items-center gap-2">
                <input type="checkbox" formControlName="isHidden" class="rounded border-gray-300" />
                <span class="text-sm text-gray-700">Hidden from members</span>
              </label>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Sort Order</label>
                <input type="number" formControlName="sortOrder" class="input" min="0" placeholder="0" />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">GL Account Code</label>
                <input type="text" formControlName="glAccountCode" class="input" placeholder="e.g., 4010" />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Cost Center</label>
                <input type="text" formControlName="costCenter" class="input" placeholder="e.g., MEMBERSHIP" />
              </div>
            </div>
          </div>

          <!-- Actions -->
          <div class="flex justify-end gap-4">
            <a routerLink="/club/fees" class="btn-secondary">Cancel</a>
            <button
              type="submit"
              [disabled]="form.invalid || isSaving()"
              class="btn-primary"
              [class.opacity-50]="form.invalid || isSaving()"
            >
              @if (isSaving()) {
                <app-loading-spinner size="sm" class="mr-2"></app-loading-spinner>
              }
              {{ isEdit() ? 'Update Fee' : 'Create Fee' }}
            </button>
          </div>
        </form>
      }
    </div>
  `
})
export class FeeFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private feeService = inject(FeeService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private notificationService = inject(NotificationService);

  isLoading = signal(false);
  isSaving = signal(false);
  isEdit = signal(false);
  feeId: string | null = null;

  feeTypes = Object.values(FeeType);
  frequencies = Object.values(FeeFrequency);

  form: FormGroup = this.fb.group({
    name: ['', Validators.required],
    code: [''],
    description: [''],
    type: [FeeType.Membership, Validators.required],
    frequency: [FeeFrequency.Annual, Validators.required],
    amount: [0, [Validators.required, Validators.min(0)]],
    currency: ['GBP'],
    dueDayOfMonth: [null],
    isTaxable: [false],
    taxRate: [null],
    hasLateFee: [false],
    gracePeriodDays: [null],
    lateFeeAmount: [null],
    lateFeePercentage: [null],
    allowPartialPayment: [false],
    allowPaymentPlan: [false],
    maxInstallments: [null],
    isRefundable: [true],
    refundableDays: [30],
    allowEarlyPaymentDiscount: [false],
    earlyPaymentDiscountPercent: [null],
    earlyPaymentDays: [null],
    autoGenerate: [false],
    autoRemind: [false],
    isActive: [true],
    isRequired: [false],
    isHidden: [false],
    sortOrder: [0],
    glAccountCode: [''],
    costCenter: ['']
  });

  ngOnInit(): void {
    this.feeId = this.route.snapshot.paramMap.get('id');
    if (this.feeId) {
      this.isEdit.set(true);
      this.loadFee(this.feeId);
    }
  }

  loadFee(id: string): void {
    this.isLoading.set(true);
    this.feeService.getFee(id).subscribe({
      next: (fee) => {
        this.form.patchValue({
          name: fee.name,
          code: fee.code,
          description: fee.description,
          type: fee.type,
          frequency: fee.frequency,
          amount: fee.amount,
          currency: fee.currency,
          dueDayOfMonth: fee.dueDayOfMonth,
          isTaxable: fee.isTaxable,
          taxRate: fee.taxRate,
          hasLateFee: fee.hasLateFee,
          gracePeriodDays: fee.gracePeriodDays,
          lateFeeAmount: fee.lateFeeAmount,
          lateFeePercentage: fee.lateFeePercentage,
          allowPartialPayment: fee.allowPartialPayment,
          allowPaymentPlan: fee.allowPaymentPlan,
          maxInstallments: fee.maxInstallments,
          isRefundable: fee.isRefundable,
          refundableDays: fee.refundableDays,
          allowEarlyPaymentDiscount: fee.allowEarlyPaymentDiscount,
          earlyPaymentDiscountPercent: fee.earlyPaymentDiscountPercent,
          earlyPaymentDays: fee.earlyPaymentDays,
          autoGenerate: fee.autoGenerate,
          autoRemind: fee.autoRemind,
          isActive: fee.isActive,
          isRequired: fee.isRequired,
          isHidden: fee.isHidden,
          sortOrder: fee.sortOrder,
          glAccountCode: fee.glAccountCode,
          costCenter: fee.costCenter
        });
        this.isLoading.set(false);
      },
      error: () => {
        this.notificationService.error('Failed to load fee');
        this.router.navigate(['/club/fees']);
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    this.isSaving.set(true);
    const data = this.form.value;

    const request$ = this.isEdit()
      ? this.feeService.updateFee(this.feeId!, data)
      : this.feeService.createFee(data);

    request$.subscribe({
      next: () => {
        this.notificationService.success(this.isEdit() ? 'Fee updated successfully' : 'Fee created successfully');
        this.router.navigate(['/club/fees']);
      },
      error: () => {
        this.notificationService.error(this.isEdit() ? 'Failed to update fee' : 'Failed to create fee');
        this.isSaving.set(false);
      }
    });
  }

  getFeeTypeLabel(type: FeeType): string {
    return FeeTypeLabels[type] || type;
  }

  getFrequencyLabel(frequency: FeeFrequency): string {
    return FeeFrequencyLabels[frequency] || frequency;
  }
}
