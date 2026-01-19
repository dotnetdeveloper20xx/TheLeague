# The League - API Specification Document

## Overview

The League API is a RESTful web service built with ASP.NET Core 8.0. All endpoints return JSON responses and follow standard HTTP semantics.

### Base URL
- **Development:** `http://localhost:7000/api`
- **Production:** `https://api.theleague.com/api`

### Authentication
All protected endpoints require a valid JWT Bearer token in the Authorization header:
```
Authorization: Bearer <access_token>
```

### Response Format

**Success Response:**
```json
{
  "success": true,
  "message": null,
  "data": { ... }
}
```

**Error Response:**
```json
{
  "success": false,
  "message": "Error description",
  "errors": ["Validation error 1", "Validation error 2"],
  "data": null
}
```

### Pagination
List endpoints support pagination via query parameters:
- `page` (default: 1)
- `pageSize` (default: 20, max: 100)

**Paginated Response:**
```json
{
  "success": true,
  "data": {
    "items": [...],
    "totalCount": 150,
    "page": 1,
    "pageSize": 20,
    "totalPages": 8
  }
}
```

---

## Authentication Endpoints

### POST /api/auth/register
Register a new user account.

**Request Body:**
```json
{
  "email": "john.smith@example.com",
  "password": "SecurePass123",
  "confirmPassword": "SecurePass123",
  "firstName": "John",
  "lastName": "Smith",
  "clubId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "userId": "abc123",
    "email": "john.smith@example.com",
    "requiresEmailVerification": true
  }
}
```

---

### POST /api/auth/login
Authenticate and receive JWT tokens.

**Request Body:**
```json
{
  "email": "john.smith@example.com",
  "password": "SecurePass123"
}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2g...",
    "expiresAt": "2024-01-15T14:30:00Z",
    "user": {
      "id": "abc123",
      "email": "john.smith@example.com",
      "firstName": "John",
      "lastName": "Smith",
      "role": "Member",
      "clubId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "clubName": "Willow Creek CC"
    }
  }
}
```

---

### POST /api/auth/refresh
Refresh an expired access token.

**Request Body:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2g..."
}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "bmV3IHJlZnJlc2ggdG9rZW4...",
    "expiresAt": "2024-01-15T14:45:00Z"
  }
}
```

---

### POST /api/auth/forgot-password
Request a password reset email.

**Request Body:**
```json
{
  "email": "john.smith@example.com"
}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "message": "If the email exists, a reset link has been sent."
}
```

---

### POST /api/auth/reset-password
Reset password with token.

**Request Body:**
```json
{
  "email": "john.smith@example.com",
  "token": "reset-token-from-email",
  "newPassword": "NewSecurePass456",
  "confirmPassword": "NewSecurePass456"
}
```

---

### PUT /api/auth/change-password
Change password for authenticated user.

**Authorization:** Required (any role)

**Request Body:**
```json
{
  "currentPassword": "SecurePass123",
  "newPassword": "NewSecurePass456",
  "confirmPassword": "NewSecurePass456"
}
```

---

### GET /api/auth/me
Get current authenticated user profile.

**Authorization:** Required (any role)

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "id": "abc123",
    "email": "john.smith@example.com",
    "firstName": "John",
    "lastName": "Smith",
    "role": "ClubManager",
    "clubId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "clubName": "Willow Creek CC",
    "memberId": "guid-of-linked-member"
  }
}
```

---

## Super Admin Endpoints

### GET /api/admin/clubs
List all clubs in the system.

**Authorization:** SuperAdmin

**Query Parameters:**
- `search` - Search by name
- `isActive` - Filter by active status
- `page`, `pageSize` - Pagination

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "name": "Willow Creek Cricket Club",
        "slug": "willow-creek",
        "clubType": "Cricket",
        "isActive": true,
        "memberCount": 350,
        "createdAt": "2024-01-01T00:00:00Z"
      }
    ],
    "totalCount": 25,
    "page": 1,
    "pageSize": 20
  }
}
```

---

### POST /api/admin/clubs
Create a new club.

**Authorization:** SuperAdmin

**Request Body:**
```json
{
  "name": "Riverside Football Club",
  "slug": "riverside-fc",
  "description": "Community football club",
  "clubType": "Football",
  "contactEmail": "info@riversidefc.com",
  "contactPhone": "01onal1 123456",
  "address": "123 Riverside Park, London",
  "primaryColor": "#1E40AF",
  "secondaryColor": "#3B82F6"
}
```

---

### GET /api/admin/clubs/{id}
Get detailed club information.

**Authorization:** SuperAdmin

---

### PUT /api/admin/clubs/{id}
Update club details.

**Authorization:** SuperAdmin

---

### GET /api/admin/dashboard
Get system-wide dashboard statistics.

**Authorization:** SuperAdmin

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "totalClubs": 25,
    "activeClubs": 23,
    "totalMembers": 8500,
    "totalRevenue": 125000.00,
    "recentClubs": [...],
    "systemHealth": "Healthy"
  }
}
```

