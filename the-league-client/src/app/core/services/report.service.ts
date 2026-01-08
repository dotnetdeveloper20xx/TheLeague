import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { MonthlyData, ChartData } from '../models';

export interface MembershipReport {
  totalMembers: number;
  activeMembers: number;
  pendingMembers: number;
  expiredMembers: number;
  newMembersThisMonth: number;
  churnRate: number;
  membersByType: ChartData[];
  membersByStatus: ChartData[];
  memberGrowth: MonthlyData[];
  ageDistribution: ChartData[];
}

export interface FinancialReport {
  totalRevenue: number;
  revenueThisMonth: number;
  revenueLastMonth: number;
  revenueGrowth: number;
  outstandingPayments: number;
  revenueByType: ChartData[];
  revenueByMethod: ChartData[];
  monthlyRevenue: MonthlyData[];
  topMembersByRevenue: { memberName: string; totalPaid: number }[];
}

export interface AttendanceReport {
  totalSessions: number;
  averageAttendance: number;
  totalBookings: number;
  noShowRate: number;
  attendanceByCategory: ChartData[];
  attendanceByDay: ChartData[];
  weeklyAttendance: MonthlyData[];
  popularSessions: { sessionTitle: string; averageAttendance: number }[];
}

export interface EventReport {
  totalEvents: number;
  upcomingEvents: number;
  totalRegistrations: number;
  averageAttendance: number;
  ticketRevenue: number;
  eventsByType: ChartData[];
  registrationTrend: MonthlyData[];
}

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private endpoint = 'reports';

  constructor(private api: ApiService) {}

  getMembershipReport(params?: { dateFrom?: Date; dateTo?: Date }): Observable<MembershipReport> {
    return this.api.get<MembershipReport>(`${this.endpoint}/membership`, params);
  }

  getFinancialReport(params?: { dateFrom?: Date; dateTo?: Date }): Observable<FinancialReport> {
    return this.api.get<FinancialReport>(`${this.endpoint}/financial`, params);
  }

  getAttendanceReport(params?: { dateFrom?: Date; dateTo?: Date }): Observable<AttendanceReport> {
    return this.api.get<AttendanceReport>(`${this.endpoint}/attendance`, params);
  }

  getEventReport(params?: { dateFrom?: Date; dateTo?: Date }): Observable<EventReport> {
    return this.api.get<EventReport>(`${this.endpoint}/events`, params);
  }

  exportMembershipReport(params?: { dateFrom?: Date; dateTo?: Date; format?: 'csv' | 'pdf' }): Observable<Blob> {
    return this.api.get<Blob>(`${this.endpoint}/membership/export`, params);
  }

  exportFinancialReport(params?: { dateFrom?: Date; dateTo?: Date; format?: 'csv' | 'pdf' }): Observable<Blob> {
    return this.api.get<Blob>(`${this.endpoint}/financial/export`, params);
  }
}
