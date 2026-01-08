import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData } from 'chart.js';
import { ClubService } from '../../../core/services/club.service';
import { LoadingSpinnerComponent } from '../../../shared/components';
import { ClubDashboard } from '../../../core/models';
import { CurrencyFormatPipe, DateFormatPipe } from '../../../shared/pipes';

@Component({
  selector: 'app-club-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, BaseChartDirective, LoadingSpinnerComponent, CurrencyFormatPipe, DateFormatPipe],
  template: `
    <div class="space-y-6">
      <!-- Header -->
      <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 class="text-2xl font-bold text-gray-900">Club Dashboard</h1>
          <p class="text-gray-500 mt-1">Welcome back! Here's what's happening with your club.</p>
        </div>
        <div class="flex gap-3">
          <a routerLink="/club/members/new" class="btn-primary inline-flex items-center">
            <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z" />
            </svg>
            Add Member
          </a>
          <a routerLink="/club/sessions/new" class="btn-secondary inline-flex items-center">
            <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
            </svg>
            New Session
          </a>
        </div>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg" message="Loading dashboard..."></app-loading-spinner>
        </div>
      } @else {
        <!-- Stats Cards -->
        <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
          <div class="card bg-gradient-to-br from-blue-500 to-blue-600 text-white">
            <div class="flex items-center justify-between">
              <div>
                <p class="text-blue-100 text-sm">Total Members</p>
                <p class="text-3xl font-bold mt-1">{{ dashboard()?.totalMembers || 0 }}</p>
              </div>
              <div class="w-12 h-12 bg-white/20 rounded-lg flex items-center justify-center">
                <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" />
                </svg>
              </div>
            </div>
            <div class="mt-4 flex items-center text-sm text-blue-100">
              <span class="text-green-300 font-medium">{{ dashboard()?.activeMembers || 0 }} active</span>
              <span class="mx-2">|</span>
              <span>{{ dashboard()?.pendingMembers || 0 }} pending</span>
            </div>
          </div>

          <div class="card bg-gradient-to-br from-green-500 to-green-600 text-white">
            <div class="flex items-center justify-between">
              <div>
                <p class="text-green-100 text-sm">Revenue This Month</p>
                <p class="text-3xl font-bold mt-1">{{ dashboard()?.revenueThisMonth || 0 | currencyFormat }}</p>
              </div>
              <div class="w-12 h-12 bg-white/20 rounded-lg flex items-center justify-center">
                <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
                </svg>
              </div>
            </div>
            <div class="mt-4 text-sm text-green-100">
              Total: {{ dashboard()?.totalRevenue || 0 | currencyFormat }}
            </div>
          </div>

          <div class="card bg-gradient-to-br from-purple-500 to-purple-600 text-white">
            <div class="flex items-center justify-between">
              <div>
                <p class="text-purple-100 text-sm">Upcoming Sessions</p>
                <p class="text-3xl font-bold mt-1">{{ dashboard()?.upcomingSessions || 0 }}</p>
              </div>
              <div class="w-12 h-12 bg-white/20 rounded-lg flex items-center justify-center">
                <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                </svg>
              </div>
            </div>
            <a routerLink="/club/sessions" class="mt-4 text-sm text-purple-100 hover:text-white inline-flex items-center">
              View all sessions
              <svg class="w-4 h-4 ml-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
              </svg>
            </a>
          </div>

          <div class="card bg-gradient-to-br from-orange-500 to-orange-600 text-white">
            <div class="flex items-center justify-between">
              <div>
                <p class="text-orange-100 text-sm">Upcoming Events</p>
                <p class="text-3xl font-bold mt-1">{{ dashboard()?.upcomingEvents || 0 }}</p>
              </div>
              <div class="w-12 h-12 bg-white/20 rounded-lg flex items-center justify-center">
                <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0z" />
                </svg>
              </div>
            </div>
            <a routerLink="/club/events" class="mt-4 text-sm text-orange-100 hover:text-white inline-flex items-center">
              View all events
              <svg class="w-4 h-4 ml-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
              </svg>
            </a>
          </div>
        </div>

        <!-- Charts Row -->
        <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
          <!-- Member Growth Chart -->
          <div class="card">
            <h3 class="card-header">Member Growth</h3>
            <div class="h-64">
              <canvas baseChart
                [data]="memberGrowthChartData"
                [options]="lineChartOptions"
                type="line">
              </canvas>
            </div>
          </div>

          <!-- Members by Type Chart -->
          <div class="card">
            <h3 class="card-header">Members by Type</h3>
            <div class="h-64">
              <canvas baseChart
                [data]="membersByTypeChartData"
                [options]="pieChartOptions"
                type="doughnut">
              </canvas>
            </div>
          </div>
        </div>

        <!-- Recent Activity -->
        <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
          <!-- Recent Payments -->
          <div class="card">
            <div class="flex items-center justify-between mb-4">
              <h3 class="card-header mb-0">Recent Payments</h3>
              <a routerLink="/club/payments" class="text-primary-600 hover:text-primary-700 text-sm font-medium">
                View All
              </a>
            </div>
            <div class="space-y-3">
              @for (payment of dashboard()?.recentPayments || []; track payment.id) {
                <div class="flex items-center justify-between p-3 bg-gray-50 rounded-lg">
                  <div>
                    <p class="font-medium text-gray-900">{{ payment.memberName }}</p>
                    <p class="text-sm text-gray-500">{{ payment.type }} - {{ payment.date | dateFormat }}</p>
                  </div>
                  <div class="text-right">
                    <p class="font-medium text-gray-900">{{ payment.amount | currencyFormat }}</p>
                    <span class="badge text-xs" [class]="payment.status === 'Completed' ? 'badge-success' : 'badge-warning'">
                      {{ payment.status }}
                    </span>
                  </div>
                </div>
              } @empty {
                <p class="text-gray-500 text-center py-4">No recent payments</p>
              }
            </div>
          </div>

          <!-- Quick Actions -->
          <div class="card">
            <h3 class="card-header">Quick Actions</h3>
            <div class="grid grid-cols-2 gap-4">
              <a routerLink="/club/members" class="p-4 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors text-center">
                <svg class="w-8 h-8 mx-auto text-blue-600 mb-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" />
                </svg>
                <p class="font-medium text-gray-900">Manage Members</p>
              </a>
              <a routerLink="/club/payments" class="p-4 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors text-center">
                <svg class="w-8 h-8 mx-auto text-green-600 mb-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 10h18M7 15h1m4 0h1m-7 4h12a3 3 0 003-3V8a3 3 0 00-3-3H6a3 3 0 00-3 3v8a3 3 0 003 3z" />
                </svg>
                <p class="font-medium text-gray-900">View Payments</p>
              </a>
              <a routerLink="/club/sessions" class="p-4 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors text-center">
                <svg class="w-8 h-8 mx-auto text-purple-600 mb-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                </svg>
                <p class="font-medium text-gray-900">Manage Sessions</p>
              </a>
              <a routerLink="/club/reports" class="p-4 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors text-center">
                <svg class="w-8 h-8 mx-auto text-orange-600 mb-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
                </svg>
                <p class="font-medium text-gray-900">View Reports</p>
              </a>
            </div>
          </div>
        </div>
      }
    </div>
  `
})
export class ClubDashboardComponent implements OnInit {
  private clubService = inject(ClubService);

