import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SystemConfigService } from '../../../core/services/system-config.service';
import { SystemConfiguration, UpdateSystemConfigurationRequest } from '../../../core/models/system-config.model';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-feature-flags',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, LoadingSpinnerComponent],
  template: `
    <div class="p-6">
      <!-- Header -->
      <div class="flex items-center justify-between mb-6">
        <div class="flex items-center">
          <a routerLink="../" class="text-gray-400 hover:text-white mr-4">
            <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" />
            </svg>
          </a>
          <div>
            <h1 class="text-2xl font-bold text-white">Feature Flags</h1>
            <p class="text-gray-400 mt-1">Control platform features and access</p>
          </div>
        </div>
        <button (click)="save()" class="btn-primary" [disabled]="isSaving()">
          {{ isSaving() ? 'Saving...' : 'Save Changes' }}
        </button>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner />
        </div>
      } @else if (config()) {
        <div class="max-w-2xl space-y-6">
          <!-- Maintenance Mode -->
          <div class="card">
            <div class="flex items-start justify-between">
              <div class="flex-1">
                <div class="flex items-center">
                  <h3 class="text-lg font-semibold text-white">Maintenance Mode</h3>
                  @if (maintenanceMode) {
                    <span class="ml-3 badge badge-error">Active</span>
                  }
                </div>
                <p class="text-gray-400 text-sm mt-1">
                  When enabled, only administrators can access the platform. Users will see a maintenance message.
                </p>
              </div>
              <label class="relative inline-flex items-center cursor-pointer ml-4">
                <input type="checkbox" [(ngModel)]="maintenanceMode" class="sr-only peer">
                <div class="w-11 h-6 bg-gray-700 peer-focus:outline-none peer-focus:ring-2 peer-focus:ring-primary-500/50 rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-primary-600"></div>
              </label>
            </div>
            @if (maintenanceMode) {
              <div class="mt-4 pt-4 border-t border-gray-700">
                <label class="form-label">Maintenance Message</label>
                <textarea [(ngModel)]="maintenanceMessage" class="form-input w-full" rows="3"
                          placeholder="We're currently performing scheduled maintenance. Please check back later."></textarea>
              </div>
            }
          </div>

          <!-- Allow New Registrations -->
          <div class="card">
            <div class="flex items-start justify-between">
              <div class="flex-1">
                <div class="flex items-center">
                  <h3 class="text-lg font-semibold text-white">Allow New Registrations</h3>
                  @if (allowNewRegistrations) {
                    <span class="ml-3 badge badge-success">Enabled</span>
                  } @else {
                    <span class="ml-3 badge badge-gray">Disabled</span>
                  }
                </div>
                <p class="text-gray-400 text-sm mt-1">
                  When disabled, new users cannot create accounts. Existing users can still log in.
                </p>
              </div>
              <label class="relative inline-flex items-center cursor-pointer ml-4">
                <input type="checkbox" [(ngModel)]="allowNewRegistrations" class="sr-only peer">
                <div class="w-11 h-6 bg-gray-700 peer-focus:outline-none peer-focus:ring-2 peer-focus:ring-primary-500/50 rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-primary-600"></div>
              </label>
            </div>
          </div>

          <!-- Email Notifications -->
          <div class="card">
            <div class="flex items-start justify-between">
              <div class="flex-1">
                <div class="flex items-center">
                  <h3 class="text-lg font-semibold text-white">Email Notifications</h3>
                  @if (enableEmailNotifications) {
                    <span class="ml-3 badge badge-success">Enabled</span>
                  } @else {
                    <span class="ml-3 badge badge-gray">Disabled</span>
                  }
                </div>
                <p class="text-gray-400 text-sm mt-1">
                  When disabled, the system will not send any automated emails (welcome emails, payment receipts, etc).
                </p>
              </div>
              <label class="relative inline-flex items-center cursor-pointer ml-4">
                <input type="checkbox" [(ngModel)]="enableEmailNotifications" class="sr-only peer">
                <div class="w-11 h-6 bg-gray-700 peer-focus:outline-none peer-focus:ring-2 peer-focus:ring-primary-500/50 rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-primary-600"></div>
              </label>
            </div>
          </div>

          <!-- Info Banner -->
          <div class="p-4 bg-blue-500/10 border border-blue-500/30 rounded-lg">
            <div class="flex items-start">
              <svg class="w-5 h-5 text-blue-400 mt-0.5 mr-3 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                  d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
              <div>
                <h4 class="text-blue-400 font-medium">Instant Effect</h4>
                <p class="text-blue-400/80 text-sm mt-1">
                  Feature flag changes take effect immediately after saving. No application restart is required.
                </p>
              </div>
            </div>
          </div>
        </div>
      }
    </div>
  `
})
export class FeatureFlagsComponent implements OnInit {
  private configService = inject(SystemConfigService);
  private notificationService = inject(NotificationService);

  config = signal<SystemConfiguration | null>(null);
  isLoading = signal(true);
  isSaving = signal(false);

  maintenanceMode = false;
  maintenanceMessage = '';
  allowNewRegistrations = true;
  enableEmailNotifications = true;

  ngOnInit() {
    this.loadConfiguration();
  }

  loadConfiguration() {
    this.isLoading.set(true);
    this.configService.getConfiguration().subscribe({
      next: (config) => {
        this.config.set(config);
        this.maintenanceMode = config.maintenanceMode;
        this.maintenanceMessage = config.maintenanceMessage || '';
        this.allowNewRegistrations = config.allowNewRegistrations;
        this.enableEmailNotifications = config.enableEmailNotifications;
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Failed to load configuration', error);
        this.isLoading.set(false);
      }
    });
  }

  save() {
    this.isSaving.set(true);
    const request: UpdateSystemConfigurationRequest = {
      maintenanceMode: this.maintenanceMode,
      maintenanceMessage: this.maintenanceMessage || undefined,
      allowNewRegistrations: this.allowNewRegistrations,
      enableEmailNotifications: this.enableEmailNotifications
    };

    this.configService.updateConfiguration(request).subscribe({
      next: (config) => {
        this.config.set(config);
        this.isSaving.set(false);
        this.notificationService.success('Feature flags saved successfully');
      },
      error: (error) => {
        console.error('Failed to save configuration', error);
        this.isSaving.set(false);
        this.notificationService.error('Failed to save feature flags');
      }
    });
  }
}
