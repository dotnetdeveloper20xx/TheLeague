export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  confirmPassword: string;
  firstName: string;
  lastName: string;
  phone?: string;
}

export interface ApiResponse<T> {
  success: boolean;
  message?: string;
  data: T;
}

export interface AuthData {
  token: string;
  refreshToken: string;
  expiration: string;
  user: User;
}

export type AuthResponse = ApiResponse<AuthData>;

export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  fullName: string;
  role: UserRole;
  clubId?: string;
  memberId?: string;
  clubName?: string;
}

export enum UserRole {
  SuperAdmin = 0,
  ClubManager = 1,
  Member = 2
}

export interface ForgotPasswordRequest {
  email: string;
}

export interface ResetPasswordRequest {
  email: string;
  token: string;
  newPassword: string;
  confirmPassword: string;
}
