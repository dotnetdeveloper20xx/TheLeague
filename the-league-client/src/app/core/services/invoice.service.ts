import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import {
  Invoice,
  InvoiceListItem,
  InvoiceCreateRequest,
  InvoiceUpdateRequest,
  InvoiceLineItemCreateRequest,
  InvoiceFilterRequest,
  RecordPaymentRequest,
  InvoiceSummary
} from '../models/invoice.model';
import { PagedResult } from '../models';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {
  private endpoint = 'invoices';

  constructor(private api: ApiService) {}

  getInvoices(filter?: InvoiceFilterRequest): Observable<PagedResult<InvoiceListItem>> {
    return this.api.get<PagedResult<InvoiceListItem>>(this.endpoint, filter as Record<string, unknown>);
  }

  getSummary(): Observable<InvoiceSummary> {
    return this.api.get<InvoiceSummary>(`${this.endpoint}/summary`);
  }

  getOverdueInvoices(): Observable<InvoiceListItem[]> {
    return this.api.get<InvoiceListItem[]>(`${this.endpoint}/overdue`);
  }

  getInvoice(id: string): Observable<Invoice> {
    return this.api.get<Invoice>(`${this.endpoint}/${id}`);
  }

  getInvoiceByNumber(invoiceNumber: string): Observable<Invoice> {
    return this.api.get<Invoice>(`${this.endpoint}/by-number/${invoiceNumber}`);
  }

  getMemberInvoices(memberId: string): Observable<InvoiceListItem[]> {
    return this.api.get<InvoiceListItem[]>(`${this.endpoint}/member/${memberId}`);
  }

  createInvoice(data: InvoiceCreateRequest): Observable<Invoice> {
    return this.api.post<Invoice>(this.endpoint, data);
  }

  updateInvoice(id: string, data: InvoiceUpdateRequest): Observable<Invoice> {
    return this.api.put<Invoice>(`${this.endpoint}/${id}`, data);
  }

  deleteInvoice(id: string): Observable<void> {
    return this.api.delete<void>(`${this.endpoint}/${id}`);
  }

  // Line Items
  addLineItem(invoiceId: string, data: InvoiceLineItemCreateRequest): Observable<Invoice> {
    return this.api.post<Invoice>(`${this.endpoint}/${invoiceId}/line-items`, data);
  }

  updateLineItem(invoiceId: string, lineItemId: string, data: InvoiceLineItemCreateRequest): Observable<Invoice> {
    return this.api.put<Invoice>(`${this.endpoint}/${invoiceId}/line-items/${lineItemId}`, data);
  }

  removeLineItem(invoiceId: string, lineItemId: string): Observable<Invoice> {
    return this.api.delete<Invoice>(`${this.endpoint}/${invoiceId}/line-items/${lineItemId}`);
  }

  // Status Operations
  sendInvoice(id: string, sendEmail = true, customMessage?: string): Observable<Invoice> {
    return this.api.post<Invoice>(`${this.endpoint}/${id}/send`, { sendEmail, customMessage });
  }

  markAsPaid(id: string): Observable<Invoice> {
    return this.api.post<Invoice>(`${this.endpoint}/${id}/mark-paid`, {});
  }

  voidInvoice(id: string, reason: string): Observable<Invoice> {
    return this.api.post<Invoice>(`${this.endpoint}/${id}/void`, { reason });
  }

  sendReminder(id: string): Observable<Invoice> {
    return this.api.post<Invoice>(`${this.endpoint}/${id}/send-reminder`, {});
  }

  // Payments
  recordPayment(id: string, data: RecordPaymentRequest): Observable<Invoice> {
    return this.api.post<Invoice>(`${this.endpoint}/${id}/payments`, data);
  }
}
