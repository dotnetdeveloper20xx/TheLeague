import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CompetitionService } from '../../../core/services/competition.service';
import { VenueService } from '../../../core/services/venue.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent } from '../../../shared/components';
import {
  Competition,
  CompetitionCreateRequest,
  CompetitionType,
  CompetitionStatus,
  SkillLevel,
  AgeGroup,
  Season,
  Venue
} from '../../../core/models';

@Component({
  selector: 'app-competition-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, LoadingSpinnerComponent],
  template: `
    <div class="space-y-6">
      <!-- Header -->
      <div class="flex items-center gap-4">
        <a routerLink="/club/competitions" class="btn-secondary">
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
          </svg>
        </a>
        <div>
          <h1 class="text-2xl font-bold text-gray-900">{{ isEditMode() ? 'Edit Competition' : 'New Competition' }}</h1>
          <p class="text-gray-500 mt-1">{{ isEditMode() ? 'Update competition details' : 'Create a new league or tournament' }}</p>
        </div>
      </div>

      @if (isLoading()) {
        <div class="flex justify-center py-12">
          <app-loading-spinner size="lg" message="Loading..."></app-loading-spinner>
        </div>
      } @else {
        <form [formGroup]="form" (ngSubmit)="onSubmit()" class="space-y-6">
          <!-- Basic Information -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Basic Information</h2>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div class="md:col-span-2">
                <label class="block text-sm font-medium text-gray-700 mb-1">
                  Competition Name <span class="text-red-500">*</span>
                </label>
                <input
                  type="text"
                  formControlName="name"
                  class="input-field"
                  placeholder="e.g., Summer League 2025"
                />
                @if (form.get('name')?.invalid && form.get('name')?.touched) {
                  <p class="text-red-500 text-sm mt-1">Competition name is required</p>
                }
              </div>

              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Code</label>
                <input
                  type="text"
                  formControlName="code"
                  class="input-field"
                  placeholder="e.g., SL2025"
                />
              </div>

              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">
                  Type <span class="text-red-500">*</span>
                </label>
                <select formControlName="type" class="input-field">
                  @for (type of competitionTypes; track type) {
                    <option [value]="type">{{ formatType(type) }}</option>
                  }
                </select>
              </div>

              <div class="md:col-span-2">
                <label class="block text-sm font-medium text-gray-700 mb-1">Description</label>
                <textarea
                  formControlName="description"
                  rows="3"
                  class="input-field"
                  placeholder="Describe the competition..."
                ></textarea>
              </div>

              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Sport</label>
                <input
                  type="text"
                  formControlName="sport"
                  class="input-field"
                  placeholder="e.g., Football, Tennis"
                />
              </div>

              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Category</label>
                <input
                  type="text"
                  formControlName="category"
                  class="input-field"
                  placeholder="e.g., Senior, Youth"
                />
              </div>

              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Division</label>
                <input
                  type="text"
                  formControlName="division"
                  class="input-field"
                  placeholder="e.g., Division 1, Premier"
                />
              </div>

              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Season</label>
                <select formControlName="seasonId" class="input-field">
                  <option [ngValue]="null">No Season</option>
                  @for (season of seasons(); track season.id) {
                    <option [value]="season.id">{{ season.name }}</option>
                  }
                </select>
              </div>
            </div>
          </div>

          <!-- Dates -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Dates</h2>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Start Date</label>
                <input type="date" formControlName="startDate" class="input-field" />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">End Date</label>
                <input type="date" formControlName="endDate" class="input-field" />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Registration Opens</label>
                <input type="date" formControlName="registrationOpenDate" class="input-field" />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Registration Closes</label>
                <input type="date" formControlName="registrationCloseDate" class="input-field" />
              </div>
            </div>
          </div>

          <!-- Team Settings -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Team Settings</h2>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div class="md:col-span-2">
                <label class="flex items-center">
                  <input type="checkbox" formControlName="isTeamBased" class="rounded border-gray-300 text-primary-600 mr-2" />
                  <span class="text-sm font-medium text-gray-700">Team-based Competition</span>
                </label>
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Minimum Teams</label>
                <input type="number" formControlName="minTeams" class="input-field" min="2" />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Maximum Teams</label>
                <input type="number" formControlName="maxTeams" class="input-field" min="2" />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Min Players per Team</label>
                <input type="number" formControlName="minPlayersPerTeam" class="input-field" min="1" />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Max Players per Team</label>
                <input type="number" formControlName="maxPlayersPerTeam" class="input-field" min="1" />
              </div>
            </div>
          </div>

          <!-- Competition Format -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Competition Format</h2>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Format</label>
                <input
                  type="text"
                  formControlName="format"
                  class="input-field"
                  placeholder="e.g., Round Robin, Single Elimination"
                />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Number of Rounds</label>
                <input type="number" formControlName="numberOfRounds" class="input-field" min="1" />
              </div>
              <div class="md:col-span-2">
                <label class="flex items-center">
                  <input type="checkbox" formControlName="homeAndAway" class="rounded border-gray-300 text-primary-600 mr-2" />
                  <span class="text-sm font-medium text-gray-700">Home and Away Fixtures</span>
                </label>
              </div>
            </div>
          </div>

          <!-- Points System -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Points System</h2>
            <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Points for Win</label>
                <input type="number" formControlName="pointsForWin" class="input-field" min="0" />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Points for Draw</label>
                <input type="number" formControlName="pointsForDraw" class="input-field" min="0" />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Points for Loss</label>
                <input type="number" formControlName="pointsForLoss" class="input-field" min="0" />
              </div>
            </div>
          </div>

          <!-- Eligibility -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Eligibility</h2>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Skill Level</label>
                <select formControlName="skillLevel" class="input-field">
                  @for (level of skillLevels; track level) {
                    <option [value]="level">{{ formatSkillLevel(level) }}</option>
                  }
                </select>
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Age Group</label>
                <select formControlName="ageGroup" class="input-field">
                  @for (group of ageGroups; track group) {
                    <option [value]="group">{{ formatAgeGroup(group) }}</option>
                  }
                </select>
              </div>
            </div>
          </div>

          <!-- Entry Fees & Prizes -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Entry Fees & Prizes</h2>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Entry Fee</label>
                <input type="number" formControlName="entryFee" class="input-field" min="0" step="0.01" placeholder="0.00" />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Venue</label>
                <select formControlName="venueId" class="input-field">
                  <option [ngValue]="null">No Venue</option>
                  @for (venue of venues(); track venue.id) {
                    <option [value]="venue.id">{{ venue.name }}</option>
                  }
                </select>
              </div>
              <div class="md:col-span-2">
                <label class="flex items-center">
                  <input type="checkbox" formControlName="hasPrizes" class="rounded border-gray-300 text-primary-600 mr-2" />
                  <span class="text-sm font-medium text-gray-700">Has Prizes</span>
                </label>
              </div>
              @if (form.get('hasPrizes')?.value) {
                <div>
                  <label class="block text-sm font-medium text-gray-700 mb-1">Total Prize Money</label>
                  <input type="number" formControlName="totalPrizeMoney" class="input-field" min="0" step="0.01" placeholder="0.00" />
                </div>
                <div class="md:col-span-2">
                  <label class="block text-sm font-medium text-gray-700 mb-1">Prize Description</label>
                  <textarea
                    formControlName="prizeDescription"
                    rows="2"
                    class="input-field"
                    placeholder="Describe the prizes..."
                  ></textarea>
                </div>
              }
            </div>
          </div>

          <!-- Contact Information -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Contact Information</h2>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Organizer Name</label>
                <input type="text" formControlName="organizerName" class="input-field" placeholder="Name of organizer" />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Contact Email</label>
                <input type="email" formControlName="contactEmail" class="input-field" placeholder="email@example.com" />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Contact Phone</label>
                <input type="tel" formControlName="contactPhone" class="input-field" placeholder="Phone number" />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Image URL</label>
                <input type="url" formControlName="imageUrl" class="input-field" placeholder="https://..." />
              </div>
            </div>
          </div>

          <!-- Rules -->
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Rules & Regulations</h2>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Competition Rules</label>
              <textarea
                formControlName="rules"
                rows="5"
                class="input-field"
                placeholder="Enter competition rules and regulations..."
              ></textarea>
            </div>
          </div>

          <!-- Publishing -->
          <div class="card">
            <label class="flex items-center">
              <input type="checkbox" formControlName="isPublished" class="rounded border-gray-300 text-primary-600 mr-2" />
              <span class="text-sm font-medium text-gray-700">Publish Competition</span>
            </label>
            <p class="text-sm text-gray-500 mt-1">Published competitions are visible to members</p>
          </div>

          <!-- Actions -->
          <div class="flex justify-end gap-4">
            <a routerLink="/club/competitions" class="btn-secondary">Cancel</a>
            <button type="submit" [disabled]="isSaving() || form.invalid" class="btn-primary">
              @if (isSaving()) {
                <svg class="animate-spin -ml-1 mr-2 h-4 w-4 text-white" fill="none" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                Saving...
              } @else {
                {{ isEditMode() ? 'Update Competition' : 'Create Competition' }}
              }
            </button>
          </div>
        </form>
      }
    </div>
  `
})
export class CompetitionFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private competitionService = inject(CompetitionService);
  private venueService = inject(VenueService);
  private notificationService = inject(NotificationService);

  isLoading = signal(true);
  isSaving = signal(false);
  isEditMode = signal(false);
  competitionId = signal<string | null>(null);
  seasons = signal<Season[]>([]);
  venues = signal<Venue[]>([]);

  competitionTypes = Object.values(CompetitionType);
  skillLevels = Object.values(SkillLevel);
  ageGroups = Object.values(AgeGroup);

  form: FormGroup = this.fb.group({
    name: ['', Validators.required],
    code: [''],
    description: [''],
    type: [CompetitionType.League, Validators.required],
    sport: [''],
    category: [''],
    division: [''],
    skillLevel: [SkillLevel.AllLevels],
    ageGroup: [AgeGroup.AllAges],
    startDate: [null],
    endDate: [null],
    registrationOpenDate: [null],
    registrationCloseDate: [null],
    seasonId: [null],
    venueId: [null],
    format: [''],
    numberOfRounds: [null],
    homeAndAway: [true],
    isTeamBased: [true],
    minTeams: [2],
    maxTeams: [null],
    minPlayersPerTeam: [null],
    maxPlayersPerTeam: [null],
    entryFee: [null],
    hasPrizes: [false],
    prizeDescription: [''],
    totalPrizeMoney: [null],
    pointsForWin: [3],
    pointsForDraw: [1],
    pointsForLoss: [0],
    rules: [''],
    organizerName: [''],
    contactEmail: [''],
    contactPhone: [''],
    imageUrl: [''],
    isPublished: [false]
  });

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.competitionId.set(id);
      this.isEditMode.set(true);
    }

    this.loadData();
  }

  loadData(): void {
    // Load seasons
    this.competitionService.getSeasons().subscribe({
      next: (seasons) => this.seasons.set(seasons),
      error: () => {}
    });

    // Load venues
    this.venueService.getVenues().subscribe({
      next: (venues) => this.venues.set(venues),
      error: () => {}
    });

    if (this.isEditMode()) {
      this.loadCompetition();
    } else {
      this.isLoading.set(false);
    }
  }

  loadCompetition(): void {
    const id = this.competitionId();
    if (!id) return;

    this.competitionService.getCompetition(id).subscribe({
      next: (competition) => {
        this.form.patchValue({
          name: competition.name,
          code: competition.code,
          description: competition.description,
          type: competition.type,
          sport: competition.sport,
          category: competition.category,
          division: competition.division,
          skillLevel: competition.skillLevel,
          ageGroup: competition.ageGroup,
          startDate: competition.startDate ? this.formatDateForInput(competition.startDate) : null,
          endDate: competition.endDate ? this.formatDateForInput(competition.endDate) : null,
          registrationOpenDate: competition.registrationOpenDate ? this.formatDateForInput(competition.registrationOpenDate) : null,
          registrationCloseDate: competition.registrationCloseDate ? this.formatDateForInput(competition.registrationCloseDate) : null,
          seasonId: competition.season?.id || null,
          venueId: competition.venue?.id || null,
          format: competition.format,
          numberOfRounds: competition.numberOfRounds,
          isTeamBased: competition.isTeamBased,
          minTeams: competition.minTeams,
          maxTeams: competition.maxTeams,
          minPlayersPerTeam: competition.minPlayersPerTeam,
          maxPlayersPerTeam: competition.maxPlayersPerTeam,
          entryFee: competition.entryFee,
          hasPrizes: competition.hasPrizes,
          prizeDescription: competition.prizeDescription,
          totalPrizeMoney: competition.totalPrizeMoney,
          pointsForWin: competition.pointsForWin,
          pointsForDraw: competition.pointsForDraw,
          pointsForLoss: competition.pointsForLoss,
          organizerName: competition.organizerName,
          contactEmail: competition.contactEmail,
          imageUrl: competition.imageUrl,
          isPublished: competition.isPublished
        });
        this.isLoading.set(false);
      },
      error: () => {
        this.notificationService.error('Failed to load competition');
        this.router.navigate(['/club/competitions']);
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    this.isSaving.set(true);
    const formValue = this.form.value;

    const data: CompetitionCreateRequest = {
      ...formValue,
      startDate: formValue.startDate ? new Date(formValue.startDate) : undefined,
      endDate: formValue.endDate ? new Date(formValue.endDate) : undefined,
      registrationOpenDate: formValue.registrationOpenDate ? new Date(formValue.registrationOpenDate) : undefined,
      registrationCloseDate: formValue.registrationCloseDate ? new Date(formValue.registrationCloseDate) : undefined
    };

    if (this.isEditMode()) {
      this.competitionService.updateCompetition(this.competitionId()!, data).subscribe({
        next: () => {
          this.notificationService.success('Competition updated successfully');
          this.router.navigate(['/club/competitions', this.competitionId()]);
        },
        error: () => {
          this.notificationService.error('Failed to update competition');
          this.isSaving.set(false);
        }
      });
    } else {
      this.competitionService.createCompetition(data).subscribe({
        next: (competition) => {
          this.notificationService.success('Competition created successfully');
          this.router.navigate(['/club/competitions', competition.id]);
        },
        error: () => {
          this.notificationService.error('Failed to create competition');
          this.isSaving.set(false);
        }
      });
    }
  }

  private formatDateForInput(date: Date | string): string {
    const d = new Date(date);
    return d.toISOString().split('T')[0];
  }

  formatType(type: CompetitionType): string {
    const labels: Record<CompetitionType, string> = {
      [CompetitionType.League]: 'League',
      [CompetitionType.Tournament]: 'Tournament',
      [CompetitionType.Cup]: 'Cup',
      [CompetitionType.Friendly]: 'Friendly',
      [CompetitionType.Championship]: 'Championship',
      [CompetitionType.Qualifier]: 'Qualifier',
      [CompetitionType.Playoff]: 'Playoff',
      [CompetitionType.RoundRobin]: 'Round Robin',
      [CompetitionType.Ladder]: 'Ladder',
      [CompetitionType.TimeTrial]: 'Time Trial',
      [CompetitionType.Other]: 'Other'
    };
    return labels[type] || type;
  }

  formatSkillLevel(level: SkillLevel): string {
    const labels: Record<SkillLevel, string> = {
      [SkillLevel.Beginner]: 'Beginner',
      [SkillLevel.Elementary]: 'Elementary',
      [SkillLevel.Intermediate]: 'Intermediate',
      [SkillLevel.UpperIntermediate]: 'Upper Intermediate',
      [SkillLevel.Advanced]: 'Advanced',
      [SkillLevel.Expert]: 'Expert',
      [SkillLevel.AllLevels]: 'All Levels'
    };
    return labels[level] || level;
  }

  formatAgeGroup(group: AgeGroup): string {
    const labels: Record<AgeGroup, string> = {
      [AgeGroup.Infant]: 'Infant (0-2)',
      [AgeGroup.Toddler]: 'Toddler (3-5)',
      [AgeGroup.Child]: 'Child (6-8)',
      [AgeGroup.PreTeen]: 'Pre-Teen (9-12)',
      [AgeGroup.Teen]: 'Teen (13-17)',
      [AgeGroup.Adult]: 'Adult (18-64)',
      [AgeGroup.Senior]: 'Senior (65+)',
      [AgeGroup.AllAges]: 'All Ages'
    };
    return labels[group] || group;
  }
}
