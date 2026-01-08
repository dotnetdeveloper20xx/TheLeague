export * from './auth.model';
export * from './club.model';
export * from './member.model';

// Session models
export interface Session {
  id: string;
  title: string;
  description?: string;
  category: SessionCategory;
  startTime: Date;
  endTime: Date;
  capacity: number;
  currentBookings: number;
  availableSpots: number;
  isRecurring: boolean;
  recurringScheduleId?: string;
  sessionFee?: number;
  isCancelled: boolean;
  cancellationReason?: string;
  venue?: Venue;
  bookings?: SessionBooking[];
}

export interface SessionBooking {
  id: string;
  memberId: string;
  memberName: string;
  familyMemberId?: string;
  familyMemberName?: string;
  bookedAt: Date;
  status: BookingStatus;
  attended: boolean;
  checkedInAt?: Date;
  notes?: string;
}

export interface SessionFilter {
  dateFrom?: Date;
  dateTo?: Date;
  category?: SessionCategory;
  venueId?: string;
  includeCancelled?: boolean;
  page?: number;
  pageSize?: number;
}

export enum SessionCategory {
  AllAges = 'AllAges',
  Juniors = 'Juniors',
  Seniors = 'Seniors',
  Beginners = 'Beginners',
  Advanced = 'Advanced',
  Mixed = 'Mixed'
}

export enum BookingStatus {
  Confirmed = 'Confirmed',
  Cancelled = 'Cancelled',
  NoShow = 'NoShow',
  Attended = 'Attended'
}

// Event models
export interface Event {
  id: string;
  title: string;
  description?: string;
  type: EventType;
  startDateTime: Date;
  endDateTime: Date;
  location?: string;
  capacity?: number;
  currentAttendees: number;
  availableSpots?: number;
  isTicketed: boolean;
  ticketPrice?: number;
  memberTicketPrice?: number;
  ticketSalesEndDate?: Date;
  requiresRSVP: boolean;
  rsvpDeadline?: Date;
  isCancelled: boolean;
  cancellationReason?: string;
  isPublished: boolean;
  imageUrl?: string;
  venue?: Venue;
}

export enum EventType {
  Social = 'Social',
  Tournament = 'Tournament',
  AGM = 'AGM',
  Training = 'Training',
  Fundraiser = 'Fundraiser',
  Competition = 'Competition',
  Meeting = 'Meeting',
  Other = 'Other'
}

// Venue models
export interface Venue {
  id: string;
  name: string;
  description?: string;
  address?: string;
  postCode?: string;
  capacity?: number;
  facilities?: string;
  imageUrl?: string;
  isActive: boolean;
  isPrimary: boolean;
}

// Payment models
export interface Payment {
  id: string;
  memberId: string;
  memberName: string;
  membershipId?: string;
  eventTicketId?: string;
  amount: number;
  currency: string;
  status: PaymentStatus;
  method: PaymentMethod;
  type: PaymentType;
  description?: string;
  paymentDate: Date;
  processedDate?: Date;
  receiptNumber?: string;
  receiptUrl?: string;
  manualPaymentReference?: string;
  recordedBy?: string;
}

export enum PaymentStatus {
  Pending = 'Pending',
  Processing = 'Processing',
  Completed = 'Completed',
  Failed = 'Failed',
  Refunded = 'Refunded',
  Cancelled = 'Cancelled'
}

export enum PaymentMethod {
  Stripe = 'Stripe',
  PayPal = 'PayPal',
  BankTransfer = 'BankTransfer',
  Cash = 'Cash',
  Cheque = 'Cheque',
  Other = 'Other'
}

export enum PaymentType {
  Membership = 'Membership',
  EventTicket = 'EventTicket',
  SessionFee = 'SessionFee',
  Other = 'Other'
}

// MembershipType models
export interface MembershipType {
  id: string;
  name: string;
  description?: string;
  annualFee: number;
  monthlyFee?: number;
  sessionFee?: number;
  minAge?: number;
  maxAge?: number;
  maxFamilyMembers?: number;
  isActive: boolean;
  allowOnlineSignup: boolean;
  sortOrder: number;
  includesBooking: boolean;
  includesEvents: boolean;
  maxSessionsPerWeek?: number;
  memberCount: number;
}

// Paged result
export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}
