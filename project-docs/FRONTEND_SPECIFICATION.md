# The League - Frontend Specification Document

## Overview

The League frontend is a Single Page Application (SPA) built with Angular 19 using standalone components and Tailwind CSS for styling. The application provides three distinct user experiences based on role: Super Admin dashboard, Club Manager dashboard, and Member Portal.

---

## Technology Stack

| Technology | Version | Purpose |
|------------|---------|---------|
| Angular | 19.2 | SPA Framework |
| TypeScript | 5.7 | Type-safe JavaScript |
| Tailwind CSS | 3.4 | Utility-first CSS |
| RxJS | 7.8 | Reactive programming |
| Chart.js | 4.5 | Data visualization |
| ng2-charts | 6.0 | Angular Chart.js wrapper |
| Playwright | 1.57 | E2E testing |
| Karma + Jasmine | - | Unit testing |

---

## Application Structure

```
the-league-client/
├── src/
│   ├── app/
│   │   ├── core/                    # Singleton services, guards, models
│   │   │   ├── guards/
│   │   │   │   ├── auth.guard.ts
│   │   │   │   └── role.guard.ts
│   │   │   ├── interceptors/
│   │   │   │   ├── auth.interceptor.ts
│   │   │   │   └── error.interceptor.ts
│   │   │   ├── models/
│   │   │   │   ├── auth.model.ts
│   │   │   │   ├── member.model.ts
│   │   │   │   ├── club.model.ts
│   │   │   │   └── ...
│   │   │   └── services/
│   │   │       ├── api.service.ts
│   │   │       ├── auth.service.ts
│   │   │       ├── member.service.ts
│   │   │       └── ...
│   │   │
│   │   ├── shared/                  # Reusable components and pipes
│   │   │   ├── components/
│   │   │   │   ├── notification/
│   │   │   │   ├── loading-spinner/
│   │   │   │   ├── confirm-dialog/
│   │   │   │   ├── pagination/
│   │   │   │   ├── status-badge/
│   │   │   │   └── empty-state/
│   │   │   └── pipes/
│   │   │       ├── date-format.pipe.ts
│   │   │       ├── currency-format.pipe.ts
│   │   │       └── truncate.pipe.ts
│   │   │
│   │   ├── layouts/                 # Layout wrapper components
│   │   │   ├── admin-layout/
│   │   │   ├── portal-layout/
│   │   │   └── unauthorized/
│   │   │
│   │   ├── features/                # Feature modules (lazy-loaded)
│   │   │   ├── auth/
│   │   │   │   ├── login/
│   │   │   │   ├── register/
│   │   │   │   ├── forgot-password/
│   │   │   │   ├── reset-password/
│   │   │   │   └── auth.routes.ts
│   │   │   │
│   │   │   ├── admin/               # Super Admin features
│   │   │   │   ├── dashboard/
│   │   │   │   ├── clubs/
│   │   │   │   ├── users/
│   │   │   │   ├── system-config/
│   │   │   │   └── admin.routes.ts
│   │   │   │
│   │   │   ├── club/                # Club Manager features
│   │   │   │   ├── dashboard/
│   │   │   │   ├── members/
│   │   │   │   ├── sessions/
│   │   │   │   ├── events/
│   │   │   │   ├── payments/
│   │   │   │   ├── memberships/
│   │   │   │   ├── competitions/
│   │   │   │   ├── invoices/
│   │   │   │   ├── fees/
│   │   │   │   ├── venues/
│   │   │   │   ├── reports/
│   │   │   │   ├── settings/
│   │   │   │   └── club.routes.ts
│   │   │   │
│   │   │   └── portal/              # Member Portal features
│   │   │       ├── dashboard/
│   │   │       ├── sessions/
│   │   │       ├── events/
│   │   │       ├── payments/
│   │   │       ├── profile/
│   │   │       ├── family/
│   │   │       ├── settings/
│   │   │       └── portal.routes.ts
│   │   │
│   │   ├── app.component.ts
│   │   ├── app.config.ts
│   │   └── app.routes.ts
│   │
│   ├── environments/
│   │   ├── environment.ts
│   │   └── environment.prod.ts
│   │
│   └── styles.css                   # Global styles + Tailwind directives
│
├── tailwind.config.js
├── angular.json
└── package.json
```