  isLoading = signal(true);
  dashboard = signal<ClubDashboard | null>(null);

  // Chart configurations
  lineChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: { legend: { display: false } },
    scales: { y: { beginAtZero: true } }
  };

  pieChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: { legend: { position: 'right' } }
  };

  memberGrowthChartData: ChartData<'line'> = {
    labels: ['Aug', 'Sep', 'Oct', 'Nov', 'Dec', 'Jan'],
    datasets: [{
      data: [20, 25, 30, 35, 42, 45],
      borderColor: '#3B82F6',
      backgroundColor: 'rgba(59, 130, 246, 0.1)',
      fill: true,
      tension: 0.4
    }]
  };

  membersByTypeChartData: ChartData<'doughnut'> = {
    labels: ['Adult', 'Junior', 'Family', 'Senior'],
    datasets: [{
      data: [25, 15, 8, 7],
      backgroundColor: ['#3B82F6', '#10B981', '#F59E0B', '#8B5CF6']
    }]
  };

  ngOnInit(): void {
    this.loadDashboard();
  }

  private loadDashboard(): void {
    this.clubService.getDashboard().subscribe({
      next: (data) => {
        this.dashboard.set(data);

        // Update chart data if available
        if (data.memberGrowth?.length) {
          this.memberGrowthChartData = {
            labels: data.memberGrowth.map(m => m.month),
            datasets: [{
              data: data.memberGrowth.map(m => m.value),
              borderColor: '#3B82F6',
              backgroundColor: 'rgba(59, 130, 246, 0.1)',
              fill: true,
              tension: 0.4
            }]
          };
        }

        if (data.membersByType?.length) {
          this.membersByTypeChartData = {
            labels: data.membersByType.map(m => m.label),
            datasets: [{
              data: data.membersByType.map(m => m.value),
              backgroundColor: ['#3B82F6', '#10B981', '#F59E0B', '#8B5CF6', '#EC4899']
            }]
          };
        }

        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }
}
