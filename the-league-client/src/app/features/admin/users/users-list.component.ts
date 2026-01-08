import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-users-list',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="space-y-6">
      <div>
        <h1 class="text-2xl font-bold text-gray-900">System Users</h1>
        <p class="text-gray-500 mt-1">Manage platform administrators</p>
      </div>

      <div class="card">
        <p class="text-gray-500 text-center py-8">User management coming soon...</p>
      </div>
    </div>
  `
})
export class UsersListComponent {}
