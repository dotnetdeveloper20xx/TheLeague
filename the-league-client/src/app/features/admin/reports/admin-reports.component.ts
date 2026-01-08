import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData } from 'chart.js';

@Component({
  selector: 'app-admin-reports',
  standalone: true,
  imports: [CommonModule, BaseChartDirective],
  template: `
    <div class="space-y-6">
      <div>
        <h1 class="text-2xl font-bold text-gray-900">Platform Reports</h1>
        <p class="text-gray-500 mt-1">Overview of platform-wide analytics</p>
      </div>

      <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <!-- Revenue by Club -->
        <div class="card">
          <h3 class="card-header">Revenue by Club</h3>
          <div class="h-64">
            <canvas baseChart
              [data]="revenueByClubData"
              [options]="barChartOptions"
              type="bar">
            </canvas>
          </div>
        </div>

        <!-- Members by Club -->
        <div class="card">
          <h3 class="card-header">Members by Club</h3>
          <div class="h-64">
            <canvas baseChart
              [data]="membersByClubData"
              [options]="barChartOptions"
              type="bar">
            </canvas>
          </div>
        </div>

        <!-- Platform Growth -->
        <div class="card lg:col-span-2">
          <h3 class="card-header">Platform Growth</h3>
          <div class="h-64">
            <canvas baseChart
              [data]="platformGrowthData"
              [options]="lineChartOptions"
              type="line">
            </canvas>
          </div>
        </div>
      </div>
    </div>
  `
})
export class AdminReportsComponent {
  barChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { display: false }
    }
  };

  lineChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { position: 'top' }
    }
  };

  revenueByClubData: ChartData<'bar'> = {
    labels: ['Riverside Tennis', 'Downtown Basketball', 'Lakeside Swimming'],
    datasets: [{
      data: [15000, 12000, 18000],
      backgroundColor: '#3B82F6'
    }]
  };

  membersByClubData: ChartData<'bar'> = {
    labels: ['Riverside Tennis', 'Downtown Basketball', 'Lakeside Swimming'],
    datasets: [{
      data: [45, 38, 52],
      backgroundColor: '#10B981'
    }]
  };

  platformGrowthData: ChartData<'line'> = {
    labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
    datasets: [
      {
        label: 'Clubs',
        data: [1, 1, 2, 2, 3, 3],
        borderColor: '#3B82F6',
        tension: 0.4
      },
      {
        label: 'Members',
        data: [20, 35, 55, 80, 110, 135],
        borderColor: '#10B981',
        tension: 0.4
      }
    ]
  };
}
