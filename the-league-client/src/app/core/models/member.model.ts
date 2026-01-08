export interface Member {
  id: string;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  phone?: string;
  dateOfBirth?: Date;
  address?: string;
  city?: string;
  postCode?: string;
  profilePhotoUrl?: string;
  emergencyContactName?: string;
  emergencyContactPhone?: string;
  emergencyContactRelation?: string;
  medicalConditions?: string;
  allergies?: string;
  isFamilyAccount: boolean;
  status: MemberStatus;
  joinedDate: Date;
  isActive: boolean;
  emailVerified: boolean;
  currentMembership?: MembershipSummary;
  familyMembers?: FamilyMember[];
}

export interface MemberListItem {
  id: string;
  fullName: string;
  email: string;
  phone?: string;
  status: MemberStatus;
  joinedDate: Date;
  membershipType?: string;
  membershipExpiry?: Date;
  isFamilyAccount: boolean;
  familyMemberCount: number;
}

export interface MemberCreateRequest {
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  dateOfBirth?: Date;
  address?: string;
  city?: string;
  postCode?: string;
  emergencyContactName?: string;
  emergencyContactPhone?: string;
  emergencyContactRelation?: string;
  medicalConditions?: string;
  allergies?: string;
  isFamilyAccount: boolean;
  membershipTypeId?: string;
  familyMembers?: FamilyMemberCreate[];
}

export interface MemberUpdateRequest {
  firstName?: string;
  lastName?: string;
  phone?: string;
  dateOfBirth?: Date;
  address?: string;
  city?: string;
  postCode?: string;
  emergencyContactName?: string;
  emergencyContactPhone?: string;
  emergencyContactRelation?: string;
  medicalConditions?: string;
  allergies?: string;
  status?: MemberStatus;
  isActive?: boolean;
}

export interface FamilyMember {
  id: string;
  firstName: string;
  lastName: string;
  fullName: string;
  dateOfBirth?: Date;
  relation: FamilyMemberRelation;
  medicalConditions?: string;
  allergies?: string;
  isActive: boolean;
}

export interface FamilyMemberCreate {
  firstName: string;
  lastName: string;
  dateOfBirth?: Date;
  relation: FamilyMemberRelation;
  medicalConditions?: string;
  allergies?: string;
}

export interface MembershipSummary {
  id: string;
  membershipType: string;
  startDate: Date;
  endDate: Date;
  status: MembershipStatus;
  amountDue: number;
  autoRenew: boolean;
}

export enum MemberStatus {
  Pending = 'Pending',
  Active = 'Active',
  Expired = 'Expired',
  Suspended = 'Suspended',
  Cancelled = 'Cancelled'
}

export enum FamilyMemberRelation {
  Spouse = 'Spouse',
  Child = 'Child',
  Sibling = 'Sibling',
  Parent = 'Parent',
  Other = 'Other'
}

export enum MembershipStatus {
  Active = 'Active',
  PendingPayment = 'PendingPayment',
  Expired = 'Expired',
  Cancelled = 'Cancelled'
}

export interface MemberFilter {
  searchTerm?: string;
  status?: MemberStatus;
  membershipTypeId?: string;
  isFamilyAccount?: boolean;
  joinedAfter?: Date;
  joinedBefore?: Date;
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortDescending?: boolean;
}
