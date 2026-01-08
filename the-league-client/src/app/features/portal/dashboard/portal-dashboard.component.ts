import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PortalService, PortalDashboard } from '../../../core/services/portal.service';
import { LoadingSpinnerComponent, StatusBadgeComponent } from '../../../shared/components';
import { DateFormatPipe, CurrencyFormatPipe } from '../../../shared/pipes';

@Component({
  selector: 'app-portal-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, LoadingSpinnerComponent, StatusBadgeComponent, DateFormatPipe, CurrencyFormatPipe],
  template: `
    <div class="space-y-6">
      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg" message="Loading your dashboard..."></app-loading-spinner>
        </div>
      } @else if (dashboard()) {
        <!-- Welcome -->
        <div class="bg-gradient-to-r from-primary-600 to-primary-700 rounded-2xl p-6 text-white">
          <h1 class="text-2xl font-bold">Welcome back, {{ dashboard()!.member.firstName }}!</h1>
          <p class="text-primary-100 mt-1">Here's what's happening with your membership.</p>
        </div>

        <!-- Membership Card -->
        @if (dashboard()!.membership) {
          <div class="card bg-gradient-to-br from-gray-900 to-gray-800 text-white">
            <div class="flex justify-between items-start">
              <div>
                <p class="text-gray-400 text-sm">Membership</p>
                <p class="text-xl font-bold mt-1">{{ dashboard()!.membership!.membershipType }}</p>
              </div>
              <app-status-badge [status]="dashboard()!.membership!.status" type="membership"></app-status-badge>
            </div>
            <div class="mt-6 flex gap-8">
              <div>
                <p class="text-gray-400 text-sm">Valid Until</p>
                <p class="font-medium">{{ dashboard()!.membership!.endDate | dateFormat }}</p>
              </div>
              @if (dashboard()!.membership!.amountDue > 0) {
                <div>
                  <p class="text-gray-400 text-sm">Amount Due</p>
                  <p class="font-medium text-yellow-400">{{ dashboard()!.membership!.amountDue | currencyFormat }}</p>
                </div>
              }
            </div>
            @if (dashboard()!.membership!.amountDue > 0) {
              <button class="mt-4 btn-primary w-full">Pay Now</button>
            }
          </div>
        } @else {
          <div class="card border-2 border-dashed border-gray-300 text-center py-8">
            <p class="text-gray-500 mb-4">You don't have an active membership</p>
            <button class="btn-primary">View Membership Options</button>
          </div>
        }

        <!-- Quick Stats -->
        <div class="grid grid-cols-2 lg:grid-cols-4 gap-4">
          <a routerLink="/portal/sessions" class="card hover:shadow-lg transition-shadow text-center">
            <p class="text-3xl font-bold text-primary-600">{{ dashboard()!.upcomingSessions?.length || 0 }}</p>
            <p class="text-gray-500 text-sm">Upcoming Sessions</p>
          </a>
          <a routerLink="/portal/events" class="card hover:shadow-lg transition-shadow text-center">
            <p class="text-3xl font-bold text-purple-600">{{ dashboard()!.upcomingEvents?.length || 0 }}</p>
            <p class="text-gray-500 text-sm">Upcoming Events</p>
          </a>
          <a routerLink="/portal/family" class="card hover:shadow-lg transition-shadow text-center">
            <p class="text-3xl font-bold text-green-600">{{ dashboard()!.member.familyMembers?.length || 0 }}</p>
            <p class="text-gray-500 text-sm">Family Members</p>
          </a>
          <a routerLink="/portal/payments" class="card hover:shadow-lg transition-shadow text-center">
            <p class="text-3xl font-bold text-blue-600">{{ dashboard()!.recentPayments?.length || 0 }}</p>
            <p class="text-gray-500 text-sm">Recent Payments</p>
          </a>
        </div>

        <!-- Upcoming Sessions -->
        <div class="card">
          <div class="flex items-center justify-between mb-4">
            <h2 class="text-lg font-semibold text-gray-900">Upcoming Sessions</h2>
            <a routerLink="/portal/sessions" class="text-primary-600 hover:text-primary-700 text-sm font-medium">View All</a>
          </div>
          @if (dashboard()!.upcomingSessions?.length) {
            <div class="space-y-3">
              @for (session of dashboard()!.upcomingSessions!.slice(0, 3); track session.id) {
                <div class="flex items-center justify-between p-4 bg-gray-50 rounded-lg">
                  <div>
                    <p class="font-medium text-gray-900">{{ session.title }}</p>
                    <p class="text-sm text-gray-500">{{ session.startTime | dateFormat:'datetime' }}</p>
                  </div>
                  <span class="badge badge-info">{{ session.category }}</span>
                </div>
              }
            </div>
          } @else {
            <p class="text-gray-500 text-center py-4">No upcoming sessions</p>
          }
        </div>

        <!-- Upcoming Events -->
        <div class="card">
          <div class="flex items-center justify-between mb-4">
            <h2 class="text-lg font-semibold text-gray-900">Upcoming Events</h2>
            <a routerLink="/portal/events" class="text-primary-600 hover:text-primary-700 text-sm font-medium">View All</a>
          </div>
          @if (dashboard()!.upcomingEvents?.length) {
            <div class="space-y-3">
              @for (event of dashboard()!.upcomingEvents!.slice(0, 3); track event.id) {
                <div class="flex items-center justify-between p-4 bg-gray-50 rounded-lg">
                  <div>
                    <p class="font-medium text-gray-900">{{ event.title }}</p>
                    <p class="text-sm text-gray-500">{{ event.startDateTime | dateFormat:'datetime' }}</p>
                  </div>
                  <span class="badge badge-info">{{ event.type }}</span>
                </div>
              }
            </div>
          } @else {
            <p class="text-gray-500 text-center py-4">No upcoming events</p>
          }
        </div>
      }
    </div>
  `
})
export class PortalDashboardComponent implements OnInit {
  private portalService = inject(PortalService);

  isLoading = signal(true);
  dashboard = signal<PortalDashboard | null>(null);

  ngOnInit(): void {
    this.portalService.getDashboard().subscribe({
      next: (data) => {
        this.dashboard.set(data);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }
}
