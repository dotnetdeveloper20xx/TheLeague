# Project Vision

## The Problem We're Solving

Sports clubs across the UK - cricket clubs, football clubs, hockey clubs, rugby clubs - face a common challenge: **membership management is a nightmare**.

Most clubs still operate with:
- Excel spreadsheets tracking members (often outdated)
- Paper forms for registration
- Manual bank statement checking for payment verification
- WhatsApp groups for session coordination
- Email chains for event communication
- No visibility into who's paid, who's lapsed, or who's actually attending

This creates problems:
- Volunteers spend hours on admin instead of running activities
- Money gets lost (unpaid fees, uncollected payments)
- Members get frustrated with poor communication
- New members have a confusing onboarding experience
- Committees have no data for decision-making

---

## Our Solution

The League is a **multi-tenant SaaS platform** purpose-built for sports club management. One platform serves many clubs, each with complete data isolation.

### Core Value Proposition

| For Club Committees | For Members |
|---------------------|-------------|
| Single source of truth for membership data | Self-service registration and renewals |
| Automated payment tracking and reminders | Easy session/class booking |
| Online registration reduces paperwork | Family account management |
| Financial reporting at a glance | Payment history and receipts |
| Session attendance tracking | Event discovery and RSVP |
| Competition/league management | Competition standings and fixtures |

---

## Target Users

### 1. Super Administrators (Platform Operators)
- Manage club onboarding
- Monitor system health
- Configure payment/email providers
- Handle platform-wide settings

### 2. Club Managers (Club Committee Members)
- Club secretary, treasurer, membership secretary
- Manage members, sessions, events
- Process payments and generate invoices
- Run reports for committee meetings
- Configure club settings and membership types

### 3. Members (End Users)
- Individual club members
- Parents managing children's memberships
- Browse and book sessions
- View payment history
- Update personal details

---

## Feature Overview

### Phase 1: Core (Complete)
- Multi-tenant club management
- Member CRUD with family accounts
- Membership types and enrollment
- Session management and booking
- Event management
- Manual payment recording
- Basic reporting

### Phase 2: Financial (Complete)
- Invoice generation
- Payment plans
- Fee configuration
- Refund processing
- Financial audit trail

### Phase 3: Operations (Complete)
- Venue/facility management
- Equipment tracking
- Program/course management
- Competition/league system

### Phase 4: Advanced (In Progress)
- Online payments (Stripe integration)
- Email notifications (SendGrid)
- Advanced reporting
- Mobile optimization

---

## Domain Language (Glossary)

Understanding these terms is essential:

| Term | Definition |
|------|------------|
| **Club** | A tenant in the system - one sports club organization |
| **Member** | An individual registered with a club |
| **Family Member** | Dependents (children/spouse) linked to a primary member account |
| **Membership Type** | A plan/tier (e.g., "Senior Playing", "Junior", "Social") |
| **Membership** | The link between a Member and a MembershipType with dates/status |
| **Session** | A scheduled activity (training, practice, class) |
| **Booking** | A member's reservation for a session |
| **Event** | A one-off club activity (AGM, tournament, social) |
| **Venue** | A physical location where activities happen |
| **Fee** | A configurable charge (membership fee, session fee, etc.) |
| **Invoice** | A bill sent to a member |
| **Payment** | A financial transaction recording money received |
| **Competition** | A league, tournament, or cup competition |

---

## Multi-Tenancy Explained

This is the most important architectural concept:

```
┌─────────────────────────────────────────────────────────────────┐
│                    The League Platform                          │
├─────────────────────────────────────────────────────────────────┤
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐             │
│  │ Teddington  │  │  Highbury   │  │  Richmond   │  ...more    │
│  │ Cricket Club│  │ United FC   │  │ Hockey Club │  clubs      │
│  ├─────────────┤  ├─────────────┤  ├─────────────┤             │
│  │ Members: 50 │  │ Members: 80 │  │ Members: 45 │             │
│  │ Sessions: 20│  │ Sessions: 30│  │ Sessions: 25│             │
│  │ Events: 10  │  │ Events: 15  │  │ Events: 12  │             │
│  └─────────────┘  └─────────────┘  └─────────────┘             │
│                                                                 │
│  SHARED: Database, API, Infrastructure                          │
│  ISOLATED: Data (via ClubId), Branding, Settings                │
└─────────────────────────────────────────────────────────────────┘
```

Every query is filtered by `ClubId`. A club manager logging in gets a JWT token containing their `clubId` claim. The `TenantMiddleware` extracts this and the `TenantService` provides it to all services for data filtering.

---

## Business Model Context

While you won't deal with billing directly, understanding the model helps:

- Clubs pay a subscription (monthly/annual)
- Pricing likely tiered by member count
- Additional revenue from payment processing fees
- Value proposition: saves volunteer time, reduces revenue leakage

---

## Success Metrics (What Matters)

When evaluating features or fixes, consider:

1. **Time Saved** - How much admin time does this save club volunteers?
2. **Money Captured** - Does this help clubs collect payments they'd otherwise miss?
3. **Member Experience** - Is this easy for non-technical members to use?
4. **Data Accuracy** - Does this improve the quality of club data?

---

## What's Next?

Now that you understand the "why", let's get you set up:

→ [02_LOCAL_SETUP.md](./02_LOCAL_SETUP.md) - Get the application running locally
