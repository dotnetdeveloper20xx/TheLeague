import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SystemConfigService } from '../../../core/services/system-config.service';
import { SystemConfiguration, UpdateSystemConfigurationRequest } from '../../../core/models/system-config.model';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-appearance-config',
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
            <h1 class="text-2xl font-bold text-white">Appearance</h1>
            <p class="text-gray-400 mt-1">Customize platform branding and appearance</p>
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
          <!-- Platform Name -->
          <div class="card">
            <h2 class="text-lg font-semibold text-white mb-4">Platform Identity</h2>
            <div class="space-y-4">
              <div>
                <label class="form-label">Platform Name</label>
                <input type="text" [(ngModel)]="platformName" class="form-input w-full"
                       placeholder="The League">
                <p class="text-gray-500 text-sm mt-1">This name appears in the header, emails, and throughout the platform</p>
              </div>
              <div>
                <label class="form-label">Logo URL</label>
                <input type="url" [(ngModel)]="logoUrl" class="form-input w-full"
                       placeholder="https://example.com/logo.png">
                <p class="text-gray-500 text-sm mt-1">URL to your logo image (recommended size: 200x50px)</p>
              </div>
              @if (logoUrl) {
                <div class="mt-4 p-4 bg-gray-800 rounded-lg">
                  <p class="text-gray-400 text-sm mb-2">Preview:</p>
                  <img [src]="logoUrl" alt="Logo preview" class="max-h-12"
                       (error)="onLogoError($event)">
                </div>
              }
            </div>
          </div>

          <!-- Primary Color -->
          <div class="card">
            <h2 class="text-lg font-semibold text-white mb-4">Brand Color</h2>
            <div class="space-y-4">
              <div>
                <label class="form-label">Primary Color</label>
                <div class="flex items-center gap-4">
                  <input type="color" [(ngModel)]="primaryColor"
                         class="w-16 h-10 rounded border border-gray-600 cursor-pointer">
                  <input type="text" [(ngModel)]="primaryColor" class="form-input w-32"
                         placeholder="#6366f1" pattern="^#[0-9A-Fa-f]{6}$">
                </div>
                <p class="text-gray-500 text-sm mt-1">Used for buttons, links, and accent elements</p>
              </div>

              <!-- Color Preview -->
              <div class="mt-4 p-4 bg-gray-800 rounded-lg">
                <p class="text-gray-400 text-sm mb-3">Preview:</p>
                <div class="flex gap-3">
                  <button class="px-4 py-2 rounded-lg text-white font-medium"
                          [style.backgroundColor]="primaryColor">
                    Primary Button
                  </button>
                  <button class="px-4 py-2 rounded-lg font-medium border-2"
                          [style.borderColor]="primaryColor"
                          [style.color]="primaryColor">
                    Secondary Button
                  </button>
                  <span class="px-3 py-1 rounded-full text-sm"
                        [style.backgroundColor]="primaryColor + '20'"
                        [style.color]="primaryColor">
                    Badge
                  </span>
                </div>
              </div>
            </div>
          </div>

          <!-- Preset Colors -->
          <div class="card">
            <h2 class="text-lg font-semibold text-white mb-4">Quick Presets</h2>
            <div class="flex flex-wrap gap-3">
              @for (preset of colorPresets; track preset.name) {
                <button (click)="selectPreset(preset.color)"
                        class="flex items-center gap-2 px-3 py-2 rounded-lg border border-gray-700 hover:border-gray-600 transition-colors"
                        [class.border-primary-500]="primaryColor === preset.color">
                  <span class="w-4 h-4 rounded" [style.backgroundColor]="preset.color"></span>
                  <span class="text-gray-300 text-sm">{{ preset.name }}</span>
                </button>
              }
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
                  Appearance changes take effect immediately after saving. Users may need to refresh their browser to see the changes.
                </p>
              </div>
            </div>
          </div>
        </div>
      }
    </div>
  `
})
export class AppearanceConfigComponent implements OnInit {
  private configService = inject(SystemConfigService);
  private notificationService = inject(NotificationService);

  config = signal<SystemConfiguration | null>(null);
  isLoading = signal(true);
  isSaving = signal(false);

  platformName = '';
  primaryColor = '#6366f1';
  logoUrl = '';

  colorPresets = [
    { name: 'Indigo', color: '#6366f1' },
    { name: 'Blue', color: '#3b82f6' },
    { name: 'Emerald', color: '#10b981' },
    { name: 'Rose', color: '#f43f5e' },
    { name: 'Amber', color: '#f59e0b' },
    { name: 'Purple', color: '#8b5cf6' },
    { name: 'Cyan', color: '#06b6d4' },
    { name: 'Pink', color: '#ec4899' }
  ];

  ngOnInit() {
    this.loadConfiguration();
  }

  loadConfiguration() {
    this.isLoading.set(true);
    this.configService.getConfiguration().subscribe({
      next: (config) => {
        this.config.set(config);
        this.platformName = config.platformName;
        this.primaryColor = config.primaryColor;
        this.logoUrl = config.logoUrl || '';
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Failed to load configuration', error);
        this.isLoading.set(false);
      }
    });
  }

  selectPreset(color: string) {
    this.primaryColor = color;
  }

  onLogoError(event: Event) {
    const img = event.target as HTMLImageElement;
    img.style.display = 'none';
  }

  save() {
    this.isSaving.set(true);
    const request: UpdateSystemConfigurationRequest = {
      platformName: this.platformName,
      primaryColor: this.primaryColor,
      logoUrl: this.logoUrl || undefined
    };

    this.configService.updateConfiguration(request).subscribe({
      next: (config) => {
        this.config.set(config);
        this.isSaving.set(false);
        this.notificationService.success('Appearance settings saved successfully');
      },
      error: (error) => {
        console.error('Failed to save configuration', error);
        this.isSaving.set(false);
        this.notificationService.error('Failed to save appearance settings');
      }
    });
  }
}
