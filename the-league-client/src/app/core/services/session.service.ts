import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import {
  Session,
  SessionBooking,
  SessionFilter,
  PagedResult
} from '../models';

export interface SessionCreateRequest {
  title: string;
  description?: string;
  category: string;
  startTime: Date;
  endTime: Date;
  capacity: number;
  sessionFee?: number;
  venueId?: string;
  recurringScheduleId?: string;
}

export interface BookSessionRequest {
  memberId: string;
  familyMemberId?: string;
  notes?: string;
}

@Injectable({
  providedIn: 'root'
})
export class SessionService {
  private endpoint = 'sessions';

  constructor(private api: ApiService) {}

  getSessions(filter?: SessionFilter): Observable<PagedResult<Session>> {
    return this.api.get<PagedResult<Session>>(this.endpoint, filter);
  }

  getSession(id: string): Observable<Session> {
    return this.api.get<Session>(`${this.endpoint}/${id}`);
  }

  createSession(data: SessionCreateRequest): Observable<Session> {
    return this.api.post<Session>(this.endpoint, data);
  }

  updateSession(id: string, data: Partial<SessionCreateRequest>): Observable<Session> {
    return this.api.put<Session>(`${this.endpoint}/${id}`, data);
  }

  deleteSession(id: string): Observable<void> {
    return this.api.delete<void>(`${this.endpoint}/${id}`);
  }

  cancelSession(id: string, reason: string): Observable<Session> {
    return this.api.post<Session>(`${this.endpoint}/${id}/cancel`, { reason });
  }

  // Bookings
  getSessionBookings(sessionId: string): Observable<SessionBooking[]> {
    return this.api.get<SessionBooking[]>(`${this.endpoint}/${sessionId}/bookings`);
  }

  bookSession(sessionId: string, data: BookSessionRequest): Observable<SessionBooking> {
    return this.api.post<SessionBooking>(`${this.endpoint}/${sessionId}/book`, data);
  }

  cancelBooking(sessionId: string, bookingId: string): Observable<void> {
    return this.api.delete<void>(`${this.endpoint}/${sessionId}/bookings/${bookingId}`);
  }

  checkInBooking(sessionId: string, bookingId: string): Observable<SessionBooking> {
    return this.api.post<SessionBooking>(`${this.endpoint}/${sessionId}/bookings/${bookingId}/checkin`, {});
  }

  markAttendance(sessionId: string, bookingId: string, attended: boolean): Observable<SessionBooking> {
    return this.api.post<SessionBooking>(`${this.endpoint}/${sessionId}/bookings/${bookingId}/attendance`, { attended });
  }

  // Member's sessions
  getMemberSessions(memberId: string, filter?: { upcoming?: boolean }): Observable<Session[]> {
    return this.api.get<Session[]>(`members/${memberId}/sessions`, filter);
  }

  getMemberBookings(memberId: string): Observable<SessionBooking[]> {
    return this.api.get<SessionBooking[]>(`members/${memberId}/bookings`);
  }
}
