import { Component, inject, OnInit, OnDestroy, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData } from 'chart.js';
import { Subject, takeUntil } from 'rxjs';
import { ClubService } from '../../../core/services/club.service';
import { LoadingSpinnerComponent } from '../../../shared/components';
import { Club, PagedResult } from '../../../core/models';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, BaseChartDirective, LoadingSpinnerComponent],
  template: `
    <div class="space-y-6">
      <!-- Header -->
      <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 class="text-2xl font-bold text-gray-900">Super Admin Dashboard</h1>
          <p class="text-gray-500 mt-1">Platform overview and management</p>
        </div>
        <a routerLink="/admin/clubs/new" class="btn-primary inline-flex items-center">
          <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
          </svg>
          Add New Club
        </a>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg" message="Loading dashboard..."></app-loading-spinner>
        </div>
      } @else {
        <!-- Stats Cards -->
        <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
          <div class="card">
            <div class="flex items-center">
              <div class="w-12 h-12 rounded-lg bg-blue-100 flex items-center justify-center">
                <svg class="w-6 h-6 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4" />
                </svg>
              </div>
              <div class="ml-4">
                <p class="text-sm font-medium text-gray-500">Total Clubs</p>
                <p class="text-2xl font-bold text-gray-900">{{ stats().totalClubs }}</p>
              </div>
            </div>
          </div>

          <div class="card">
            <div class="flex items-center">
              <div class="w-12 h-12 rounded-lg bg-green-100 flex items-center justify-center">
                <svg class="w-6 h-6 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" />
                </svg>
              </div>
              <div class="ml-4">
                <p class="text-sm font-medium text-gray-500">Total Members</p>
                <p class="text-2xl font-bold text-gray-900">{{ stats().totalMembers }}</p>
              </div>
            </div>
          </div>

          <div class="card">
            <div class="flex items-center">
              <div class="w-12 h-12 rounded-lg bg-yellow-100 flex items-center justify-center">
                <svg class="w-6 h-6 text-yellow-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
                </svg>
              </div>
              <div class="ml-4">
                <p class="text-sm font-medium text-gray-500">Active Members</p>
                <p class="text-2xl font-bold text-gray-900">{{ stats().activeMembers }}</p>
              </div>
            </div>
          </div>

          <div class="card">
            <div class="flex items-center">
              <div class="w-12 h-12 rounded-lg bg-purple-100 flex items-center justify-center">
                <svg class="w-6 h-6 text-purple-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
                </svg>
              </div>
              <div class="ml-4">
                <p class="text-sm font-medium text-gray-500">Total Revenue</p>
                <p class="text-2xl font-bold text-gray-900">{{ stats().totalRevenue | currency:'GBP' }}</p>
              </div>
            </div>
          </div>
        </div>

        <!-- Charts Row -->
        <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
          <!-- Club Distribution Chart -->
          <div class="card">
            <h3 class="card-header">Clubs by Type</h3>
            <div class="h-64">
              <canvas baseChart
                [data]="clubTypeChartData"
                [options]="pieChartOptions"
                type="doughnut">
              </canvas>
            </div>
          </div>

          <!-- Member Growth Chart -->
          <div class="card">
            <h3 class="card-header">Member Growth (Last 6 Months)</h3>
            <div class="h-64">
              <canvas baseChart
                [data]="memberGrowthChartData"
                [options]="lineChartOptions"
                type="line">
              </canvas>
            </div>
          </div>
        </div>

        <!-- Clubs List -->
        <div class="card">
          <div class="flex items-center justify-between mb-4">
            <h3 class="card-header mb-0">Registered Clubs</h3>
            <a routerLink="/admin/clubs" class="text-primary-600 hover:text-primary-700 text-sm font-medium">
              View All
            </a>
          </div>

          <div class="overflow-x-auto">
            <table class="table">
              <thead>
                <tr>
                  <th>Club Name</th>
                  <th>Type</th>
                  <th>Members</th>
                  <th>Status</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                @for (club of clubs(); track club.id) {
                  <tr>
                    <td>
                      <div class="flex items-center">
                        @if (club.logoUrl) {
                          <img [src]="club.logoUrl" [alt]="club.name" class="w-10 h-10 rounded-lg object-cover" />
                        } @else {
                          <div class="w-10 h-10 rounded-lg bg-primary-100 flex items-center justify-center text-primary-600 font-bold">
                            {{ club.name[0] }}
                          </div>
                        }
                        <div class="ml-3">
                          <p class="font-medium text-gray-900">{{ club.name }}</p>
                          <p class="text-sm text-gray-500">{{ club.slug }}</p>
                        </div>
                      </div>
                    </td>
                    <td>{{ club.clubType }}</td>
                    <td>{{ club.memberCount || 0 }}</td>
                    <td>
                      <span class="badge" [class]="club.isActive ? 'badge-success' : 'badge-gray'">
                        {{ club.isActive ? 'Active' : 'Inactive' }}
                      </span>
                    </td>
                    <td>
                      <a [routerLink]="['/admin/clubs', club.id]" class="text-primary-600 hover:text-primary-700">
                        View
                      </a>
                    </td>
                  </tr>
                }
              </tbody>
            </table>
          </div>
        </div>
      }
    </div>
  `
})
export class AdminDashboardComponent implements OnInit, OnDestroy {
  private clubService = inject(ClubService);
  private destroy$ = new Subject<void>();

  isLoading = signal(true);
  clubs = signal<Club[]>([]);
  stats = signal({
    totalClubs: 0,
    totalMembers: 0,
    activeMembers: 0,
    totalRevenue: 0
  });

  // Chart configurations
  pieChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'right'
      }
    }
  };

  lineChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        display: false
      }
    },
    scales: {
      y: {
        beginAtZero: true
      }
    }
  };

  clubTypeChartData: ChartData<'doughnut'> = {
    labels: ['Cricket', 'Football', 'Tennis', 'Swimming', 'Other'],
    datasets: [{
      data: [3, 2, 2, 1, 1],
      backgroundColor: ['#3B82F6', '#10B981', '#F59E0B', '#8B5CF6', '#6B7280']
    }]
  };

  memberGrowthChartData: ChartData<'line'> = {
    labels: ['Aug', 'Sep', 'Oct', 'Nov', 'Dec', 'Jan'],
    datasets: [{
      data: [65, 78, 90, 105, 120, 135],
      borderColor: '#3B82F6',
      backgroundColor: 'rgba(59, 130, 246, 0.1)',
      fill: true,
      tension: 0.4
    }]
  };

  ngOnInit(): void {
    this.loadDashboard();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadDashboard(): void {
    this.clubService.getClubs({ page: 1, pageSize: 10 }).pipe(takeUntil(this.destroy$)).subscribe({
      next: (result: PagedResult<Club>) => {
        this.clubs.set(result.items);

        // Calculate stats from clubs
        let totalMembers = 0;
        let activeMembers = 0;
        result.items.forEach(club => {
          totalMembers += club.memberCount || 0;
          activeMembers += club.activeMembers || 0;
        });

        this.stats.set({
          totalClubs: result.totalCount,
          totalMembers,
          activeMembers,
          totalRevenue: 45000 // Mock value
        });

        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }
}
