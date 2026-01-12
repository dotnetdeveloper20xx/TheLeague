import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SystemConfigService } from '../../../core/services/system-config.service';
import { SystemConfiguration, UpdateSystemConfigurationRequest, ProviderTestResult } from '../../../core/models/system-config.model';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-email-config',
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
            <h1 class="text-2xl font-bold text-white">Email Settings</h1>
            <p class="text-gray-400 mt-1">Configure email provider and settings</p>
          </div>
        </div>
        <div class="flex gap-3">
          <button (click)="sendTestEmail()" class="btn-secondary" [disabled]="isTesting()">
            {{ isTesting() ? 'Sending...' : 'Send Test Email' }}
          </button>
          <button (click)="save()" class="btn-primary" [disabled]="isSaving()">
            {{ isSaving() ? 'Saving...' : 'Save Changes' }}
          </button>
        </div>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner />
        </div>
      } @else if (config()) {
        <div class="max-w-2xl">
          <!-- Provider Selection -->
          <div class="card mb-6">
            <h2 class="text-lg font-semibold text-white mb-4">Provider</h2>
            <div class="space-y-3">
              <label class="flex items-start p-4 border border-gray-700 rounded-lg cursor-pointer transition-colors"
                     [class.border-primary-500]="selectedProvider() === 'Mock'"
                     [class.bg-primary-50]="selectedProvider() === 'Mock'">
                <input type="radio" name="provider" value="Mock" [(ngModel)]="selectedProvider"
                       class="mt-1 mr-4" (change)="onProviderChange()">
                <div>
                  <span class="text-white font-medium">Mock (Development)</span>
                  <p class="text-gray-400 text-sm mt-1">Emails are logged to console. No actual emails are sent. Perfect for development and testing.</p>
                </div>
              </label>
              <label class="flex items-start p-4 border border-gray-700 rounded-lg cursor-pointer transition-colors"
                     [class.border-primary-500]="selectedProvider() === 'SendGrid'"
                     [class.bg-primary-50]="selectedProvider() === 'SendGrid'">
                <input type="radio" name="provider" value="SendGrid" [(ngModel)]="selectedProvider"
                       class="mt-1 mr-4" (change)="onProviderChange()">
                <div>
                  <span class="text-white font-medium">SendGrid (Production)</span>
                  <p class="text-gray-400 text-sm mt-1">Real email delivery via SendGrid. Requires API key.</p>
                </div>
              </label>
            </div>
          </div>

          <!-- Mock Settings -->
          @if (selectedProvider() === 'Mock') {
            <div class="card mb-6">
              <h2 class="text-lg font-semibold text-white mb-4">Mock Settings</h2>
              <div class="space-y-4">
                <div>
                  <label class="form-label">Processing Delay (ms)</label>
                  <input type="number" [(ngModel)]="mockDelayMs" class="form-input w-full"
                         min="0" max="10000" step="100">
                  <p class="text-gray-500 text-sm mt-1">Simulates email sending time for realistic loading states</p>
                </div>
              </div>
            </div>
          }

          <!-- SendGrid Settings -->
          @if (selectedProvider() === 'SendGrid') {
            <div class="card mb-6">
              <h2 class="text-lg font-semibold text-white mb-4">SendGrid Settings</h2>
              <div class="space-y-4">
                <div>
                  <label class="form-label">API Key</label>
                  <input type="password" [(ngModel)]="sendGridApiKey" class="form-input w-full"
                         [placeholder]="config()!.sendGridConfigured ? '••••••••' : 'SG.xxxxx...'">
                  <p class="text-gray-500 text-sm mt-1">Leave empty to keep existing key</p>
                </div>
              </div>
            </div>
          }

          <!-- Default Sender Settings -->
          <div class="card mb-6">
            <h2 class="text-lg font-semibold text-white mb-4">Default Sender</h2>
            <div class="space-y-4">
              <div>
                <label class="form-label">From Email</label>
                <input type="email" [(ngModel)]="defaultFromEmail" class="form-input w-full"
                       placeholder="noreply@theleague.com">
              </div>
              <div>
                <label class="form-label">From Name</label>
                <input type="text" [(ngModel)]="defaultFromName" class="form-input w-full"
                       placeholder="The League">
              </div>
            </div>
          </div>

          <!-- Warning Banner -->
          @if (providerChanged()) {
            <div class="p-4 bg-amber-500/10 border border-amber-500/30 rounded-lg mb-6">
              <div class="flex items-start">
                <svg class="w-5 h-5 text-amber-400 mt-0.5 mr-3 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                    d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
                </svg>
                <div>
                  <h4 class="text-amber-400 font-medium">Application Restart Required</h4>
                  <p class="text-amber-400/80 text-sm mt-1">
                    Changing the email provider requires an application restart to take effect.
                    Your changes will be saved but won't apply until restart.
                  </p>
                </div>
              </div>
            </div>
          }

          <!-- Test Result -->
          @if (testResult()) {
            <div class="p-4 rounded-lg mb-6 border"
                 [class.bg-emerald-50]="testResult()!.success"
                 [class.border-emerald-500]="testResult()!.success"
                 [class.bg-red-50]="!testResult()!.success"
                 [class.border-red-500]="!testResult()!.success">
              <div class="flex items-center">
                @if (testResult()!.success) {
                  <svg class="w-5 h-5 text-emerald-400 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
                  </svg>
                } @else {
                  <svg class="w-5 h-5 text-red-400 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                  </svg>
                }
                <div>
                  <span [class.text-emerald-400]="testResult()!.success" [class.text-red-400]="!testResult()!.success">
                    {{ testResult()!.message }}
                  </span>
                </div>
              </div>
            </div>
          }

          <!-- Test Email Input -->
          @if (showTestEmailInput()) {
            <div class="card mb-6">
              <h2 class="text-lg font-semibold text-white mb-4">Send Test Email</h2>
              <div class="flex gap-3">
                <input type="email" [(ngModel)]="testEmailAddress" class="form-input flex-1"
                       placeholder="Enter email address">
                <button (click)="confirmSendTestEmail()" class="btn-primary" [disabled]="!testEmailAddress || isTesting()">
                  Send
                </button>
                <button (click)="cancelTestEmail()" class="btn-secondary">
                  Cancel
                </button>
              </div>
            </div>
          }
        </div>
      }
    </div>
  `
})
export class EmailConfigComponent implements OnInit {
  private configService = inject(SystemConfigService);
  private notificationService = inject(NotificationService);

  config = signal<SystemConfiguration | null>(null);
  isLoading = signal(true);
  isSaving = signal(false);
  isTesting = signal(false);
  testResult = signal<ProviderTestResult | null>(null);
  showTestEmailInput = signal(false);

  selectedProvider = signal<string>('Mock');
  originalProvider = '';
  mockDelayMs = 500;
  sendGridApiKey = '';
  defaultFromEmail = '';
  defaultFromName = '';
  testEmailAddress = '';

  ngOnInit() {
    this.loadConfiguration();
  }

  loadConfiguration() {
    this.isLoading.set(true);
    this.configService.getConfiguration().subscribe({
      next: (config) => {
        this.config.set(config);
        this.selectedProvider.set(config.emailProvider);
        this.originalProvider = config.emailProvider;
        this.mockDelayMs = config.mockEmailDelayMs;
        this.defaultFromEmail = config.defaultFromEmail;
        this.defaultFromName = config.defaultFromName;
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Failed to load configuration', error);
        this.isLoading.set(false);
      }
    });
  }

  onProviderChange() {
    this.testResult.set(null);
  }

  providerChanged(): boolean {
    return this.selectedProvider() !== this.originalProvider;
  }

  sendTestEmail() {
    this.showTestEmailInput.set(true);
    this.testEmailAddress = '';
  }

  cancelTestEmail() {
    this.showTestEmailInput.set(false);
    this.testEmailAddress = '';
  }

  confirmSendTestEmail() {
    if (!this.testEmailAddress) return;

    this.isTesting.set(true);
    this.testResult.set(null);
    this.configService.sendTestEmail({ toEmail: this.testEmailAddress }).subscribe({
      next: (result) => {
        this.testResult.set(result);
        this.isTesting.set(false);
        this.showTestEmailInput.set(false);
      },
      error: (error) => {
        this.testResult.set({
          success: false,
          provider: this.selectedProvider(),
          message: error.message || 'Failed to send test email',
          testedAt: new Date().toISOString()
        });
        this.isTesting.set(false);
      }
    });
  }

  save() {
    this.isSaving.set(true);
    const request: UpdateSystemConfigurationRequest = {
      emailProvider: this.selectedProvider(),
      mockEmailDelayMs: this.mockDelayMs,
      sendGridApiKey: this.sendGridApiKey || undefined,
      defaultFromEmail: this.defaultFromEmail,
      defaultFromName: this.defaultFromName
    };

    this.configService.updateConfiguration(request).subscribe({
      next: (config) => {
        this.config.set(config);
        this.originalProvider = config.emailProvider;
        this.isSaving.set(false);
        this.notificationService.success('Email settings saved successfully');
      },
      error: (error) => {
        console.error('Failed to save configuration', error);
        this.isSaving.set(false);
        this.notificationService.error('Failed to save email settings');
      }
    });
  }
}
