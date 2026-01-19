# Frontend Guide

Deep dive into the Angular client application.

---

## Technology Stack

| Component | Technology |
|-----------|------------|
| Framework | Angular 19.2 |
| Language | TypeScript 5.x |
| Styling | Tailwind CSS + SCSS |
| HTTP Client | Angular HttpClient |
| State | RxJS BehaviorSubject |
| Charting | ng2-charts (Chart.js) |
| Build | Angular CLI / Webpack |
| Testing | Jasmine + Karma (unit), Playwright (e2e) |

---

## Project Structure

```
the-league-client/
├── src/
│   ├── app/
│   │   ├── core/                      # Singleton services & utilities
│   │   │   ├── services/              # API services
│   │   │   │   ├── api.service.ts     # Base HTTP wrapper
│   │   │   │   ├── auth.service.ts    # Authentication
│   │   │   │   ├── member.service.ts  # Member API
│   │   │   │   ├── session.service.ts # Session API
│   │   │   │   └── ...
│   │   │   ├── guards/                # Route guards
│   │   │   │   ├── auth.guard.ts
│   │   │   │   ├── role.guard.ts
│   │   │   │   └── ...
│   │   │   ├── interceptors/          # HTTP interceptors
│   │   │   │   ├── auth.interceptor.ts
│   │   │   │   └── error.interceptor.ts
│   │   │   └── models/                # TypeScript interfaces
│   │   │
│   │   ├── features/                  # Feature modules
│   │   │   ├── auth/                  # Login, register, etc.
│   │   │   ├── admin/                 # SuperAdmin views
│   │   │   ├── club/                  # ClubManager views
│   │   │   └── portal/                # Member portal
│   │   │
│   │   ├── shared/                    # Reusable components
│   │   │   ├── components/
│   │   │   │   ├── notification/
│   │   │   │   ├── pagination/
│   │   │   │   ├── loading-spinner/
│   │   │   │   └── ...
│   │   │   └── pipes/
│   │   │
│   │   ├── layouts/                   # Page layouts
│   │   │   ├── admin-layout/          # Sidebar + header for managers
│   │   │   └── portal-layout/         # Member portal layout
│   │   │
│   │   ├── app.routes.ts              # Root routing
│   │   ├── app.component.ts           # Root component
│   │   └── app.config.ts              # Application config
│   │
│   ├── environments/
│   │   ├── environment.ts             # Development
│   │   └── environment.prod.ts        # Production
│   │
│   ├── styles.scss                    # Global styles
│   └── index.html
│
├── angular.json                       # Angular CLI config
├── tailwind.config.js                 # Tailwind config
├── package.json
└── tsconfig.json
```

---

## Routing Structure

### Root Routes (`app.routes.ts`)

```typescript
export const routes: Routes = [
  { path: '', redirectTo: '/auth/login', pathMatch: 'full' },

  // Public routes (no auth required)
  {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.routes')
  },

  // Admin routes (SuperAdmin only)
  {
    path: 'admin',
    component: AdminLayoutComponent,
    canActivate: [authGuard, superAdminGuard],
    loadChildren: () => import('./features/admin/admin.routes')
  },

  // Club routes (ClubManager)
  {
    path: 'club',
    component: AdminLayoutComponent,
    canActivate: [authGuard, clubManagerGuard],
    loadChildren: () => import('./features/club/club.routes')
  },

  // Portal routes (Member)
  {
    path: 'portal',
    component: PortalLayoutComponent,
    canActivate: [authGuard, memberGuard],
    loadChildren: () => import('./features/portal/portal.routes')
  },

  { path: 'unauthorized', component: UnauthorizedComponent },
  { path: '**', redirectTo: '/auth/login' }
];
```

### Feature Routes

**Club Manager Routes:**
```
/club                    → Dashboard
/club/members            → Member list
/club/members/new        → Create member
/club/members/:id        → Member detail/edit
/club/sessions           → Session list
/club/sessions/new       → Create session
/club/events             → Event list
/club/payments           → Payment list
/club/memberships        → Membership list
/club/memberships/types  → Membership type config
/club/venues             → Venue management
/club/reports            → Reports
/club/settings           → Club settings
```

**Member Portal Routes:**
```
/portal                  → Dashboard
/portal/sessions         → Browse sessions
/portal/events           → View events
/portal/payments         → Payment history
/portal/family           → Family members
/portal/profile          → Edit profile
/portal/settings         → Personal settings
```

---

## Component Architecture

### Standalone Components

All components use Angular's standalone component pattern:

```typescript
// Example: member-list.component.ts

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    FormsModule,
    PaginationComponent,
    StatusBadgeComponent,
    LoadingSpinnerComponent
  ],
  templateUrl: './member-list.component.html'
})
export class MemberListComponent implements OnInit {
  members = signal<MemberListDto[]>([]);
  loading = signal(true);
  totalCount = signal(0);
  currentPage = signal(1);

  private memberService = inject(MemberService);

  ngOnInit() {
    this.loadMembers();
  }

  loadMembers(page = 1) {
    this.loading.set(true);
    this.memberService.getMembers(page).subscribe({
      next: (result) => {
        this.members.set(result.items);
        this.totalCount.set(result.totalCount);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }
}
```

