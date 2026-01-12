import { Routes } from '@angular/router';

export const ADMIN_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./dashboard/admin-dashboard.component').then(m => m.AdminDashboardComponent)
  },
  {
    path: 'clubs',
    loadComponent: () => import('./clubs/clubs-list.component').then(m => m.ClubsListComponent)
  },
  {
    path: 'clubs/new',
    loadComponent: () => import('./clubs/club-form.component').then(m => m.ClubFormComponent)
  },
  {
    path: 'clubs/:id',
    loadComponent: () => import('./clubs/club-detail.component').then(m => m.ClubDetailComponent)
  },
  {
    path: 'users',
    loadComponent: () => import('./users/users-list.component').then(m => m.UsersListComponent)
  },
  {
    path: 'reports',
    loadComponent: () => import('./reports/admin-reports.component').then(m => m.AdminReportsComponent)
  },
  {
    path: 'settings',
    loadComponent: () => import('./settings/admin-settings.component').then(m => m.AdminSettingsComponent)
  },
  {
    path: 'system-config',
    children: [
      {
        path: '',
        loadComponent: () => import('./system-config/system-config-dashboard.component').then(m => m.SystemConfigDashboardComponent)
      },
      {
        path: 'payment',
        loadComponent: () => import('./system-config/payment-config.component').then(m => m.PaymentConfigComponent)
      },
      {
        path: 'email',
        loadComponent: () => import('./system-config/email-config.component').then(m => m.EmailConfigComponent)
      },
      {
        path: 'features',
        loadComponent: () => import('./system-config/feature-flags.component').then(m => m.FeatureFlagsComponent)
      },
      {
        path: 'appearance',
        loadComponent: () => import('./system-config/appearance-config.component').then(m => m.AppearanceConfigComponent)
      },
      {
        path: 'audit',
        loadComponent: () => import('./system-config/audit-log.component').then(m => m.AuditLogComponent)
      }
    ]
  }
];
