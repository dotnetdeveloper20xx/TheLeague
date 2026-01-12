import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { SystemConfigService } from '../../../core/services/system-config.service';
import { ConfigurationAuditLog } from '../../../core/models/system-config.model';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-audit-log',
  standalone: true,
  imports: [CommonModule, RouterLink, LoadingSpinnerComponent, DatePipe],
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
            <h1 class="text-2xl font-bold text-white">Audit Log</h1>
            <p class="text-gray-400 mt-1">View history of configuration changes</p>
          </div>
        </div>
        <button (click)="loadAuditLog()" class="btn-secondary" [disabled]="isLoading()">
          <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
              d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
          </svg>
          Refresh
        </button>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner />
        </div>
      } @else if (auditLogs().length === 0) {
        <div class="card text-center py-12">
          <svg class="w-16 h-16 mx-auto text-gray-600 mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
              d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
          </svg>
          <h3 class="text-lg font-medium text-gray-400 mb-2">No audit entries</h3>
          <p class="text-gray-500">Configuration changes will be logged here</p>
        </div>
      } @else {
        <div class="space-y-4">
          @for (log of auditLogs(); track log.id) {
            <div class="card">
              <div class="flex items-start justify-between">
                <div class="flex items-start">
                  <!-- Icon based on action -->
                  <div class="p-2 rounded-lg mr-4"
                       [ngClass]="{
                         'bg-blue-500': log.action === 'Updated',
                         'bg-green-500': log.action === 'Created',
                         'bg-gray-500': log.action === 'Viewed'
                       }"
                       style="--tw-bg-opacity: 0.2">
                    @if (log.action === 'Updated') {
                      <svg class="w-5 h-5 text-blue-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                          d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                      </svg>
                    } @else if (log.action === 'Created') {
                      <svg class="w-5 h-5 text-green-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
                      </svg>
                    } @else {
                      <svg class="w-5 h-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                          d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                          d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                      </svg>
                    }
                  </div>
                  <div>
                    <div class="flex items-center gap-2 mb-1">
                      <span class="text-white font-medium">{{ log.action }}</span>
                      <span class="badge badge-gray">{{ log.section }}</span>
                    </div>
                    @if (log.propertyChanged) {
                      <p class="text-gray-300 text-sm">
                        <span class="text-gray-400">Property:</span> {{ log.propertyChanged }}
                      </p>
                    }
                    @if (log.oldValue || log.newValue) {
                      <div class="mt-2 text-sm">
                        @if (log.oldValue) {
                          <div class="flex items-center gap-2">
                            <span class="text-red-400">- {{ log.oldValue }}</span>
                          </div>
                        }
                        @if (log.newValue) {
                          <div class="flex items-center gap-2">
                            <span class="text-green-400">+ {{ log.newValue }}</span>
                          </div>
                        }
                      </div>
                    }
                    <div class="mt-2 flex items-center gap-4 text-sm text-gray-500">
                      <span>By {{ log.changedBy }}</span>
                      @if (log.ipAddress) {
                        <span>from {{ log.ipAddress }}</span>
                      }
                    </div>
                  </div>
                </div>
                <div class="text-right">
                  <p class="text-gray-400 text-sm">{{ log.timestamp | date:'medium' }}</p>
                  <p class="text-gray-500 text-xs mt-1">{{ getRelativeTime(log.timestamp) }}</p>
                </div>
              </div>
            </div>
          }

          <!-- Load More -->
          @if (hasMore()) {
            <div class="text-center py-4">
              <button (click)="loadMore()" class="btn-secondary" [disabled]="isLoadingMore()">
                {{ isLoadingMore() ? 'Loading...' : 'Load More' }}
              </button>
            </div>
          }
        </div>
      }
    </div>
  `
})
export class AuditLogComponent implements OnInit {
  private configService = inject(SystemConfigService);

  auditLogs = signal<ConfigurationAuditLog[]>([]);
  isLoading = signal(true);
  isLoadingMore = signal(false);
  hasMore = signal(false);
  currentLimit = 20;

  ngOnInit() {
    this.loadAuditLog();
  }

  loadAuditLog() {
    this.isLoading.set(true);
    this.currentLimit = 20;
    this.configService.getAuditLog(this.currentLimit).subscribe({
      next: (logs) => {
        this.auditLogs.set(logs);
        this.hasMore.set(logs.length >= this.currentLimit);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Failed to load audit log', error);
        this.isLoading.set(false);
      }
    });
  }

  loadMore() {
    this.isLoadingMore.set(true);
    this.currentLimit += 20;
    this.configService.getAuditLog(this.currentLimit).subscribe({
      next: (logs) => {
        this.auditLogs.set(logs);
        this.hasMore.set(logs.length >= this.currentLimit);
        this.isLoadingMore.set(false);
      },
      error: (error) => {
        console.error('Failed to load more audit logs', error);
        this.isLoadingMore.set(false);
      }
    });
  }

  getRelativeTime(timestamp: string): string {
    const date = new Date(timestamp);
    const now = new Date();
    const diffMs = now.getTime() - date.getTime();
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMs / 3600000);
    const diffDays = Math.floor(diffMs / 86400000);

    if (diffMins < 1) return 'Just now';
    if (diffMins < 60) return `${diffMins} minute${diffMins === 1 ? '' : 's'} ago`;
    if (diffHours < 24) return `${diffHours} hour${diffHours === 1 ? '' : 's'} ago`;
    if (diffDays < 7) return `${diffDays} day${diffDays === 1 ? '' : 's'} ago`;
    return '';
  }
}
