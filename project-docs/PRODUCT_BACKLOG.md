# The League - Product Backlog

## Overview

This document contains the complete product backlog organized by Epic. Each user story follows the format:
- **Title:** Brief description
- **User Story:** As a [role], I want [feature], so that [benefit]
- **Acceptance Criteria:** Specific testable requirements
- **Story Points:** Relative effort estimate (1, 2, 3, 5, 8, 13)
- **Priority:** Must Have / Should Have / Could Have / Won't Have (MoSCoW)

---

## Epic 1: Authentication & User Management

### US-1.1: User Registration
**Priority:** Must Have | **Story Points:** 5

**User Story:**
As a prospective member, I want to create an account with my email, so that I can access the member portal.

**Acceptance Criteria:**
- [ ] Registration form collects email, password, first name, last name
- [ ] Password must be minimum 6 characters with uppercase, lowercase, and digit
- [ ] Email must be unique in the system
- [ ] Confirmation password must match
- [ ] User receives verification email (if enabled)
- [ ] User cannot login until email is verified (if enabled)
- [ ] Success message displays after registration

---

### US-1.2: User Login
**Priority:** Must Have | **Story Points:** 3

**User Story:**
As a registered user, I want to log in with my email and password, so that I can access my account.

**Acceptance Criteria:**
- [ ] Login form accepts email and password
- [ ] Invalid credentials display appropriate error message
- [ ] Successful login redirects to role-appropriate dashboard
- [ ] JWT access token stored securely (memory/localStorage)
- [ ] Session persists across page refreshes

---

### US-1.3: Token Refresh
**Priority:** Must Have | **Story Points:** 3

**User Story:**
As a logged-in user, I want my session to remain active, so that I don't have to log in repeatedly.

**Acceptance Criteria:**
- [ ] Access token refreshed automatically before expiry
- [ ] Refresh token used to obtain new access token
- [ ] Failed refresh redirects to login page
- [ ] Expired refresh token requires re-authentication

---

### US-1.4: Password Reset
**Priority:** Must Have | **Story Points:** 3

**User Story:**
As a user who forgot my password, I want to reset it via email, so that I can regain access to my account.

**Acceptance Criteria:**
- [ ] Forgot password form accepts email address
- [ ] Reset email sent with unique token link
- [ ] Token valid for 24 hours
- [ ] Reset form validates new password
- [ ] Success message after password change
- [ ] Old password invalidated

---

### US-1.5: Change Password
**Priority:** Should Have | **Story Points:** 2

**User Story:**
As a logged-in user, I want to change my password, so that I can maintain account security.

**Acceptance Criteria:**
- [ ] Form requires current password and new password
- [ ] New password must meet complexity requirements
- [ ] Confirmation password must match
- [ ] Success notification displayed

---

### US-1.6: Role-Based Access Control
**Priority:** Must Have | **Story Points:** 5

**User Story:**
As a system administrator, I want users to have appropriate access based on their role, so that data is protected.

**Acceptance Criteria:**
- [ ] SuperAdmin can access all system areas
- [ ] ClubManager can only access assigned club data
- [ ] Member can only access self-service portal
- [ ] Unauthorized access attempts show error page
- [ ] Role stored in JWT claims

---

## Epic 2: Club Management

### US-2.1: Club Creation (Admin)
**Priority:** Must Have | **Story Points:** 5

**User Story:**
As a Super Admin, I want to create new clubs, so that organizations can join the platform.

**Acceptance Criteria:**
- [ ] Form captures club name, slug, type, contact details
- [ ] Slug must be unique and URL-friendly
- [ ] Logo upload supported (file or URL)
- [ ] Brand colors configurable
- [ ] Club created in active status

---

### US-2.2: Club Listing (Admin)
**Priority:** Must Have | **Story Points:** 3

**User Story:**
As a Super Admin, I want to view all clubs, so that I can manage the platform.

**Acceptance Criteria:**
- [ ] Paginated list of all clubs
- [ ] Search by club name
- [ ] Filter by active/inactive status
- [ ] Filter by club type
- [ ] Display member count per club

