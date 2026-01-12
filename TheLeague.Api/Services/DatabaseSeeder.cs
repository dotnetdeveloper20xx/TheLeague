using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using TheLeague.Core.Entities;
using TheLeague.Core.Enums;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Services;

public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<DatabaseSeeder> _logger;

    private SeedDataRoot? _seedData;
    private readonly Dictionary<string, Guid> _clubIdMap = new();
    private readonly Dictionary<string, Guid> _memberIdMap = new();
    private readonly Dictionary<string, MembershipType> _membershipTypeMap = new();
    private readonly Dictionary<string, Venue> _venueMap = new();
    private readonly Dictionary<string, Member> _memberMap = new();

    public DatabaseSeeder(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IWebHostEnvironment environment,
        ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _environment = environment;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        // Load seed data from JSON file
        await LoadSeedDataAsync();

        await SeedRolesAsync();
        await SeedSuperAdminAsync();
        await SeedSystemConfigurationAsync();
        await SeedClubsAsync();
        await _context.SaveChangesAsync();
    }

    private async Task LoadSeedDataAsync()
    {
        var seedFilePath = Path.Combine(_environment.ContentRootPath, "..", "seedData.json");

        if (!File.Exists(seedFilePath))
        {
            _logger.LogWarning("Seed data file not found at {Path}, using defaults", seedFilePath);
            _seedData = new SeedDataRoot();
            return;
        }

        try
        {
            var jsonContent = await File.ReadAllTextAsync(seedFilePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _seedData = JsonSerializer.Deserialize<SeedDataRoot>(jsonContent, options) ?? new SeedDataRoot();
            _logger.LogInformation("Loaded seed data from {Path}", seedFilePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load seed data from {Path}", seedFilePath);
            _seedData = new SeedDataRoot();
        }
    }

    private async Task SeedSystemConfigurationAsync()
    {
        if (_context.SystemConfigurations.Any())
        {
            return;
        }

        var configData = _seedData?.SystemConfiguration ?? new SystemConfigurationSeed();

        var config = new SystemConfiguration
        {
            Id = Guid.NewGuid(),
            PaymentProvider = configData.PaymentProvider,
            MockPaymentDelayMs = configData.MockPaymentDelayMs,
            MockPaymentFailureRate = configData.MockPaymentFailureRate,
            EmailProvider = configData.EmailProvider,
            MockEmailDelayMs = configData.MockEmailDelayMs,
            DefaultFromEmail = configData.DefaultFromEmail,
            DefaultFromName = configData.DefaultFromName,
            MaintenanceMode = configData.MaintenanceMode,
            AllowNewRegistrations = configData.AllowNewRegistrations,
            EnableEmailNotifications = configData.EnableEmailNotifications,
            PlatformName = configData.PlatformName,
            PrimaryColor = configData.PrimaryColor,
            CreatedAt = DateTime.UtcNow,
            LastModifiedAt = DateTime.UtcNow,
            LastModifiedBy = "System",
            Version = 1
        };

        _context.SystemConfigurations.Add(config);

        _context.ConfigurationAuditLogs.Add(new ConfigurationAuditLog
        {
            Id = Guid.NewGuid(),
            Action = "Created",
            Section = "System",
            PropertyChanged = null,
            OldValue = null,
            NewValue = "Default configuration created",
            ChangedBy = "System",
            Timestamp = DateTime.UtcNow,
            IpAddress = null
        });
    }

    private async Task SeedRolesAsync()
    {
        var roles = new[] { "SuperAdmin", "ClubManager", "Member" };
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private async Task SeedSuperAdminAsync()
    {
        var adminData = _seedData?.Users?.SuperAdmin ?? new SuperAdminSeed
        {
            Email = "admin@theleague.com",
            Password = "Admin123!",
            FirstName = "Super",
            LastName = "Admin"
        };

        var existingAdmin = await _userManager.FindByEmailAsync(adminData.Email);

        if (existingAdmin == null)
        {
            var admin = new ApplicationUser
            {
                UserName = adminData.Email,
                Email = adminData.Email,
                EmailConfirmed = true,
                FirstName = adminData.FirstName,
                LastName = adminData.LastName,
                Role = UserRole.SuperAdmin,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(admin, adminData.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, "SuperAdmin");
                _logger.LogInformation("Created super admin: {Email}", adminData.Email);
            }
        }
    }

    private async Task SeedClubsAsync()
    {
        if (_context.Clubs.Any())
        {
            return;
        }

        var clubs = _seedData?.Clubs ?? new List<ClubSeed>();

        if (!clubs.Any())
        {
            _logger.LogWarning("No clubs in seed data, creating default clubs");
            await SeedDefaultClubsAsync();
            return;
        }

        foreach (var clubData in clubs)
        {
            var clubId = Guid.NewGuid();
            _clubIdMap[clubData.Id] = clubId;

            var clubType = clubData.Type switch
            {
                "Tennis" => ClubType.Tennis,
                "Multi-Sport" or "MultiSport" => ClubType.MultiSport,
                "Golf" => ClubType.Golf,
                "Swimming" => ClubType.Swimming,
                "Football" => ClubType.Football,
                "Cricket" => ClubType.Cricket,
                _ => ClubType.Other
            };

            var club = new Club
            {
                Id = clubId,
                Name = clubData.Name,
                Slug = clubData.Name.ToLower().Replace(" ", "-"),
                Description = clubData.Description,
                ClubType = clubType,
                ContactEmail = clubData.Email,
                ContactPhone = clubData.Phone,
                Address = $"{clubData.Address}, {clubData.City} {clubData.PostCode}",
                Website = clubData.Website,
                IsActive = clubData.IsActive,
                CreatedAt = DateTime.UtcNow.AddMonths(-12),
                LogoUrl = clubData.LogoUrl,
                PrimaryColor = clubData.PrimaryColor,
                SecondaryColor = clubData.SecondaryColor,
                PreferredPaymentProvider = PaymentProvider.Stripe
            };
            _context.Clubs.Add(club);

            var settings = new ClubSettings
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                AllowOnlineRegistration = true,
                RequireEmergencyContact = true,
                RequireMedicalInfo = false,
                AllowFamilyAccounts = true,
                AllowOnlinePayments = true,
                AllowManualPayments = true,
                AutoSendPaymentReminders = true,
                PaymentReminderDaysBefore = 14,
                AllowMemberBookings = true,
                MaxAdvanceBookingDays = 14,
                CancellationNoticePeriodHours = 24,
                EnableWaitlist = true,
                SendWelcomeEmail = true,
                SendBookingConfirmations = true,
                SendPaymentReceipts = true
            };
            _context.ClubSettings.Add(settings);

            _logger.LogInformation("Created club: {ClubName}", clubData.Name);
        }

        await _context.SaveChangesAsync();

        // Seed club managers
        await SeedClubManagersAsync();

        // Seed data for the first club (the demo club)
        var firstClubId = _clubIdMap.Values.First();
        var firstClubSeedId = _clubIdMap.Keys.First();

        await SeedMembershipTypesAsync(firstClubId, firstClubSeedId);
        await SeedVenuesAsync(firstClubId, firstClubSeedId);
        await _context.SaveChangesAsync();

        await SeedMembersAsync(firstClubId, firstClubSeedId);
        await _context.SaveChangesAsync();

        await SeedFeesAsync(firstClubId, firstClubSeedId);
        await SeedRecurringSchedulesAsync(firstClubId, firstClubSeedId);
        await SeedSessionsAsync(firstClubId, firstClubSeedId);
        await SeedEventsAsync(firstClubId, firstClubSeedId);
        await _context.SaveChangesAsync();

        await SeedCompetitionsAsync(firstClubId, firstClubSeedId);
        await SeedPaymentsAsync(firstClubId);
        await SeedInvoicesAsync(firstClubId);
        await _context.SaveChangesAsync();

        // Create bookings for sessions
        CreateBookings();
        await _context.SaveChangesAsync();

        // Seed communication templates
        SeedCommunicationTemplates(firstClubId);
        await _context.SaveChangesAsync();
    }

    private async Task SeedClubManagersAsync()
    {
        var managers = _seedData?.Users?.ClubManagers ?? new List<ClubManagerSeed>();

        foreach (var managerData in managers)
        {
            if (!_clubIdMap.TryGetValue(managerData.ClubId, out var clubId))
            {
                continue;
            }

            var existingManager = await _userManager.FindByEmailAsync(managerData.Email);
            if (existingManager != null) continue;

            var manager = new ApplicationUser
            {
                UserName = managerData.Email,
                Email = managerData.Email,
                EmailConfirmed = true,
                FirstName = managerData.FirstName,
                LastName = managerData.LastName,
                ClubId = clubId,
                Role = UserRole.ClubManager,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(manager, managerData.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(manager, "ClubManager");
                _logger.LogInformation("Created club manager: {Email}", managerData.Email);
            }
        }
    }

    private async Task SeedMembershipTypesAsync(Guid clubId, string seedClubId)
    {
        var types = _seedData?.MembershipTypes?.Where(t => t.ClubId == seedClubId).ToList()
            ?? new List<MembershipTypeSeed>();

        foreach (var typeData in types)
        {
            var category = typeData.Name switch
            {
                var n when n.Contains("Junior") => MembershipCategory.Junior,
                var n when n.Contains("Senior") => MembershipCategory.Senior,
                var n when n.Contains("Family") => MembershipCategory.Family,
                var n when n.Contains("Student") => MembershipCategory.Student,
                _ => MembershipCategory.Individual
            };

            var membershipType = new MembershipType
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                Name = typeData.Name,
                ShortDescription = typeData.Name,
                Description = typeData.Description,
                Category = category,
                ColorCode = category switch
                {
                    MembershipCategory.Junior => "#F59E0B",
                    MembershipCategory.Senior => "#6366F1",
                    MembershipCategory.Family => "#8B5CF6",
                    MembershipCategory.Student => "#EC4899",
                    _ => "#3B82F6"
                },
                BasePrice = typeData.AnnualFee,
                AnnualFee = typeData.AnnualFee,
                MonthlyFee = typeData.MonthlyFee,
                SessionFee = typeData.SessionFee,
                DefaultBillingCycle = BillingCycle.Annual,
                AllowMonthlyPayment = true,
                ProRataEnabled = true,
                AccessType = AccessType.FullAccess,
                MinAge = typeData.MinAge,
                MaxAge = typeData.MaxAge,
                MaxFamilyMembers = typeData.MaxFamilyMembers > 0 ? typeData.MaxFamilyMembers : null,
                IsActive = typeData.IsActive,
                AllowOnlineSignup = typeData.AllowOnlineSignup,
                ShowOnWebsite = true,
                IncludesBooking = typeData.IncludesBooking,
                IncludesEvents = typeData.IncludesEvents,
                IncludesClasses = true,
                MaxSessionsPerWeek = typeData.MaxSessionsPerWeek,
                AdvanceBookingDays = 14,
                AllowFreeze = true,
                MaxFreezeDays = 60,
                RenewalReminderDays = 30,
                GracePeriodDays = 14,
                SortOrder = typeData.SortOrder
            };

            _context.MembershipTypes.Add(membershipType);
            _membershipTypeMap[$"{seedClubId}:{typeData.Name}"] = membershipType;
            _logger.LogInformation("Created membership type: {Name}", typeData.Name);
        }
    }

    private async Task SeedVenuesAsync(Guid clubId, string seedClubId)
    {
        var venues = _seedData?.Venues?.Where(v => v.ClubId == seedClubId).ToList()
            ?? new List<VenueSeed>();

        var index = 0;
        foreach (var venueData in venues)
        {
            var venue = new Venue
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                Name = venueData.Name,
                Description = venueData.Description,
                AddressLine1 = venueData.Address,
                PostCode = venueData.PostCode,
                TotalCapacity = venueData.Capacity,
                Notes = venueData.Facilities,
                IsActive = venueData.IsActive,
                IsPrimary = venueData.IsPrimary
            };

            _context.Venues.Add(venue);
            _venueMap[$"{seedClubId}:{index}"] = venue;
            index++;
            _logger.LogInformation("Created venue: {Name}", venueData.Name);
        }
    }

    private async Task SeedMembersAsync(Guid clubId, string seedClubId)
    {
        var members = _seedData?.Members?.Where(m => m.ClubId == seedClubId).ToList()
            ?? new List<MemberSeed>();

        var memberIndex = 1;
        foreach (var memberData in members)
        {
            var memberId = Guid.NewGuid();
            _memberIdMap[memberData.Email] = memberId;

            var status = memberData.Status switch
            {
                "Active" => MemberStatus.Active,
                "Expired" => MemberStatus.Expired,
                "Suspended" => MemberStatus.Suspended,
                "Pending" => MemberStatus.Pending,
                _ => MemberStatus.Active
            };

            var joinedDate = DateTime.Parse(memberData.JoinedDate);

            var member = new Member
            {
                Id = memberId,
                ClubId = clubId,
                MemberNumber = $"MBR-{memberIndex:D4}",
                QRCodeData = $"{clubId}|{memberId}|MBR-{memberIndex:D4}",
                FirstName = memberData.FirstName,
                LastName = memberData.LastName,
                Email = memberData.Email,
                Phone = memberData.Phone,
                DateOfBirth = DateTime.Parse(memberData.DateOfBirth),
                Gender = Gender.PreferNotToSay,
                Address = memberData.Address,
                City = memberData.City,
                PostCode = memberData.PostCode,
                Country = "United Kingdom",
                JoinedDate = joinedDate,
                Status = status,
                IsActive = status == MemberStatus.Active,
                EmergencyContactName = memberData.EmergencyContactName,
                EmergencyContactPhone = memberData.EmergencyContactPhone,
                EmergencyContactRelation = memberData.EmergencyContactRelation,
                MedicalNotes = memberData.MedicalConditions,
                Allergies = memberData.Allergies,
                IsFamilyAccount = memberData.IsFamilyAccount,
                ApplicationStatus = ApplicationStatus.Approved,
                ApplicationDate = joinedDate.AddDays(-7),
                ApprovalDate = joinedDate,
                WaiverAccepted = true,
                WaiverAcceptedDate = joinedDate,
                TermsAccepted = true,
                TermsAcceptedDate = joinedDate,
                EmailOptIn = true,
                SmsOptIn = false,
                MarketingOptIn = true
            };

            // Create user account
            var user = new ApplicationUser
            {
                UserName = memberData.Email,
                Email = memberData.Email,
                EmailConfirmed = true,
                FirstName = memberData.FirstName,
                LastName = memberData.LastName,
                ClubId = clubId,
                MemberId = memberId,
                Role = UserRole.Member,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, memberData.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Member");
                member.UserId = user.Id;
            }

            _context.Members.Add(member);
            _memberMap[memberData.Email] = member;

            // Create membership
            var membershipTypeKey = $"{seedClubId}:{memberData.MembershipType}";
            if (_membershipTypeMap.TryGetValue(membershipTypeKey, out var membershipType))
            {
                var membershipStatus = status switch
                {
                    MemberStatus.Active => MembershipStatus.Active,
                    MemberStatus.Expired => MembershipStatus.Expired,
                    MemberStatus.Pending => MembershipStatus.PendingPayment,
                    _ => MembershipStatus.Active
                };

                var endDate = status == MemberStatus.Expired
                    ? joinedDate.AddYears(1)
                    : DateTime.UtcNow.AddMonths(6);

                var membership = new Membership
                {
                    Id = Guid.NewGuid(),
                    ClubId = clubId,
                    MemberId = memberId,
                    MembershipTypeId = membershipType.Id,
                    StartDate = joinedDate,
                    EndDate = endDate,
                    BillingCycle = BillingCycle.Annual,
                    Status = membershipStatus,
                    AmountPaid = membershipType.AnnualFee,
                    AmountDue = 0,
                    AutoRenew = status == MemberStatus.Active,
                    LastPaymentDate = joinedDate
                };
                _context.Memberships.Add(membership);
            }

            // Create family members
            if (memberData.FamilyMembers != null)
            {
                foreach (var familyData in memberData.FamilyMembers)
                {
                    var relation = familyData.Relation switch
                    {
                        "Spouse" => FamilyMemberRelation.Spouse,
                        "Child" => FamilyMemberRelation.Child,
                        "Parent" => FamilyMemberRelation.Parent,
                        "Sibling" => FamilyMemberRelation.Sibling,
                        _ => FamilyMemberRelation.Other
                    };

                    var familyMember = new FamilyMember
                    {
                        Id = Guid.NewGuid(),
                        ClubId = clubId,
                        PrimaryMemberId = memberId,
                        FirstName = familyData.FirstName,
                        LastName = familyData.LastName,
                        DateOfBirth = DateTime.Parse(familyData.DateOfBirth),
                        Relation = relation,
                        MedicalConditions = familyData.MedicalConditions,
                        Allergies = familyData.Allergies,
                        IsActive = true
                    };
                    _context.FamilyMembers.Add(familyMember);
                }
            }

            memberIndex++;
            _logger.LogInformation("Created member: {Name}", $"{memberData.FirstName} {memberData.LastName}");
        }
    }

    private async Task SeedFeesAsync(Guid clubId, string seedClubId)
    {
        var fees = _seedData?.Fees?.Where(f => f.ClubId == seedClubId).ToList()
            ?? new List<FeeSeed>();

        foreach (var feeData in fees)
        {
            var feeType = feeData.Type switch
            {
                "Membership" => FeeType.Membership,
                "Session" => FeeType.ClassFee,
                "Training" => FeeType.Training,
                "Competition" => FeeType.EventParticipation,
                "Equipment" => FeeType.EquipmentRental,
                _ => FeeType.Other
            };

            var frequency = feeData.Frequency switch
            {
                "Annual" => FeeFrequency.Annual,
                "Monthly" => FeeFrequency.Monthly,
                "Quarterly" => FeeFrequency.Quarterly,
                _ => FeeFrequency.OneTime
            };

            var fee = new Fee
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                Name = feeData.Name,
                Description = feeData.Description,
                Type = feeType,
                Amount = feeData.Amount,
                Currency = feeData.Currency,
                Frequency = frequency,
                IsActive = feeData.IsActive,
                IsTaxable = feeData.Taxable,
                TaxRate = feeData.TaxRate
            };

            _context.Fees.Add(fee);
        }
    }

    private async Task SeedRecurringSchedulesAsync(Guid clubId, string seedClubId)
    {
        var schedules = _seedData?.RecurringSchedules?.Where(s => s.ClubId == seedClubId).ToList()
            ?? new List<RecurringScheduleSeed>();

        foreach (var scheduleData in schedules)
        {
            var venueKey = $"{seedClubId}:{scheduleData.VenueIndex}";
            if (!_venueMap.TryGetValue(venueKey, out var venue))
            {
                continue;
            }

            var category = scheduleData.Category switch
            {
                "Juniors" => SessionCategory.Juniors,
                "Seniors" => SessionCategory.Seniors,
                "Beginners" => SessionCategory.Beginners,
                "Advanced" => SessionCategory.Advanced,
                "Mixed" => SessionCategory.Mixed,
                _ => SessionCategory.AllAges
            };

            var dayOfWeek = Enum.Parse<DayOfWeek>(scheduleData.DayOfWeek);
            var startTime = TimeSpan.Parse(scheduleData.StartTime);
            var endTime = TimeSpan.Parse(scheduleData.EndTime);

            var schedule = new RecurringSchedule
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                VenueId = venue.Id,
                Title = scheduleData.Title,
                Description = scheduleData.Description,
                Category = category,
                DayOfWeek = dayOfWeek,
                StartTime = startTime,
                EndTime = endTime,
                Capacity = scheduleData.Capacity,
                SessionFee = scheduleData.SessionFee > 0 ? scheduleData.SessionFee : null,
                IsActive = scheduleData.IsActive
            };

            _context.RecurringSchedules.Add(schedule);
        }
    }

    private async Task SeedSessionsAsync(Guid clubId, string seedClubId)
    {
        var sessions = _seedData?.Sessions?.Where(s => s.ClubId == seedClubId).ToList()
            ?? new List<SessionSeed>();

        foreach (var sessionData in sessions)
        {
            var venueKey = $"{seedClubId}:{sessionData.VenueIndex}";
            if (!_venueMap.TryGetValue(venueKey, out var venue))
            {
                continue;
            }

            var category = sessionData.Category switch
            {
                "Juniors" => SessionCategory.Juniors,
                "Seniors" => SessionCategory.Seniors,
                "Beginners" => SessionCategory.Beginners,
                "Advanced" => SessionCategory.Advanced,
                "Mixed" => SessionCategory.Mixed,
                _ => SessionCategory.AllAges
            };

            var date = DateTime.UtcNow.Date.AddDays(sessionData.DaysFromNow);
            var startTime = TimeSpan.Parse(sessionData.StartTime);
            var endTime = TimeSpan.Parse(sessionData.EndTime);

            var session = new Session
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                VenueId = venue.Id,
                Title = sessionData.Title,
                Description = sessionData.Description,
                Category = category,
                StartTime = date.Add(startTime),
                EndTime = date.Add(endTime),
                Capacity = sessionData.Capacity,
                CurrentBookings = 0,
                SessionFee = sessionData.SessionFee > 0 ? sessionData.SessionFee : null,
                IsCancelled = false
            };

            _context.Sessions.Add(session);
        }
    }

    private async Task SeedEventsAsync(Guid clubId, string seedClubId)
    {
        var events = _seedData?.Events?.Where(e => e.ClubId == seedClubId).ToList()
            ?? new List<EventSeed>();

        foreach (var eventData in events)
        {
            var venueKey = $"{seedClubId}:{eventData.VenueIndex}";
            if (!_venueMap.TryGetValue(venueKey, out var venue))
            {
                continue;
            }

            var eventType = eventData.Type switch
            {
                "Tournament" => EventType.Tournament,
                "Social" => EventType.Social,
                "Training" => EventType.Training,
                "AGM" => EventType.AGM,
                "Fundraiser" => EventType.Fundraiser,
                "Competition" => EventType.Competition,
                _ => EventType.Other
            };

            var skillLevel = eventData.SkillLevel switch
            {
                "Beginner" => SkillLevel.Beginner,
                "Intermediate" => SkillLevel.Intermediate,
                "Advanced" => SkillLevel.Advanced,
                _ => SkillLevel.AllLevels
            };

            var startDate = DateTime.UtcNow.Date.AddDays(eventData.DaysFromNow);
            var startTime = TimeSpan.Parse(eventData.StartTime);
            var endTime = TimeSpan.Parse(eventData.EndTime);

            var evt = new Event
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                VenueId = venue.Id,
                Title = eventData.Title,
                Description = eventData.Description,
                Type = eventType,
                StartDateTime = startDate.Add(startTime),
                EndDateTime = startDate.AddDays(eventData.Duration - 1).Add(endTime),
                Capacity = eventData.Capacity,
                CurrentAttendees = 0,
                IsTicketed = eventData.IsTicketed,
                TicketPrice = eventData.TicketPrice,
                MemberTicketPrice = eventData.MemberTicketPrice,
                RequiresRSVP = eventData.RequiresRSVP,
                MembersOnly = eventData.IsMembersOnly,
                SkillLevel = skillLevel,
                Status = EventStatus.Upcoming,
                IsPublished = true
            };

            _context.Events.Add(evt);
        }
    }

    private async Task SeedCompetitionsAsync(Guid clubId, string seedClubId)
    {
        var competitions = _seedData?.Competitions?.Where(c => c.ClubId == seedClubId).ToList()
            ?? new List<CompetitionSeed>();

        foreach (var compData in competitions)
        {
            var competition = new Competition
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                Name = compData.Name,
                Description = compData.Description,
                Sport = compData.Sport,
                Format = compData.Format, // Format is a string in Competition
                StartDate = DateTime.Parse(compData.StartDate),
                EndDate = DateTime.Parse(compData.EndDate),
                EntryFee = compData.EntryFee,
                IsPublished = compData.IsPublished,
                Status = CompetitionStatus.InProgress
            };

            _context.Competitions.Add(competition);

            // Create teams
            foreach (var teamData in compData.Teams)
            {
                // Find captain member if exists
                var captainMember = _memberMap.Values
                    .FirstOrDefault(m => $"{m.FirstName} {m.LastName}" == teamData.Captain);

                var team = new CompetitionTeam
                {
                    Id = Guid.NewGuid(),
                    ClubId = clubId,
                    CompetitionId = competition.Id,
                    Name = teamData.Name,
                    CaptainId = captainMember?.Id,
                    HomeColors = teamData.Color,
                    Played = 0,
                    Won = 0,
                    Drawn = 0,
                    Lost = 0,
                    GoalsFor = 0,
                    GoalsAgainst = 0,
                    Points = 0
                };

                _context.CompetitionTeams.Add(team);
            }
        }
    }

    private async Task SeedPaymentsAsync(Guid clubId)
    {
        var payments = _seedData?.Payments ?? new List<PaymentSeed>();

        foreach (var paymentData in payments)
        {
            if (!_memberIdMap.TryGetValue(paymentData.MemberEmail, out var memberId))
            {
                continue;
            }

            var paymentType = paymentData.Type switch
            {
                "Membership" => PaymentType.Membership,
                "EventTicket" => PaymentType.EventTicket,
                "SessionFee" => PaymentType.SessionFee,
                _ => PaymentType.Other
            };

            var paymentMethod = paymentData.Method switch
            {
                "Stripe" => PaymentMethod.Stripe,
                "PayPal" => PaymentMethod.PayPal,
                "Cash" => PaymentMethod.Cash,
                "BankTransfer" => PaymentMethod.BankTransfer,
                "Cheque" => PaymentMethod.Cheque,
                _ => PaymentMethod.Stripe
            };

            var paymentStatus = paymentData.Status switch
            {
                "Completed" => PaymentStatus.Completed,
                "Pending" => PaymentStatus.Pending,
                "Failed" => PaymentStatus.Failed,
                "Refunded" => PaymentStatus.Refunded,
                _ => PaymentStatus.Completed
            };

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                MemberId = memberId,
                Amount = paymentData.Amount,
                NetAmount = paymentData.Amount,
                Currency = "GBP",
                Type = paymentType,
                Method = paymentMethod,
                Status = paymentStatus,
                Description = paymentData.Description,
                ReferenceNumber = $"txn_{Guid.NewGuid():N}",
                PaymentDate = DateTime.UtcNow.AddDays(-paymentData.DaysAgo)
            };

            _context.Payments.Add(payment);
        }
    }

    private async Task SeedInvoicesAsync(Guid clubId)
    {
        var invoices = _seedData?.Invoices ?? new List<InvoiceSeed>();

        foreach (var invoiceData in invoices)
        {
            if (!_memberIdMap.TryGetValue(invoiceData.MemberEmail, out var memberId))
            {
                continue;
            }

            var invoiceStatus = invoiceData.Status switch
            {
                "Paid" => InvoiceStatus.Paid,
                "Pending" => InvoiceStatus.Sent,
                "Overdue" => InvoiceStatus.Overdue,
                "Cancelled" => InvoiceStatus.Voided,
                _ => InvoiceStatus.Sent
            };

            var invoiceDate = DateTime.UtcNow.AddDays(-invoiceData.DaysAgo);
            var dueDate = DateTime.UtcNow.AddDays(invoiceData.DueDate);

            var invoice = new Invoice
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                MemberId = memberId,
                InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMM}-{Guid.NewGuid().ToString()[..4].ToUpper()}",
                InvoiceDate = invoiceDate,
                DueDate = dueDate,
                SubTotal = invoiceData.Amount,
                TaxAmount = 0,
                TotalAmount = invoiceData.Amount,
                PaidAmount = invoiceStatus == InvoiceStatus.Paid ? invoiceData.Amount : 0,
                BalanceDue = invoiceStatus == InvoiceStatus.Paid ? 0 : invoiceData.Amount,
                Status = invoiceStatus,
                Notes = invoiceData.Description
            };

            _context.Invoices.Add(invoice);

            // Add line items
            foreach (var lineItemData in invoiceData.LineItems)
            {
                var lineItem = new InvoiceLineItem
                {
                    Id = Guid.NewGuid(),
                    InvoiceId = invoice.Id,
                    Description = lineItemData.Description,
                    Quantity = 1,
                    UnitPrice = lineItemData.Amount,
                    SubTotal = lineItemData.Amount,
                    Total = lineItemData.Amount
                };
                _context.InvoiceLineItems.Add(lineItem);
            }
        }
    }

    private void CreateBookings()
    {
        var sessions = _context.Sessions.Local.ToList();
        var members = _memberMap.Values.Where(m => m.Status == MemberStatus.Active).ToList();
        var random = new Random(42);

        foreach (var session in sessions.Take(5))
        {
            var bookingsCount = Math.Min(random.Next(2, 6), session.Capacity);
            var selectedMembers = members.OrderBy(_ => random.Next()).Take(bookingsCount).ToList();

            foreach (var member in selectedMembers)
            {
                var booking = new SessionBooking
                {
                    Id = Guid.NewGuid(),
                    ClubId = session.ClubId,
                    SessionId = session.Id,
                    MemberId = member.Id,
                    BookedAt = DateTime.UtcNow.AddDays(-random.Next(1, 7)),
                    Status = BookingStatus.Confirmed,
                    Attended = false
                };

                _context.SessionBookings.Add(booking);
                session.CurrentBookings++;
            }
        }
    }

    private void SeedCommunicationTemplates(Guid clubId)
    {
        var templates = new[]
        {
            new CommunicationTemplate
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                Name = "Welcome Email",
                Subject = "Welcome to {{ClubName}}!",
                Body = "Dear {{MemberName}},\n\nWelcome to {{ClubName}}! We're thrilled to have you as a member.\n\nYour membership details:\n- Membership Type: {{MembershipType}}\n- Start Date: {{StartDate}}\n\nIf you have any questions, please don't hesitate to contact us.\n\nBest regards,\nThe {{ClubName}} Team",
                Type = TemplateType.Email,
                Category = TemplateCategory.Welcome,
                IsActive = true
            },
            new CommunicationTemplate
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                Name = "Payment Reminder",
                Subject = "Payment Reminder - {{ClubName}}",
                Body = "Dear {{MemberName}},\n\nThis is a friendly reminder that your membership payment of {{Amount}} is due on {{DueDate}}.\n\nPlease log in to your account to make the payment.\n\nThank you,\nThe {{ClubName}} Team",
                Type = TemplateType.Email,
                Category = TemplateCategory.PaymentReminder,
                IsActive = true
            },
            new CommunicationTemplate
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                Name = "Booking Confirmation",
                Subject = "Booking Confirmed - {{SessionTitle}}",
                Body = "Dear {{MemberName}},\n\nYour booking has been confirmed!\n\nSession: {{SessionTitle}}\nDate: {{SessionDate}}\nTime: {{SessionTime}}\nVenue: {{Venue}}\n\nWe look forward to seeing you!\n\nBest regards,\nThe {{ClubName}} Team",
                Type = TemplateType.Email,
                Category = TemplateCategory.BookingConfirmation,
                IsActive = true
            },
            new CommunicationTemplate
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                Name = "Renewal Reminder",
                Subject = "Your membership is expiring soon - {{ClubName}}",
                Body = "Dear {{MemberName}},\n\nYour {{MembershipType}} membership will expire on {{ExpiryDate}}.\n\nRenew now to continue enjoying all club benefits without interruption.\n\nRenewal amount: {{RenewalAmount}}\n\nThank you for being a valued member!\n\nThe {{ClubName}} Team",
                Type = TemplateType.Email,
                Category = TemplateCategory.MembershipRenewal,
                IsActive = true
            }
        };

        _context.CommunicationTemplates.AddRange(templates);
    }

    private async Task SeedDefaultClubsAsync()
    {
        // Fallback to creating default clubs if no seed data
        var club1Id = Guid.NewGuid();
        var club1 = new Club
        {
            Id = club1Id,
            Name = "Riverside Tennis Club",
            Slug = "riverside",
            Description = "Premier tennis club in London with excellent facilities",
            ClubType = ClubType.Tennis,
            ContactEmail = "info@riversidetc.com",
            ContactPhone = "+44 20 1234 5678",
            Address = "123 River Road, London SW1A 1AA",
            Website = "https://riversidetc.com",
            IsActive = true,
            CreatedAt = DateTime.UtcNow.AddMonths(-12),
            PrimaryColor = "#2E7D32",
            SecondaryColor = "#81C784"
        };
        _context.Clubs.Add(club1);

        var settings = new ClubSettings
        {
            Id = Guid.NewGuid(),
            ClubId = club1Id,
            AllowOnlineRegistration = true,
            RequireEmergencyContact = true,
            AllowFamilyAccounts = true,
            AllowOnlinePayments = true,
            AllowManualPayments = true,
            AllowMemberBookings = true,
            MaxAdvanceBookingDays = 14
        };
        _context.ClubSettings.Add(settings);
    }
}
