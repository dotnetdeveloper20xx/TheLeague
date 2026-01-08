import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin-settings',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="space-y-6">
      <div>
        <h1 class="text-2xl font-bold text-gray-900">Platform Settings</h1>
        <p class="text-gray-500 mt-1">Configure global platform settings</p>
      </div>

      <div class="card max-w-2xl">
        <h3 class="card-header">General Settings</h3>
        <p class="text-gray-500 text-center py-8">Platform settings configuration coming soon...</p>
      </div>
    </div>
  `
})
export class AdminSettingsComponent {}
