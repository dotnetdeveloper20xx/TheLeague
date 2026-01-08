import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import {
  Club,
  ClubCreateRequest,
  ClubDashboard,
  PagedResult
} from '../models';

@Injectable({
  providedIn: 'root'
})
export class ClubService {
  private endpoint = 'clubs';

  constructor(private api: ApiService) {}

  getClubs(params?: { page?: number; pageSize?: number; searchTerm?: string }): Observable<PagedResult<Club>> {
    return this.api.get<PagedResult<Club>>(this.endpoint, params);
  }

  getClub(id: string): Observable<Club> {
    return this.api.get<Club>(`${this.endpoint}/${id}`);
  }

  getClubBySlug(slug: string): Observable<Club> {
    return this.api.get<Club>(`${this.endpoint}/slug/${slug}`);
  }

  createClub(data: ClubCreateRequest): Observable<Club> {
    return this.api.post<Club>(this.endpoint, data);
  }

  updateClub(id: string, data: Partial<ClubCreateRequest>): Observable<Club> {
    return this.api.put<Club>(`${this.endpoint}/${id}`, data);
  }

  deleteClub(id: string): Observable<void> {
    return this.api.delete<void>(`${this.endpoint}/${id}`);
  }

  getDashboard(): Observable<ClubDashboard> {
    return this.api.get<ClubDashboard>(`${this.endpoint}/dashboard`);
  }

  uploadLogo(id: string, file: File): Observable<{ logoUrl: string }> {
    return this.api.upload<{ logoUrl: string }>(`${this.endpoint}/${id}/logo`, file);
  }
}
