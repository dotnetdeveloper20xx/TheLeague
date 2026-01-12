import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { SystemConfigService } from '../../../core/services/system-config.service';
import { SystemConfiguration } from '../../../core/models/system-config.model';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-system-config-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, LoadingSpinnerComponent],
  template: `
    <div class="p-6">
      <div class="mb-6">
        <h1 class="text-2xl font-bold text-white">System Configuration</h1>
        <p class="text-gray-400 mt-1">Manage global platform settings, providers, and features</p>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner />
        </div>
      } @else if (config()) {
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <!-- Payment Provider Card -->
          <div class="card hover:border-primary-500/50 transition-colors">
            <div class="flex items-start justify-between mb-4">
              <div class="p-3 bg-primary-500/20 rounded-lg">
                <svg class="w-6 h-6 text-primary-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                    d="M3 10h18M7 15h1m4 0h1m-7 4h12a3 3 0 003-3V8a3 3 0 00-3-3H6a3 3 0 00-3 3v8a3 3 0 003 3z" />
                </svg>
              </div>
              <span class="badge" [class.badge-success]="config()!.paymentProvider === 'Mock'"
                    [class.badge-primary]="config()!.paymentProvider !== 'Mock'">
                {{ config()!.paymentProvider }}
              </span>
            </div>
            <h3 class="text-lg font-semibold text-white mb-1">Payment Provider</h3>
            <p class="text-gray-400 text-sm mb-4">
              {{ config()!.paymentProvider === 'Mock' ? 'Development mode with simulated payments' : 'Live payment processing' }}
            </p>
            <a routerLink="payment" class="btn-secondary w-full text-center">Configure</a>
          </div>

          <!-- Email Provider Card -->
          <div class="card hover:border-primary-500/50 transition-colors">
            <div class="flex items-start justify-between mb-4">
              <div class="p-3 bg-green-500/20 rounded-lg">
                <svg class="w-6 h-6 text-green-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                    d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
                </svg>
              </div>
              <span class="badge" [class.badge-success]="config()!.emailProvider === 'Mock'"
                    [class.badge-primary]="config()!.emailProvider !== 'Mock'">
                {{ config()!.emailProvider }}
              </span>
            </div>
            <h3 class="text-lg font-semibold text-white mb-1">Email Provider</h3>
            <p class="text-gray-400 text-sm mb-4">
              {{ config()!.emailProvider === 'Mock' ? 'Emails logged to console' : 'Live email delivery' }}
            </p>
            <a routerLink="email" class="btn-secondary w-full text-center">Configure</a>
          </div>

          <!-- Feature Flags Card -->
          <div class="card hover:border-primary-500/50 transition-colors">
            <div class="flex items-start justify-between mb-4">
              <div class="p-3 bg-yellow-500/20 rounded-lg">
                <svg class="w-6 h-6 text-yellow-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                    d="M3 21v-4m0 0V5a2 2 0 012-2h6.5l1 1H21l-3 6 3 6h-8.5l-1-1H5a2 2 0 00-2 2zm9-13.5V9" />
                </svg>
              </div>
              <span class="badge badge-gray">{{ getEnabledFlagsCount() }} enabled</span>
            </div>
            <h3 class="text-lg font-semibold text-white mb-1">Feature Flags</h3>
            <p class="text-gray-400 text-sm mb-4">
              Control platform features and maintenance mode
            </p>
            <a routerLink="features" class="btn-secondary w-full text-center">Configure</a>
          </div>

          <!-- Appearance Card -->
          <div class="card hover:border-primary-500/50 transition-colors">
            <div class="flex items-start justify-between mb-4">
              <div class="p-3 bg-purple-500/20 rounded-lg">
                <svg class="w-6 h-6 text-purple-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                    d="M7 21a4 4 0 01-4-4V5a2 2 0 012-2h4a2 2 0 012 2v12a4 4 0 01-4 4zm0 0h12a2 2 0 002-2v-4a2 2 0 00-2-2h-2.343M11 7.343l1.657-1.657a2 2 0 012.828 0l2.829 2.829a2 2 0 010 2.828l-8.486 8.485M7 17h.01" />
                </svg>
              </div>
              <div class="w-6 h-6 rounded" [style.backgroundColor]="config()!.primaryColor"></div>
            </div>
            <h3 class="text-lg font-semibold text-white mb-1">Appearance</h3>
            <p class="text-gray-400 text-sm mb-4">
              {{ config()!.platformName }} - Branding & colors
            </p>
            <a routerLink="appearance" class="btn-secondary w-full text-center">Configure</a>
          </div>

          <!-- Audit Log Card -->
          <div class="card hover:border-primary-500/50 transition-colors">
            <div class="flex items-start justify-between mb-4">
              <div class="p-3 bg-blue-500/20 rounded-lg">
                <svg class="w-6 h-6 text-blue-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                    d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-3 7h3m-3 4h3m-6-4h.01M9 16h.01" />
                </svg>
              </div>
              <span class="badge badge-gray">v{{ config()!.version }}</span>
            </div>
            <h3 class="text-lg font-semibold text-white mb-1">Audit Log</h3>
            <p class="text-gray-400 text-sm mb-4">
              Last modified by {{ config()!.lastModifiedBy }}
            </p>
            <a routerLink="audit" class="btn-secondary w-full text-center">View Log</a>
          </div>

          <!-- System Health Card -->
          <div class="card hover:border-primary-500/50 transition-colors">
            <div class="flex items-start justify-between mb-4">
              <div class="p-3 bg-emerald-500/20 rounded-lg">
                <svg class="w-6 h-6 text-emerald-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                    d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" />
                </svg>
              </div>
              <span class="flex items-center text-emerald-400 text-sm">
                <span class="w-2 h-2 bg-emerald-400 rounded-full mr-2 animate-pulse"></span>
                Healthy
              </span>
            </div>
            <h3 class="text-lg font-semibold text-white mb-1">System Health</h3>
            <p class="text-gray-400 text-sm mb-4">
              All systems operational
            </p>
            <button (click)="testProviders()" class="btn-secondary w-full" [disabled]="isTesting()">
              {{ isTesting() ? 'Testing...' : 'Test Providers' }}
            </button>
          </div>
        </div>

        <!-- Info Banner -->
        <div class="mt-8 p-4 bg-amber-500/10 border border-amber-500/30 rounded-lg">
          <div class="flex items-start">
            <svg class="w-5 h-5 text-amber-400 mt-0.5 mr-3 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
            </svg>
            <div>
              <h4 class="text-amber-400 font-medium">Provider Changes Require Restart</h4>
              <p class="text-amber-400/80 text-sm mt-1">
                Changing the payment or email provider requires an application restart to take effect.
                Feature flags and appearance settings are applied immediately.
              </p>
            </div>
          </div>
        </div>
      }
    </div>
  `
})
export class SystemConfigDashboardComponent implements OnInit {
  private configService = inject(SystemConfigService);

  config = signal<SystemConfiguration | null>(null);
  isLoading = signal(true);
  isTesting = signal(false);

  ngOnInit() {
    this.loadConfiguration();
  }

  loadConfiguration() {
    this.isLoading.set(true);
    this.configService.getConfiguration().subscribe({
      next: (config) => {
        this.config.set(config);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Failed to load configuration', error);
        this.isLoading.set(false);
      }
    });
  }

  getEnabledFlagsCount(): number {
    const c = this.config();
    if (!c) return 0;
    let count = 0;
    if (c.allowNewRegistrations) count++;
    if (c.enableEmailNotifications) count++;
    if (!c.maintenanceMode) count++; // "not in maintenance" counts as enabled
    return count;
  }

  testProviders() {
    this.isTesting.set(true);
    Promise.all([
      this.configService.testPaymentProvider().toPromise(),
      this.configService.testEmailProvider().toPromise()
    ]).then(([paymentResult, emailResult]) => {
      console.log('Payment test:', paymentResult);
      console.log('Email test:', emailResult);
      this.isTesting.set(false);
      // Could show a notification here
    }).catch(error => {
      console.error('Provider test failed', error);
      this.isTesting.set(false);
    });
  }
}
