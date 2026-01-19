# The League - Sprint Plan

## Overview

This document outlines the sprint breakdown for The League project. The project follows a 2-week sprint cadence with approximately 40 story points capacity per sprint (assuming 2-3 developers).

### Sprint Parameters
- **Sprint Duration:** 2 weeks (10 working days)
- **Sprint Capacity:** ~40 story points
- **Total Estimated Story Points:** ~245
- **Estimated Sprints:** 7-8 sprints

---

## Sprint 0: Project Setup & Foundation
**Duration:** 1 week (reduced sprint for setup)

### Goals
- Development environment setup
- Solution architecture implementation
- Core infrastructure configuration

### Tasks

| Task | Description | Points | Status |
|------|-------------|--------|--------|
| T-0.1 | Create solution structure (Api, Core, Infrastructure, Tests) | 2 | Done |
| T-0.2 | Configure Entity Framework Core with SQL Server | 2 | Done |
| T-0.3 | Set up ASP.NET Core Identity | 3 | Done |
| T-0.4 | Configure JWT Authentication | 3 | Done |
| T-0.5 | Implement TenantMiddleware for multi-tenancy | 3 | Done |
| T-0.6 | Set up Swagger documentation | 1 | Done |
| T-0.7 | Create Angular project with Tailwind CSS | 2 | Done |
| T-0.8 | Configure CORS and API connectivity | 1 | Done |
| T-0.9 | Create base entity classes and enums | 2 | Done |
| T-0.10 | Set up initial database migrations | 2 | Done |

**Sprint 0 Total:** 21 points

### Definition of Done
- [x] Solution compiles without errors
- [x] Database migrations run successfully
- [x] Swagger UI accessible at /swagger
- [x] Angular app runs and connects to API
- [x] Basic authentication flow working

---

## Sprint 1: Authentication & Core Entities
**Duration:** 2 weeks

### Goals
- Complete authentication system
- Implement core domain entities
- Basic club and member CRUD

### User Stories

| ID | User Story | Points | Status |
|----|------------|--------|--------|
| US-1.1 | User Registration | 5 | Done |
| US-1.2 | User Login | 3 | Done |
| US-1.3 | Token Refresh | 3 | Done |
| US-1.4 | Password Reset | 3 | Done |
| US-1.6 | Role-Based Access Control | 5 | Done |
| US-2.1 | Club Creation (Admin) | 5 | Done |
| US-2.2 | Club Listing (Admin) | 3 | Done |
| US-2.3 | Club Profile Management | 3 | Done |

**Sprint 1 Total:** 30 points

### Technical Tasks
- Create ApplicationUser entity extending IdentityUser
- Implement Club entity with settings
- Create AuthController with all endpoints
- Build Angular login, register, forgot-password pages
- Implement auth interceptor and guards
- Create admin club management pages

### Definition of Done
- [ ] All user stories pass acceptance criteria
- [ ] Unit tests cover service layer
- [ ] API endpoints documented in Swagger
- [ ] Angular components follow design patterns
- [ ] Code reviewed and merged

---

## Sprint 2: Member Management
**Duration:** 2 weeks

### Goals
- Complete member CRUD operations
- Family member support
- Member status management

### User Stories

| ID | User Story | Points | Status |
|----|------------|--------|--------|
| US-3.1 | Member Creation | 5 | Done |
| US-3.2 | Member Listing | 5 | Done |
| US-3.3 | Member Profile View | 5 | Done |
| US-3.4 | Member Profile Edit | 3 | Done |
| US-3.5 | Family Member Management | 5 | Done |
| US-3.6 | Member Notes | 3 | Done |
| US-3.7 | Member Status Management | 3 | Done |
| US-2.4 | Club Settings Configuration | 5 | Done |

**Sprint 2 Total:** 34 points

### Technical Tasks
- Create Member and FamilyMember entities
- Implement MemberService with full CRUD
- Build member list page with search and filters
- Create member detail page with tabs
- Implement family member sub-form
- Create member notes component
- Build club settings page

### Definition of Done
- [ ] Members can be created, viewed, edited, searched
- [ ] Family members can be added to primary accounts
- [ ] Notes can be added with categories
- [ ] Member status transitions work correctly
- [ ] Club settings save and apply

---

## Sprint 3: Membership & Payment Foundation
**Duration:** 2 weeks

### Goals
- Membership type configuration
- Membership enrollment and management
- Manual payment recording

### User Stories

