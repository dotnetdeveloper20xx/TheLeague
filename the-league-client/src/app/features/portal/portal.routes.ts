import { Routes } from '@angular/router';

export const PORTAL_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./dashboard/portal-dashboard.component').then(m => m.PortalDashboardComponent)
  },
  {
    path: 'sessions',
    loadComponent: () => import('./sessions/portal-sessions.component').then(m => m.PortalSessionsComponent)
  },
  {
    path: 'events',
    loadComponent: () => import('./events/portal-events.component').then(m => m.PortalEventsComponent)
  },
  {
    path: 'payments',
    loadComponent: () => import('./payments/portal-payments.component').then(m => m.PortalPaymentsComponent)
  },
  {
    path: 'family',
    loadComponent: () => import('./family/family-members.component').then(m => m.FamilyMembersComponent)
  },
  {
    path: 'profile',
    loadComponent: () => import('./profile/member-profile.component').then(m => m.MemberProfileComponent)
  },
  {
    path: 'settings',
    loadComponent: () => import('./settings/portal-settings.component').then(m => m.PortalSettingsComponent)
  }
];
