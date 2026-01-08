import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-status-badge',
  standalone: true,
  imports: [CommonModule],
  template: `
    <span
      class="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium"
      [ngClass]="badgeClass"
    >
      {{ displayText }}
    </span>
  `
})
export class StatusBadgeComponent {
  @Input() status!: string;
  @Input() type: 'member' | 'membership' | 'payment' | 'booking' | 'event' = 'member';

  get displayText(): string {
    return this.status?.replace(/([A-Z])/g, ' $1').trim() || '';
  }

  get badgeClass(): string {
    const statusLower = this.status?.toLowerCase() || '';

    // Success statuses
    if (['active', 'completed', 'confirmed', 'attended', 'published'].includes(statusLower)) {
      return 'bg-green-100 text-green-800';
    }

    // Warning statuses
    if (['pending', 'pendingpayment', 'processing'].includes(statusLower)) {
      return 'bg-yellow-100 text-yellow-800';
    }

    // Danger statuses
    if (['expired', 'cancelled', 'failed', 'suspended', 'noshow'].includes(statusLower)) {
      return 'bg-red-100 text-red-800';
    }

    // Info statuses
    if (['refunded'].includes(statusLower)) {
      return 'bg-blue-100 text-blue-800';
    }

    // Default
    return 'bg-gray-100 text-gray-800';
  }
}