---

## Club Management Endpoints

### GET /api/club/profile
Get current club profile.

**Authorization:** ClubManager

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "Willow Creek Cricket Club",
    "slug": "willow-creek",
    "description": "Founded in 1892...",
    "logoUrl": "/uploads/clubs/willow-creek/logo.png",
    "primaryColor": "#1E40AF",
    "secondaryColor": "#3B82F6",
    "contactEmail": "info@willowcreekcc.com",
    "contactPhone": "01onal1 987654",
    "address": "Cricket Ground Lane, Willow Creek",
    "website": "https://willowcreekcc.com",
    "clubType": "Cricket",
    "settings": {
      "allowOnlineRegistration": true,
      "requireEmergencyContact": true,
      "allowOnlinePayments": true,
      "enableWaitlist": true
    }
  }
}
```

---

### PUT /api/club/profile
Update club profile.

**Authorization:** ClubManager

---

### PUT /api/club/settings
Update club settings.

**Authorization:** ClubManager

**Request Body:**
```json
{
  "allowOnlineRegistration": true,
  "requireEmergencyContact": true,
  "requireMedicalInfo": false,
  "allowFamilyAccounts": true,
  "allowOnlinePayments": true,
  "autoSendPaymentReminders": true,
  "paymentReminderDaysBefore": 14,
  "maxAdvanceBookingDays": 30,
  "enableWaitlist": true
}
```

---

### GET /api/club/dashboard
Get club dashboard data.

**Authorization:** ClubManager

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "memberStats": {
      "total": 350,
      "active": 320,
      "pending": 15,
      "expired": 15
    },
    "financialStats": {
      "revenueThisMonth": 12500.00,
      "revenueThisYear": 85000.00,
      "outstandingPayments": 3200.00,
      "overdueInvoices": 8
    },
    "sessionStats": {
      "upcomingSessions": 12,
      "totalBookingsThisWeek": 145,
      "averageAttendance": 78.5
    },
    "recentMembers": [...],
    "upcomingEvents": [...],
    "recentPayments": [...]
  }
}
```

---

## Member Endpoints

### GET /api/members
List club members with filtering.

**Authorization:** ClubManager

**Query Parameters:**
- `search` - Search name, email, member number
- `status` - Filter by status (Active, Pending, Expired)
- `membershipTypeId` - Filter by membership type
- `page`, `pageSize` - Pagination

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "member-guid",
        "memberNumber": "MBR-001",
        "firstName": "John",
        "lastName": "Smith",
        "email": "john.smith@example.com",
        "phone": "07700 900123",
        "status": "Active",
        "membershipType": "Senior",
        "joinedDate": "2024-01-01T00:00:00Z",
        "profilePhotoUrl": "/uploads/members/photo.jpg"
      }
    ],
    "totalCount": 350
  }
}
```

---

### POST /api/members
Create a new member (admin registration).

**Authorization:** ClubManager

**Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Smith",
  "email": "john.smith@example.com",
  "phone": "07700 900123",
  "dateOfBirth": "1985-06-15",
  "gender": "Male",
  "address": "123 High Street",
  "city": "London",
  "postCode": "SW1A 1AA",
  "emergencyContactName": "Jane Smith",
  "emergencyContactPhone": "07700 900456",
  "emergencyContactRelation": "Spouse",
  "membershipTypeId": "membership-type-guid",
  "createUserAccount": true
}
```

---

### GET /api/members/{id}
Get detailed member profile.

