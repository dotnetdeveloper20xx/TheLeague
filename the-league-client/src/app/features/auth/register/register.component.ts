import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent } from '../../../shared/components';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, LoadingSpinnerComponent],
  template: `
    <div class="min-h-screen bg-gradient-to-br from-primary-600 to-primary-800 flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
      <div class="max-w-md w-full">
        <div class="bg-white rounded-2xl shadow-xl p-8">
          <!-- Logo and Title -->
          <div class="text-center mb-8">
            <div class="mx-auto w-16 h-16 bg-primary-100 rounded-full flex items-center justify-center mb-4">
              <svg class="w-10 h-10 text-primary-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z" />
              </svg>
            </div>
            <h2 class="text-2xl font-bold text-gray-900">Create an Account</h2>
            <p class="text-gray-500 mt-2">Join The League today</p>
          </div>

          <!-- Register Form -->
          <form [formGroup]="registerForm" (ngSubmit)="onSubmit()" class="space-y-5">
            <div class="grid grid-cols-2 gap-4">
              <div>
                <label for="firstName" class="form-label">First Name</label>
                <input
                  type="text"
                  id="firstName"
                  formControlName="firstName"
                  class="form-input"
                  placeholder="John"
                  [class.border-red-500]="registerForm.get('firstName')?.invalid && registerForm.get('firstName')?.touched"
                />
                @if (registerForm.get('firstName')?.invalid && registerForm.get('firstName')?.touched) {
                  <p class="form-error">First name is required</p>
                }
              </div>

              <div>
                <label for="lastName" class="form-label">Last Name</label>
                <input
                  type="text"
                  id="lastName"
                  formControlName="lastName"
                  class="form-input"
                  placeholder="Doe"
                  [class.border-red-500]="registerForm.get('lastName')?.invalid && registerForm.get('lastName')?.touched"
                />
                @if (registerForm.get('lastName')?.invalid && registerForm.get('lastName')?.touched) {
                  <p class="form-error">Last name is required</p>
                }
              </div>
            </div>

            <div>
              <label for="email" class="form-label">Email Address</label>
              <input
                type="email"
                id="email"
                formControlName="email"
                class="form-input"
                placeholder="you@example.com"
                [class.border-red-500]="registerForm.get('email')?.invalid && registerForm.get('email')?.touched"
              />
              @if (registerForm.get('email')?.invalid && registerForm.get('email')?.touched) {
                <p class="form-error">Please enter a valid email address</p>
              }
            </div>

            <div>
              <label for="phone" class="form-label">Phone Number (Optional)</label>
              <input
                type="tel"
                id="phone"
                formControlName="phone"
                class="form-input"
                placeholder="+44 7123 456789"
              />
            </div>

            <div>
              <label for="password" class="form-label">Password</label>
              <div class="relative">
                <input
                  [type]="showPassword ? 'text' : 'password'"
                  id="password"
                  formControlName="password"
                  class="form-input pr-10"
                  placeholder="Create a password"
                  [class.border-red-500]="registerForm.get('password')?.invalid && registerForm.get('password')?.touched"
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
              @if (registerForm.get('password')?.invalid && registerForm.get('password')?.touched) {
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
                placeholder="Confirm your password"
                [class.border-red-500]="registerForm.get('confirmPassword')?.invalid && registerForm.get('confirmPassword')?.touched"
              />
              @if (registerForm.hasError('passwordMismatch') && registerForm.get('confirmPassword')?.touched) {
                <p class="form-error">Passwords do not match</p>
              }
            </div>

            <div class="flex items-start">
              <input
                type="checkbox"
                id="terms"
                formControlName="terms"
                class="mt-1 rounded border-gray-300 text-primary-600 focus:ring-primary-500"
              />
              <label for="terms" class="ml-2 text-sm text-gray-600">
                I agree to the <a href="#" class="text-primary-600 hover:underline">Terms of Service</a>
                and <a href="#" class="text-primary-600 hover:underline">Privacy Policy</a>
              </label>
            </div>
            @if (registerForm.get('terms')?.invalid && registerForm.get('terms')?.touched) {
              <p class="form-error -mt-3">You must accept the terms and conditions</p>
            }

            <button
              type="submit"
              [disabled]="registerForm.invalid || isLoading"
              class="w-full btn-primary py-3 flex items-center justify-center"
            >
              @if (isLoading) {
                <app-loading-spinner size="sm" containerClass="mr-2"></app-loading-spinner>
                Creating account...
              } @else {
                Create Account
              }
            </button>
          </form>

          <!-- Login Link -->
          <p class="mt-6 text-center text-sm text-gray-600">
            Already have an account?
            <a routerLink="/auth/login" class="text-primary-600 hover:text-primary-500 font-medium">
              Sign in
            </a>
          </p>
        </div>
      </div>
    </div>
  `
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private notificationService = inject(NotificationService);
  private router = inject(Router);

  registerForm: FormGroup;
  isLoading = false;
  showPassword = false;

  constructor() {
    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: [''],
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]/)
      ]],
      confirmPassword: ['', Validators.required],
      terms: [false, Validators.requiredTrue]
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    if (password && confirmPassword && password.value !== confirmPassword.value) {
      return { passwordMismatch: true };
    }
    return null;
  }

  onSubmit(): void {
    if (this.registerForm.invalid) return;

    this.isLoading = true;
    const { firstName, lastName, email, phone, password, confirmPassword } = this.registerForm.value;

    this.authService.register({ firstName, lastName, email, phone, password, confirmPassword }).subscribe({
      next: () => {
        this.notificationService.success('Account created successfully!');
        this.authService.navigateToDefaultRoute();
      },
      error: (error) => {
        this.isLoading = false;
        this.notificationService.error(error.error?.message || 'Failed to create account');
      }
    });
  }
}
