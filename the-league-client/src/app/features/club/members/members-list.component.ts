import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MemberService } from '../../../core/services/member.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent, PaginationComponent, StatusBadgeComponent, EmptyStateComponent, ConfirmDialogComponent } from '../../../shared/components';
import { DateFormatPipe } from '../../../shared/pipes';
import { MemberListItem, MemberStatus, PagedResult } from '../../../core/models';

@Component({
  selector: 'app-members-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, LoadingSpinnerComponent, PaginationComponent, StatusBadgeComponent, EmptyStateComponent, ConfirmDialogComponent, DateFormatPipe],
  template: `
    <div class="space-y-6">
      <!-- Header -->
      <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 class="text-2xl font-bold text-gray-900">Members</h1>
          <p class="text-gray-500 mt-1">Manage club members</p>
        </div>
        <a routerLink="/club/members/new" class="btn-primary inline-flex items-center">
          <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z" />
          </svg>
          Add Member
        </a>
      </div>

      <!-- Filters -->
      <div class="card">
        <div class="flex flex-col lg:flex-row gap-4">
          <div class="flex-1">
            <input
              type="text"
              [(ngModel)]="searchTerm"
              (ngModelChange)="onSearch()"
              placeholder="Search by name or email..."
              class="form-input"
            />
          </div>
          <div class="flex gap-4">
            <select [(ngModel)]="statusFilter" (ngModelChange)="onSearch()" class="form-input">
              <option value="">All Status</option>
              @for (status of memberStatuses; track status) {
                <option [value]="status">{{ status }}</option>
              }
            </select>
            <select [(ngModel)]="familyFilter" (ngModelChange)="onSearch()" class="form-input">
              <option value="">All Types</option>
              <option value="true">Family Accounts</option>
              <option value="false">Individual</option>
            </select>
          </div>
        </div>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg" message="Loading members..."></app-loading-spinner>
        </div>
      } @else if (members().length === 0) {
        <app-empty-state
          icon="users"
          title="No members found"
          [message]="searchTerm ? 'No members match your search criteria.' : 'Get started by adding your first member.'"
          [actionText]="searchTerm ? '' : 'Add Member'"
          (action)="navigateToCreate()"
        ></app-empty-state>
      } @else {
        <!-- Members Table -->
        <div class="card overflow-hidden p-0">
          <div class="overflow-x-auto">
            <table class="table">
              <thead>
                <tr>
                  <th>Member</th>
                  <th>Contact</th>
                  <th>Membership</th>
                  <th>Status</th>
                  <th>Joined</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                @for (member of members(); track member.id) {
                  <tr>
                    <td>
                      <div class="flex items-center">
                        <div class="w-10 h-10 rounded-full bg-primary-100 flex items-center justify-center text-primary-600 font-medium">
                          {{ member.fullName.charAt(0) }}
                        </div>
                        <div class="ml-3">
                          <p class="font-medium text-gray-900">{{ member.fullName }}</p>
                          @if (member.isFamilyAccount) {
                            <p class="text-sm text-gray-500">
                              <span class="inline-flex items-center">
                                <svg class="w-4 h-4 mr-1 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" />
                                </svg>
                                Family ({{ member.familyMemberCount }} members)
                              </span>
                            </p>
                          }
                        </div>
                      </div>
                    </td>
                    <td>
                      <p class="text-gray-900">{{ member.email }}</p>
                      <p class="text-sm text-gray-500">{{ member.phone || '-' }}</p>
                    </td>
                    <td>
                      @if (member.membershipType) {
                        <p class="text-gray-900">{{ member.membershipType }}</p>
                        @if (member.membershipExpiry) {
                          <p class="text-sm text-gray-500">Expires: {{ member.membershipExpiry | dateFormat:'short' }}</p>
                        }
                      } @else {
                        <span class="text-gray-400">No membership</span>
                      }
                    </td>
                    <td>
                      <app-status-badge [status]="member.status" type="member"></app-status-badge>
                    </td>
                    <td>{{ member.joinedDate | dateFormat }}</td>
                    <td>
                      <div class="flex items-center gap-2">
                        <a [routerLink]="['/club/members', member.id]" class="text-primary-600 hover:text-primary-700 text-sm font-medium">
                          View
                        </a>
                        <button (click)="confirmDelete(member)" class="text-red-600 hover:text-red-700 text-sm font-medium">
                          Delete
                        </button>
                      </div>
                    </td>
                  </tr>
                }
              </tbody>
            </table>
          </div>
        </div>

        <!-- Pagination -->
        @if (totalCount() > pageSize) {
          <app-pagination
            [currentPage]="currentPage()"
            [pageSize]="pageSize"
            [totalCount]="totalCount()"
            (pageChange)="onPageChange($event)"
          ></app-pagination>
        }
      }
    </div>

    <!-- Delete Confirmation -->
    <app-confirm-dialog
      [isOpen]="showDeleteDialog()"
      title="Delete Member"
      [message]="'Are you sure you want to delete ' + (selectedMember()?.fullName || '') + '? This action cannot be undone.'"
      confirmText="Delete"
      type="danger"
      (confirm)="deleteMember()"
      (cancel)="showDeleteDialog.set(false)"
    ></app-confirm-dialog>
  `
})
export class MembersListComponent implements OnInit {
  private memberService = inject(MemberService);
  private notificationService = inject(NotificationService);

  isLoading = signal(true);
  members = signal<MemberListItem[]>([]);
  totalCount = signal(0);
  currentPage = signal(1);
  pageSize = 10;

  searchTerm = '';
  statusFilter = '';
  familyFilter = '';

  memberStatuses = Object.values(MemberStatus);

  showDeleteDialog = signal(false);
  selectedMember = signal<MemberListItem | null>(null);

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers(): void {
    this.isLoading.set(true);
    this.memberService.getMembers({
      searchTerm: this.searchTerm || undefined,
      status: this.statusFilter ? this.statusFilter as MemberStatus : undefined,
      isFamilyAccount: this.familyFilter ? this.familyFilter === 'true' : undefined,
      page: this.currentPage(),
      pageSize: this.pageSize
    }).subscribe({
      next: (result: PagedResult<MemberListItem>) => {
        this.members.set(result.items);
        this.totalCount.set(result.totalCount);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }

  onSearch(): void {
    this.currentPage.set(1);
    this.loadMembers();
  }

  onPageChange(page: number): void {
    this.currentPage.set(page);
    this.loadMembers();
  }

  confirmDelete(member: MemberListItem): void {
    this.selectedMember.set(member);
    this.showDeleteDialog.set(true);
  }

  deleteMember(): void {
    const member = this.selectedMember();
    if (!member) return;

    this.memberService.deleteMember(member.id).subscribe({
      next: () => {
        this.notificationService.success(`${member.fullName} has been deleted`);
        this.showDeleteDialog.set(false);
        this.loadMembers();
      },
      error: () => {
        this.notificationService.error('Failed to delete member');
      }
    });
  }

  navigateToCreate(): void {
    window.location.href = '/club/members/new';
  }
}