**Authorization:** ClubManager

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "id": "member-guid",
    "memberNumber": "MBR-001",
    "firstName": "John",
    "lastName": "Smith",
    "email": "john.smith@example.com",
    "phone": "07700 900123",
    "dateOfBirth": "1985-06-15",
    "age": 39,
    "gender": "Male",
    "address": "123 High Street",
    "city": "London",
    "postCode": "SW1A 1AA",
    "status": "Active",
    "joinedDate": "2024-01-01T00:00:00Z",
    "emergencyContact": {
      "name": "Jane Smith",
      "phone": "07700 900456",
      "relation": "Spouse"
    },
    "currentMembership": {
      "id": "membership-guid",
      "type": "Senior",
      "startDate": "2024-01-01",
      "endDate": "2024-12-31",
      "status": "Active"
    },
    "familyMembers": [
      {
        "id": "family-member-guid",
        "firstName": "Tom",
        "lastName": "Smith",
        "relation": "Child",
        "dateOfBirth": "2012-03-20"
      }
    ],
    "recentBookings": [...],
    "paymentSummary": {
      "totalPaid": 450.00,
      "outstanding": 0
    }
  }
}
```

---

### PUT /api/members/{id}
Update member details.

**Authorization:** ClubManager

---

### DELETE /api/members/{id}
Deactivate a member (soft delete).

**Authorization:** ClubManager

---

### GET /api/members/{id}/family
Get family members for a primary account.

**Authorization:** ClubManager or Member (own profile)

---

### POST /api/members/{id}/family
Add a family member.

**Authorization:** ClubManager or Member (own profile)

**Request Body:**
```json
{
  "firstName": "Tom",
  "lastName": "Smith",
  "dateOfBirth": "2012-03-20",
  "relation": "Child",
  "medicalConditions": null,
  "allergies": null
}
```

---

## Membership Endpoints

### GET /api/membership-types
List available membership types.

**Authorization:** ClubManager

**Response:** `200 OK`
```json
{
  "success": true,
  "data": [
    {
      "id": "type-guid",
      "name": "Senior",
      "description": "Full adult membership",
      "basePrice": 180.00,
      "monthlyFee": 18.00,
      "minAge": 18,
      "maxAge": null,
      "isActive": true,
      "includesBooking": true,
      "includesEvents": true
    }
  ]
}
```

---

### POST /api/membership-types
Create a new membership type.

**Authorization:** ClubManager

---

### GET /api/memberships
List all memberships.

**Authorization:** ClubManager

**Query Parameters:**
- `status` - Filter by status
- `membershipTypeId` - Filter by type
- `expiringWithinDays` - Filter expiring soon

---

### POST /api/memberships
Create a membership for a member.

**Authorization:** ClubManager

**Request Body:**
```json
{
  "memberId": "member-guid",
  "membershipTypeId": "type-guid",
  "startDate": "2024-01-01",
  "endDate": "2024-12-31",
  "paymentType": "Annual",
  "autoRenew": false
}
```

---

### POST /api/memberships/{id}/renew
Renew an existing membership.

**Authorization:** ClubManager or Member (own membership)

---

## Session Endpoints

### GET /api/sessions
List sessions with date filtering.

**Authorization:** ClubManager or Member

**Query Parameters:**
- `startDate` - Filter from date
- `endDate` - Filter to date
- `category` - Filter by category
- `venueId` - Filter by venue
- `includeFullyBooked` - Include sessions at capacity

**Response:** `200 OK`
```json
{
  "success": true,
  "data": [
    {
      "id": "session-guid",
      "title": "U13s Training",
      "description": "Weekly training session",
      "category": "U13",
      "startTime": "2024-01-15T18:00:00Z",
      "endTime": "2024-01-15T19:30:00Z",
      "venue": {
        "id": "venue-guid",
        "name": "Main Ground"
      },
      "capacity": 25,
      "currentBookings": 18,
      "availableSpots": 7,
      "sessionFee": null,
      "isRecurring": true,
      "isCancelled": false
    }
  ]
}
```

---

### POST /api/sessions
Create a new session.

**Authorization:** ClubManager

**Request Body:**
```json
{
  "title": "U13s Training",
  "description": "Weekly training session",
  "category": "U13",
  "venueId": "venue-guid",
  "startTime": "2024-01-15T18:00:00Z",
  "endTime": "2024-01-15T19:30:00Z",
  "capacity": 25,
  "sessionFee": null
}
```

---

### GET /api/sessions/{id}
Get session details with bookings.

**Authorization:** ClubManager or Member

---

### POST /api/sessions/{id}/book
Book a session for self or family member.

**Authorization:** Member

**Request Body:**
```json
{
  "familyMemberId": null
}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "bookingId": "booking-guid",
    "sessionId": "session-guid",
    "status": "Confirmed",
    "bookedAt": "2024-01-10T10:30:00Z"
  }
}
```

---

### DELETE /api/sessions/{id}/book
Cancel a session booking.

**Authorization:** Member (own booking) or ClubManager

---

### POST /api/sessions/{id}/waitlist
Join session waitlist.

**Authorization:** Member

---

### GET /api/sessions/{id}/attendance
Get attendance list for a session.

**Authorization:** ClubManager

---

### POST /api/sessions/{id}/attendance
Mark attendance for session.

**Authorization:** ClubManager

**Request Body:**
```json
{
  "bookingId": "booking-guid",
  "attended": true,
  "checkedInAt": "2024-01-15T17:55:00Z"
}
```

---

## Event Endpoints

### GET /api/events
List events.

**Authorization:** ClubManager or Member

**Query Parameters:**
- `startDate`, `endDate` - Date range filter
- `type` - Event type filter
- `includePublished` - Published events only

---

### POST /api/events
Create a new event.

**Authorization:** ClubManager

**Request Body:**
```json
{
  "title": "Annual Awards Night",
  "description": "Celebrating our 2024 season achievements",
  "type": "Social",
  "venueId": "venue-guid",
  "startDateTime": "2024-12-14T19:00:00Z",
  "endDateTime": "2024-12-14T23:00:00Z",
  "capacity": 150,
  "isTicketed": true,
  "ticketPrice": 35.00,
  "memberTicketPrice": 25.00,
  "ticketSalesEndDate": "2024-12-10T23:59:59Z",
  "imageUrl": null
}
```

---

### GET /api/events/{id}
Get event details.

**Authorization:** ClubManager or Member

---

### POST /api/events/{id}/rsvp
RSVP to an event.

**Authorization:** Member

**Request Body:**
```json
{
  "response": "Attending",
  "guestCount": 1,
  "dietaryRequirements": "Vegetarian",
  "notes": null
}
```

---

### POST /api/events/{id}/purchase
Purchase event tickets.

**Authorization:** Member

**Request Body:**
```json
{
  "quantity": 2,
  "paymentMethodId": "pm_stripe_method_id"
}
```

---

## Payment Endpoints

### GET /api/payments
List payments.

**Authorization:** ClubManager

**Query Parameters:**
- `memberId` - Filter by member
- `status` - Filter by status
- `method` - Filter by payment method
- `startDate`, `endDate` - Date range

---

### POST /api/payments/manual
Record a manual payment (cash, bank transfer).

**Authorization:** ClubManager

**Request Body:**
```json
{
  "memberId": "member-guid",
  "amount": 180.00,
  "method": "BankTransfer",
  "type": "Membership",
  "description": "Senior membership 2024",
  "membershipId": "membership-guid",
  "reference": "Bank ref: ABC123",
  "paymentDate": "2024-01-05"
}
```

---

### POST /api/payments/stripe/create-intent
Create a Stripe payment intent.

**Authorization:** Member

**Request Body:**
```json
{
  "amount": 180.00,
  "membershipId": "membership-guid"
}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "clientSecret": "pi_xxx_secret_xxx",
    "paymentIntentId": "pi_xxx"
  }
}
```

---

### POST /api/payments/{id}/refund
Process a refund.

**Authorization:** ClubManager

**Request Body:**
```json
{
  "amount": 50.00,
  "reason": "Partial refund for cancelled event"
}
```

---

## Invoice Endpoints

### GET /api/invoices
List invoices.

**Authorization:** ClubManager

**Query Parameters:**
- `memberId` - Filter by member
- `status` - Filter by status (Draft, Sent, Paid, Overdue)

---

### POST /api/invoices
Create an invoice.

**Authorization:** ClubManager

**Request Body:**
```json
{
  "memberId": "member-guid",
  "dueDate": "2024-02-01",
  "lineItems": [
    {
      "description": "Senior Membership 2024",
      "quantity": 1,
      "unitPrice": 180.00,
      "feeId": "fee-guid"
    }
  ],
  "notes": "Thank you for renewing your membership"
}
```

---

### POST /api/invoices/{id}/send
Send invoice via email.

**Authorization:** ClubManager

---

### POST /api/invoices/{id}/void
Void an invoice.

**Authorization:** ClubManager

---

## Venue Endpoints

### GET /api/venues
List club venues.

**Authorization:** ClubManager or Member

---

### POST /api/venues
Create a new venue.

**Authorization:** ClubManager

**Request Body:**
```json
{
  "name": "Main Ground",
  "description": "Primary cricket ground",
  "address": "Cricket Lane, Willow Creek",
  "postCode": "WC1 1CC",
  "capacity": 500,
  "facilities": "Pavilion, Nets, Scoreboard",
  "isPrimary": true
}
```

---

## Competition Endpoints

### GET /api/competitions
List competitions.

**Authorization:** ClubManager

---

### POST /api/competitions
Create a competition.

**Authorization:** ClubManager

**Request Body:**
```json
{
  "name": "Summer League 2024",
  "description": "10-team round-robin league",
  "type": "League",
  "format": "RoundRobin",
  "startDate": "2024-04-01",
  "endDate": "2024-09-30",
  "entryFee": 50.00,
  "pointsForWin": 3,
  "pointsForDraw": 1
}
```

---

### POST /api/competitions/{id}/teams
Register a team.

**Authorization:** ClubManager

---

### POST /api/competitions/{id}/generate-fixtures
Generate match fixtures.

**Authorization:** ClubManager

---

### GET /api/competitions/{id}/standings
Get league standings.

**Authorization:** ClubManager or Member

**Response:** `200 OK`
```json
{
  "success": true,
  "data": [
    {
      "position": 1,
      "team": {
        "id": "team-guid",
        "name": "Team A"
      },
      "played": 10,
      "won": 8,
      "drawn": 1,
      "lost": 1,
      "goalsFor": 25,
      "goalsAgainst": 10,
      "goalDifference": 15,
      "points": 25
    }
  ]
}
```

---

### POST /api/competitions/matches/{id}/result
Record match result.

**Authorization:** ClubManager

**Request Body:**
```json
{
  "homeScore": 3,
  "awayScore": 1,
  "status": "Completed",
  "notes": "Great match"
}
```

---

## Reports Endpoints

### GET /api/reports/membership-stats
Get membership statistics.

**Authorization:** ClubManager

---

### GET /api/reports/financial-summary
Get financial summary.

**Authorization:** ClubManager

**Query Parameters:**
- `startDate`, `endDate` - Report period

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "totalRevenue": 85000.00,
    "revenueByType": {
      "Membership": 65000.00,
      "Events": 15000.00,
      "Sessions": 5000.00
    },
    "revenueByMethod": {
      "Stripe": 70000.00,
      "PayPal": 5000.00,
      "BankTransfer": 8000.00,
      "Cash": 2000.00
    },
    "outstandingTotal": 3200.00,
    "monthlyTrend": [...]
  }
}
```