---

### US-2.3: Club Profile Management
**Priority:** Must Have | **Story Points:** 3

**User Story:**
As a Club Manager, I want to update my club's profile, so that members see accurate information.

**Acceptance Criteria:**
- [ ] Edit club name, description, contact details
- [ ] Upload/change club logo
- [ ] Update brand colors
- [ ] Changes reflect immediately

---

### US-2.4: Club Settings Configuration
**Priority:** Should Have | **Story Points:** 5

**User Story:**
As a Club Manager, I want to configure club settings, so that the system works for my club's needs.

**Acceptance Criteria:**
- [ ] Toggle online registration
- [ ] Configure emergency contact requirements
- [ ] Set booking advance window (days)
- [ ] Configure cancellation notice period
- [ ] Enable/disable waitlist
- [ ] Configure payment reminders

---

### US-2.5: Club Dashboard
**Priority:** Must Have | **Story Points:** 8

**User Story:**
As a Club Manager, I want to see a dashboard with key metrics, so that I can understand club performance.

**Acceptance Criteria:**
- [ ] Display total member count by status
- [ ] Show revenue this month/year
- [ ] List upcoming sessions and bookings
- [ ] Show upcoming events
- [ ] Display outstanding payments total
- [ ] Show recent member activity

---

## Epic 3: Member Management

### US-3.1: Member Creation
**Priority:** Must Have | **Story Points:** 5

**User Story:**
As a Club Manager, I want to add new members manually, so that I can register people who sign up in person.

**Acceptance Criteria:**
- [ ] Form captures personal details (name, email, phone, DOB, address)
- [ ] Emergency contact section
- [ ] Medical information section (optional)
- [ ] Option to create user account
- [ ] Member number auto-generated
- [ ] Success message with member number

---

### US-3.2: Member Listing
**Priority:** Must Have | **Story Points:** 5

**User Story:**
As a Club Manager, I want to view and search all members, so that I can find member information quickly.

**Acceptance Criteria:**
- [ ] Paginated member list with photo, name, status
- [ ] Search by name, email, member number
- [ ] Filter by status (Active, Pending, Expired)
- [ ] Filter by membership type
- [ ] Sort by name, join date, status
- [ ] Export to Excel/CSV

---

### US-3.3: Member Profile View
**Priority:** Must Have | **Story Points:** 5

**User Story:**
As a Club Manager, I want to view complete member details, so that I can access all their information.

**Acceptance Criteria:**
- [ ] Display all personal information
- [ ] Show current membership details
- [ ] List family members
- [ ] Show booking history
- [ ] Display payment history
- [ ] Show internal notes
- [ ] Display attendance statistics

---

### US-3.4: Member Profile Edit
**Priority:** Must Have | **Story Points:** 3

**User Story:**
As a Club Manager, I want to update member information, so that records stay accurate.

**Acceptance Criteria:**
- [ ] Edit all personal fields
- [ ] Update contact information
- [ ] Change member status
- [ ] Add/edit emergency contacts
- [ ] Update medical information

---

### US-3.5: Family Member Management
**Priority:** Should Have | **Story Points:** 5

**User Story:**
As a Club Manager, I want to add family members to an account, so that families can share a membership.

**Acceptance Criteria:**
- [ ] Add family members (spouse, children, etc.)
- [ ] Capture name, DOB, relationship
- [ ] Optional medical information
- [ ] Family members can book sessions
- [ ] Family members appear in primary's profile

---

### US-3.6: Member Notes
**Priority:** Should Have | **Story Points:** 3

**User Story:**
As a Club Manager, I want to add internal notes to member profiles, so that I can record important information.

**Acceptance Criteria:**
- [ ] Add notes with category (General, Medical, Payment, etc.)
- [ ] Notes timestamped with author
- [ ] Notes visible only to staff
- [ ] Notes displayed in chronological order

---

### US-3.7: Member Status Management
**Priority:** Must Have | **Story Points:** 3

