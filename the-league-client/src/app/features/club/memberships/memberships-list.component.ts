import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-memberships-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="space-y-6">
      <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 class="text-2xl font-bold text-gray-900">Memberships</h1>
          <p class="text-gray-500 mt-1">View and manage member subscriptions</p>
        </div>
        <a routerLink="/club/memberships/types" class="btn-secondary">
          Manage Types
        </a>
      </div>

      <div class="card">
        <p class="text-gray-500 text-center py-8">Membership management coming soon...</p>
      </div>
    </div>
  `
})
export class MembershipsListComponent {}
