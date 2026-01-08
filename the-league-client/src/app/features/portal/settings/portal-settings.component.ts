import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-portal-settings',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="space-y-6">
      <h1 class="text-2xl font-bold text-gray-900">Settings</h1>

      <div class="card max-w-2xl">
        <h3 class="text-lg font-medium text-gray-900 mb-4">Notification Preferences</h3>
        <div class="space-y-4">
          <label class="flex items-center justify-between p-4 bg-gray-50 rounded-lg">
            <div>
              <p class="font-medium text-gray-900">Email Notifications</p>
              <p class="text-sm text-gray-500">Receive updates about sessions and events</p>
            </div>
            <input type="checkbox" checked class="rounded border-gray-300 text-primary-600 focus:ring-primary-500" />
          </label>
          <label class="flex items-center justify-between p-4 bg-gray-50 rounded-lg">
            <div>
              <p class="font-medium text-gray-900">Payment Reminders</p>
              <p class="text-sm text-gray-500">Get notified about upcoming payments</p>
            </div>
            <input type="checkbox" checked class="rounded border-gray-300 text-primary-600 focus:ring-primary-500" />
          </label>
          <label class="flex items-center justify-between p-4 bg-gray-50 rounded-lg">
            <div>
              <p class="font-medium text-gray-900">Session Reminders</p>
              <p class="text-sm text-gray-500">Receive reminders before booked sessions</p>
            </div>
            <input type="checkbox" checked class="rounded border-gray-300 text-primary-600 focus:ring-primary-500" />
          </label>
        </div>
      </div>

      <div class="card max-w-2xl">
        <h3 class="text-lg font-medium text-gray-900 mb-4">Account</h3>
        <button class="btn-secondary">Change Password</button>
      </div>
    </div>
  `
})
export class PortalSettingsComponent {}