**User Story:**
As a Club Manager, I want to change member status, so that I can manage active and inactive members.

**Acceptance Criteria:**
- [ ] Set status: Active, Pending, Suspended, Expired
- [ ] Require reason for status change
- [ ] Status change logged with timestamp
- [ ] Suspended members cannot log in
- [ ] Expired members see renewal prompt

---

## Epic 4: Membership Management

### US-4.1: Membership Type Creation
**Priority:** Must Have | **Story Points:** 5

**User Story:**
As a Club Manager, I want to create membership types, so that I can offer different membership options.

**Acceptance Criteria:**
- [ ] Define name and description
- [ ] Set annual and monthly pricing
- [ ] Configure age restrictions
- [ ] Set family member limits
- [ ] Define booking privileges
- [ ] Set display order

---

### US-4.2: Membership Enrollment
**Priority:** Must Have | **Story Points:** 5

**User Story:**
As a Club Manager, I want to enroll members in memberships, so that they have active access.

**Acceptance Criteria:**
- [ ] Select member and membership type
- [ ] Set start and end dates
- [ ] Choose payment type (annual/monthly)
- [ ] Calculate and display amount due
- [ ] Record payment if received
- [ ] Generate invoice if not paid

---

### US-4.3: Membership Renewal
**Priority:** Must Have | **Story Points:** 5

**User Story:**
As a Club Manager, I want to renew memberships, so that members maintain continuous access.

**Acceptance Criteria:**
- [ ] Identify expiring memberships
- [ ] One-click renewal option
- [ ] Adjust start/end dates
- [ ] Pro-rate if early renewal
- [ ] Generate renewal invoice

---

### US-4.4: Membership Cancellation
**Priority:** Should Have | **Story Points:** 3

**User Story:**
As a Club Manager, I want to cancel memberships, so that I can handle member departures.

**Acceptance Criteria:**
- [ ] Select cancellation reason
- [ ] Set effective date
- [ ] Calculate prorated refund (optional)
- [ ] Update member status
- [ ] Send cancellation confirmation

---

### US-4.5: Membership Freeze
**Priority:** Could Have | **Story Points:** 5

**User Story:**
As a Club Manager, I want to freeze memberships temporarily, so that members can pause during absences.

**Acceptance Criteria:**
- [ ] Set freeze start and end dates
- [ ] Select freeze reason
- [ ] Extend membership end date accordingly
- [ ] Prevent bookings during freeze
- [ ] Auto-resume at end of freeze

---

## Epic 5: Session & Booking Management

### US-5.1: Session Creation
**Priority:** Must Have | **Story Points:** 3

**User Story:**
As a Club Manager, I want to create training sessions, so that members can book and attend.

**Acceptance Criteria:**
- [ ] Set title, description, category
- [ ] Select venue
- [ ] Set date, start time, end time
- [ ] Define capacity
- [ ] Optional session fee

---

### US-5.2: Recurring Schedule Creation
**Priority:** Should Have | **Story Points:** 5

**User Story:**
As a Club Manager, I want to create recurring session schedules, so that sessions are auto-generated.

**Acceptance Criteria:**
- [ ] Define day of week and time
- [ ] Set schedule start and end dates
- [ ] Specify capacity and venue
- [ ] Choose category
- [ ] Auto-generate sessions on schedule

---

### US-5.3: Session Listing (Manager)
**Priority:** Must Have | **Story Points:** 3

**User Story:**
As a Club Manager, I want to view all sessions, so that I can manage the schedule.

**Acceptance Criteria:**
- [ ] Calendar view of sessions
- [ ] List view with filters
- [ ] Filter by date range, category, venue
- [ ] Show booking count per session
- [ ] Color-coded by category

---

### US-5.4: Session Booking (Manager)
**Priority:** Must Have | **Story Points:** 3

**User Story:**
As a Club Manager, I want to book members into sessions, so that I can register walk-ins.

