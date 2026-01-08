import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData } from 'chart.js';
import { ReportService, MembershipReport, FinancialReport } from '../../../core/services/report.service';
import { LoadingSpinnerComponent } from '../../../shared/components';
import { CurrencyFormatPipe } from '../../../shared/pipes';

@Component({
  selector: 'app-club-reports',
  standalone: true,
  imports: [CommonModule, BaseChartDirective, LoadingSpinnerComponent, CurrencyFormatPipe],
  template: `
    <div class="space-y-6">
      <div>
        <h1 class="text-2xl font-bold text-gray-900">Reports</h1>
        <p class="text-gray-500 mt-1">Club analytics and insights</p>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg"></app-loading-spinner>
        </div>
      } @else {
        <!-- Membership Stats -->
        <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
          <div class="card">
            <p class="text-sm text-gray-500">Total Members</p>
            <p class="text-2xl font-bold text-gray-900">{{ membershipReport()?.totalMembers || 0 }}</p>
          </div>
          <div class="card">
            <p class="text-sm text-gray-500">Active Members</p>
            <p class="text-2xl font-bold text-green-600">{{ membershipReport()?.activeMembers || 0 }}</p>
          </div>
          <div class="card">
            <p class="text-sm text-gray-500">New This Month</p>
            <p class="text-2xl font-bold text-blue-600">{{ membershipReport()?.newMembersThisMonth || 0 }}</p>
          </div>
          <div class="card">
            <p class="text-sm text-gray-500">Churn Rate</p>
            <p class="text-2xl font-bold text-orange-600">{{ (membershipReport()?.churnRate || 0) | number:'1.1-1' }}%</p>
          </div>
        </div>

        <!-- Financial Stats -->
        <div class="grid grid-cols-1 sm:grid-cols-3 gap-4">
          <div class="card bg-green-50">
            <p class="text-sm text-green-600">Total Revenue</p>
            <p class="text-2xl font-bold text-green-700">{{ financialReport()?.totalRevenue || 0 | currencyFormat }}</p>
          </div>
          <div class="card bg-blue-50">
            <p class="text-sm text-blue-600">This Month</p>
            <p class="text-2xl font-bold text-blue-700">{{ financialReport()?.revenueThisMonth || 0 | currencyFormat }}</p>
          </div>
          <div class="card bg-yellow-50">
            <p class="text-sm text-yellow-600">Outstanding</p>
            <p class="text-2xl font-bold text-yellow-700">{{ financialReport()?.outstandingPayments || 0 | currencyFormat }}</p>
          </div>
        </div>

        <!-- Charts -->
        <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
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

          <div class="card">
            <h3 class="card-header">Monthly Revenue</h3>
            <div class="h-64">
              <canvas baseChart
                [data]="revenueChartData"
                [options]="barChartOptions"
                type="bar">
              </canvas>
            </div>
          </div>

          <div class="card">
            <h3 class="card-header">Revenue by Type</h3>
            <div class="h-64">
              <canvas baseChart
                [data]="revenueByTypeChartData"
                [options]="pieChartOptions"
                type="pie">
              </canvas>
            </div>
          </div>
        </div>
      }
    </div>
  `
})
export class ClubReportsComponent implements OnInit {
  private reportService = inject(ReportService);

  isLoading = signal(true);
  membershipReport = signal<MembershipReport | null>(null);
  financialReport = signal<FinancialReport | null>(null);

  lineChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: { legend: { display: false } }
  };

  pieChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: { legend: { position: 'right' } }
  };

  barChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: { legend: { display: false } }
  };

  memberGrowthChartData: ChartData<'line'> = {
    labels: ['Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
    datasets: [{
      data: [30, 35, 40, 42, 45, 48],
      borderColor: '#3B82F6',
      backgroundColor: 'rgba(59, 130, 246, 0.1)',
      fill: true,
      tension: 0.4
    }]
  };

  membersByTypeChartData: ChartData<'doughnut'> = {
    labels: ['Adult', 'Junior', 'Family', 'Senior'],
    datasets: [{
      data: [25, 15, 10, 8],
      backgroundColor: ['#3B82F6', '#10B981', '#F59E0B', '#8B5CF6']
    }]
  };

  revenueChartData: ChartData<'bar'> = {
    labels: ['Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
    datasets: [{
      data: [2500, 3200, 2800, 3500, 4000, 3800],
      backgroundColor: '#10B981'
    }]
  };

  revenueByTypeChartData: ChartData<'pie'> = {
    labels: ['Memberships', 'Events', 'Sessions', 'Other'],
    datasets: [{
      data: [60, 20, 15, 5],
      backgroundColor: ['#3B82F6', '#10B981', '#F59E0B', '#8B5CF6']
    }]
  };

  ngOnInit(): void {
    this.loadReports();
  }

  private loadReports(): void {
    this.reportService.getMembershipReport().subscribe({
      next: (report) => {
        this.membershipReport.set(report);
        if (report.memberGrowth?.length) {
          this.memberGrowthChartData = {
            labels: report.memberGrowth.map(m => m.month),
            datasets: [{
              data: report.memberGrowth.map(m => m.value),
              borderColor: '#3B82F6',
              backgroundColor: 'rgba(59, 130, 246, 0.1)',
              fill: true,
              tension: 0.4
            }]
          };
        }
        if (report.membersByType?.length) {
          this.membersByTypeChartData = {
            labels: report.membersByType.map(m => m.label),
            datasets: [{
              data: report.membersByType.map(m => m.value),
              backgroundColor: ['#3B82F6', '#10B981', '#F59E0B', '#8B5CF6', '#EC4899']
            }]
          };
        }
      }
    });

    this.reportService.getFinancialReport().subscribe({
      next: (report) => {
        this.financialReport.set(report);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }
}