---

### GET /api/reports/attendance
Get attendance reports.

**Authorization:** ClubManager

---

## Member Portal Endpoints

### GET /api/portal/dashboard
Get member's portal dashboard.

**Authorization:** Member

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "member": {
      "firstName": "John",
      "lastName": "Smith",
      "memberNumber": "MBR-001",
      "profilePhotoUrl": "/uploads/photo.jpg"
    },
    "membership": {
      "type": "Senior",
      "status": "Active",
      "expiresOn": "2024-12-31"
    },
    "upcomingBookings": [...],
    "upcomingEvents": [...],
    "balance": {
      "outstanding": 0,
      "nextPaymentDue": null
    },
    "familyMembers": [...]
  }
}
```

---

### GET /api/portal/sessions
Get available sessions for booking.

**Authorization:** Member

---

### GET /api/portal/bookings
Get member's bookings.

**Authorization:** Member

---

### GET /api/portal/payments
Get member's payment history.

**Authorization:** Member

---

### GET /api/portal/family
Get member's family members.

**Authorization:** Member

---

## System Configuration Endpoints

### GET /api/system-configuration
Get system configuration.

**Authorization:** SuperAdmin

---

### PUT /api/system-configuration/payment
Update payment provider configuration.

**Authorization:** SuperAdmin

---

### PUT /api/system-configuration/email
Update email provider configuration.

**Authorization:** SuperAdmin

---

### PUT /api/system-configuration/features
Update feature flags.

**Authorization:** SuperAdmin

---

### GET /api/system-configuration/audit-log
Get configuration audit log.

**Authorization:** SuperAdmin

---

## Error Codes

| HTTP Status | Meaning |
|-------------|---------|
| 200 | Success |
| 201 | Created |
| 400 | Bad Request (validation error) |
| 401 | Unauthorized (not authenticated) |
| 403 | Forbidden (not authorized for resource) |
| 404 | Not Found |
| 409 | Conflict (duplicate, booking clash) |
| 422 | Unprocessable Entity (business rule violation) |
| 500 | Internal Server Error |

---

*Document Version: 1.0*
*Last Updated: Pre-Development Planning Phase*