---

## Routing Architecture

### Main Routes

```typescript
// app.routes.ts
export const routes: Routes = [
  { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
  {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES)
  },
  {
    path: 'admin',
    canActivate: [superAdminGuard],
    loadComponent: () => import('./layouts/admin-layout/admin-layout.component'),
    loadChildren: () => import('./features/admin/admin.routes').then(m => m.ADMIN_ROUTES)
  },
  {
    path: 'club',
    canActivate: [clubManagerGuard],
    loadComponent: () => import('./layouts/admin-layout/admin-layout.component'),
    loadChildren: () => import('./features/club/club.routes').then(m => m.CLUB_ROUTES)
  },
  {
    path: 'portal',
    canActivate: [memberGuard],
    loadComponent: () => import('./layouts/portal-layout/portal-layout.component'),
    loadChildren: () => import('./features/portal/portal.routes').then(m => m.PORTAL_ROUTES)
  },
  { path: 'unauthorized', loadComponent: () => import('./layouts/unauthorized/unauthorized.component') },
  { path: '**', redirectTo: 'auth/login' }
];
```

### Feature Routes Example

```typescript
// features/club/club.routes.ts
export const CLUB_ROUTES: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', loadComponent: () => import('./dashboard/club-dashboard.component') },
  { path: 'members', loadComponent: () => import('./members/members-list.component') },
  { path: 'members/new', loadComponent: () => import('./members/member-form.component') },
  { path: 'members/:id', loadComponent: () => import('./members/member-detail.component') },
  { path: 'members/:id/edit', loadComponent: () => import('./members/member-form.component') },
  // ... additional routes
];
```

---

## Component Patterns

### Standalone Component Structure

```typescript
// Example: members-list.component.ts
import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MemberService } from '@core/services/member.service';
import { PaginationComponent } from '@shared/components/pagination/pagination.component';
import { StatusBadgeComponent } from '@shared/components/status-badge/status-badge.component';
import { Member, MemberQueryParams } from '@core/models/member.model';

@Component({
  selector: 'app-members-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, PaginationComponent, StatusBadgeComponent],
  template: `
    <div class="container mx-auto px-4 py-6">
      <!-- Page Header -->
      <div class="flex justify-between items-center mb-6">
        <h1 class="text-2xl font-bold text-gray-900">Members</h1>
        <a routerLink="/club/members/new"
           class="btn-primary">
          Add Member
        </a>
      </div>

      <!-- Search & Filters -->
      <div class="bg-white rounded-lg shadow p-4 mb-6">
        <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
          <input type="text"
                 [(ngModel)]="searchTerm"
                 (ngModelChange)="onSearch()"
                 placeholder="Search members..."
                 class="input-field">
          <!-- Additional filters -->
        </div>
      </div>

      <!-- Members Table -->
      <div class="bg-white rounded-lg shadow overflow-hidden">
        @if (loading()) {
          <div class="p-8 text-center">Loading...</div>
        } @else if (members().length === 0) {
          <app-empty-state message="No members found" />
        } @else {
          <table class="min-w-full divide-y divide-gray-200">
            <!-- Table content -->
          </table>
        }
      </div>

      <!-- Pagination -->
      <app-pagination
        [totalItems]="totalCount()"
        [currentPage]="currentPage()"
        [pageSize]="pageSize()"
        (pageChange)="onPageChange($event)" />
    </div>
  `
})
export class MembersListComponent implements OnInit {
  private memberService = inject(MemberService);

  members = signal<Member[]>([]);
  loading = signal(false);
  totalCount = signal(0);
  currentPage = signal(1);
  pageSize = signal(20);
  searchTerm = '';

  ngOnInit() {
    this.loadMembers();
  }

  loadMembers() {
    this.loading.set(true);
    this.memberService.getMembers({
      search: this.searchTerm,
      page: this.currentPage(),
      pageSize: this.pageSize()
    }).subscribe({
      next: (result) => {
        this.members.set(result.items);
        this.totalCount.set(result.totalCount);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  onPageChange(page: number) {
    this.currentPage.set(page);
    this.loadMembers();
  }

  onSearch() {
    this.currentPage.set(1);
    this.loadMembers();
  }
}
```

