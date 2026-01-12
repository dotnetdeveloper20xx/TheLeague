import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  SystemConfiguration,
  UpdateSystemConfigurationRequest,
  ConfigurationAuditLog,
  ProviderTestResult,
  SendTestEmailRequest
} from '../models/system-config.model';

@Injectable({ providedIn: 'root' })
export class SystemConfigService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/admin/system-config`;

  /**
   * Gets the current system configuration
   */
  getConfiguration(): Observable<SystemConfiguration> {
    return this.http.get<SystemConfiguration>(this.baseUrl);
  }

  /**
   * Updates the system configuration
   */
  updateConfiguration(request: UpdateSystemConfigurationRequest): Observable<SystemConfiguration> {
    return this.http.put<SystemConfiguration>(this.baseUrl, request);
  }

  /**
   * Gets the configuration audit log
   */
  getAuditLog(limit?: number): Observable<ConfigurationAuditLog[]> {
    if (limit) {
      return this.http.get<ConfigurationAuditLog[]>(`${this.baseUrl}/audit`, { params: { limit: limit.toString() } });
    }
    return this.http.get<ConfigurationAuditLog[]>(`${this.baseUrl}/audit`);
  }

  /**
   * Tests the payment provider connection
   */
  testPaymentProvider(): Observable<ProviderTestResult> {
    return this.http.post<ProviderTestResult>(`${this.baseUrl}/test-payment`, {});
  }

  /**
   * Tests the email provider connection
   */
  testEmailProvider(): Observable<ProviderTestResult> {
    return this.http.post<ProviderTestResult>(`${this.baseUrl}/test-email`, {});
  }

  /**
   * Sends a test email
   */
  sendTestEmail(request: SendTestEmailRequest): Observable<ProviderTestResult> {
    return this.http.post<ProviderTestResult>(`${this.baseUrl}/send-test-email`, request);
  }

  /**
   * Triggers an application restart
   */
  restart(): Observable<{ message: string; requestedBy: string; requestedAt: string }> {
    return this.http.post<{ message: string; requestedBy: string; requestedAt: string }>(
      `${this.baseUrl}/restart`,
      {}
    );
  }

  /**
   * Checks if the application is healthy (for restart polling)
   */
  healthCheck(): Observable<{ status: string; timestamp: string }> {
    return this.http.get<{ status: string; timestamp: string }>(`${this.baseUrl}/health`);
  }
}
