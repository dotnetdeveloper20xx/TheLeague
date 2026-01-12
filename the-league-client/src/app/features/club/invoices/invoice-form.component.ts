import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { InvoiceService } from '../../../core/services/invoice.service';
import { MemberService } from '../../../core/services/member.service';
import { FeeService } from '../../../core/services/fee.service';
import { NotificationService } from '../../../core/services';
import { LoadingSpinnerComponent } from '../../../shared/components';
import { MemberListItem, FeeListItem, FeeType, FeeTypeLabels } from '../../../core/models';

@Component({
  selector: 'app-invoice-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, LoadingSpinnerComponent],
  template: `
    <div class="space-y-6">
      <div class="flex items-center gap-4">
        <a routerLink="/club/invoices" class="text-gray-500 hover:text-gray-700">
          <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
          </svg>
        </a>
        <div>
          <h1 class="text-2xl font-bold text-gray-900">Create Invoice</h1>
          <p class="text-gray-500 mt-1">Create a new invoice for a member</p>
        </div>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg"></app-loading-spinner>
        </div>
      } @else {
        <form [formGroup]="form" (ngSubmit)="onSubmit()" class="space-y-6">
          <!-- Member Selection -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Bill To</h2>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Member *</label>
                <select formControlName="memberId" class="input" (change)="onMemberChange()">
                  <option value="">Select a member</option>
                  @for (member of members(); track member.id) {
                    <option [value]="member.id">{{ member.fullName }} ({{ member.email }})</option>
                  }
                </select>
                @if (form.get('memberId')?.invalid && form.get('memberId')?.touched) {
                  <p class="text-red-500 text-sm mt-1">Member is required</p>
                }
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Billing Email</label>
                <input type="email" formControlName="billingEmail" class="input" />
              </div>
              <div class="md:col-span-2">
                <label class="block text-sm font-medium text-gray-700 mb-1">Billing Name</label>
                <input type="text" formControlName="billingName" class="input" />
              </div>
              <div class="md:col-span-2">
                <label class="block text-sm font-medium text-gray-700 mb-1">Billing Address</label>
                <input type="text" formControlName="billingAddress" class="input" />
              </div>
            </div>
          </div>

          <!-- Invoice Details -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Invoice Details</h2>
            <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Invoice Date</label>
                <input type="date" formControlName="invoiceDate" class="input" />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Due Date *</label>
                <input type="date" formControlName="dueDate" class="input" />
                @if (form.get('dueDate')?.invalid && form.get('dueDate')?.touched) {
                  <p class="text-red-500 text-sm mt-1">Due date is required</p>
                }
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Payment Terms (days)</label>
                <input type="number" formControlName="paymentTermsDays" class="input" min="0" />
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
                <label class="block text-sm font-medium text-gray-700 mb-1">PO Number</label>
                <input type="text" formControlName="purchaseOrderNumber" class="input" placeholder="Optional" />
              </div>
            </div>
          </div>

          <!-- Line Items -->
          <div class="card">
            <div class="flex justify-between items-center mb-4">
              <h2 class="text-lg font-semibold text-gray-900">Line Items</h2>
              <div class="flex gap-2">
                <select #feeSelect class="input w-48">
                  <option value="">Add from fees...</option>
                  @for (fee of fees(); track fee.id) {
                    <option [value]="fee.id">{{ fee.name }} ({{ fee.currency }} {{ fee.amount }})</option>
                  }
                </select>
                <button type="button" (click)="addFeeItem(feeSelect)" class="btn-secondary">Add Fee</button>
                <button type="button" (click)="addLineItem()" class="btn-primary">Add Item</button>
              </div>
            </div>

            <div formArrayName="lineItems" class="space-y-4">
              @for (item of lineItemsArray.controls; track $index; let i = $index) {
                <div [formGroupName]="i" class="border rounded-lg p-4 bg-gray-50">
                  <div class="flex justify-between items-start mb-3">
                    <span class="text-sm text-gray-500">Item {{ i + 1 }}</span>
                    <button type="button" (click)="removeLineItem(i)" class="text-red-500 hover:text-red-700">
                      <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                      </svg>
                    </button>
                  </div>
                  <div class="grid grid-cols-12 gap-3">
                    <div class="col-span-6">
                      <label class="block text-xs text-gray-500 mb-1">Description *</label>
                      <input type="text" formControlName="description" class="input" placeholder="Description" />
                    </div>
                    <div class="col-span-2">
                      <label class="block text-xs text-gray-500 mb-1">Qty</label>
                      <input type="number" formControlName="quantity" class="input" min="1" />
                    </div>
                    <div class="col-span-2">
                      <label class="block text-xs text-gray-500 mb-1">Unit Price</label>
                      <input type="number" formControlName="unitPrice" class="input" step="0.01" min="0" />
                    </div>
                    <div class="col-span-2">
                      <label class="block text-xs text-gray-500 mb-1">Total</label>
                      <input type="text" [value]="getLineItemTotal(i) | number:'1.2-2'" class="input bg-gray-100" readonly />
                    </div>
                  </div>
                  <div class="grid grid-cols-12 gap-3 mt-3">
                    <div class="col-span-4">
                      <label class="block text-xs text-gray-500 mb-1">Service Period</label>
                      <input type="text" formControlName="servicePeriod" class="input" placeholder="e.g., Jan-Dec 2025" />
                    </div>
                    <div class="col-span-4">
                      <label class="block text-xs text-gray-500 mb-1">Tax Rate (%)</label>
                      <input type="number" formControlName="taxRate" class="input" step="0.01" min="0" placeholder="e.g., 20" />
                    </div>
                    <div class="col-span-4">
                      <label class="block text-xs text-gray-500 mb-1">Fee Type</label>
                      <select formControlName="feeType" class="input">
                        <option [ngValue]="null">None</option>
                        @for (type of feeTypes; track type) {
                          <option [value]="type">{{ getFeeTypeLabel(type) }}</option>
                        }
                      </select>
                    </div>
                  </div>
                </div>
              }
            </div>

            @if (lineItemsArray.length === 0) {
              <p class="text-gray-500 text-center py-8">No line items added. Click "Add Item" to add items to this invoice.</p>
            }

            <!-- Totals -->
            @if (lineItemsArray.length > 0) {
              <div class="border-t mt-6 pt-4">
                <div class="flex justify-end">
                  <div class="w-64 space-y-2">
                    <div class="flex justify-between">
                      <span class="text-gray-600">Subtotal</span>
                      <span>{{ form.get('currency')?.value }} {{ getSubTotal() | number:'1.2-2' }}</span>
                    </div>
                    <div class="flex justify-between">
                      <span class="text-gray-600">Tax</span>
                      <span>{{ form.get('currency')?.value }} {{ getTaxTotal() | number:'1.2-2' }}</span>
                    </div>
                    <div class="flex justify-between text-lg font-bold border-t pt-2">
                      <span>Total</span>
                      <span>{{ form.get('currency')?.value }} {{ getGrandTotal() | number:'1.2-2' }}</span>
                    </div>
                  </div>
                </div>
              </div>
            }
          </div>

          <!-- Notes -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Notes</h2>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Notes (visible to member)</label>
                <textarea formControlName="notes" rows="3" class="input" placeholder="Payment instructions, thank you message, etc."></textarea>
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Internal Notes (staff only)</label>
                <textarea formControlName="internalNotes" rows="3" class="input" placeholder="Internal notes..."></textarea>
              </div>
            </div>
          </div>

          <!-- Actions -->
          <div class="flex justify-end gap-4">
            <a routerLink="/club/invoices" class="btn-secondary">Cancel</a>
            <button
              type="submit"
              [disabled]="form.invalid || lineItemsArray.length === 0 || isSaving()"
              class="btn-primary"
              [class.opacity-50]="form.invalid || lineItemsArray.length === 0 || isSaving()"
            >
              @if (isSaving()) {
                <app-loading-spinner size="sm" class="mr-2"></app-loading-spinner>
              }
              Create Invoice
            </button>
          </div>
        </form>
      }
    </div>
  `
})
export class InvoiceFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private invoiceService = inject(InvoiceService);
  private memberService = inject(MemberService);
  private feeService = inject(FeeService);
  private router = inject(Router);
  private notificationService = inject(NotificationService);

  isLoading = signal(true);
  isSaving = signal(false);
  members = signal<MemberListItem[]>([]);
  fees = signal<FeeListItem[]>([]);

  feeTypes = Object.values(FeeType);

  form: FormGroup = this.fb.group({
    memberId: ['', Validators.required],
    dueDate: ['', Validators.required],
    invoiceDate: [new Date().toISOString().split('T')[0]],
    currency: ['GBP'],
    purchaseOrderNumber: [''],
    billingName: [''],
    billingAddress: [''],
    billingEmail: [''],
    paymentTermsDays: [30],
    notes: [''],
    internalNotes: [''],
    lineItems: this.fb.array([])
  });

  get lineItemsArray(): FormArray {
    return this.form.get('lineItems') as FormArray;
  }

  ngOnInit(): void {
    this.loadData();
    // Set default due date to 30 days from now
    const dueDate = new Date();
    dueDate.setDate(dueDate.getDate() + 30);
    this.form.patchValue({ dueDate: dueDate.toISOString().split('T')[0] });
  }

  loadData(): void {
    this.isLoading.set(true);
    Promise.all([
      this.memberService.getMembers({ pageSize: 1000 }).toPromise(),
      this.feeService.getAllFees().toPromise()
    ]).then(([membersResult, fees]) => {
      this.members.set(membersResult?.items || []);
      this.fees.set(fees || []);
      this.isLoading.set(false);
    }).catch(() => {
      this.isLoading.set(false);
    });
  }

  onMemberChange(): void {
    const memberId = this.form.get('memberId')?.value;
    const member = this.members().find(m => m.id === memberId);
    if (member) {
      this.form.patchValue({
        billingName: member.fullName,
        billingEmail: member.email
      });
    }
  }

  addLineItem(): void {
    this.lineItemsArray.push(this.fb.group({
      description: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      unitPrice: [0, [Validators.required, Validators.min(0)]],
      feeId: [null],
      feeType: [null],
      taxRate: [null],
      servicePeriod: [''],
      sortOrder: [this.lineItemsArray.length]
    }));
  }

  addFeeItem(selectEl: HTMLSelectElement): void {
    const feeId = selectEl.value;
    if (!feeId) return;

    const fee = this.fees().find(f => f.id === feeId);
    if (fee) {
      this.lineItemsArray.push(this.fb.group({
        description: [fee.name, Validators.required],
        quantity: [1, [Validators.required, Validators.min(1)]],
        unitPrice: [fee.amount, [Validators.required, Validators.min(0)]],
        feeId: [fee.id],
        feeType: [fee.type],
        taxRate: [null],
        servicePeriod: [''],
        sortOrder: [this.lineItemsArray.length]
      }));
      selectEl.value = '';
    }
  }

  removeLineItem(index: number): void {
    this.lineItemsArray.removeAt(index);
  }

  getLineItemTotal(index: number): number {
    const item = this.lineItemsArray.at(index);
    const qty = item.get('quantity')?.value || 0;
    const price = item.get('unitPrice')?.value || 0;
    const taxRate = item.get('taxRate')?.value || 0;
    const subtotal = qty * price;
    const tax = subtotal * (taxRate / 100);
    return subtotal + tax;
  }

  getSubTotal(): number {
    return this.lineItemsArray.controls.reduce((sum, item) => {
      const qty = item.get('quantity')?.value || 0;
      const price = item.get('unitPrice')?.value || 0;
      return sum + (qty * price);
    }, 0);
  }

  getTaxTotal(): number {
    return this.lineItemsArray.controls.reduce((sum, item) => {
      const qty = item.get('quantity')?.value || 0;
      const price = item.get('unitPrice')?.value || 0;
      const taxRate = item.get('taxRate')?.value || 0;
      return sum + ((qty * price) * (taxRate / 100));
    }, 0);
  }

  getGrandTotal(): number {
    return this.getSubTotal() + this.getTaxTotal();
  }

  onSubmit(): void {
    if (this.form.invalid || this.lineItemsArray.length === 0) return;

    this.isSaving.set(true);
    const formValue = this.form.value;

    const request = {
      memberId: formValue.memberId,
      dueDate: new Date(formValue.dueDate),
      invoiceDate: formValue.invoiceDate ? new Date(formValue.invoiceDate) : undefined,
      currency: formValue.currency,
      purchaseOrderNumber: formValue.purchaseOrderNumber || undefined,
      billingName: formValue.billingName || undefined,
      billingAddress: formValue.billingAddress || undefined,
      billingEmail: formValue.billingEmail || undefined,
      paymentTermsDays: formValue.paymentTermsDays,
      notes: formValue.notes || undefined,
      internalNotes: formValue.internalNotes || undefined,
      lineItems: formValue.lineItems.map((item: any, index: number) => ({
        description: item.description,
        unitPrice: item.unitPrice,
        quantity: item.quantity,
        feeId: item.feeId || undefined,
        feeType: item.feeType || undefined,
        taxRate: item.taxRate || undefined,
        servicePeriod: item.servicePeriod || undefined,
        sortOrder: index
      }))
    };

    this.invoiceService.createInvoice(request).subscribe({
      next: (invoice) => {
        this.notificationService.success('Invoice created successfully');
        this.router.navigate(['/club/invoices', invoice.id]);
      },
      error: () => {
        this.notificationService.error('Failed to create invoice');
        this.isSaving.set(false);
      }
    });
  }

  getFeeTypeLabel(type: FeeType): string {
    return FeeTypeLabels[type] || type;
  }
}
