export interface SystemConfiguration {
  id: string;

  // Payment Configuration
  paymentProvider: string;
  mockPaymentDelayMs: number;
  mockPaymentFailureRate: number;
  stripePublishableKey?: string;
  stripeSecretKeyMasked?: string;
  stripeWebhookSecretMasked?: string;
  stripeConfigured: boolean;

  // Email Configuration
  emailProvider: string;
  mockEmailDelayMs: number;
  sendGridApiKeyMasked?: string;
  sendGridConfigured: boolean;
  defaultFromEmail: string;
  defaultFromName: string;

  // Feature Flags
  maintenanceMode: boolean;
  maintenanceMessage?: string;
  allowNewRegistrations: boolean;
  enableEmailNotifications: boolean;

  // Appearance
  platformName: string;
  primaryColor: string;
  logoUrl?: string;

  // Audit
  lastModifiedAt: string;
  lastModifiedBy: string;
  version: number;
}

export interface UpdateSystemConfigurationRequest {
  // Payment Configuration
  paymentProvider?: string;
  mockPaymentDelayMs?: number;
  mockPaymentFailureRate?: number;
  stripePublishableKey?: string;
  stripeSecretKey?: string;
  stripeWebhookSecret?: string;

  // Email Configuration
  emailProvider?: string;
  mockEmailDelayMs?: number;
  sendGridApiKey?: string;
  defaultFromEmail?: string;
  defaultFromName?: string;

  // Feature Flags
  maintenanceMode?: boolean;
  maintenanceMessage?: string;
  allowNewRegistrations?: boolean;
  enableEmailNotifications?: boolean;

  // Appearance
  platformName?: string;
  primaryColor?: string;
  logoUrl?: string;
}

export interface ConfigurationAuditLog {
  id: string;
  action: string;
  section: string;
  propertyChanged?: string;
  oldValue?: string;
  newValue?: string;
  changedBy: string;
  timestamp: string;
  ipAddress?: string;
}

export interface ProviderTestResult {
  success: boolean;
  provider: string;
  message: string;
  testedAt: string;
}

export interface SendTestEmailRequest {
  toEmail: string;
}
