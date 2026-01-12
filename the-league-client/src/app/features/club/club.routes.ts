import { Routes } from '@angular/router';

export const CLUB_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./dashboard/club-dashboard.component').then(m => m.ClubDashboardComponent)
  },
  {
    path: 'members',
    loadComponent: () => import('./members/members-list.component').then(m => m.MembersListComponent)
  },
  {
    path: 'members/new',
    loadComponent: () => import('./members/member-form.component').then(m => m.MemberFormComponent)
  },
  {
    path: 'members/:id',
    loadComponent: () => import('./members/member-detail.component').then(m => m.MemberDetailComponent)
  },
  {
    path: 'memberships',
    loadComponent: () => import('./memberships/memberships-list.component').then(m => m.MembershipsListComponent)
  },
  {
    path: 'memberships/types',
    loadComponent: () => import('./memberships/membership-types.component').then(m => m.MembershipTypesComponent)
  },
  {
    path: 'sessions',
    loadComponent: () => import('./sessions/sessions-list.component').then(m => m.SessionsListComponent)
  },
  {
    path: 'sessions/new',
    loadComponent: () => import('./sessions/session-form.component').then(m => m.SessionFormComponent)
  },
  {
    path: 'sessions/:id',
    loadComponent: () => import('./sessions/session-detail.component').then(m => m.SessionDetailComponent)
  },
  {
    path: 'events',
    loadComponent: () => import('./events/events-list.component').then(m => m.EventsListComponent)
  },
  {
    path: 'events/new',
    loadComponent: () => import('./events/event-form.component').then(m => m.EventFormComponent)
  },
  {
    path: 'events/:id',
    loadComponent: () => import('./events/event-detail.component').then(m => m.EventDetailComponent)
  },
  {
    path: 'competitions',
    loadComponent: () => import('./competitions/competitions-list.component').then(m => m.CompetitionsListComponent)
  },
  {
    path: 'competitions/new',
    loadComponent: () => import('./competitions/competition-form.component').then(m => m.CompetitionFormComponent)
  },
  {
    path: 'competitions/:id',
    loadComponent: () => import('./competitions/competition-detail.component').then(m => m.CompetitionDetailComponent)
  },
  {
    path: 'competitions/:id/edit',
    loadComponent: () => import('./competitions/competition-form.component').then(m => m.CompetitionFormComponent)
  },
  {
    path: 'payments',
    loadComponent: () => import('./payments/payments-list.component').then(m => m.PaymentsListComponent)
  },
  {
    path: 'payments/:id',
    loadComponent: () => import('./payments/payment-detail.component').then(m => m.PaymentDetailComponent)
  },
  {
    path: 'venues',
    loadComponent: () => import('./venues/venues-list.component').then(m => m.VenuesListComponent)
  },
  {
    path: 'fees',
    loadComponent: () => import('./fees/fees-list.component').then(m => m.FeesListComponent)
  },
  {
    path: 'fees/new',
    loadComponent: () => import('./fees/fee-form.component').then(m => m.FeeFormComponent)
  },
  {
    path: 'fees/:id',
    loadComponent: () => import('./fees/fee-form.component').then(m => m.FeeFormComponent)
  },
  {
    path: 'invoices',
    loadComponent: () => import('./invoices/invoices-list.component').then(m => m.InvoicesListComponent)
  },
  {
    path: 'invoices/new',
    loadComponent: () => import('./invoices/invoice-form.component').then(m => m.InvoiceFormComponent)
  },
  {
    path: 'invoices/:id',
    loadComponent: () => import('./invoices/invoice-detail.component').then(m => m.InvoiceDetailComponent)
  },
  {
    path: 'invoices/:id/edit',
    loadComponent: () => import('./invoices/invoice-form.component').then(m => m.InvoiceFormComponent)
  },
  {
    path: 'reports',
    loadComponent: () => import('./reports/club-reports.component').then(m => m.ClubReportsComponent)
  },
  {
    path: 'settings',
    loadComponent: () => import('./settings/club-settings.component').then(m => m.ClubSettingsComponent)
  }
];