---

## Service Layer

### Base API Service

```typescript
// core/services/api.service.ts
import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;

  get<T>(path: string, params?: any): Observable<T> {
    let httpParams = new HttpParams();
    if (params) {
      Object.keys(params).forEach(key => {
        if (params[key] !== null && params[key] !== undefined) {
          httpParams = httpParams.set(key, params[key]);
        }
      });
    }
    return this.http.get<T>(`${this.baseUrl}${path}`, { params: httpParams });
  }

  post<T>(path: string, body: any): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}${path}`, body);
  }

  put<T>(path: string, body: any): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}${path}`, body);
  }

  delete<T>(path: string): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}${path}`);
  }
}
```

### Feature Service Example

```typescript
// core/services/member.service.ts
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from './api.service';
import { ApiResponse, PagedResult } from '@core/models/api.model';
import { Member, CreateMemberRequest, MemberQueryParams } from '@core/models/member.model';

@Injectable({ providedIn: 'root' })
export class MemberService {
  private api = inject(ApiService);

  getMembers(params: MemberQueryParams): Observable<PagedResult<Member>> {
    return this.api.get<ApiResponse<PagedResult<Member>>>('/members', params)
      .pipe(map(response => response.data));
  }

  getMember(id: string): Observable<Member> {
    return this.api.get<ApiResponse<Member>>(`/members/${id}`)
      .pipe(map(response => response.data));
  }

  createMember(request: CreateMemberRequest): Observable<Member> {
    return this.api.post<ApiResponse<Member>>('/members', request)
      .pipe(map(response => response.data));
  }

  updateMember(id: string, request: Partial<Member>): Observable<Member> {
    return this.api.put<ApiResponse<Member>>(`/members/${id}`, request)
      .pipe(map(response => response.data));
  }

  deleteMember(id: string): Observable<void> {
    return this.api.delete<void>(`/members/${id}`);
  }
}
```

---

## Authentication Flow

### Auth Service

```typescript
// core/services/auth.service.ts
import { Injectable, inject, signal, computed } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap, BehaviorSubject } from 'rxjs';
import { ApiService } from './api.service';
import { LoginRequest, LoginResponse, User } from '@core/models/auth.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private api = inject(ApiService);
  private router = inject(Router);

  private currentUserSubject = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  isAuthenticated = computed(() => !!this.currentUserSubject.value);

  get currentUser(): User | null {
    return this.currentUserSubject.value;
  }

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.api.post<ApiResponse<LoginResponse>>('/auth/login', request)
      .pipe(
        tap(response => {
          if (response.success) {
            localStorage.setItem('accessToken', response.data.accessToken);
            localStorage.setItem('refreshToken', response.data.refreshToken);
            this.currentUserSubject.next(response.data.user);
            this.navigateByRole(response.data.user.role);
          }
        })
      );
  }

  logout() {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    this.currentUserSubject.next(null);
    this.router.navigate(['/auth/login']);
  }

  private navigateByRole(role: string) {
    switch (role) {
      case 'SuperAdmin':
        this.router.navigate(['/admin/dashboard']);
        break;
      case 'ClubManager':
        this.router.navigate(['/club/dashboard']);
        break;
      case 'Member':
        this.router.navigate(['/portal/dashboard']);
        break;
    }
  }
}
```

### Auth Guard

```typescript
// core/guards/auth.guard.ts
import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '@core/services/auth.service';

