export interface Club {
  id: string;
  name: string;
  slug: string;
  description?: string;
  logoUrl?: string;
  primaryColor: string;
  secondaryColor: string;
  contactEmail?: string;
  contactPhone?: string;
  address?: string;
  website?: string;
  clubType: ClubType;
  isActive: boolean;
  createdAt: Date;
  memberCount?: number;
  activeMembers?: number;
}

export enum ClubType {
  Cricket = 'Cricket',
  Football = 'Football',
  Rugby = 'Rugby',
  Tennis = 'Tennis',
  Golf = 'Golf',
  Hockey = 'Hockey',
  Swimming = 'Swimming',
  Athletics = 'Athletics',
  MultiSport = 'MultiSport',
  CommunityGroup = 'CommunityGroup',
  YouthOrganization = 'YouthOrganization',
  Other = 'Other'
}

export interface ClubCreateRequest {
  name: string;
  slug: string;
  description?: string;
  clubType: ClubType;
  contactEmail?: string;
  contactPhone?: string;
  address?: string;
  website?: string;
  primaryColor?: string;
  secondaryColor?: string;
}

export interface ClubDashboard {
  club: Club;
  totalMembers: number;
  activeMembers: number;
  pendingMembers: number;
  expiredMembers: number;
  totalRevenue: number;
  revenueThisMonth: number;
  upcomingSessions: number;
  upcomingEvents: number;
  recentPayments: PaymentSummary[];
  memberGrowth: MonthlyData[];
  membersByType: ChartData[];
}

export interface PaymentSummary {
  id: string;
  memberName: string;
  amount: number;
  date: Date;
  type: string;
  status: string;
}

export interface MonthlyData {
  month: string;
  value: number;
}

export interface ChartData {
  label: string;
  value: number;
}
