import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SystemConfigService } from '../../../core/services/system-config.service';
import { SystemConfiguration, UpdateSystemConfigurationRequest, ProviderTestResult } from '../../../core/models/system-config.model';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-payment-config',
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
            <h1 class="text-2xl font-bold text-white">Payment Settings</h1>
            <p class="text-gray-400 mt-1">Configure payment provider and settings</p>
          </div>
        </div>
        <div class="flex gap-3">
          <button (click)="testConnection()" class="btn-secondary" [disabled]="isTesting()">
            {{ isTesting() ? 'Testing...' : 'Test Connection' }}
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
                  <p class="text-gray-400 text-sm mt-1">Simulates card payments with configurable delays and failure rates. Perfect for development and testing.</p>
                </div>
              </label>
              <label class="flex items-start p-4 border border-gray-700 rounded-lg cursor-pointer transition-colors"
                     [class.border-primary-500]="selectedProvider() === 'Stripe'"
                     [class.bg-primary-50]="selectedProvider() === 'Stripe'">
                <input type="radio" name="provider" value="Stripe" [(ngModel)]="selectedProvider"
                       class="mt-1 mr-4" (change)="onProviderChange()">
                <div>
                  <span class="text-white font-medium">Stripe (Production)</span>
                  <p class="text-gray-400 text-sm mt-1">Real payment processing with Stripe. Requires API keys.</p>
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
                  <p class="text-gray-500 text-sm mt-1">Simulates real payment gateway response time</p>
                </div>
                <div>
                  <label class="form-label">Failure Rate (%)</label>
                  <input type="number" [(ngModel)]="mockFailureRate" class="form-input w-full"
                         min="0" max="100" step="1">
                  <p class="text-gray-500 text-sm mt-1">Percentage of payments that will randomly fail (for testing error handling)</p>
                </div>
              </div>
            </div>
          }

          <!-- Stripe Settings -->
          @if (selectedProvider() === 'Stripe') {
            <div class="card mb-6">
              <h2 class="text-lg font-semibold text-white mb-4">Stripe Settings</h2>
              <div class="space-y-4">
                <div>
                  <label class="form-label">Publishable Key</label>
                  <input type="text" [(ngModel)]="stripePublishableKey" class="form-input w-full"
                         placeholder="pk_live_...">
                </div>
                <div>
                  <label class="form-label">Secret Key</label>
                  <input type="password" [(ngModel)]="stripeSecretKey" class="form-input w-full"
                         [placeholder]="config()!.stripeConfigured ? '••••••••' : 'sk_live_...'">
                  <p class="text-gray-500 text-sm mt-1">Leave empty to keep existing key</p>
                </div>
                <div>
                  <label class="form-label">Webhook Secret</label>
                  <input type="password" [(ngModel)]="stripeWebhookSecret" class="form-input w-full"
                         [placeholder]="config()!.stripeConfigured ? '••••••••' : 'whsec_...'">
                  <p class="text-gray-500 text-sm mt-1">Leave empty to keep existing secret</p>
                </div>
              </div>
            </div>
          }

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
                    Changing the payment provider requires an application restart to take effect.
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
        </div>
      }
    </div>
  `
})
export class PaymentConfigComponent implements OnInit {
  private configService = inject(SystemConfigService);
  private notificationService = inject(NotificationService);
  private router = inject(Router);

  config = signal<SystemConfiguration | null>(null);
  isLoading = signal(true);
  isSaving = signal(false);
  isTesting = signal(false);
  testResult = signal<ProviderTestResult | null>(null);

  selectedProvider = signal<string>('Mock');
  originalProvider = '';
  mockDelayMs = 1500;
  mockFailureRate = 0;
  stripePublishableKey = '';
  stripeSecretKey = '';
  stripeWebhookSecret = '';

  ngOnInit() {
    this.loadConfiguration();
  }

  loadConfiguration() {
    this.isLoading.set(true);
    this.configService.getConfiguration().subscribe({
      next: (config) => {
        this.config.set(config);
        this.selectedProvider.set(config.paymentProvider);
        this.originalProvider = config.paymentProvider;
        this.mockDelayMs = config.mockPaymentDelayMs;
        this.mockFailureRate = config.mockPaymentFailureRate * 100;
        this.stripePublishableKey = config.stripePublishableKey || '';
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

  testConnection() {
    this.isTesting.set(true);
    this.testResult.set(null);
    this.configService.testPaymentProvider().subscribe({
      next: (result) => {
        this.testResult.set(result);
        this.isTesting.set(false);
      },
      error: (error) => {
        this.testResult.set({
          success: false,
          provider: this.selectedProvider(),
          message: error.message || 'Connection test failed',
          testedAt: new Date().toISOString()
        });
        this.isTesting.set(false);
      }
    });
  }

  save() {
    this.isSaving.set(true);
    const request: UpdateSystemConfigurationRequest = {
      paymentProvider: this.selectedProvider(),
      mockPaymentDelayMs: this.mockDelayMs,
      mockPaymentFailureRate: this.mockFailureRate / 100,
      stripePublishableKey: this.stripePublishableKey || undefined,
      stripeSecretKey: this.stripeSecretKey || undefined,
      stripeWebhookSecret: this.stripeWebhookSecret || undefined
    };

    this.configService.updateConfiguration(request).subscribe({
      next: (config) => {
        this.config.set(config);
        this.originalProvider = config.paymentProvider;
        this.isSaving.set(false);
        this.notificationService.success('Payment settings saved successfully');
      },
      error: (error) => {
        console.error('Failed to save configuration', error);
        this.isSaving.set(false);
        this.notificationService.error('Failed to save payment settings');
      }
    });
  }
}
