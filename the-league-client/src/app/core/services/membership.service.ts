import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { MembershipType, MembershipSummary, PagedResult } from '../models';

export interface MembershipTypeCreateRequest {
  name: string;
  description?: string;
  annualFee: number;
  monthlyFee?: number;
  sessionFee?: number;
  minAge?: number;
  maxAge?: number;
  maxFamilyMembers?: number;
  isActive: boolean;
  allowOnlineSignup: boolean;
  sortOrder: number;
  includesBooking: boolean;
  includesEvents: boolean;
  maxSessionsPerWeek?: number;
}

export interface MembershipCreateRequest {
  memberId: string;
  membershipTypeId: string;
  startDate: Date;
  autoRenew?: boolean;
}

export interface Membership {
  id: string;
  memberId: string;
  memberName: string;
  membershipTypeId: string;
  membershipTypeName: string;
  startDate: Date;
  endDate: Date;
  status: string;
  amountDue: number;
  amountPaid: number;
  autoRenew: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class MembershipService {
  private endpoint = 'memberships';
  private typesEndpoint = 'membership-types';

  constructor(private api: ApiService) {}

  // Membership Types
  getMembershipTypes(): Observable<MembershipType[]> {
    return this.api.get<MembershipType[]>(this.typesEndpoint);
  }

  getMembershipType(id: string): Observable<MembershipType> {
    return this.api.get<MembershipType>(`${this.typesEndpoint}/${id}`);
  }

  createMembershipType(data: MembershipTypeCreateRequest): Observable<MembershipType> {
    return this.api.post<MembershipType>(this.typesEndpoint, data);
  }

  updateMembershipType(id: string, data: Partial<MembershipTypeCreateRequest>): Observable<MembershipType> {
    return this.api.put<MembershipType>(`${this.typesEndpoint}/${id}`, data);
  }

  deleteMembershipType(id: string): Observable<void> {
    return this.api.delete<void>(`${this.typesEndpoint}/${id}`);
  }

  // Memberships
  getMemberships(params?: {
    status?: string;
    membershipTypeId?: string;
    page?: number;
    pageSize?: number;
  }): Observable<PagedResult<Membership>> {
    return this.api.get<PagedResult<Membership>>(this.endpoint, params);
  }

  getMembership(id: string): Observable<Membership> {
    return this.api.get<Membership>(`${this.endpoint}/${id}`);
  }

  createMembership(data: MembershipCreateRequest): Observable<Membership> {
    return this.api.post<Membership>(this.endpoint, data);
  }

  renewMembership(id: string): Observable<Membership> {
    return this.api.post<Membership>(`${this.endpoint}/${id}/renew`, {});
  }

  cancelMembership(id: string): Observable<Membership> {
    return this.api.post<Membership>(`${this.endpoint}/${id}/cancel`, {});
  }

  setAutoRenew(id: string, autoRenew: boolean): Observable<Membership> {
    return this.api.patch<Membership>(`${this.endpoint}/${id}/auto-renew`, { autoRenew });
  }

  // Member's membership
  getMemberMembership(memberId: string): Observable<MembershipSummary | null> {
    return this.api.get<MembershipSummary | null>(`members/${memberId}/membership`);
  }
}