| ID | User Story | Points | Status |
|----|------------|--------|--------|
| US-4.1 | Membership Type Creation | 5 | Done |
| US-4.2 | Membership Enrollment | 5 | Done |
| US-4.3 | Membership Renewal | 5 | Done |
| US-4.4 | Membership Cancellation | 3 | Done |
| US-7.1 | Manual Payment Recording | 3 | Done |
| US-7.3 | Payment History View | 3 | Done |
| US-2.5 | Club Dashboard | 8 | Done |

**Sprint 3 Total:** 32 points

### Technical Tasks
- Create MembershipType and Membership entities
- Implement Payment entity and PaymentService
- Build membership type management page
- Create membership enrollment form
- Build payment recording form
- Implement club dashboard with KPIs
- Create payment history list

### Definition of Done
- [ ] Membership types configurable per club
- [ ] Members can be enrolled in memberships
- [ ] Manual payments recorded and tracked
- [ ] Dashboard shows accurate statistics
- [ ] Payment history displays correctly

---

## Sprint 4: Sessions & Bookings
**Duration:** 2 weeks

### Goals
- Session management
- Booking system for managers and members
- Recurring schedules

### User Stories

| ID | User Story | Points | Status |
|----|------------|--------|--------|
| US-5.1 | Session Creation | 3 | Done |
| US-5.2 | Recurring Schedule Creation | 5 | Done |
| US-5.3 | Session Listing (Manager) | 3 | Done |
| US-5.4 | Session Booking (Manager) | 3 | Done |
| US-5.5 | Session Booking (Member) | 5 | Done |
| US-5.6 | Booking Cancellation | 3 | Done |
| US-5.7 | Waitlist Management | 5 | Done |
| US-5.8 | Attendance Tracking | 5 | Done |

**Sprint 4 Total:** 32 points

### Technical Tasks
- Create Session and SessionBooking entities
- Implement RecurringSchedule entity
- Build session management pages
- Create booking flow for members
- Implement waitlist logic
- Build attendance marking interface
- Create Venue management

### Definition of Done
- [ ] Sessions can be created individually and from schedules
- [ ] Members can browse and book sessions
- [ ] Bookings can be cancelled with deadline enforcement
- [ ] Waitlist automatically promotes members
- [ ] Attendance can be marked for sessions

---

## Sprint 5: Events & Payments Integration
**Duration:** 2 weeks

### Goals
- Event management system
- Online payment integration (Stripe)
- Invoice system

### User Stories

| ID | User Story | Points | Status |
|----|------------|--------|--------|
| US-6.1 | Event Creation | 5 | Done |
| US-6.2 | Event Listing | 3 | Done |
| US-6.3 | Event RSVP | 3 | Done |
| US-6.4 | Event Ticket Purchase | 8 | Done |
| US-7.2 | Online Payment (Stripe) | 8 | Done |
| US-7.4 | Invoice Generation | 5 | Done |
| US-7.5 | Invoice Emailing | 3 | Done |

**Sprint 5 Total:** 35 points

### Technical Tasks
- Create Event and EventRSVP entities
- Implement Stripe payment provider
- Build event management pages
- Create ticket purchase flow
- Implement invoice system
- Build email service integration

### Definition of Done
- [ ] Events can be created with ticketing options
- [ ] Members can RSVP to events
- [ ] Stripe payments process successfully
- [ ] Invoices generated and emailed
- [ ] Payment receipts sent

---

## Sprint 6: Member Portal & Reports
**Duration:** 2 weeks

### Goals
- Complete member self-service portal
- Basic reporting system
- Competition foundation

### User Stories

| ID | User Story | Points | Status |
|----|------------|--------|--------|
| US-10.1 | Portal Dashboard | 5 | Done |
| US-10.2 | Profile Self-Service | 3 | Done |
| US-10.3 | Payment History | 2 | Done |
| US-10.4 | Family Member Management | 3 | Done |
| US-9.1 | Membership Reports | 5 | Done |
| US-9.2 | Financial Reports | 5 | Done |
| US-8.1 | Competition Creation | 5 | Done |
| US-8.2 | Team Registration | 5 | Done |

**Sprint 6 Total:** 33 points

### Technical Tasks
- Build portal layout and navigation
- Create portal dashboard with widgets
- Implement profile edit page
- Build payment history view
- Create report pages with charts
- Implement Competition entities
- Build team management UI

### Definition of Done
- [ ] Members can access self-service portal
- [ ] Portal shows personalized dashboard
- [ ] Members can edit profiles and manage family
- [ ] Reports display accurate data
- [ ] Competitions can be created with teams

---

## Sprint 7: Competition & System Admin
**Duration:** 2 weeks

### Goals
- Complete competition management
- System configuration
- Polish and bug fixes

### User Stories

