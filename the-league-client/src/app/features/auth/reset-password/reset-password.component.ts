import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent } from '../../../shared/components';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, LoadingSpinnerComponent],
  template: `
    <div class="min-h-screen bg-gradient-to-br from-primary-600 to-primary-800 flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
      <div class="max-w-md w-full">
        <div class="bg-white rounded-2xl shadow-xl p-8">
          @if (!resetSuccess) {
            <!-- Reset Form -->
            <div class="text-center mb-8">
              <div class="mx-auto w-16 h-16 bg-primary-100 rounded-full flex items-center justify-center mb-4">
                <svg class="w-10 h-10 text-primary-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z" />
                </svg>
              </div>
              <h2 class="text-2xl font-bold text-gray-900">Reset Password</h2>
              <p class="text-gray-500 mt-2">Enter your new password below</p>
            </div>

            <form [formGroup]="resetPasswordForm" (ngSubmit)="onSubmit()" class="space-y-6">
              <div>
                <label for="newPassword" class="form-label">New Password</label>
                <div class="relative">
                  <input
                    [type]="showPassword ? 'text' : 'password'"
                    id="newPassword"
                    formControlName="newPassword"
                    class="form-input pr-10"
                    placeholder="Enter new password"
                    [class.border-red-500]="resetPasswordForm.get('newPassword')?.invalid && resetPasswordForm.get('newPassword')?.touched"
                  />
                  <button
                    type="button"
                    (click)="showPassword = !showPassword"
                    class="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-600"
                  >
                    @if (showPassword) {
                      <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21" />
                      </svg>
                    } @else {
                      <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                      </svg>
                    }
                  </button>
                </div>
                @if (resetPasswordForm.get('newPassword')?.invalid && resetPasswordForm.get('newPassword')?.touched) {
                  <p class="form-error">Password must be at least 8 characters with uppercase, lowercase, number, and special character</p>
                }
              </div>

              <div>
                <label for="confirmPassword" class="form-label">Confirm Password</label>
                <input
                  type="password"
                  id="confirmPassword"
                  formControlName="confirmPassword"
                  class="form-input"
                  placeholder="Confirm new password"
                  [class.border-red-500]="resetPasswordForm.get('confirmPassword')?.invalid && resetPasswordForm.get('confirmPassword')?.touched"
                />
                @if (resetPasswordForm.hasError('passwordMismatch') && resetPasswordForm.get('confirmPassword')?.touched) {
                  <p class="form-error">Passwords do not match</p>
                }
              </div>

              <button
                type="submit"
                [disabled]="resetPasswordForm.invalid || isLoading"
                class="w-full btn-primary py-3 flex items-center justify-center"
              >
                @if (isLoading) {
                  <app-loading-spinner size="sm" containerClass="mr-2"></app-loading-spinner>
                  Resetting...
                } @else {
                  Reset Password
                }
              </button>
            </form>
          } @else {
            <!-- Success Message -->
            <div class="text-center">
              <div class="mx-auto w-16 h-16 bg-green-100 rounded-full flex items-center justify-center mb-4">
                <svg class="w-10 h-10 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
                </svg>
              </div>
              <h2 class="text-2xl font-bold text-gray-900 mb-2">Password Reset!</h2>
              <p class="text-gray-500 mb-6">
                Your password has been successfully reset. You can now sign in with your new password.
              </p>
              <a routerLink="/auth/login" class="btn-primary inline-block py-3 px-8">
                Sign In
              </a>
            </div>
          }
        </div>
      </div>
    </div>
  `
})
export class ResetPasswordComponent implements OnInit {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private notificationService = inject(NotificationService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  resetPasswordForm: FormGroup;
  isLoading = false;
  showPassword = false;
  resetSuccess = false;
  token = '';
  email = '';

  constructor() {
    this.resetPasswordForm = this.fb.group({
      newPassword: ['', [
        Validators.required,
        Validators.minLength(8),
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]/)
      ]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  ngOnInit(): void {
    this.token = this.route.snapshot.queryParams['token'] || '';
    this.email = this.route.snapshot.queryParams['email'] || '';

    if (!this.token || !this.email) {
      this.notificationService.error('Invalid reset link');
      this.router.navigate(['/auth/forgot-password']);
    }
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('newPassword');
    const confirmPassword = control.get('confirmPassword');

    if (password && confirmPassword && password.value !== confirmPassword.value) {
      return { passwordMismatch: true };
    }
    return null;
  }

  onSubmit(): void {
    if (this.resetPasswordForm.invalid) return;

    this.isLoading = true;
    const { newPassword, confirmPassword } = this.resetPasswordForm.value;

    this.authService.resetPassword({
      email: this.email,
      token: this.token,
      newPassword,
      confirmPassword
    }).subscribe({
      next: () => {
        this.isLoading = false;
        this.resetSuccess = true;
      },
      error: (error) => {
        this.isLoading = false;
        this.notificationService.error(error.error?.message || 'Failed to reset password. The link may have expired.');
      }
    });
  }
}
