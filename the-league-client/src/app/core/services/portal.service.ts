import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Member, Session, Event, Payment, MembershipSummary, MemberUpdateRequest } from '../models';

export interface PortalDashboard {
  member: Member;
  membership: MembershipSummary | null;
  upcomingSessions: Session[];
  upcomingEvents: Event[];
  recentPayments: Payment[];
  notifications: PortalNotification[];
}

export interface PortalNotification {
  id: string;
  type: string;
  title: string;
  message: string;
  createdAt: Date;
  isRead: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class PortalService {
  private endpoint = 'portal';

  constructor(private api: ApiService) {}

  getDashboard(): Observable<PortalDashboard> {
    return this.api.get<PortalDashboard>(`${this.endpoint}/dashboard`);
  }

  getProfile(): Observable<Member> {
    return this.api.get<Member>(`${this.endpoint}/profile`);
  }

  updateProfile(data: MemberUpdateRequest): Observable<Member> {
    return this.api.put<Member>(`${this.endpoint}/profile`, data);
  }

  uploadProfilePhoto(file: File): Observable<{ profilePhotoUrl: string }> {
    return this.api.upload<{ profilePhotoUrl: string }>(`${this.endpoint}/profile/photo`, file);
  }

  getMembership(): Observable<MembershipSummary | null> {
    return this.api.get<MembershipSummary | null>(`${this.endpoint}/membership`);
  }

  getUpcomingSessions(): Observable<Session[]> {
    return this.api.get<Session[]>(`${this.endpoint}/sessions/upcoming`);
  }

  getMyBookings(): Observable<Session[]> {
    return this.api.get<Session[]>(`${this.endpoint}/sessions/my-bookings`);
  }

  bookSession(sessionId: string, familyMemberId?: string): Observable<void> {
    return this.api.post<void>(`${this.endpoint}/sessions/${sessionId}/book`, { familyMemberId });
  }

  cancelBooking(sessionId: string): Observable<void> {
    return this.api.delete<void>(`${this.endpoint}/sessions/${sessionId}/booking`);
  }

  getUpcomingEvents(): Observable<Event[]> {
    return this.api.get<Event[]>(`${this.endpoint}/events/upcoming`);
  }

  getMyEvents(): Observable<Event[]> {
    return this.api.get<Event[]>(`${this.endpoint}/events/my-registrations`);
  }

  registerForEvent(eventId: string, ticketQuantity?: number): Observable<void> {
    return this.api.post<void>(`${this.endpoint}/events/${eventId}/register`, { ticketQuantity });
  }

  cancelEventRegistration(eventId: string): Observable<void> {
    return this.api.delete<void>(`${this.endpoint}/events/${eventId}/registration`);
  }

  getPaymentHistory(): Observable<Payment[]> {
    return this.api.get<Payment[]>(`${this.endpoint}/payments`);
  }

  getNotifications(): Observable<PortalNotification[]> {
    return this.api.get<PortalNotification[]>(`${this.endpoint}/notifications`);
  }

  markNotificationRead(id: string): Observable<void> {
    return this.api.post<void>(`${this.endpoint}/notifications/${id}/read`, {});
  }

  markAllNotificationsRead(): Observable<void> {
    return this.api.post<void>(`${this.endpoint}/notifications/read-all`, {});
  }
}
