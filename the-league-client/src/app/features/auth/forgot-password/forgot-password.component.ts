import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent } from '../../../shared/components';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, LoadingSpinnerComponent],
  template: `
    <div class="min-h-screen bg-gradient-to-br from-primary-600 to-primary-800 flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
      <div class="max-w-md w-full">
        <div class="bg-white rounded-2xl shadow-xl p-8">
          @if (!emailSent) {
            <!-- Request Form -->
            <div class="text-center mb-8">
              <div class="mx-auto w-16 h-16 bg-primary-100 rounded-full flex items-center justify-center mb-4">
                <svg class="w-10 h-10 text-primary-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 7a2 2 0 012 2m4 0a6 6 0 01-7.743 5.743L11 17H9v2H7v2H4a1 1 0 01-1-1v-2.586a1 1 0 01.293-.707l5.964-5.964A6 6 0 1121 9z" />
                </svg>
              </div>
              <h2 class="text-2xl font-bold text-gray-900">Forgot Password?</h2>
              <p class="text-gray-500 mt-2">Enter your email to receive a reset link</p>
            </div>

            <form [formGroup]="forgotPasswordForm" (ngSubmit)="onSubmit()" class="space-y-6">
              <div>
                <label for="email" class="form-label">Email Address</label>
                <input
                  type="email"
                  id="email"
                  formControlName="email"
                  class="form-input"
                  placeholder="you@example.com"
                  [class.border-red-500]="forgotPasswordForm.get('email')?.invalid && forgotPasswordForm.get('email')?.touched"
                />
                @if (forgotPasswordForm.get('email')?.invalid && forgotPasswordForm.get('email')?.touched) {
                  <p class="form-error">Please enter a valid email address</p>
                }
              </div>

              <button
                type="submit"
                [disabled]="forgotPasswordForm.invalid || isLoading"
                class="w-full btn-primary py-3 flex items-center justify-center"
              >
                @if (isLoading) {
                  <app-loading-spinner size="sm" containerClass="mr-2"></app-loading-spinner>
                  Sending...
                } @else {
                  Send Reset Link
                }
              </button>
            </form>
          } @else {
            <!-- Success Message -->
            <div class="text-center">
              <div class="mx-auto w-16 h-16 bg-green-100 rounded-full flex items-center justify-center mb-4">
                <svg class="w-10 h-10 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
                </svg>
              </div>
              <h2 class="text-2xl font-bold text-gray-900 mb-2">Check Your Email</h2>
              <p class="text-gray-500 mb-6">
                We've sent a password reset link to<br />
                <strong class="text-gray-700">{{ submittedEmail }}</strong>
              </p>
              <p class="text-sm text-gray-500 mb-6">
                Didn't receive the email? Check your spam folder or
                <button (click)="emailSent = false" class="text-primary-600 hover:underline">try again</button>
              </p>
            </div>
          }

          <!-- Back to Login -->
          <p class="mt-6 text-center text-sm text-gray-600">
            <a routerLink="/auth/login" class="text-primary-600 hover:text-primary-500 font-medium inline-flex items-center">
              <svg class="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
              </svg>
              Back to Sign in
            </a>
          </p>
        </div>
      </div>
    </div>
  `
})
export class ForgotPasswordComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private notificationService = inject(NotificationService);

  forgotPasswordForm: FormGroup;
  isLoading = false;
  emailSent = false;
  submittedEmail = '';

  constructor() {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  onSubmit(): void {
    if (this.forgotPasswordForm.invalid) return;

    this.isLoading = true;
    const { email } = this.forgotPasswordForm.value;
    this.submittedEmail = email;

    this.authService.forgotPassword({ email }).subscribe({
      next: () => {
        this.isLoading = false;
        this.emailSent = true;
      },
      error: () => {
        this.isLoading = false;
        // Still show success to prevent email enumeration
        this.emailSent = true;
      }
    });
  }
}