**Acceptance Criteria:**
- [ ] Search and select member
- [ ] Option to book family member
- [ ] Show current booking count
- [ ] Prevent overbooking
- [ ] Confirm booking created

---

### US-5.5: Session Booking (Member)
**Priority:** Must Have | **Story Points:** 5

**User Story:**
As a Member, I want to book myself into sessions, so that I can reserve my spot.

**Acceptance Criteria:**
- [ ] Browse available sessions
- [ ] Filter by date, category
- [ ] See available spots
- [ ] Book for self or family member
- [ ] Receive booking confirmation
- [ ] View booking in my bookings list

---

### US-5.6: Booking Cancellation
**Priority:** Must Have | **Story Points:** 3

**User Story:**
As a Member, I want to cancel my bookings, so that I can free up spots I can't use.

**Acceptance Criteria:**
- [ ] Cancel booking from my bookings list
- [ ] Enforce cancellation deadline
- [ ] Spot becomes available
- [ ] Cancellation confirmation
- [ ] Notify waitlist if enabled

---

### US-5.7: Waitlist Management
**Priority:** Should Have | **Story Points:** 5

**User Story:**
As a Member, I want to join a waitlist for full sessions, so that I can get a spot if one opens.

**Acceptance Criteria:**
- [ ] Join waitlist when session full
- [ ] See waitlist position
- [ ] Receive notification when spot available
- [ ] Auto-book or manual confirmation
- [ ] Waitlist expires after time limit

---

### US-5.8: Attendance Tracking
**Priority:** Should Have | **Story Points:** 5

**User Story:**
As a Club Manager, I want to mark attendance, so that I can track who attended sessions.

**Acceptance Criteria:**
- [ ] View booked members for session
- [ ] Mark attended/no-show
- [ ] Optional check-in time
- [ ] Track attendance statistics
- [ ] No-show visible in member profile

---

## Epic 6: Event Management

### US-6.1: Event Creation
**Priority:** Must Have | **Story Points:** 5

**User Story:**
As a Club Manager, I want to create events, so that members can see and register for them.

**Acceptance Criteria:**
- [ ] Set title, description, type
- [ ] Choose date, time, venue
- [ ] Define capacity
- [ ] Configure as ticketed or RSVP
- [ ] Set ticket prices (regular and member)
- [ ] Upload event image

---

### US-6.2: Event Listing
**Priority:** Must Have | **Story Points:** 3

**User Story:**
As a Member, I want to view upcoming events, so that I can participate.

**Acceptance Criteria:**
- [ ] List upcoming events
- [ ] Filter by type
- [ ] Show date, time, location
- [ ] Display available spots
- [ ] Show ticket prices

---

### US-6.3: Event RSVP
**Priority:** Should Have | **Story Points:** 3

**User Story:**
As a Member, I want to RSVP to events, so that organizers know I'm coming.

**Acceptance Criteria:**
- [ ] Choose Attending/Maybe/Not Attending
- [ ] Specify guest count
- [ ] Add dietary requirements
- [ ] Change RSVP before deadline
- [ ] View my RSVP status

---

### US-6.4: Event Ticket Purchase
**Priority:** Should Have | **Story Points:** 8

**User Story:**
As a Member, I want to purchase event tickets, so that I can attend ticketed events.

**Acceptance Criteria:**
- [ ] Select number of tickets
- [ ] Apply member pricing if applicable
- [ ] Complete payment via Stripe/PayPal
- [ ] Receive tickets with QR codes
- [ ] View purchased tickets

---

### US-6.5: Event Attendee Management
**Priority:** Should Have | **Story Points:** 3

**User Story:**
As a Club Manager, I want to view event attendees, so that I can manage the event.

**Acceptance Criteria:**
- [ ] View RSVP list with responses
- [ ] View ticket holders
- [ ] Export attendee list
- [ ] Check-in attendees
- [ ] Track attendance

---

## Epic 7: Payment Management

### US-7.1: Manual Payment Recording
**Priority:** Must Have | **Story Points:** 3