export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isAuthenticated()) {
    return true;
  }

  router.navigate(['/auth/login']);
  return false;
};
```

### Role Guards

```typescript
// core/guards/role.guard.ts
import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '@core/services/auth.service';

export const superAdminGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.currentUser?.role === 'SuperAdmin') {
    return true;
  }

  router.navigate(['/unauthorized']);
  return false;
};

export const clubManagerGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const role = authService.currentUser?.role;

  if (role === 'SuperAdmin' || role === 'ClubManager') {
    return true;
  }

  router.navigate(['/unauthorized']);
  return false;
};

export const memberGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.currentUser) {
    return true;
  }

  router.navigate(['/auth/login']);
  return false;
};
```

---

## HTTP Interceptors

### Auth Interceptor

```typescript
// core/interceptors/auth.interceptor.ts
import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('accessToken');

  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(req);
};
```

### Error Interceptor

```typescript
// core/interceptors/error.interceptor.ts
import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { NotificationService } from '@core/services/notification.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const notification = inject(NotificationService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        localStorage.removeItem('accessToken');
        router.navigate(['/auth/login']);
      } else if (error.status === 403) {
        router.navigate(['/unauthorized']);
      } else if (error.status >= 500) {
        notification.error('Server error. Please try again later.');
      }

      return throwError(() => error);
    })
  );
};
```

---

## Shared Components

### Notification Component

```typescript
// shared/components/notification/notification.component.ts
@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [CommonModule],
  template: `
    @for (notification of notifications(); track notification.id) {
      <div class="fixed top-4 right-4 z-50 max-w-md"
           [class]="getNotificationClass(notification.type)">
        <div class="flex items-center gap-2 p-4 rounded-lg shadow-lg">
          <span>{{ notification.message }}</span>
          <button (click)="dismiss(notification.id)" class="text-white/80 hover:text-white">
            &times;
          </button>
        </div>
      </div>
    }
  `
})
export class NotificationComponent {
  notifications = inject(NotificationService).notifications;

  getNotificationClass(type: string): string {
    const base = 'transition-all duration-300';
    switch (type) {
      case 'success': return `${base} bg-green-500 text-white`;
      case 'error': return `${base} bg-red-500 text-white`;
      case 'warning': return `${base} bg-yellow-500 text-white`;
      default: return `${base} bg-blue-500 text-white`;
    }
  }

  dismiss(id: string) {
    inject(NotificationService).dismiss(id);
  }
}
```

### Pagination Component

```typescript
// shared/components/pagination/pagination.component.ts
@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="flex items-center justify-between px-4 py-3 bg-white border-t">
      <div class="text-sm text-gray-700">
        Showing {{ startItem }} to {{ endItem }} of {{ totalItems }} results
      </div>
      <div class="flex gap-2">
        <button (click)="onPrevious()"
                [disabled]="currentPage === 1"
                class="btn-secondary disabled:opacity-50">
          Previous
        </button>
        @for (page of visiblePages; track page) {
          <button (click)="onPageSelect(page)"
                  [class.bg-blue-600]="page === currentPage"
                  [class.text-white]="page === currentPage"
                  class="px-3 py-1 rounded border">
            {{ page }}
          </button>
        }
        <button (click)="onNext()"
                [disabled]="currentPage === totalPages"
                class="btn-secondary disabled:opacity-50">
          Next
        </button>
      </div>
    </div>
  `
})
export class PaginationComponent {
  @Input() totalItems = 0;
  @Input() currentPage = 1;
  @Input() pageSize = 20;
  @Output() pageChange = new EventEmitter<number>();

  get totalPages(): number {
    return Math.ceil(this.totalItems / this.pageSize);
  }

  get visiblePages(): number[] {
    // Logic to show pages around current page
  }

  onPageSelect(page: number) {
    this.pageChange.emit(page);
  }
}
```

