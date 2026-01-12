import { PaymentMethod, PaymentStatus } from './index';
import { FeeType } from './fee.model';

export interface Invoice {
  id: string;
  invoiceNumber: string;
  memberId: string;
  memberName: string;
  memberEmail?: string;
  invoiceDate: Date;
  dueDate: Date;
  paidDate?: Date;
  sentDate?: Date;
  subTotal: number;
  discountAmount?: number;
  taxAmount?: number;
  totalAmount: number;
  paidAmount: number;
  balanceDue: number;
  currency: string;
  status: InvoiceStatus;
  collectionStatus: CollectionStatus;
  billingName?: string;
  billingAddress?: string;
  billingEmail?: string;
  paymentTermsDays: number;
  isOverdue: boolean;
  daysOverdue: number;
  remindersSent: number;
  notes?: string;
  pdfUrl?: string;
  createdAt: Date;
  lineItems: InvoiceLineItem[];
  payments: InvoicePayment[];
}

export interface InvoiceListItem {
  id: string;
  invoiceNumber: string;
  memberId: string;
  memberName: string;
  invoiceDate: Date;
  dueDate: Date;
  totalAmount: number;
  paidAmount: number;
  balanceDue: number;
  currency: string;
  status: InvoiceStatus;
  isOverdue: boolean;
  daysOverdue: number;
}

export interface InvoiceLineItem {
  id: string;
  description: string;
  feeType?: FeeType;
  quantity: number;
  unitPrice: number;
  discountAmount?: number;
  subTotal: number;
  taxAmount?: number;
  total: number;
  servicePeriod?: string;
  sortOrder: number;
}

export interface InvoicePayment {
  id: string;
  amount: number;
  paymentDate: Date;
  method: PaymentMethod;
  status: PaymentStatus;
  receiptNumber?: string;
}

export interface InvoiceCreateRequest {
  memberId: string;
  dueDate: Date;
  invoiceDate?: Date;
  purchaseOrderNumber?: string;
  billingName?: string;
  billingAddress?: string;
  billingCity?: string;
  billingPostcode?: string;
  billingCountry?: string;
  billingEmail?: string;
  paymentTermsDays?: number;
  allowPartialPayment?: boolean;
  allowOnlinePayment?: boolean;
  notes?: string;
  internalNotes?: string;
  currency?: string;
  lineItems?: InvoiceLineItemCreateRequest[];
}

export interface InvoiceUpdateRequest {
  dueDate?: Date;
  purchaseOrderNumber?: string;
  billingName?: string;
  billingAddress?: string;
  billingCity?: string;
  billingPostcode?: string;
  billingCountry?: string;
  billingEmail?: string;
  paymentTermsDays?: number;
  allowPartialPayment?: boolean;
  allowOnlinePayment?: boolean;
  notes?: string;
  internalNotes?: string;
}

export interface InvoiceLineItemCreateRequest {
  description: string;
  unitPrice: number;
  quantity?: number;
  feeId?: string;
  feeType?: FeeType;
  discountAmount?: number;
  discountPercent?: number;
  taxRate?: number;
  servicePeriod?: string;
  serviceStartDate?: Date;
  serviceEndDate?: Date;
  glAccountCode?: string;
  costCenter?: string;
  sortOrder?: number;
}

export interface InvoiceFilterRequest {
  memberId?: string;
  status?: InvoiceStatus;
  collectionStatus?: CollectionStatus;
  isOverdue?: boolean;
  dateFrom?: Date;
  dateTo?: Date;
  dueDateFrom?: Date;
  dueDateTo?: Date;
  search?: string;
  page?: number;
  pageSize?: number;
}

export interface RecordPaymentRequest {
  amount: number;
  method: PaymentMethod;
  paymentDate?: Date;
  reference?: string;
  notes?: string;
}

export interface InvoiceSummary {
  totalInvoices: number;
  draftCount: number;
  sentCount: number;
  paidCount: number;
  overdueCount: number;
  totalOutstanding: number;
  totalOverdue: number;
  totalPaidThisMonth: number;
}

export enum InvoiceStatus {
  Draft = 'Draft',
  Sent = 'Sent',
  Viewed = 'Viewed',
  PartiallyPaid = 'PartiallyPaid',
  Paid = 'Paid',
  Overdue = 'Overdue',
  Voided = 'Voided',
  Disputed = 'Disputed',
  WrittenOff = 'WrittenOff'
}

export enum CollectionStatus {
  Current = 'Current',
  Overdue30 = 'Overdue30',
  Overdue60 = 'Overdue60',
  Overdue90 = 'Overdue90',
  Overdue120Plus = 'Overdue120Plus',
  InArrangement = 'InArrangement',
  SentToCollection = 'SentToCollection',
  WrittenOff = 'WrittenOff',
  Disputed = 'Disputed'
}

export const InvoiceStatusLabels: Record<InvoiceStatus, string> = {
  [InvoiceStatus.Draft]: 'Draft',
  [InvoiceStatus.Sent]: 'Sent',
  [InvoiceStatus.Viewed]: 'Viewed',
  [InvoiceStatus.PartiallyPaid]: 'Partially Paid',
  [InvoiceStatus.Paid]: 'Paid',
  [InvoiceStatus.Overdue]: 'Overdue',
  [InvoiceStatus.Voided]: 'Voided',
  [InvoiceStatus.Disputed]: 'Disputed',
  [InvoiceStatus.WrittenOff]: 'Written Off'
};

export const InvoiceStatusColors: Record<InvoiceStatus, string> = {
  [InvoiceStatus.Draft]: 'badge-gray',
  [InvoiceStatus.Sent]: 'badge-info',
  [InvoiceStatus.Viewed]: 'badge-info',
  [InvoiceStatus.PartiallyPaid]: 'badge-warning',
  [InvoiceStatus.Paid]: 'badge-success',
  [InvoiceStatus.Overdue]: 'badge-danger',
  [InvoiceStatus.Voided]: 'badge-gray',
  [InvoiceStatus.Disputed]: 'badge-warning',
  [InvoiceStatus.WrittenOff]: 'badge-gray'
};
