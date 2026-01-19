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
- [Architecture](#architecture)
- [Technology Stack](#technology-stack)
- [Screenshots](#screenshots)
  - [Authentication](#authentication)
  - [Super Admin Portal](#super-admin-portal)
  - [Club Manager Portal](#club-manager-portal)
  - [Member Portal](#member-portal)
  - [Forms](#forms)
  - [Responsive Design](#responsive-design)
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

### For Club Managers
- **Member Management** - Complete member lifecycle from registration to renewal
- **Session Scheduling** - Training sessions with booking and attendance
- **Event Planning** - Club events with ticketing and RSVPs
- **Payment Processing** - Record payments, generate invoices, track outstanding balances
- **Membership Types** - Configurable membership categories with pricing
- **Venue Management** - Facility booking and resource allocation
- **Reporting** - Financial and membership analytics
- **Club Settings** - Branding, notifications, and operational configuration

### For Members
- **Self-Service Portal** - View and update personal information
- **Session Booking** - Browse and book available training sessions
- **Event Registration** - Discover and register for club events
- **Payment History** - View invoices and payment records
- **Family Management** - Manage family member accounts
- **Notifications** - Stay informed about club activities

---

## Architecture

The platform follows **Clean Architecture** principles with clear separation of concerns:

```
┌─────────────────────────────────────────────────────────────────┐
│                         Presentation                             │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │              Angular 19 SPA (Standalone Components)          ││
│  │    Tailwind CSS | RxJS | Reactive Forms | Route Guards       ││
│  └─────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────┘
                                  │
                            HTTP/REST API
                                  │
┌─────────────────────────────────────────────────────────────────┐
│                        Application Layer                         │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │                    ASP.NET Core 8 Web API                    ││
│  │   Controllers | Services | DTOs | Middleware | Providers     ││
│  └─────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────┘
                                  │
┌─────────────────────────────────────────────────────────────────┐
│                         Domain Layer                             │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │           Entities | Enums | Business Rules                  ││
│  └─────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────┘
                                  │
┌─────────────────────────────────────────────────────────────────┐
│                      Infrastructure Layer                        │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │    Entity Framework Core 8 | SQL Server | Tenant Service     ││
│  └─────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────┘
```

### Security Model

- **JWT Authentication** with refresh token rotation
- **Role-Based Access Control** (SuperAdmin, ClubManager, Member, Coach, Staff)
- **Tenant Isolation** via ClubId discriminator on all queries
- **CORS Protection** with configurable origins
- **Password Policy** enforcement via ASP.NET Identity

---

## Technology Stack

### Backend
| Technology | Version | Purpose |
|------------|---------|---------|
| ASP.NET Core | 8.0 | Web API Framework |
| Entity Framework Core | 8.0 | ORM / Data Access |
| SQL Server | 2022 | Database |
| ASP.NET Identity | 8.0 | Authentication |
| JWT Bearer | - | Token Authentication |
| Swagger/OpenAPI | - | API Documentation |

### Frontend
| Technology | Version | Purpose |
|------------|---------|---------|
| Angular | 19.2 | SPA Framework |
| TypeScript | 5.7 | Type-safe JavaScript |
| Tailwind CSS | 3.4 | Utility-first CSS |
| RxJS | 7.8 | Reactive Extensions |
| Playwright | - | E2E Testing |

### External Integrations
| Provider | Purpose | Status |
|----------|---------|--------|
| Stripe | Payment Processing | Mock/Production |
| PayPal | Payment Processing | Mock/Production |
| SendGrid | Email Delivery | Mock/Production |

---

## Screenshots

### Authentication

<details>
<summary><strong>Login Page</strong></summary>

![Login Page](docs/screenshots/24-login-page.png)

*Secure login with email and password authentication*
</details>

<details>
<summary><strong>Registration Page</strong></summary>

![Registration Page](docs/screenshots/25-register-page.png)

*New user registration with validation*
</details>

<details>
<summary><strong>Forgot Password</strong></summary>

![Forgot Password](docs/screenshots/26-forgot-password.png)

*Password reset request flow*
</details>

---

### Super Admin Portal

Platform administrators have access to system-wide management capabilities.

<details>
<summary><strong>Admin Dashboard</strong></summary>

![Admin Dashboard](docs/screenshots/01-admin-dashboard.png)

*Platform overview with key metrics across all clubs*
</details>

<details>
<summary><strong>Clubs Management</strong></summary>

![Clubs List](docs/screenshots/02-admin-clubs-list.png)

*Manage all sports clubs on the platform*
</details>

<details>
<summary><strong>Users Management</strong></summary>

![Users Management](docs/screenshots/03-admin-users.png)

*System-wide user administration*
</details>

<details>
<summary><strong>Platform Reports</strong></summary>

![Admin Reports](docs/screenshots/04-admin-reports.png)

*Cross-platform analytics and reporting*
</details>

<details>
<summary><strong>System Settings</strong></summary>

![Admin Settings](docs/screenshots/05-admin-settings.png)

*Global platform configuration*
</details>

---

### Club Manager Portal

Club managers have comprehensive tools for managing their organisation.

<details>
<summary><strong>Club Dashboard</strong></summary>

![Club Dashboard](docs/screenshots/06-club-dashboard.png)

*Club overview with member counts, upcoming sessions, and recent activity*
</details>

<details>
<summary><strong>Members List</strong></summary>

![Members List](docs/screenshots/07a-club-members-list.png)

*Comprehensive member directory with search and filtering*
</details>

<details>
<summary><strong>Member Search</strong></summary>

![Member Search](docs/screenshots/07b-club-members-search.png)

*Advanced member search functionality*
</details>

<details>
<summary><strong>Member Detail</strong></summary>

![Member Detail](docs/screenshots/08-club-member-detail.png)

*Complete member profile with history and relationships*
</details>

<details>
<summary><strong>Sessions Management</strong></summary>

![Sessions List](docs/screenshots/09a-club-sessions-list.png)

*Training session scheduling and management*
</details>

<details>
<summary><strong>Events Management</strong></summary>

![Events List](docs/screenshots/10-club-events-list.png)

*Club event planning and coordination*
</details>

<details>
<summary><strong>Payments</strong></summary>

![Payments](docs/screenshots/11-club-payments.png)

*Payment recording and financial tracking*
</details>

<details>
<summary><strong>Memberships</strong></summary>

![Memberships](docs/screenshots/12-club-memberships.png)

*Active membership management*
</details>

<details>
<summary><strong>Membership Types</strong></summary>

![Membership Types](docs/screenshots/13-club-membership-types.png)

*Configurable membership categories and pricing*
</details>

<details>
<summary><strong>Venues</strong></summary>

![Venues](docs/screenshots/14-club-venues.png)

*Facility and venue management*
</details>

<details>
<summary><strong>Club Reports</strong></summary>

![Club Reports](docs/screenshots/15-club-reports.png)

*Club-specific analytics and reporting*
</details>

<details>
<summary><strong>Club Settings</strong></summary>

![Club Settings](docs/screenshots/16-club-settings.png)

*Club configuration and branding*
</details>

---

### Member Portal

Members have access to a self-service portal for managing their club activities.

<details>
<summary><strong>Portal Dashboard</strong></summary>

![Portal Dashboard](docs/screenshots/17-portal-dashboard.png)

*Member's personal dashboard*
</details>

<details>
<summary><strong>Available Sessions</strong></summary>

![Portal Sessions](docs/screenshots/18-portal-sessions.png)

*Browse and book training sessions*
</details>

<details>
<summary><strong>Upcoming Events</strong></summary>

![Portal Events](docs/screenshots/19-portal-events.png)

*Discover and register for club events*
</details>

<details>
<summary><strong>Payment History</strong></summary>

![Portal Payments](docs/screenshots/20-portal-payments.png)

*View invoices and payment records*
</details>

<details>
<summary><strong>Family Members</strong></summary>

![Portal Family](docs/screenshots/21-portal-family.png)

*Manage family member accounts*
</details>

<details>
<summary><strong>My Profile</strong></summary>

![Portal Profile](docs/screenshots/22-portal-profile.png)

*View and update personal information*
</details>

<details>
<summary><strong>Portal Settings</strong></summary>

![Portal Settings](docs/screenshots/23-portal-settings.png)

*Personal preferences and notifications*
</details>

---

### Forms

Data entry forms with validation and user-friendly interfaces.

<details>
<summary><strong>Create Member Form</strong></summary>

![Create Member Form](docs/screenshots/27-create-member-form.png)

*Comprehensive member registration form*
</details>

<details>
<summary><strong>Create Session Form</strong></summary>

![Create Session Form](docs/screenshots/28-create-session-form.png)

*Training session creation with capacity and pricing*
</details>

<details>
<summary><strong>Create Event Form</strong></summary>

![Create Event Form](docs/screenshots/29-create-event-form.png)

*Event planning with details and ticketing*
</details>

---

### Responsive Design

The platform is fully responsive across all device sizes.

<details>
<summary><strong>Mobile - Login</strong></summary>

![Mobile Login](docs/screenshots/36-mobile-login.png)

*Mobile-optimised login experience*
</details>

<details>
<summary><strong>Mobile - Dashboard</strong></summary>

![Mobile Dashboard](docs/screenshots/37-mobile-dashboard.png)

*Touch-friendly mobile dashboard*
</details>

<details>
<summary><strong>Tablet - Members</strong></summary>

![Tablet Members](docs/screenshots/38-tablet-members.png)

*Tablet-optimised member management*
</details>

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

---

## Project Structure

```
LeagueMembershipManagementPortal/
├── TheLeague.Api/                    # ASP.NET Core Web API
│   ├── Controllers/                  # API Endpoints
│   ├── Services/                     # Business Logic
│   ├── DTOs/                         # Data Transfer Objects
│   ├── Middleware/                   # Request Pipeline
│   ├── Providers/                    # External Integrations
│   └── Program.cs                    # Application Entry Point
│
├── TheLeague.Core/                   # Domain Layer
│   ├── Entities/                     # Domain Models
│   └── Enums/                        # Enumeration Types
│
├── TheLeague.Infrastructure/         # Data Access Layer
│   └── Data/
│       ├── ApplicationDbContext.cs   # EF Core Context
│       ├── Migrations/               # Database Migrations
│       └── TenantService.cs          # Multi-tenancy Support
│
├── TheLeague.Tests/                  # Test Projects
│
├── the-league-client/                # Angular Frontend
│   └── src/app/
│       ├── core/                     # Services, Guards, Interceptors
│       ├── features/                 # Feature Modules
│       ├── shared/                   # Reusable Components
│       └── layouts/                  # Page Layouts
│
├── docs/                             # Documentation Assets
│   └── screenshots/                  # Application Screenshots
│
├── onboarding/                       # Developer Onboarding Guides
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
| `/api/members` | GET/POST | Member management |
| `/api/members/{id}` | GET/PUT/DELETE | Individual member operations |
| `/api/sessions` | GET/POST | Session management |
| `/api/sessions/{id}/book` | POST | Book a session |
| `/api/events` | GET/POST | Event management |
| `/api/payments` | GET/POST | Payment recording |
| `/api/club/dashboard` | GET | Club dashboard data |
| `/api/admin/clubs` | GET/POST | Platform club management |

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
| Highbury United FC | president@highburyfc.com | President123! |
| Richmond Hockey Club | secretary@richmondhc.com | Secretary123! |
| Wimbledon Rugby Club | chairman@wimbledonrfc.com | Chairman123! |

### Members
| Club | Email | Password |
|------|-------|----------|
| Teddington CC | james.anderson@email.com | Member123! |
| Highbury FC | marcus.rashford@email.com | Member123! |

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

---

## License

This project is proprietary software. All rights reserved.

---

## Contact

For enquiries regarding this project, please contact the development team.

---

*Built with enterprise-grade architecture for scalability, security, and maintainability.*
