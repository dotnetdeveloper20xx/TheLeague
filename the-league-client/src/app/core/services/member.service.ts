import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import {
  Member,
  MemberListItem,
  MemberCreateRequest,
  MemberUpdateRequest,
  MemberFilter,
  FamilyMember,
  FamilyMemberCreate,
  PagedResult
} from '../models';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  private endpoint = 'members';

  constructor(private api: ApiService) {}

  getMembers(filter?: MemberFilter): Observable<PagedResult<MemberListItem>> {
    return this.api.get<PagedResult<MemberListItem>>(this.endpoint, filter);
  }

  getMember(id: string): Observable<Member> {
    return this.api.get<Member>(`${this.endpoint}/${id}`);
  }

  createMember(data: MemberCreateRequest): Observable<Member> {
    return this.api.post<Member>(this.endpoint, data);
  }

  updateMember(id: string, data: MemberUpdateRequest): Observable<Member> {
    return this.api.put<Member>(`${this.endpoint}/${id}`, data);
  }

  deleteMember(id: string): Observable<void> {
    return this.api.delete<void>(`${this.endpoint}/${id}`);
  }

  // Family members
  getFamilyMembers(memberId: string): Observable<FamilyMember[]> {
    return this.api.get<FamilyMember[]>(`${this.endpoint}/${memberId}/family`);
  }

  addFamilyMember(memberId: string, data: FamilyMemberCreate): Observable<FamilyMember> {
    return this.api.post<FamilyMember>(`${this.endpoint}/${memberId}/family`, data);
  }

  updateFamilyMember(memberId: string, familyMemberId: string, data: FamilyMemberCreate): Observable<FamilyMember> {
    return this.api.put<FamilyMember>(`${this.endpoint}/${memberId}/family/${familyMemberId}`, data);
  }

  removeFamilyMember(memberId: string, familyMemberId: string): Observable<void> {
    return this.api.delete<void>(`${this.endpoint}/${memberId}/family/${familyMemberId}`);
  }

  // Profile photo
  uploadProfilePhoto(id: string, file: File): Observable<{ profilePhotoUrl: string }> {
    return this.api.upload<{ profilePhotoUrl: string }>(`${this.endpoint}/${id}/photo`, file);
  }

  // Status changes
  suspendMember(id: string, reason?: string): Observable<Member> {
    return this.api.post<Member>(`${this.endpoint}/${id}/suspend`, { reason });
  }

  reactivateMember(id: string): Observable<Member> {
    return this.api.post<Member>(`${this.endpoint}/${id}/reactivate`, {});
  }

  // Export
  exportMembers(filter?: MemberFilter): Observable<Blob> {
    return this.api.get<Blob>(`${this.endpoint}/export`, filter);
  }
}
