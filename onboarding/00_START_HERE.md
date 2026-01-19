# Welcome to The League

Hey there! Welcome to the team. This guide will get you up to speed on The League - a multi-tenant SaaS platform for sports club membership management.

---

## What Does This Application Do?

The League is a comprehensive membership management platform designed for sports clubs (cricket, football, hockey, rugby, etc.). It enables clubs to manage their entire operation digitally - from member registrations and payments to session bookings and competition management.

The platform serves three distinct user types through role-based portals:
- **Super Admins** manage the entire platform, onboarding new clubs and monitoring system health
- **Club Managers** handle day-to-day club operations - members, sessions, events, payments, and reporting
- **Members** access a self-service portal to book sessions, view events, manage family members, and track their payments

What makes this interesting technically is the **multi-tenant architecture**. Each club is a completely isolated tenant sharing the same database but with strict data separation via `ClubId` filtering. A club manager at Teddington Cricket Club will never see data from Highbury United FC.

---

## Why It Matters

Sports clubs in the UK traditionally manage memberships via spreadsheets, paper forms, and manual bank reconciliation. The League modernizes this with:
- Online registration and renewals
- Automated payment tracking and reminders
- Session/class booking with capacity management
- Family account support (parents managing children's memberships)
- Competition and league management
- Financial reporting and invoice generation

---

## Your Reading Roadmap

Follow these guides in order for the smoothest onboarding:

| Day | Guide | What You'll Learn |
|-----|-------|-------------------|
| 1 | [02_LOCAL_SETUP.md](./02_LOCAL_SETUP.md) | Get the app running locally |
| 1 | [01_PROJECT_VISION.md](./01_PROJECT_VISION.md) | Understand the business context |
| 2 | [03_ARCHITECTURE_OVERVIEW.md](./03_ARCHITECTURE_OVERVIEW.md) | See the big picture |
| 2 | [04_BACKEND_GUIDE.md](./04_BACKEND_GUIDE.md) | Understand the API layer |
| 3 | [05_DATABASE_GUIDE.md](./05_DATABASE_GUIDE.md) | Navigate the data model |
| 3 | [06_FRONTEND_GUIDE.md](./06_FRONTEND_GUIDE.md) | Explore the Angular client |
| 4 | [07_AUTHENTICATION_AND_SECURITY.md](./07_AUTHENTICATION_AND_SECURITY.md) | Learn auth flows |
| 4 | [08_KEY_FEATURES_WALKTHROUGH.md](./08_KEY_FEATURES_WALKTHROUGH.md) | Trace real features end-to-end |
| 5 | [09_SUPPORT_KNOWLEDGE.md](./09_SUPPORT_KNOWLEDGE.md) | Troubleshooting and debugging |
| 5 | [10_DEVELOPMENT_WORKFLOW.md](./10_DEVELOPMENT_WORKFLOW.md) | How to contribute |

---

## Key Contacts & Resources

| Resource | Contact/Link |
|----------|--------------|
| Team Lead | `[TEAM_LEAD]` |
| Slack Channel | `[SLACK_CHANNEL]` |
| Jira Board | `[JIRA_BOARD_URL]` |
| CI/CD Pipeline | `[AZURE_DEVOPS_URL]` |
| Production URL | `[PRODUCTION_URL]` |
| Staging URL | `[STAGING_URL]` |

---

## By End of Week 1, You Should Be Able To...

- [ ] Run both backend and frontend locally
- [ ] Log in as SuperAdmin, ClubManager, and Member roles
- [ ] Explain the multi-tenant architecture to someone else
- [ ] Navigate the codebase and find any controller, service, or component
- [ ] Create a new member and book them into a session
- [ ] Trace a request from Angular component to database and back
- [ ] Make a small bug fix or feature enhancement
- [ ] Create a pull request following team conventions
- [ ] Answer basic support questions about the application

---

## Quick Reference

### Demo Credentials

| Role | Email | Password |
|------|-------|----------|
| Super Admin | admin@theleague.com | Admin123! |
| Club Manager (Teddington CC) | chairman@teddingtoncc.com | Chairman123! |
| Club Manager (Highbury United) | chairman@highburyunited.com | Chairman123! |
| Member | james.anderson@email.com | Member123! |

### URLs (Local Development)

| Service | URL |
|---------|-----|
| Frontend | http://localhost:4200 |
| Backend API | http://localhost:7000 |
| Swagger Docs | http://localhost:7000/swagger |

---

## Existing Documentation

We have comprehensive planning docs in `/project-docs/`:
- `PROJECT_VISION.md` - Business goals and target market
- `TECHNICAL_ARCHITECTURE.md` - Architecture decisions
- `API_SPECIFICATION.md` - Full API reference
- `DATABASE_DESIGN.md` - Entity relationships
- `FRONTEND_SPECIFICATION.md` - UI/UX specifications
- `DEPLOYMENT_RUNBOOK.md` - Deployment procedures

These are reference docs - this onboarding package distills what you need to know first.

---

Ready? Start with [02_LOCAL_SETUP.md](./02_LOCAL_SETUP.md) to get the app running!