**User Story:**
As a Club Manager, I want to record manual payments, so that I can track cash and bank transfers.

**Acceptance Criteria:**
- [ ] Select member
- [ ] Enter amount and method (Cash, Bank Transfer, Cheque)
- [ ] Link to membership/invoice
- [ ] Add reference notes
- [ ] Payment appears in history

---

### US-7.2: Online Payment (Stripe)
**Priority:** Must Have | **Story Points:** 8

**User Story:**
As a Member, I want to pay by card, so that I can complete payments conveniently.

**Acceptance Criteria:**
- [ ] Secure card entry form
- [ ] Amount and description displayed
- [ ] Payment processed via Stripe
- [ ] Success/failure feedback
- [ ] Receipt generated

---

### US-7.3: Payment History View
**Priority:** Must Have | **Story Points:** 3

**User Story:**
As a Club Manager, I want to view all payments, so that I can track club finances.

**Acceptance Criteria:**
- [ ] Paginated payment list
- [ ] Filter by date, status, method
- [ ] Search by member
- [ ] Export to Excel

---

### US-7.4: Invoice Generation
**Priority:** Should Have | **Story Points:** 5

**User Story:**
As a Club Manager, I want to create invoices, so that I can request payment from members.

**Acceptance Criteria:**
- [ ] Select member
- [ ] Add line items with descriptions and amounts
- [ ] Set due date
- [ ] Auto-calculate totals
- [ ] Save as draft or send immediately

---

### US-7.5: Invoice Emailing
**Priority:** Should Have | **Story Points:** 3

**User Story:**
As a Club Manager, I want to email invoices, so that members receive payment requests.

**Acceptance Criteria:**
- [ ] Send invoice via email
- [ ] Include payment link
- [ ] Track sent status
- [ ] Resend option

---

### US-7.6: Refund Processing
**Priority:** Should Have | **Story Points:** 5

**User Story:**
As a Club Manager, I want to process refunds, so that I can handle cancellations appropriately.

**Acceptance Criteria:**
- [ ] Select payment to refund
- [ ] Enter full or partial amount
- [ ] Select refund reason
- [ ] Process via original payment method
- [ ] Update payment status

---

## Epic 8: Competition Management

### US-8.1: Competition Creation
**Priority:** Should Have | **Story Points:** 5

**User Story:**
As a Club Manager, I want to create competitions, so that I can organize leagues and tournaments.

**Acceptance Criteria:**
- [ ] Set name, description, type
- [ ] Choose format (League, Knockout)
- [ ] Set dates and entry fee
- [ ] Configure points system

---

### US-8.2: Team Registration
**Priority:** Should Have | **Story Points:** 5

**User Story:**
As a Club Manager, I want to register teams, so that they can participate in competitions.

**Acceptance Criteria:**
- [ ] Create team with name and captain
- [ ] Add team members
- [ ] Set home venue
- [ ] Team colors and logo

---

### US-8.3: Fixture Generation
**Priority:** Should Have | **Story Points:** 8

**User Story:**
As a Club Manager, I want to generate fixtures, so that matches are scheduled fairly.

**Acceptance Criteria:**
- [ ] Auto-generate round-robin fixtures
- [ ] Set match dates and venues
- [ ] Respect home/away balance
- [ ] Manual adjustment option

---

### US-8.4: Match Result Recording
**Priority:** Should Have | **Story Points:** 3

**User Story:**
As a Club Manager, I want to record match results, so that standings are updated.

**Acceptance Criteria:**
- [ ] Enter scores for home and away
- [ ] Add match events (goals, cards)
- [ ] Standings auto-calculate
- [ ] Result validation

---

### US-8.5: Standings Display
**Priority:** Should Have | **Story Points:** 3

**User Story:**
As a Member, I want to view league standings, so that I can track competition progress.

**Acceptance Criteria:**
- [ ] Display team positions
- [ ] Show played, won, drawn, lost
- [ ] Display points and goal difference
- [ ] Highlight top scorers

---

## Epic 9: Reports & Analytics

