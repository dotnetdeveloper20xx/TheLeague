# The League - Sports Club Management Platform

A comprehensive multi-tenant SaaS platform for managing sports club memberships, sessions, events, and payments. Built with enterprise-grade architecture using ASP.NET Core 8 and Angular 19.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-19.2-DD0031?logo=angular)](https://angular.dev/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.7-3178C6?logo=typescript)](https://www.typescriptlang.org/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)
[![Tailwind CSS](https://img.shields.io/badge/Tailwind%20CSS-3.4-06B6D4?logo=tailwindcss)](https://tailwindcss.com/)

---

## Table of Contents

- [Overview](#overview)
- [Key Features](#key-features)
- [Screenshots](#screenshots)
- [Architecture](#architecture)
- [Technology Stack](#technology-stack)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [API Documentation](#api-documentation)
- [Demo Credentials](#demo-credentials)
- [Documentation](#documentation)
- [License](#license)

---

## Overview

**The League** is an enterprise-ready platform designed for sports organisations to manage their clubs, members, training sessions, events, and financial operations. The platform supports multiple sport types including Cricket, Football, Hockey, Rugby, Tennis, and more.

### Business Problem Solved

Sports clubs traditionally rely on spreadsheets, paper records, or disconnected systems to manage their operations. The League provides a unified solution that:

- **Centralises member data** with complete profiles, family relationships, and membership history
- **Streamlines session booking** with capacity management and attendance tracking
- **Simplifies event management** from planning to ticket sales
- **Automates payment tracking** with support for multiple payment methods
- **Enables self-service** through a dedicated member portal

### Multi-Tenancy Architecture

The platform operates as a true multi-tenant SaaS solution where:

- Each sports club operates in complete data isolation
- A single deployment serves multiple organisations
- Club-specific branding and configuration
- Centralised platform administration

---

## Key Features

### For Platform Administrators (Super Admin)
- Multi-club management and provisioning
- System-wide user administration
- Platform analytics and reporting
- Global configuration management
- Payment and email provider configuration
- Feature flag management

### For Club Managers
| Feature | Description |
|---------|-------------|
| **Member Management** | Complete member lifecycle from registration to renewal |
| **Session Scheduling** | Training sessions with booking and attendance tracking |
| **Event Planning** | Club events with ticketing and RSVPs |
| **Payment Processing** | Record payments, generate invoices, track outstanding balances |
| **Membership Types** | Configurable membership categories with pricing |
| **Venue Management** | Facility booking and resource allocation |
| **Competition Management** | Leagues, tournaments, standings, and statistics |
| **Reporting** | Financial and membership analytics |
| **Club Settings** | Branding, notifications, and operational configuration |

### For Members
| Feature | Description |
|---------|-------------|
| **Self-Service Portal** | View and update personal information |
| **Session Booking** | Browse and book available training sessions |
| **Event Registration** | Discover and register for club events |
| **Payment History** | View invoices and payment records |
| **Family Management** | Manage family member accounts |
| **Notifications** | Stay informed about club activities |

---

## Screenshots

### Member Portal

The member portal provides a self-service experience for club members to manage their activities.

#### Member Dashboard
*Personal dashboard showing upcoming sessions, events, family members, and recent payments*

![Member Portal Dashboard](docs/screenshots/17-portal-dashboard.png)

#### Session Booking
*Browse available training sessions with pricing, availability, and instant booking*

![Session Booking](docs/screenshots/18-portal-sessions.png)

#### Payment History
*View complete payment history with dates, descriptions, amounts, and status*

![Payment History](docs/screenshots/20-portal-payments.png)

#### Family Members
*Manage family member accounts with status tracking and quick actions*

![Family Members](docs/screenshots/21-portal-family.png)

#### Member Profile
*Comprehensive profile management including personal info, address, emergency contacts, and medical information*

![Member Profile](docs/screenshots/22-portal-profile.png)

#### Settings
*Configure notification preferences and account settings*

![Portal Settings](docs/screenshots/23-portal-settings.png)

---

### Responsive Design

The platform is fully responsive and optimised for all device sizes.

#### Mobile Login
*Touch-friendly mobile login with demo credentials display*

![Mobile Login](docs/screenshots/36-mobile-login.png)

#### Mobile Dashboard
*Responsive club dashboard optimised for mobile devices*

![Mobile Dashboard](docs/screenshots/37-mobile-dashboard.png)

---

## Architecture

The platform follows **Clean Architecture** principles with clear separation of concerns:

```
┌─────────────────────────────────────────────────────────────────┐
│                         Presentation                            │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │              Angular 19 SPA (Standalone Components)         ││
│  │    Tailwind CSS | RxJS | Reactive Forms | Route Guards      ││
│  └─────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────┘
                                │
                          HTTP/REST API
                                │
┌─────────────────────────────────────────────────────────────────┐
│                        Application Layer                        │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │                    ASP.NET Core 8 Web API                   ││
│  │   Controllers | Services | DTOs | Middleware | Providers    ││
│  └─────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────┘
                                │
┌─────────────────────────────────────────────────────────────────┐
│                         Domain Layer                            │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │           Entities | Enums | Business Rules                 ││
│  └─────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────┘
                                │
┌─────────────────────────────────────────────────────────────────┐
│                      Infrastructure Layer                       │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │    Entity Framework Core 8 | SQL Server | Tenant Service    ││
│  └─────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────┘
```

### Security Model

| Feature | Implementation |
|---------|----------------|
| **Authentication** | JWT Bearer tokens with refresh token rotation |
| **Authorisation** | Role-Based Access Control (SuperAdmin, ClubManager, Member, Coach, Staff) |
| **Tenant Isolation** | ClubId discriminator on all database queries |
| **CORS Protection** | Configurable allowed origins |
| **Password Policy** | ASP.NET Identity with strength requirements |
| **Token Expiry** | 15-minute access tokens, 7-day refresh tokens |

---

## Technology Stack

### Backend

| Technology | Version | Purpose |
|------------|---------|---------|
| ASP.NET Core | 8.0 | Web API Framework |
| Entity Framework Core | 8.0 | ORM / Data Access |
| SQL Server | 2022 | Database |
| ASP.NET Identity | 8.0 | Authentication & User Management |
| JWT Bearer | - | Token Authentication |
| Swagger/OpenAPI | - | API Documentation |

### Frontend

| Technology | Version | Purpose |
|------------|---------|---------|
| Angular | 19.2 | SPA Framework |
| TypeScript | 5.7 | Type-safe JavaScript |
| Tailwind CSS | 3.4 | Utility-first CSS |
| RxJS | 7.8 | Reactive Extensions |
| Chart.js | - | Data Visualisation |
| Playwright | - | E2E Testing |

### External Integrations

| Provider | Purpose | Implementation |
|----------|---------|----------------|
| Stripe | Payment Processing | Provider Factory Pattern |
| PayPal | Payment Processing | Provider Factory Pattern |
| SendGrid | Email Delivery | Provider Factory Pattern |

*All external providers use a factory pattern allowing mock implementations for development and testing.*

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [SQL Server LocalDB](https://docs.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb) or SQL Server 2019+
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Quick Start

**1. Clone the repository**
```bash
git clone https://github.com/dotnetdeveloper20xx/TheLeague.git
cd TheLeague
```

**2. Set up the database**
```bash
cd TheLeague.Infrastructure
dotnet ef database update -s ../TheLeague.Api
```

**3. Start the API**
```bash
cd ../TheLeague.Api
dotnet run
```
API will be available at `http://localhost:7000`

**4. Start the Angular client**
```bash
cd ../the-league-client
npm install
npm start
```
Application will be available at `http://localhost:4200`

**5. Access Swagger documentation**
```
http://localhost:7000/swagger
```

### Application URLs

| Component | URL |
|-----------|-----|
| Frontend | http://localhost:4200 |
| API | http://localhost:7000 |
| Swagger API Docs | http://localhost:7000/swagger |

---

## Project Structure

```
LeagueMembershipManagementPortal/
├── TheLeague.Api/                    # ASP.NET Core Web API
│   ├── Controllers/                  # API Endpoints (20+ controllers)
│   ├── Services/                     # Business Logic (18+ services)
│   ├── DTOs/                         # Data Transfer Objects
│   ├── Middleware/                   # Request Pipeline & Tenant Resolution
│   ├── Providers/                    # Payment & Email Provider Factories
│   └── Program.cs                    # Application Entry Point & DI Configuration
│
├── TheLeague.Core/                   # Domain Layer
│   ├── Entities/                     # Domain Models (50+ entities)
│   ├── Enums/                        # Enumeration Types
│   └── Interfaces/                   # Service Contracts
│
├── TheLeague.Infrastructure/         # Data Access Layer
│   └── Data/
│       ├── ApplicationDbContext.cs   # EF Core Context with 50+ DbSets
│       ├── Migrations/               # Database Schema Migrations
│       └── TenantService.cs          # Multi-tenancy Implementation
│
├── TheLeague.Tests/                  # Test Projects
│
├── the-league-client/                # Angular Frontend (SPA)
│   └── src/app/
│       ├── core/                     # Services, Guards, Interceptors, Models
│       ├── features/                 # Feature Modules
│       │   ├── admin/                # Super Admin Portal
│       │   ├── auth/                 # Authentication (Login, Register, Password Reset)
│       │   ├── club/                 # Club Manager Portal
│       │   └── portal/               # Member Self-Service Portal
│       ├── shared/                   # Reusable Components & Pipes
│       └── layouts/                  # Page Layouts (Admin, Portal, Auth)
│
├── docs/                             # Documentation Assets
│   └── screenshots/                  # Application Screenshots
│
├── onboarding/                       # Developer Onboarding Guides (11 documents)
│
└── seedData.json                     # Demo Data Configuration
```

---

## API Documentation

Interactive API documentation is available via Swagger at:
```
http://localhost:7000/swagger
```

### Key Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/auth/login` | POST | User authentication |
| `/api/auth/refresh-token` | POST | Refresh JWT token |
| `/api/auth/register` | POST | User registration |
| `/api/members` | GET/POST | Member management |
| `/api/members/{id}` | GET/PUT/DELETE | Individual member operations |
| `/api/sessions` | GET/POST | Session management |
| `/api/sessions/{id}/book` | POST | Book a session |
| `/api/events` | GET/POST | Event management |
| `/api/events/{id}/register` | POST | Register for an event |
| `/api/payments` | GET/POST | Payment recording |
| `/api/invoices` | GET/POST | Invoice management |
| `/api/club/dashboard` | GET | Club dashboard data |
| `/api/portal/dashboard` | GET | Member portal dashboard |
| `/api/admin/clubs` | GET/POST | Platform club management |
| `/api/admin/dashboard` | GET | Admin platform dashboard |
| `/api/competitions` | GET/POST | Competition management |
| `/api/memberships` | GET/POST | Membership management |
| `/api/venues` | GET/POST | Venue management |
| `/api/reports/*` | GET | Various report endpoints |

---

## Demo Credentials

The platform includes seed data with the following demo accounts:

### Platform Administrator

| Role | Email | Password |
|------|-------|----------|
| Super Admin | admin@theleague.com | Admin123! |

### Club Managers

| Club | Email | Password |
|------|-------|----------|
| Teddington Cricket Club | chairman@teddingtoncc.com | Chairman123! |
| Highbury United FC | chairman@highburyunited.com | Chairman123! |
| Richmond Hockey Club | president@richmondhockey.org.uk | President123! |
| Marylebone Cricket Club | chairman@marylebone.com | Chairman123! |

### Members

| Club | Email | Password |
|------|-------|----------|
| Teddington CC | james.anderson@email.com | Member123! |
| Highbury FC | marcus.rashford@email.com | Member123! |
| Richmond HC | sam.ward@email.com | Member123! |

---

## Documentation

Comprehensive developer documentation is available in the `/onboarding` directory:

| Document | Description |
|----------|-------------|
| [00_START_HERE.md](onboarding/00_START_HERE.md) | Welcome and reading roadmap |
| [01_PROJECT_VISION.md](onboarding/01_PROJECT_VISION.md) | Business context and domain glossary |
| [02_LOCAL_SETUP.md](onboarding/02_LOCAL_SETUP.md) | Development environment setup |
| [03_ARCHITECTURE_OVERVIEW.md](onboarding/03_ARCHITECTURE_OVERVIEW.md) | System architecture |
| [04_BACKEND_GUIDE.md](onboarding/04_BACKEND_GUIDE.md) | API development guide |
| [05_DATABASE_GUIDE.md](onboarding/05_DATABASE_GUIDE.md) | Database and EF Core |
| [06_FRONTEND_GUIDE.md](onboarding/06_FRONTEND_GUIDE.md) | Angular development guide |
| [07_AUTHENTICATION_AND_SECURITY.md](onboarding/07_AUTHENTICATION_AND_SECURITY.md) | Security implementation |
| [08_KEY_FEATURES_WALKTHROUGH.md](onboarding/08_KEY_FEATURES_WALKTHROUGH.md) | Feature walkthroughs |
| [09_SUPPORT_KNOWLEDGE.md](onboarding/09_SUPPORT_KNOWLEDGE.md) | Troubleshooting guide |
| [10_DEVELOPMENT_WORKFLOW.md](onboarding/10_DEVELOPMENT_WORKFLOW.md) | Development workflow |

### Additional Documentation

- **`/project-docs/`** - Detailed feature documentation
- **`applicationFeatures.md`** - Complete feature inventory
- **`the-league-client/README.md`** - Angular CLI reference

---

## Domain Entities

The platform manages a rich domain model with 50+ entities:

### Core Entities

| Category | Entities |
|----------|----------|
| **Users & Clubs** | ApplicationUser, Club, ClubSettings |
| **Members** | Member, FamilyMember, MemberNote, MemberDocument |
| **Memberships** | Membership, MembershipType, MembershipDiscount, MembershipFreeze |
| **Sessions** | Session, SessionBooking, RecurringSchedule, Waitlist |
| **Events** | Event, EventRegistration |
| **Competitions** | Competition, Team, Match, Standings |
| **Financial** | Payment, PaymentPlan, Invoice, Refund, Fee, MemberBalance |
| **Facilities** | Venue, Facility, Equipment, GuestPass |
| **Audit** | ConfigurationAuditLog, FinancialAuditLog |

---

## Testing

### Backend Tests
```bash
cd TheLeague.Tests
dotnet test
```

### Frontend E2E Tests
```bash
cd the-league-client
npx playwright test
```

### Visual Regression Tests
```bash
cd the-league-client
npx playwright test --project=visual
```

---

## Supported Sports

The platform is designed to support various sports including:

- Cricket
- Football (Soccer)
- Hockey
- Rugby
- Tennis
- Swimming
- Athletics
- Golf
- And more...

Each club can be configured for their specific sport with appropriate terminology and features.

---

## License

This project is proprietary software. All rights reserved.

---

## Support

For enquiries regarding this project, please contact the development team.

---

*Built with enterprise-grade architecture for scalability, security, and maintainability.*