### Status Badge Component

```typescript
// shared/components/status-badge/status-badge.component.ts
@Component({
  selector: 'app-status-badge',
  standalone: true,
  template: `
    <span [class]="badgeClass">{{ status }}</span>
  `
})
export class StatusBadgeComponent {
  @Input() status = '';
  @Input() type: 'member' | 'payment' | 'booking' = 'member';

  get badgeClass(): string {
    const base = 'px-2 py-1 text-xs font-medium rounded-full';
    const statusLower = this.status.toLowerCase();

    const colors: Record<string, string> = {
      active: 'bg-green-100 text-green-800',
      completed: 'bg-green-100 text-green-800',
      confirmed: 'bg-green-100 text-green-800',
      pending: 'bg-yellow-100 text-yellow-800',
      expired: 'bg-red-100 text-red-800',
      failed: 'bg-red-100 text-red-800',
      cancelled: 'bg-gray-100 text-gray-800',
      suspended: 'bg-orange-100 text-orange-800'
    };

    return `${base} ${colors[statusLower] || 'bg-gray-100 text-gray-800'}`;
  }
}
```

---

## UI Design System

### Color Palette (Tailwind)

```javascript
// tailwind.config.js
module.exports = {
  content: ['./src/**/*.{html,ts}'],
  theme: {
    extend: {
      colors: {
        primary: {
          50: '#eff6ff',
          100: '#dbeafe',
          500: '#3b82f6',
          600: '#2563eb',
          700: '#1d4ed8',
        },
        secondary: {
          500: '#6b7280',
          600: '#4b5563',
        }
      }
    }
  }
};
```

### Common CSS Classes

```css
/* styles.css */
@tailwind base;
@tailwind components;
@tailwind utilities;

@layer components {
  .btn-primary {
    @apply px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700
           transition-colors font-medium focus:outline-none focus:ring-2
           focus:ring-blue-500 focus:ring-offset-2;
  }

  .btn-secondary {
    @apply px-4 py-2 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200
           transition-colors font-medium focus:outline-none focus:ring-2
           focus:ring-gray-500 focus:ring-offset-2;
  }

  .btn-danger {
    @apply px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700
           transition-colors font-medium;
  }

  .input-field {
    @apply w-full px-3 py-2 border border-gray-300 rounded-lg
           focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent;
  }

  .card {
    @apply bg-white rounded-lg shadow-sm border border-gray-200 p-6;
  }

  .page-header {
    @apply text-2xl font-bold text-gray-900 mb-6;
  }
}
```

---

## Page Layouts

### Admin Layout

```typescript
// layouts/admin-layout/admin-layout.component.ts
@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink],
  template: `
    <div class="min-h-screen bg-gray-100">
      <!-- Sidebar -->
      <aside class="fixed inset-y-0 left-0 w-64 bg-gray-900 text-white">
        <!-- Logo -->
        <div class="p-4 border-b border-gray-800">
          <h1 class="text-xl font-bold">The League</h1>
        </div>

        <!-- Navigation -->
        <nav class="mt-4">
          @for (item of navItems; track item.path) {
            <a [routerLink]="item.path"
               routerLinkActive="bg-gray-800"
               class="flex items-center gap-3 px-4 py-3 hover:bg-gray-800 transition-colors">
              <span class="text-gray-400">{{ item.icon }}</span>
              <span>{{ item.label }}</span>
            </a>
          }
        </nav>
      </aside>

      <!-- Main Content -->
      <div class="ml-64">
        <!-- Header -->
        <header class="bg-white shadow-sm border-b px-6 py-4">
          <div class="flex justify-between items-center">
            <h2 class="text-lg font-semibold">{{ pageTitle }}</h2>
            <div class="flex items-center gap-4">
              <span class="text-sm text-gray-600">{{ currentUser?.email }}</span>
              <button (click)="logout()" class="text-sm text-red-600 hover:text-red-800">
                Logout
              </button>
            </div>
          </div>
        </header>

        <!-- Page Content -->
        <main class="p-6">
          <router-outlet />
        </main>
      </div>
    </div>
  `
})
export class AdminLayoutComponent {
  private authService = inject(AuthService);
  currentUser = this.authService.currentUser;

  navItems = [
    { path: '/club/dashboard', label: 'Dashboard', icon: '📊' },
    { path: '/club/members', label: 'Members', icon: '👥' },
    { path: '/club/sessions', label: 'Sessions', icon: '📅' },
    { path: '/club/events', label: 'Events', icon: '🎉' },
    { path: '/club/payments', label: 'Payments', icon: '💳' },
    { path: '/club/memberships', label: 'Memberships', icon: '🎫' },
    { path: '/club/venues', label: 'Venues', icon: '🏟️' },
    { path: '/club/reports', label: 'Reports', icon: '📈' },
    { path: '/club/settings', label: 'Settings', icon: '⚙️' },
  ];

  logout() {
    this.authService.logout();
  }
}
```