### US-9.1: Membership Reports
**Priority:** Should Have | **Story Points:** 5

**User Story:**
As a Club Manager, I want to view membership reports, so that I can understand member trends.

**Acceptance Criteria:**
- [ ] Total members by status
- [ ] Members by membership type
- [ ] Age distribution
- [ ] Growth over time
- [ ] Churn rate

---

### US-9.2: Financial Reports
**Priority:** Should Have | **Story Points:** 5

**User Story:**
As a Club Manager, I want to view financial reports, so that I can monitor revenue.

**Acceptance Criteria:**
- [ ] Revenue by period
- [ ] Revenue by type (membership, events)
- [ ] Revenue by payment method
- [ ] Outstanding balances
- [ ] Monthly comparison

---

### US-9.3: Attendance Reports
**Priority:** Could Have | **Story Points:** 3

**User Story:**
As a Club Manager, I want to view attendance reports, so that I can optimize session scheduling.

**Acceptance Criteria:**
- [ ] Attendance by session
- [ ] Popular times analysis
- [ ] No-show rates
- [ ] Member attendance frequency

---

## Epic 10: Member Portal

### US-10.1: Portal Dashboard
**Priority:** Must Have | **Story Points:** 5

**User Story:**
As a Member, I want to see a dashboard when I log in, so that I can quickly access important information.

**Acceptance Criteria:**
- [ ] Display my membership status
- [ ] Show upcoming bookings
- [ ] List upcoming events I've registered for
- [ ] Show outstanding balance
- [ ] Quick links to common actions

---

### US-10.2: Profile Self-Service
**Priority:** Must Have | **Story Points:** 3

**User Story:**
As a Member, I want to update my profile, so that my information is accurate.

**Acceptance Criteria:**
- [ ] Edit contact information
- [ ] Update emergency contacts
- [ ] Upload profile photo
- [ ] Manage communication preferences

---

### US-10.3: Payment History
**Priority:** Must Have | **Story Points:** 2

**User Story:**
As a Member, I want to view my payment history, so that I can track my transactions.

**Acceptance Criteria:**
- [ ] List all my payments
- [ ] View payment details
- [ ] Download receipts
- [ ] See outstanding amounts

---

### US-10.4: Family Member Management
**Priority:** Should Have | **Story Points:** 3

**User Story:**
As a Member, I want to manage my family members, so that they can use the club facilities.

**Acceptance Criteria:**
- [ ] View family members
- [ ] Add new family members
- [ ] Edit family member details
- [ ] Book sessions for family members

---

## Epic 11: System Configuration

### US-11.1: Payment Provider Configuration
**Priority:** Should Have | **Story Points:** 5

**User Story:**
As a Super Admin, I want to configure payment providers, so that clubs can accept payments.

**Acceptance Criteria:**
- [ ] Select active provider (Mock/Stripe)
- [ ] Enter Stripe API keys
- [ ] Test connection
- [ ] Configure mock settings for testing

---

### US-11.2: Email Provider Configuration
**Priority:** Should Have | **Story Points:** 5

**User Story:**
As a Super Admin, I want to configure email providers, so that the system can send emails.

**Acceptance Criteria:**
- [ ] Select active provider (Mock/SendGrid)
- [ ] Enter SendGrid API key
- [ ] Set default sender
- [ ] Test email sending

---

### US-11.3: Feature Flags
**Priority:** Could Have | **Story Points:** 3

**User Story:**
As a Super Admin, I want to toggle system features, so that I can control availability.

**Acceptance Criteria:**
- [ ] Toggle maintenance mode
- [ ] Enable/disable registrations
- [ ] Enable/disable email notifications

---

---

## Backlog Summary

| Priority | Count | Story Points |
|----------|-------|--------------|
| Must Have | 28 | ~115 |
| Should Have | 25 | ~105 |
| Could Have | 6 | ~25 |
| **Total** | **59** | **~245** |

---

*Document Version: 1.0*
*Last Updated: Pre-Development Planning Phase*