### Template Syntax (New Control Flow)

Angular 17+ control flow syntax:

```html
<!-- member-list.component.html -->

@if (loading()) {
  <app-loading-spinner />
} @else {
  @if (members().length === 0) {
    <app-empty-state message="No members found" />
  } @else {
    <table class="w-full">
      <thead>
        <tr>
          <th>Name</th>
          <th>Email</th>
          <th>Status</th>
        </tr>
      </thead>
      <tbody>
        @for (member of members(); track member.id) {
          <tr>
            <td>{{ member.fullName }}</td>
            <td>{{ member.email }}</td>
            <td><app-status-badge [status]="member.status" /></td>
          </tr>
        }
      </tbody>
    </table>
  }

  <app-pagination
    [currentPage]="currentPage()"
    [totalCount]="totalCount()"
    (pageChange)="loadMembers($event)"
  />
}
```

---

## Services

### Base API Service

All HTTP calls go through `ApiService`:

```typescript
// core/services/api.service.ts

@Injectable({ providedIn: 'root' })
export class ApiService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  get<T>(endpoint: string, params?: any): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/${endpoint}`, { params });
  }

  post<T>(endpoint: string, body: any): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}/${endpoint}`, body);
  }

  put<T>(endpoint: string, body: any): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/${endpoint}`, body);
  }

  patch<T>(endpoint: string, body: any): Observable<T> {
    return this.http.patch<T>(`${this.baseUrl}/${endpoint}`, body);
  }

  delete<T>(endpoint: string): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}/${endpoint}`);
  }
}
```

### Feature Services

```typescript
// core/services/member.service.ts

@Injectable({ providedIn: 'root' })
export class MemberService {
  private api = inject(ApiService);

  getMembers(page = 1, pageSize = 20, search?: string): Observable<PagedResult<MemberListDto>> {
    return this.api.get('members', { page, pageSize, search });
  }

  getMember(id: string): Observable<MemberDetailDto> {
    return this.api.get(`members/${id}`);
  }

  createMember(member: CreateMemberDto): Observable<MemberDetailDto> {
    return this.api.post('members', member);
  }

  updateMember(id: string, member: UpdateMemberDto): Observable<MemberDetailDto> {
    return this.api.put(`members/${id}`, member);
  }

  deleteMember(id: string): Observable<void> {
    return this.api.delete(`members/${id}`);
  }
}
```

### Auth Service

Manages authentication state:

```typescript
// core/services/auth.service.ts

@Injectable({ providedIn: 'root' })
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  private api = inject(ApiService);
  private router = inject(Router);

  get isLoggedIn(): boolean {
    return !!this.getToken();
  }

  get currentUser(): User | null {
    return this.currentUserSubject.value;
  }

  login(email: string, password: string): Observable<AuthResponse> {
    return this.api.post<ApiResponse<AuthResponse>>('auth/login', { email, password })
      .pipe(
        map(response => {
          if (response.success && response.data) {
            this.storeTokens(response.data);
            this.currentUserSubject.next(response.data.user);
          }
          return response.data!;
        })
      );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('user');
    this.currentUserSubject.next(null);
    this.router.navigate(['/auth/login']);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  hasRole(role: string): boolean {
    const user = this.currentUser;
    return user?.role === role;
  }

  private storeTokens(auth: AuthResponse): void {
    localStorage.setItem('token', auth.token);
    localStorage.setItem('refreshToken', auth.refreshToken);
    localStorage.setItem('user', JSON.stringify(auth.user));
  }
}
```

---

## HTTP Interceptors

### Auth Interceptor

Attaches JWT to all requests:

```typescript
// core/interceptors/auth.interceptor.ts

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        // Try to refresh token or logout
        authService.logout();
      }
      return throwError(() => error);
    })
  );
};
```

### Error Interceptor

Handles HTTP errors globally:

```typescript
// core/interceptors/error.interceptor.ts

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const notification = inject(NotificationService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let message = 'An error occurred';

      switch (error.status) {
        case 400:
          message = error.error?.message || 'Invalid request';
          break;
        case 401:
          message = 'Your session has expired';
          break;
        case 403:
          message = 'You do not have permission';
          break;
        case 404:
          message = 'Resource not found';
          break;
        case 500:
          message = 'Server error. Please try again later.';
          break;
      }

      // Don't show notification for 401 (handled by auth interceptor)
      if (error.status !== 401) {
        notification.error(message);
      }

      return throwError(() => error);
    })
  );
};
```

---

## Route Guards

### Auth Guard

Requires authentication:

```typescript
// core/guards/auth.guard.ts

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isLoggedIn) {
    return true;
  }

  router.navigate(['/auth/login'], {
    queryParams: { returnUrl: state.url }
  });
  return false;
};
```

### Role Guard

Requires specific role:

```typescript
// core/guards/role.guard.ts

export const roleGuard = (allowedRoles: string[]): CanActivateFn => {
  return (route, state) => {
    const authService = inject(AuthService);
    const router = inject(Router);

    const user = authService.currentUser;

    if (user && allowedRoles.includes(user.role)) {
      return true;
    }

    router.navigate(['/unauthorized']);
    return false;
  };
};

// Pre-configured guards
export const superAdminGuard = roleGuard(['SuperAdmin']);
export const clubManagerGuard = roleGuard(['ClubManager', 'SuperAdmin']);
export const memberGuard = roleGuard(['Member']);
```

---

## Shared Components

### Notification Component

Toast notifications:

```typescript
// shared/components/notification/notification.component.ts

@Component({
  selector: 'app-notification',
  standalone: true,
  template: `
    @for (notification of notifications(); track notification.id) {
      <div class="notification" [class]="notification.type">
        {{ notification.message }}
        <button (click)="dismiss(notification.id)">×</button>
      </div>
    }
  `
})
export class NotificationComponent {
  notifications = inject(NotificationService).notifications;
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
    <div class="flex gap-2">
      <button (click)="goToPage(1)" [disabled]="currentPage() === 1">First</button>
      <button (click)="goToPage(currentPage() - 1)" [disabled]="currentPage() === 1">Prev</button>
      <span>Page {{ currentPage() }} of {{ totalPages() }}</span>
      <button (click)="goToPage(currentPage() + 1)" [disabled]="currentPage() === totalPages()">Next</button>
      <button (click)="goToPage(totalPages())" [disabled]="currentPage() === totalPages()">Last</button>
    </div>
  `
})
export class PaginationComponent {
  currentPage = input.required<number>();
  totalCount = input.required<number>();
  pageSize = input(20);
  pageChange = output<number>();

  totalPages = computed(() => Math.ceil(this.totalCount() / this.pageSize()));

  goToPage(page: number) {
    this.pageChange.emit(page);
  }
}
```

---

## Styling

### Tailwind CSS

Primary styling via Tailwind utility classes:

```html
<div class="bg-white shadow rounded-lg p-6">
  <h2 class="text-xl font-semibold text-gray-900 mb-4">Members</h2>
  <table class="min-w-full divide-y divide-gray-200">
    <thead class="bg-gray-50">
      <tr>
        <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Name</th>
      </tr>
    </thead>
  </table>
</div>
```

### Global Styles

Custom styles in `styles.scss`:

```scss
// styles.scss
@tailwind base;
@tailwind components;
@tailwind utilities;

// Custom component classes
.btn-primary {
  @apply px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition;
}

.btn-secondary {
  @apply px-4 py-2 bg-gray-200 text-gray-800 rounded hover:bg-gray-300 transition;
}

.card {
  @apply bg-white shadow rounded-lg p-6;
}
```

---

## Form Handling

### Reactive Forms

```typescript
@Component({...})
export class MemberFormComponent {
  private fb = inject(FormBuilder);
  private memberService = inject(MemberService);

  form = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    phone: [''],
    dateOfBirth: ['']
  });

  onSubmit() {
    if (this.form.valid) {
      this.memberService.createMember(this.form.value).subscribe({
        next: (member) => {
          // Handle success
        }
      });
    }
  }
}
```

---

## Quick Reference

### Common Patterns

```typescript
// Service injection
private memberService = inject(MemberService);

// Signals for state
members = signal<Member[]>([]);
loading = signal(false);

// Computed values
totalPages = computed(() => Math.ceil(this.totalCount() / this.pageSize));

// Observable subscription
this.memberService.getMembers().subscribe({
  next: (data) => this.members.set(data),
  error: (err) => console.error(err)
});

// Navigation
this.router.navigate(['/club/members', id]);

// Query params
this.route.queryParams.subscribe(params => {
  this.search = params['search'];
});
```

### File Locations

| Purpose | Location |
|---------|----------|
| API Services | `src/app/core/services/` |
| Guards | `src/app/core/guards/` |
| Interceptors | `src/app/core/interceptors/` |
| Models/Interfaces | `src/app/core/models/` |
| Feature Components | `src/app/features/[feature]/` |
| Shared Components | `src/app/shared/components/` |
| Routes | `src/app/app.routes.ts` |
| Environment Config | `src/environments/` |

### Commands

```bash
# Development server
npm start

# Production build
npm run build

# Run tests
npm test

# E2E tests
npm run e2e

# Generate component
ng generate component features/club/member-form --standalone
```

---

## Next Steps

→ [07_AUTHENTICATION_AND_SECURITY.md](./07_AUTHENTICATION_AND_SECURITY.md) - Learn the auth flow
