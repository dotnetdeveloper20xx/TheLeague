import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import {
  Payment,
  PaymentStatus,
  PaymentMethod,
  PaymentType,
  PagedResult
} from '../models';

export interface PaymentFilter {
  memberId?: string;
  status?: PaymentStatus;
  method?: PaymentMethod;
  type?: PaymentType;
  dateFrom?: Date;
  dateTo?: Date;
  page?: number;
  pageSize?: number;
}

export interface ManualPaymentRequest {
  memberId: string;
  amount: number;
  type: PaymentType;
  method: PaymentMethod;
  description?: string;
  reference?: string;
}

export interface CreatePaymentIntentRequest {
  memberId: string;
  amount: number;
  type: PaymentType;
  description?: string;
  membershipId?: string;
  eventTicketId?: string;
}

export interface PaymentIntent {
  clientSecret: string;
  paymentId: string;
  amount: number;
}

export interface RefundRequest {
  reason?: string;
  amount?: number;
}

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private endpoint = 'payments';

  constructor(private api: ApiService) {}

  getPayments(filter?: PaymentFilter): Observable<PagedResult<Payment>> {
    return this.api.get<PagedResult<Payment>>(this.endpoint, filter);
  }

  getPayment(id: string): Observable<Payment> {
    return this.api.get<Payment>(`${this.endpoint}/${id}`);
  }

  recordManualPayment(data: ManualPaymentRequest): Observable<Payment> {
    return this.api.post<Payment>(`${this.endpoint}/manual`, data);
  }

  // Stripe integration
  createPaymentIntent(data: CreatePaymentIntentRequest): Observable<PaymentIntent> {
    return this.api.post<PaymentIntent>(`${this.endpoint}/stripe/create-intent`, data);
  }

  confirmPayment(paymentId: string): Observable<Payment> {
    return this.api.post<Payment>(`${this.endpoint}/${paymentId}/confirm`, {});
  }

  // PayPal integration
  createPayPalOrder(data: CreatePaymentIntentRequest): Observable<{ orderId: string; paymentId: string }> {
    return this.api.post<{ orderId: string; paymentId: string }>(`${this.endpoint}/paypal/create-order`, data);
  }

  capturePayPalOrder(orderId: string): Observable<Payment> {
    return this.api.post<Payment>(`${this.endpoint}/paypal/capture/${orderId}`, {});
  }

  // Refunds
  refundPayment(id: string, data: RefundRequest): Observable<Payment> {
    return this.api.post<Payment>(`${this.endpoint}/${id}/refund`, data);
  }

  // Member's payments
  getMemberPayments(memberId: string): Observable<Payment[]> {
    return this.api.get<Payment[]>(`members/${memberId}/payments`);
  }

  // Receipt
  getReceipt(id: string): Observable<Blob> {
    return this.api.get<Blob>(`${this.endpoint}/${id}/receipt`);
  }

  // Reports
  getPaymentsSummary(params: { dateFrom: Date; dateTo: Date }): Observable<{
    totalAmount: number;
    totalCount: number;
    byType: { type: string; amount: number; count: number }[];
    byMethod: { method: string; amount: number; count: number }[];
  }> {
    return this.api.get(`${this.endpoint}/summary`, params);
  }
}
