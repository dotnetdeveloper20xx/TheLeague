import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Fee, FeeListItem, FeeCreateRequest, FeeFilterRequest, FeeType } from '../models/fee.model';
import { PagedResult } from '../models';

@Injectable({
  providedIn: 'root'
})
export class FeeService {
  private endpoint = 'fees';

  constructor(private api: ApiService) {}

  getFees(filter?: FeeFilterRequest): Observable<PagedResult<FeeListItem>> {
    return this.api.get<PagedResult<FeeListItem>>(this.endpoint, filter as Record<string, unknown>);
  }

  getAllFees(includeInactive = false): Observable<FeeListItem[]> {
    return this.api.get<FeeListItem[]>(`${this.endpoint}/all`, { includeInactive });
  }

  getFee(id: string): Observable<Fee> {
    return this.api.get<Fee>(`${this.endpoint}/${id}`);
  }

  getFeesByType(type: FeeType): Observable<FeeListItem[]> {
    return this.api.get<FeeListItem[]>(`${this.endpoint}/by-type/${type}`);
  }

  createFee(data: FeeCreateRequest): Observable<Fee> {
    return this.api.post<Fee>(this.endpoint, data);
  }

  updateFee(id: string, data: Partial<FeeCreateRequest>): Observable<Fee> {
    return this.api.put<Fee>(`${this.endpoint}/${id}`, data);
  }

  deleteFee(id: string): Observable<void> {
    return this.api.delete<void>(`${this.endpoint}/${id}`);
  }

  toggleActive(id: string): Observable<void> {
    return this.api.post<void>(`${this.endpoint}/${id}/toggle-active`, {});
  }
}
