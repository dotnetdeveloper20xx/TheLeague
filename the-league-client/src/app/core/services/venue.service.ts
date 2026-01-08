import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Venue } from '../models';

export interface VenueCreateRequest {
  name: string;
  description?: string;
  address?: string;
  postCode?: string;
  capacity?: number;
  facilities?: string;
  isActive: boolean;
  isPrimary: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class VenueService {
  private endpoint = 'venues';

  constructor(private api: ApiService) {}

  getVenues(params?: { includeInactive?: boolean }): Observable<Venue[]> {
    return this.api.get<Venue[]>(this.endpoint, params);
  }

  getVenue(id: string): Observable<Venue> {
    return this.api.get<Venue>(`${this.endpoint}/${id}`);
  }

  createVenue(data: VenueCreateRequest): Observable<Venue> {
    return this.api.post<Venue>(this.endpoint, data);
  }

  updateVenue(id: string, data: Partial<VenueCreateRequest>): Observable<Venue> {
    return this.api.put<Venue>(`${this.endpoint}/${id}`, data);
  }

  deleteVenue(id: string): Observable<void> {
    return this.api.delete<void>(`${this.endpoint}/${id}`);
  }

  uploadImage(id: string, file: File): Observable<{ imageUrl: string }> {
    return this.api.upload<{ imageUrl: string }>(`${this.endpoint}/${id}/image`, file);
  }

  setPrimary(id: string): Observable<Venue> {
    return this.api.post<Venue>(`${this.endpoint}/${id}/set-primary`, {});
  }
}
