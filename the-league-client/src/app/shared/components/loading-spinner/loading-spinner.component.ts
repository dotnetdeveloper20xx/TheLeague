import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-loading-spinner',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="flex items-center justify-center" [ngClass]="containerClass">
      <div
        class="animate-spin rounded-full border-t-2 border-b-2 border-primary-600"
        [ngClass]="{
          'h-4 w-4': size === 'sm',
          'h-8 w-8': size === 'md',
          'h-12 w-12': size === 'lg',
          'h-16 w-16': size === 'xl'
        }"
      ></div>
      @if (message) {
        <span class="ml-3 text-gray-600" [ngClass]="{
          'text-sm': size === 'sm',
          'text-base': size === 'md',
          'text-lg': size === 'lg',
          'text-xl': size === 'xl'
        }">{{ message }}</span>
      }
    </div>
  `
})
export class LoadingSpinnerComponent {
  @Input() size: 'sm' | 'md' | 'lg' | 'xl' = 'md';
  @Input() message?: string;
  @Input() containerClass = '';
}
