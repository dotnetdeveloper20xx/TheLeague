import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { superAdminGuard, clubManagerGuard, memberGuard } from './core/guards/role.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'auth/login',
    pathMatch: 'full'
  },
  {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES)
  },
  {
    path: 'admin',
    canActivate: [superAdminGuard],
    loadComponent: () => import('./layouts/admin-layout/admin-layout.component').then(m => m.AdminLayoutComponent),
    loadChildren: () => import('./features/admin/admin.routes').then(m => m.ADMIN_ROUTES)
  },
  {
    path: 'club',
    canActivate: [clubManagerGuard],
    loadComponent: () => import('./layouts/admin-layout/admin-layout.component').then(m => m.AdminLayoutComponent),
    loadChildren: () => import('./features/club/club.routes').then(m => m.CLUB_ROUTES)
  },
  {
    path: 'portal',
    canActivate: [memberGuard],
    loadComponent: () => import('./layouts/portal-layout/portal-layout.component').then(m => m.PortalLayoutComponent),
    loadChildren: () => import('./features/portal/portal.routes').then(m => m.PORTAL_ROUTES)
  },
  {
    path: 'unauthorized',
    loadComponent: () => import('./layouts/unauthorized/unauthorized.component').then(m => m.UnauthorizedComponent)
  },
  {
    path: '**',
    redirectTo: 'auth/login'
  }
];
