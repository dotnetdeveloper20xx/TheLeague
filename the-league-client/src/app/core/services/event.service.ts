import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import {
  Event,
  EventType,
  PagedResult
} from '../models';

export interface EventCreateRequest {
  title: string;
  description?: string;
  type: EventType;
  startDateTime: Date;
  endDateTime: Date;
  location?: string;
  capacity?: number;
  isTicketed: boolean;
  ticketPrice?: number;
  memberTicketPrice?: number;
  ticketSalesEndDate?: Date;
  requiresRSVP: boolean;
  rsvpDeadline?: Date;
  venueId?: string;
}

export interface EventFilter {
  dateFrom?: Date;
  dateTo?: Date;
  type?: EventType;
  includeCancelled?: boolean;
  includeUnpublished?: boolean;
  page?: number;
  pageSize?: number;
}

export interface EventRegistration {
  id: string;
  memberId: string;
  memberName: string;
  registeredAt: Date;
  ticketQuantity: number;
  totalPaid: number;
  status: string;
}

@Injectable({
  providedIn: 'root'
})
export class EventService {
  private endpoint = 'events';

  constructor(private api: ApiService) {}

  getEvents(filter?: EventFilter): Observable<PagedResult<Event>> {
    return this.api.get<PagedResult<Event>>(this.endpoint, filter);
  }

  getEvent(id: string): Observable<Event> {
    return this.api.get<Event>(`${this.endpoint}/${id}`);
  }

  createEvent(data: EventCreateRequest): Observable<Event> {
    return this.api.post<Event>(this.endpoint, data);
  }

  updateEvent(id: string, data: Partial<EventCreateRequest>): Observable<Event> {
    return this.api.put<Event>(`${this.endpoint}/${id}`, data);
  }

  deleteEvent(id: string): Observable<void> {
    return this.api.delete<void>(`${this.endpoint}/${id}`);
  }

  publishEvent(id: string): Observable<Event> {
    return this.api.post<Event>(`${this.endpoint}/${id}/publish`, {});
  }

  unpublishEvent(id: string): Observable<Event> {
    return this.api.post<Event>(`${this.endpoint}/${id}/unpublish`, {});
  }

  cancelEvent(id: string, reason: string): Observable<Event> {
    return this.api.post<Event>(`${this.endpoint}/${id}/cancel`, { reason });
  }

  // Registrations
  getEventRegistrations(eventId: string): Observable<EventRegistration[]> {
    return this.api.get<EventRegistration[]>(`${this.endpoint}/${eventId}/registrations`);
  }

  registerForEvent(eventId: string, data: { memberId: string; ticketQuantity?: number }): Observable<EventRegistration> {
    return this.api.post<EventRegistration>(`${this.endpoint}/${eventId}/register`, data);
  }

  cancelRegistration(eventId: string, registrationId: string): Observable<void> {
    return this.api.delete<void>(`${this.endpoint}/${eventId}/registrations/${registrationId}`);
  }

  // Member's events
  getMemberEvents(memberId: string): Observable<Event[]> {
    return this.api.get<Event[]>(`members/${memberId}/events`);
  }

  // Image upload
  uploadEventImage(id: string, file: File): Observable<{ imageUrl: string }> {
    return this.api.upload<{ imageUrl: string }>(`${this.endpoint}/${id}/image`, file);
  }
}