---

## Form Handling

### Reactive Forms Pattern

```typescript
// features/club/members/member-form.component.ts
@Component({
  selector: 'app-member-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    <form [formGroup]="form" (ngSubmit)="onSubmit()">
      <div class="card">
        <h2 class="text-xl font-semibold mb-6">{{ isEditMode ? 'Edit' : 'Add' }} Member</h2>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">First Name</label>
            <input type="text" formControlName="firstName" class="input-field">
            @if (form.get('firstName')?.errors?.['required'] && form.get('firstName')?.touched) {
              <span class="text-red-500 text-sm">First name is required</span>
            }
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Last Name</label>
            <input type="text" formControlName="lastName" class="input-field">
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Email</label>
            <input type="email" formControlName="email" class="input-field">
          </div>

          <!-- Additional fields -->
        </div>

        <div class="flex justify-end gap-4 mt-6">
          <a routerLink="/club/members" class="btn-secondary">Cancel</a>
          <button type="submit" [disabled]="form.invalid || submitting()" class="btn-primary">
            {{ submitting() ? 'Saving...' : 'Save Member' }}
          </button>
        </div>
      </div>
    </form>
  `
})
export class MemberFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private memberService = inject(MemberService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private notification = inject(NotificationService);

  form!: FormGroup;
  submitting = signal(false);
  isEditMode = false;
  memberId?: string;

  ngOnInit() {
    this.initForm();

    this.memberId = this.route.snapshot.params['id'];
    if (this.memberId) {
      this.isEditMode = true;
      this.loadMember();
    }
  }

  initForm() {
    this.form = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: [''],
      dateOfBirth: [''],
      // ... additional fields
    });
  }

  loadMember() {
    this.memberService.getMember(this.memberId!).subscribe({
      next: (member) => this.form.patchValue(member),
      error: () => this.notification.error('Failed to load member')
    });
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.submitting.set(true);
    const request = this.form.value;

    const operation = this.isEditMode
      ? this.memberService.updateMember(this.memberId!, request)
      : this.memberService.createMember(request);

    operation.subscribe({
      next: () => {
        this.notification.success(`Member ${this.isEditMode ? 'updated' : 'created'} successfully`);
        this.router.navigate(['/club/members']);
      },
      error: () => {
        this.notification.error('Failed to save member');
        this.submitting.set(false);
      }
    });
  }
}
```

---

## Responsive Design

The application uses Tailwind's responsive prefixes for mobile-first design:

- **Mobile (default):** Base styles
- **Tablet (md:):** 768px and up
- **Desktop (lg:):** 1024px and up
- **Large Desktop (xl:):** 1280px and up

### Example Responsive Grid

```html
<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
  <!-- Cards adapt to screen size -->
</div>
```

---

*Document Version: 1.0*
*Last Updated: Pre-Development Planning Phase*
