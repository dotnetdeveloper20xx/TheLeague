import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

interface NavItem {
  label: string;
  route: string;
  icon: string;
}

@Component({
  selector: 'app-portal-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <div class="min-h-screen bg-gray-50">
      <!-- Header -->
      <header class="bg-white shadow-sm sticky top-0 z-40">
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div class="flex items-center justify-between h-16">
            <!-- Logo -->
            <div class="flex items-center">
              <svg class="w-8 h-8 text-primary-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z" />
              </svg>
              <span class="ml-2 text-xl font-bold text-gray-900">The League</span>
            </div>

            <!-- Desktop Navigation -->
            <nav class="hidden md:flex items-center space-x-1">
              @for (item of navItems; track item.route) {
                <a
                  [routerLink]="item.route"
                  routerLinkActive="bg-primary-50 text-primary-700"
                  [routerLinkActiveOptions]="{ exact: item.route === '/portal' }"
                  class="px-4 py-2 rounded-lg text-gray-600 hover:bg-gray-100 hover:text-gray-900 transition-colors"
                >
                  {{ item.label }}
                </a>
              }
            </nav>

            <!-- User Menu -->
            <div class="flex items-center gap-4">
              <button class="p-2 text-gray-500 hover:text-gray-700 rounded-lg hover:bg-gray-100 relative">
                <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9" />
                </svg>
              </button>

              <div class="relative">
                <button
                  (click)="userMenuOpen.set(!userMenuOpen())"
                  class="flex items-center gap-2 p-2 rounded-lg hover:bg-gray-100"
                >
                  <div class="w-8 h-8 rounded-full bg-primary-600 flex items-center justify-center text-white text-sm font-medium">
                    {{ userInitials }}
                  </div>
                  <span class="hidden sm:block text-sm font-medium text-gray-700">{{ authService.currentUser?.firstName }}</span>
                  <svg class="w-4 h-4 text-gray-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                  </svg>
                </button>

                @if (userMenuOpen()) {
                  <div class="absolute right-0 mt-2 w-48 bg-white rounded-lg shadow-lg py-1 z-50">
                    <a routerLink="/portal/profile" class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100">
                      My Profile
                    </a>
                    <a routerLink="/portal/settings" class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100">
                      Settings
                    </a>
                    <hr class="my-1" />
                    <button
                      (click)="authService.logout()"
                      class="w-full text-left px-4 py-2 text-sm text-red-600 hover:bg-gray-100"
                    >
                      Sign out
                    </button>
                  </div>
                }
              </div>

              <!-- Mobile menu button -->
              <button
                (click)="mobileMenuOpen.set(!mobileMenuOpen())"
                class="md:hidden p-2 text-gray-500 hover:text-gray-700 rounded-lg hover:bg-gray-100"
              >
                <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
                </svg>
              </button>
            </div>
          </div>
        </div>

        <!-- Mobile Navigation -->
        @if (mobileMenuOpen()) {
          <nav class="md:hidden border-t border-gray-200 py-2 px-4">
            @for (item of navItems; track item.route) {
              <a
                [routerLink]="item.route"
                routerLinkActive="bg-primary-50 text-primary-700"
                [routerLinkActiveOptions]="{ exact: item.route === '/portal' }"
                class="block px-4 py-2 rounded-lg text-gray-600 hover:bg-gray-100"
                (click)="mobileMenuOpen.set(false)"
              >
                {{ item.label }}
              </a>
            }
          </nav>
        }
      </header>

      <!-- Click outside to close user menu -->
      @if (userMenuOpen()) {
        <div class="fixed inset-0 z-30" (click)="userMenuOpen.set(false)"></div>
      }

      <!-- Main content -->
      <main class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <router-outlet></router-outlet>
      </main>

      <!-- Footer -->
      <footer class="bg-white border-t border-gray-200 mt-auto">
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6">
          <div class="flex flex-col sm:flex-row items-center justify-between gap-4">
            <p class="text-sm text-gray-500">The League - Membership Management Portal</p>
            <div class="flex items-center gap-6 text-sm text-gray-500">
              <a href="#" class="hover:text-gray-700">Help</a>
              <a href="#" class="hover:text-gray-700">Privacy</a>
              <a href="#" class="hover:text-gray-700">Terms</a>
            </div>
          </div>
        </div>
      </footer>
    </div>
  `
})
export class PortalLayoutComponent {
  authService = inject(AuthService);
  userMenuOpen = signal(false);
  mobileMenuOpen = signal(false);

  navItems: NavItem[] = [
    { label: 'Dashboard', route: '/portal', icon: 'dashboard' },
    { label: 'Sessions', route: '/portal/sessions', icon: 'calendar' },
    { label: 'Events', route: '/portal/events', icon: 'events' },
    { label: 'Payments', route: '/portal/payments', icon: 'payments' },
    { label: 'Family', route: '/portal/family', icon: 'family' }
  ];

  get userInitials(): string {
    const user = this.authService.currentUser;
    if (!user) return '';
    return `${user.firstName?.[0] || ''}${user.lastName?.[0] || ''}`.toUpperCase();
  }
}