| ID | User Story | Points | Status |
|----|------------|--------|--------|
| US-8.3 | Fixture Generation | 8 | Done |
| US-8.4 | Match Result Recording | 3 | Done |
| US-8.5 | Standings Display | 3 | Done |
| US-11.1 | Payment Provider Configuration | 5 | Done |
| US-11.2 | Email Provider Configuration | 5 | Done |
| US-11.3 | Feature Flags | 3 | Done |
| US-1.5 | Change Password | 2 | Done |
| US-7.6 | Refund Processing | 5 | Done |

**Sprint 7 Total:** 34 points

### Technical Tasks
- Implement fixture generation algorithm
- Build match recording interface
- Create standings table component
- Build system configuration dashboard
- Implement refund processing
- Complete remaining features
- Bug fixes and polish

### Definition of Done
- [ ] Fixtures auto-generated for competitions
- [ ] Match results update standings
- [ ] System configuration fully functional
- [ ] All "Must Have" features complete
- [ ] Major bugs resolved

---

## Sprint 8: Stabilization & Launch Prep
**Duration:** 2 weeks

### Goals
- Performance optimization
- Security hardening
- Documentation completion
- Deployment preparation

### Tasks

| Task | Description | Points | Status |
|------|-------------|--------|--------|
| T-8.1 | Performance testing and optimization | 5 | Pending |
| T-8.2 | Security audit and fixes | 5 | Pending |
| T-8.3 | End-to-end testing | 5 | Pending |
| T-8.4 | User acceptance testing | 3 | Pending |
| T-8.5 | Documentation review | 2 | Pending |
| T-8.6 | Deployment scripts | 3 | Pending |
| T-8.7 | Production environment setup | 5 | Pending |
| T-8.8 | Data migration tools | 3 | Pending |
| US-9.3 | Attendance Reports | 3 | Pending |
| US-4.5 | Membership Freeze | 5 | Pending |
| US-6.5 | Event Attendee Management | 3 | Pending |

**Sprint 8 Total:** 42 points

### Definition of Done
- [ ] Performance benchmarks met
- [ ] Security vulnerabilities addressed
- [ ] E2E tests passing
- [ ] UAT sign-off received
- [ ] Production deployment successful

---

## Sprint Summary

| Sprint | Focus | Story Points | Cumulative |
|--------|-------|--------------|------------|
| Sprint 0 | Setup | 21 | 21 |
| Sprint 1 | Auth & Clubs | 30 | 51 |
| Sprint 2 | Members | 34 | 85 |
| Sprint 3 | Membership & Payments | 32 | 117 |
| Sprint 4 | Sessions & Bookings | 32 | 149 |
| Sprint 5 | Events & Online Pay | 35 | 184 |
| Sprint 6 | Portal & Reports | 33 | 217 |
| Sprint 7 | Competitions & Config | 34 | 251 |
| Sprint 8 | Stabilization | 42 | 293 |

---

## Velocity Tracking

| Sprint | Planned | Completed | Velocity |
|--------|---------|-----------|----------|
| Sprint 0 | 21 | 21 | 21 |
| Sprint 1 | 30 | - | - |
| Sprint 2 | 34 | - | - |
| Sprint 3 | 32 | - | - |
| Sprint 4 | 32 | - | - |
| Sprint 5 | 35 | - | - |
| Sprint 6 | 33 | - | - |
| Sprint 7 | 34 | - | - |
| Sprint 8 | 42 | - | - |

---

## Risk Management

### Identified Risks

| Risk | Probability | Impact | Mitigation |
|------|-------------|--------|------------|
| Stripe integration delays | Medium | High | Use mock provider for development |
| Scope creep | High | Medium | Strict backlog prioritization |
| Performance issues | Low | High | Early performance testing |
| Team availability | Medium | Medium | Cross-training, documentation |

### Contingency Plans

1. **If behind schedule:** Move "Could Have" items to backlog
2. **If Stripe issues:** Complete with mock, add Stripe in later sprint
3. **If team reduced:** Focus on "Must Have" items only

---

## Sprint Ceremonies

### Sprint Planning
- **When:** First day of sprint
- **Duration:** 2 hours
- **Outcome:** Sprint backlog committed

### Daily Standup
- **When:** Daily, same time
- **Duration:** 15 minutes
- **Focus:** Blockers and progress

### Sprint Review
- **When:** Last day of sprint
- **Duration:** 1 hour
- **Outcome:** Demo to stakeholders

### Sprint Retrospective
- **When:** Last day of sprint
- **Duration:** 1 hour
- **Outcome:** Process improvements

---

*Document Version: 1.0*
*Last Updated: Pre-Development Planning Phase*
