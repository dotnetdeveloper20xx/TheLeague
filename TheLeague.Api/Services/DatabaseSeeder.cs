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

    public DatabaseSeeder(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedSuperAdminAsync();
        await SeedClubsAsync();
        await _context.SaveChangesAsync();
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
        var adminEmail = "admin@theleague.com";
        var existingAdmin = await _userManager.FindByEmailAsync(adminEmail);

        if (existingAdmin == null)
        {
            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "Super",
                LastName = "Admin",
                Role = UserRole.SuperAdmin,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(admin, "Admin123!");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, "SuperAdmin");
            }
        }
    }

    private async Task SeedClubsAsync()
    {
        // Club 1: Riverside Tennis Club
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
            LogoUrl = null,
            PrimaryColor = "#2E7D32",
            SecondaryColor = "#81C784",
            PreferredPaymentProvider = PaymentProvider.Stripe
        };
        _context.Clubs.Add(club1);

        var club1Settings = new ClubSettings
        {
            Id = Guid.NewGuid(),
            ClubId = club1Id,
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
        _context.ClubSettings.Add(club1Settings);

        // Club 2: Downtown Basketball Academy
        var club2Id = Guid.NewGuid();
        var club2 = new Club
        {
            Id = club2Id,
            Name = "Downtown Basketball Academy",
            Slug = "downtown-basketball",
            Description = "Youth basketball training academy in Manchester",
            ClubType = ClubType.MultiSport,
            ContactEmail = "contact@downtownbasketball.com",
            ContactPhone = "+44 20 2345 6789",
            Address = "456 Court Street, Manchester M1 1AA",
            Website = "https://downtownbasketball.com",
            IsActive = true,
            CreatedAt = DateTime.UtcNow.AddMonths(-8),
            LogoUrl = null,
            PrimaryColor = "#D32F2F",
            SecondaryColor = "#EF5350",
            PreferredPaymentProvider = PaymentProvider.PayPal
        };
        _context.Clubs.Add(club2);

        var club2Settings = new ClubSettings
        {
            Id = Guid.NewGuid(),
            ClubId = club2Id,
            AllowOnlineRegistration = true,
            RequireEmergencyContact = true,
            RequireMedicalInfo = true,
            AllowFamilyAccounts = true,
            AllowOnlinePayments = true,
            AllowManualPayments = true,
            AutoSendPaymentReminders = true,
            PaymentReminderDaysBefore = 7,
            AllowMemberBookings = true,
            MaxAdvanceBookingDays = 7,
            CancellationNoticePeriodHours = 12,
            EnableWaitlist = true
        };
        _context.ClubSettings.Add(club2Settings);

        // Club 3: Lakeside Swimming Club
        var club3Id = Guid.NewGuid();
        var club3 = new Club
        {
            Id = club3Id,
            Name = "Lakeside Swimming Club",
            Slug = "lakeside-swimming",
            Description = "Competitive swimming club with Olympic-size pool",
            ClubType = ClubType.Swimming,
            ContactEmail = "hello@lakesideswim.com",
            ContactPhone = "+44 20 3456 7890",
            Address = "789 Lake Avenue, Birmingham B1 1AA",
            Website = "https://lakesideswim.com",
            IsActive = true,
            CreatedAt = DateTime.UtcNow.AddMonths(-6),
            LogoUrl = null,
            PrimaryColor = "#1976D2",
            SecondaryColor = "#64B5F6",
            PreferredPaymentProvider = PaymentProvider.Stripe
        };
        _context.Clubs.Add(club3);

        var club3Settings = new ClubSettings
        {
            Id = Guid.NewGuid(),
            ClubId = club3Id,
            AllowOnlineRegistration = true,
            RequireEmergencyContact = true,
            RequireMedicalInfo = true,
            AllowFamilyAccounts = true,
            AllowOnlinePayments = true,
            AllowManualPayments = true,
            AutoSendPaymentReminders = true,
            PaymentReminderDaysBefore = 21,
            AllowMemberBookings = true,
            MaxAdvanceBookingDays = 21,
            CancellationNoticePeriodHours = 48,
            EnableWaitlist = true
        };
        _context.ClubSettings.Add(club3Settings);

        // Seed data for each club
        await SeedClubDataAsync(club1Id, "riverside", ClubType.Tennis);
        await SeedClubDataAsync(club2Id, "downtown", ClubType.MultiSport);
        await SeedClubDataAsync(club3Id, "lakeside", ClubType.Swimming);
    }

    private async Task SeedClubDataAsync(Guid clubId, string clubSlug, ClubType clubType)
    {
        // Create club manager
        var managerEmail = $"manager@{clubSlug}.com";
        var manager = new ApplicationUser
        {
            UserName = managerEmail,
            Email = managerEmail,
            EmailConfirmed = true,
            FirstName = "Club",
            LastName = "Manager",
            ClubId = clubId,
            Role = UserRole.ClubManager,
            IsActive = true
        };

        var managerResult = await _userManager.CreateAsync(manager, "Manager123!");
        if (managerResult.Succeeded)
        {
            await _userManager.AddToRoleAsync(manager, "ClubManager");
        }

        // Create membership types
        var membershipTypes = CreateMembershipTypes(clubId);
        _context.MembershipTypes.AddRange(membershipTypes);

        // Create venues
        var venues = CreateVenues(clubId, clubType);
        _context.Venues.AddRange(venues);

        // Create members with memberships and payments
        var members = await CreateMembersAsync(clubId, clubSlug, membershipTypes);
        _context.Members.AddRange(members);

        // Create sessions
        var sessions = CreateSessions(clubId, venues, clubType);
        _context.Sessions.AddRange(sessions);

        // Create recurring schedules
        var schedules = CreateRecurringSchedules(clubId, venues, clubType);
        _context.RecurringSchedules.AddRange(schedules);

        // Create events
        var events = CreateEvents(clubId, venues, clubType);
        _context.Events.AddRange(events);

        // Create bookings
        CreateBookings(members, sessions);

        // Create payments
        var payments = CreatePayments(clubId, members, membershipTypes);
        _context.Payments.AddRange(payments);
    }

    private List<MembershipType> CreateMembershipTypes(Guid clubId)
    {
        return new List<MembershipType>
        {
            new MembershipType
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                Name = "Adult Annual",
                Description = "Full access annual membership for adults 18+",
                AnnualFee = 299.99m,
                MonthlyFee = 29.99m,
                IsActive = true,
                AllowOnlineSignup = true,
                MinAge = 18,
                IncludesBooking = true,
                IncludesEvents = true,
                SortOrder = 1
            },
            new MembershipType
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                Name = "Adult Monthly",
                Description = "Flexible monthly membership for adults",
                AnnualFee = 0m,
                MonthlyFee = 39.99m,
                IsActive = true,
                AllowOnlineSignup = true,
                MinAge = 18,
                IncludesBooking = true,
                IncludesEvents = true,
                SortOrder = 2
            },
            new MembershipType
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                Name = "Junior",
                Description = "Membership for juniors under 18",
                AnnualFee = 149.99m,
                MonthlyFee = 14.99m,
                IsActive = true,
                AllowOnlineSignup = true,
                MaxAge = 17,
                IncludesBooking = true,
                IncludesEvents = true,
                MaxSessionsPerWeek = 5,
                SortOrder = 3
            },
            new MembershipType
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                Name = "Family",
                Description = "Annual membership for families (2 adults + up to 3 children)",
                AnnualFee = 599.99m,
                MonthlyFee = 59.99m,
                IsActive = true,
                AllowOnlineSignup = true,
                MaxFamilyMembers = 5,
                IncludesBooking = true,
                IncludesEvents = true,
                SortOrder = 4
            },
            new MembershipType
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                Name = "Student",
                Description = "Discounted membership for full-time students",
                AnnualFee = 199.99m,
                MonthlyFee = 19.99m,
                IsActive = true,
                AllowOnlineSignup = true,
                MinAge = 16,
                MaxAge = 25,
                IncludesBooking = true,
                IncludesEvents = true,
                SortOrder = 5
            }
        };
    }

    private List<Venue> CreateVenues(Guid clubId, ClubType clubType)
    {
        var venues = new List<Venue>();

        switch (clubType)
        {
            case ClubType.Tennis:
                venues.AddRange(new[]
                {
                    new Venue { Id = Guid.NewGuid(), ClubId = clubId, Name = "Court 1", Description = "Indoor hard court", Capacity = 4, IsActive = true, Address = "Main Building", IsPrimary = true },
                    new Venue { Id = Guid.NewGuid(), ClubId = clubId, Name = "Court 2", Description = "Indoor hard court", Capacity = 4, IsActive = true, Address = "Main Building" },
                    new Venue { Id = Guid.NewGuid(), ClubId = clubId, Name = "Court 3", Description = "Outdoor clay court", Capacity = 4, IsActive = true, Address = "Outdoor Area" },
                    new Venue { Id = Guid.NewGuid(), ClubId = clubId, Name = "Court 4", Description = "Outdoor clay court", Capacity = 4, IsActive = true, Address = "Outdoor Area" },
                    new Venue { Id = Guid.NewGuid(), ClubId = clubId, Name = "Training Room", Description = "Fitness and conditioning", Capacity = 20, IsActive = true, Address = "Main Building" }
                });
                break;
            case ClubType.MultiSport:
                venues.AddRange(new[]
                {
                    new Venue { Id = Guid.NewGuid(), ClubId = clubId, Name = "Main Court", Description = "Full-size indoor court", Capacity = 30, IsActive = true, Address = "Sports Hall A", IsPrimary = true },
                    new Venue { Id = Guid.NewGuid(), ClubId = clubId, Name = "Practice Court", Description = "Half court for drills", Capacity = 15, IsActive = true, Address = "Sports Hall B" },
                    new Venue { Id = Guid.NewGuid(), ClubId = clubId, Name = "Gym", Description = "Strength and conditioning", Capacity = 25, IsActive = true, Address = "Building B" },
                    new Venue { Id = Guid.NewGuid(), ClubId = clubId, Name = "Video Room", Description = "Game analysis", Capacity = 20, IsActive = true, Address = "Building A" }
                });
                break;
            case ClubType.Swimming:
                venues.AddRange(new[]
                {
                    new Venue { Id = Guid.NewGuid(), ClubId = clubId, Name = "Olympic Pool", Description = "50m competition pool", Capacity = 100, IsActive = true, Address = "Aquatic Center", IsPrimary = true },
                    new Venue { Id = Guid.NewGuid(), ClubId = clubId, Name = "Training Pool", Description = "25m training pool", Capacity = 50, IsActive = true, Address = "Aquatic Center" },
                    new Venue { Id = Guid.NewGuid(), ClubId = clubId, Name = "Diving Pool", Description = "Diving and water polo", Capacity = 30, IsActive = true, Address = "Aquatic Center" },
                    new Venue { Id = Guid.NewGuid(), ClubId = clubId, Name = "Kids Pool", Description = "Learn to swim area", Capacity = 20, IsActive = true, Address = "Aquatic Center" },
                    new Venue { Id = Guid.NewGuid(), ClubId = clubId, Name = "Dry Training Area", Description = "Land-based conditioning", Capacity = 30, IsActive = true, Address = "Building B" }
                });
                break;
            default:
                venues.Add(new Venue { Id = Guid.NewGuid(), ClubId = clubId, Name = "Main Facility", Description = "Primary venue", Capacity = 50, IsActive = true, Address = "Main Building", IsPrimary = true });
                break;
        }

        return venues;
    }

    private async Task<List<Member>> CreateMembersAsync(Guid clubId, string clubSlug, List<MembershipType> membershipTypes)
    {
        var members = new List<Member>();
        var random = new Random(42);

        var firstNames = new[] { "James", "Emma", "Oliver", "Sophia", "William", "Isabella", "Benjamin", "Mia", "Lucas", "Charlotte", "Henry", "Amelia", "Alexander", "Harper", "Daniel", "Evelyn", "Michael", "Abigail", "Ethan", "Emily" };
        var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Anderson", "Taylor", "Thomas", "Moore", "Jackson", "Martin", "Lee", "Thompson", "White", "Harris" };

        for (int i = 0; i < 30; i++)
        {
            var firstName = firstNames[random.Next(firstNames.Length)];
            var lastName = lastNames[random.Next(lastNames.Length)];
            var email = $"{firstName.ToLower()}.{lastName.ToLower()}{i}@{clubSlug}.com";

            var memberId = Guid.NewGuid();
            var joinedDate = DateTime.UtcNow.AddMonths(-random.Next(1, 24));
            var member = new Member
            {
                Id = memberId,
                ClubId = clubId,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = $"+44 7{random.Next(100, 999)} {random.Next(100, 999)} {random.Next(1000, 9999)}",
                DateOfBirth = DateTime.UtcNow.AddYears(-random.Next(18, 65)).AddDays(-random.Next(0, 365)),
                Address = $"{random.Next(1, 100)} {lastNames[random.Next(lastNames.Length)]} Street",
                City = new[] { "London", "Manchester", "Birmingham", "Leeds", "Liverpool" }[random.Next(5)],
                PostCode = $"{(char)('A' + random.Next(26))}{(char)('A' + random.Next(26))}{random.Next(1, 20)} {random.Next(1, 9)}{(char)('A' + random.Next(26))}{(char)('A' + random.Next(26))}",
                JoinedDate = joinedDate,
                Status = i < 25 ? MemberStatus.Active : (i < 28 ? MemberStatus.Expired : MemberStatus.Pending),
                IsActive = i < 25,
                EmergencyContactName = $"{firstNames[random.Next(firstNames.Length)]} {lastName}",
                EmergencyContactPhone = $"+44 7{random.Next(100, 999)} {random.Next(100, 999)} {random.Next(1000, 9999)}",
                EmergencyContactRelation = new[] { "Spouse", "Parent", "Sibling", "Friend" }[random.Next(4)]
            };

            // Create user account for member
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = firstName,
                LastName = lastName,
                ClubId = clubId,
                MemberId = memberId,
                Role = UserRole.Member,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, "Member123!");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Member");
                member.UserId = user.Id;
            }

            members.Add(member);

            // Create membership for active members
            if (member.Status == MemberStatus.Active)
            {
                var membershipType = membershipTypes[random.Next(membershipTypes.Count)];
                var membership = new Membership
                {
                    Id = Guid.NewGuid(),
                    ClubId = clubId,
                    MemberId = memberId,
                    MembershipTypeId = membershipType.Id,
                    StartDate = joinedDate,
                    EndDate = joinedDate.AddYears(1),
                    PaymentType = MembershipPaymentType.Annual,
                    Status = MembershipStatus.Active,
                    AmountPaid = membershipType.AnnualFee,
                    AmountDue = 0,
                    AutoRenew = random.Next(2) == 0,
                    LastPaymentDate = joinedDate
                };
                _context.Memberships.Add(membership);
            }

            // Add family members for some members
            if (i % 5 == 0 && member.Status == MemberStatus.Active)
            {
                var familyCount = random.Next(1, 4);
                for (int j = 0; j < familyCount; j++)
                {
                    var familyMember = new FamilyMember
                    {
                        Id = Guid.NewGuid(),
                        ClubId = clubId,
                        PrimaryMemberId = memberId,
                        FirstName = firstNames[random.Next(firstNames.Length)],
                        LastName = lastName,
                        DateOfBirth = DateTime.UtcNow.AddYears(-random.Next(5, 17)),
                        Relation = j == 0 ? FamilyMemberRelation.Spouse : FamilyMemberRelation.Child,
                        IsActive = true
                    };
                    _context.FamilyMembers.Add(familyMember);
                }
            }
        }

        return members;
    }

    private List<Session> CreateSessions(Guid clubId, List<Venue> venues, ClubType clubType)
    {
        var sessions = new List<Session>();
        var random = new Random(42);

        var sessionTitles = clubType switch
        {
            ClubType.Tennis => new[] { "Singles Practice", "Doubles Clinic", "Junior Coaching", "Adult Beginner", "Tournament Prep", "Cardio Tennis" },
            ClubType.MultiSport => new[] { "Team Practice", "Skills Training", "Youth Program", "Shooting Clinic", "Game Night", "Defensive Drills" },
            ClubType.Swimming => new[] { "Lap Swimming", "Learn to Swim", "Masters Training", "Water Aerobics", "Competitive Squad", "Open Swim" },
            _ => new[] { "General Session", "Training", "Practice" }
        };

        var categories = new[] { SessionCategory.AllAges, SessionCategory.Juniors, SessionCategory.Seniors, SessionCategory.Beginners, SessionCategory.Advanced };

        // Create sessions for the next 30 days
        for (int day = 0; day < 30; day++)
        {
            var date = DateTime.UtcNow.Date.AddDays(day);
            var sessionsPerDay = random.Next(3, 8);

            for (int s = 0; s < sessionsPerDay; s++)
            {
                var venue = venues[random.Next(venues.Count)];
                var hour = 7 + (s * 2);
                if (hour > 20) continue;

                var session = new Session
                {
                    Id = Guid.NewGuid(),
                    ClubId = clubId,
                    VenueId = venue.Id,
                    Title = sessionTitles[random.Next(sessionTitles.Length)],
                    Description = $"Regular training session",
                    StartTime = date.AddHours(hour),
                    EndTime = date.AddHours(hour + 1.5),
                    Capacity = venue.Capacity ?? random.Next(8, 20),
                    CurrentBookings = 0,
                    SessionFee = random.Next(2) == 0 ? null : random.Next(5, 20),
                    IsCancelled = false,
                    Category = categories[random.Next(categories.Length)]
                };
                sessions.Add(session);
            }
        }

        // Create some past sessions
        for (int day = 1; day <= 14; day++)
        {
            var date = DateTime.UtcNow.Date.AddDays(-day);
            var sessionsPerDay = random.Next(2, 5);

            for (int s = 0; s < sessionsPerDay; s++)
            {
                var venue = venues[random.Next(venues.Count)];
                var hour = 9 + (s * 3);

                var session = new Session
                {
                    Id = Guid.NewGuid(),
                    ClubId = clubId,
                    VenueId = venue.Id,
                    Title = sessionTitles[random.Next(sessionTitles.Length)],
                    Description = $"Completed training session",
                    StartTime = date.AddHours(hour),
                    EndTime = date.AddHours(hour + 1.5),
                    Capacity = venue.Capacity ?? random.Next(10, 20),
                    CurrentBookings = random.Next(5, 15),
                    SessionFee = random.Next(2) == 0 ? null : random.Next(5, 15),
                    IsCancelled = false,
                    Category = categories[random.Next(categories.Length)]
                };
                sessions.Add(session);
            }
        }

        return sessions;
    }

    private List<RecurringSchedule> CreateRecurringSchedules(Guid clubId, List<Venue> venues, ClubType clubType)
    {
        var schedules = new List<RecurringSchedule>();
        var random = new Random(42);

        var scheduleTitles = clubType switch
        {
            ClubType.Tennis => new[] { "Morning Tennis", "Evening League", "Junior Academy", "Ladies Social" },
            ClubType.MultiSport => new[] { "Morning Hoops", "After-School Program", "Adult League", "Weekend Warriors" },
            ClubType.Swimming => new[] { "Early Bird Swim", "Lunch Swim", "Evening Squad", "Masters Practice" },
            _ => new[] { "Regular Session", "Weekly Practice" }
        };

        var categories = new[] { SessionCategory.AllAges, SessionCategory.Juniors, SessionCategory.Seniors, SessionCategory.Mixed };

        foreach (var title in scheduleTitles)
        {
            var venue = venues[random.Next(venues.Count)];
            var schedule = new RecurringSchedule
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                VenueId = venue.Id,
                Title = title,
                Description = $"Weekly {title.ToLower()} sessions",
                DayOfWeek = (DayOfWeek)random.Next(0, 7),
                StartTime = TimeSpan.FromHours(7 + random.Next(0, 12)),
                EndTime = TimeSpan.FromHours(8 + random.Next(1, 13)),
                Capacity = venue.Capacity ?? random.Next(10, 25),
                SessionFee = random.Next(3) == 0 ? null : random.Next(10, 30),
                IsActive = true,
                ScheduleStartDate = DateTime.UtcNow.AddMonths(-6),
                ScheduleEndDate = DateTime.UtcNow.AddMonths(6),
                Category = categories[random.Next(categories.Length)]
            };
            schedules.Add(schedule);
        }

        return schedules;
    }

    private List<Event> CreateEvents(Guid clubId, List<Venue> venues, ClubType clubType)
    {
        var events = new List<Event>();
        var random = new Random(42);

        var eventData = clubType switch
        {
            ClubType.Tennis => new[] { ("Summer Championship", EventType.Tournament), ("Club Open Day", EventType.Social), ("Junior Tournament", EventType.Competition), ("Mixed Doubles Night", EventType.Social), ("Charity Tennis Marathon", EventType.Fundraiser) },
            ClubType.MultiSport => new[] { ("3v3 Tournament", EventType.Tournament), ("Skills Challenge", EventType.Competition), ("Family Fun Day", EventType.Social), ("Alumni Game", EventType.Social), ("Youth Championship", EventType.Tournament) },
            ClubType.Swimming => new[] { ("Gala Competition", EventType.Competition), ("Open Water Day", EventType.Social), ("Learn to Swim Week", EventType.Training), ("Masters Meet", EventType.Competition), ("Swim-a-thon Fundraiser", EventType.Fundraiser) },
            _ => new[] { ("Club Championship", EventType.Tournament), ("Open Day", EventType.Social), ("Annual Gala", EventType.Social) }
        };

        foreach (var (title, type) in eventData)
        {
            var venue = venues[random.Next(venues.Count)];
            var daysFromNow = random.Next(-30, 90);
            var startDate = DateTime.UtcNow.Date.AddDays(daysFromNow).AddHours(10);

            var evt = new Event
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                VenueId = venue.Id,
                Title = title,
                Description = $"Join us for the {title}! A great opportunity to participate and meet other members.",
                Type = type,
                StartDateTime = startDate,
                EndDateTime = startDate.AddHours(random.Next(3, 8)),
                Location = venue.Name,
                Capacity = random.Next(50, 200),
                CurrentAttendees = daysFromNow < 0 ? random.Next(30, 100) : random.Next(0, 50),
                IsTicketed = random.Next(3) != 0,
                TicketPrice = random.Next(3) == 0 ? null : random.Next(10, 50),
                MemberTicketPrice = random.Next(3) == 0 ? null : random.Next(5, 40),
                RequiresRSVP = true,
                RSVPDeadline = startDate.AddDays(-7),
                IsCancelled = false,
                IsPublished = daysFromNow > -30,
                ImageUrl = null
            };

            events.Add(evt);

            // Create some tickets for past events
            if (daysFromNow < 0)
            {
                for (int t = 0; t < random.Next(10, 30); t++)
                {
                    var ticket = new EventTicket
                    {
                        Id = Guid.NewGuid(),
                        ClubId = clubId,
                        EventId = evt.Id,
                        MemberId = Guid.NewGuid(),
                        TicketCode = $"TKT-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                        PurchasedAt = startDate.AddDays(-random.Next(1, 30)),
                        Quantity = random.Next(1, 4),
                        UnitPrice = evt.TicketPrice ?? 0,
                        TotalAmount = (evt.TicketPrice ?? 0) * random.Next(1, 4),
                        IsUsed = true,
                        UsedAt = startDate.AddMinutes(random.Next(0, 60))
                    };
                    _context.EventTickets.Add(ticket);
                }
            }
        }

        return events;
    }

    private void CreateBookings(List<Member> members, List<Session> sessions)
    {
        var random = new Random(42);
        var activeMembers = members.Where(m => m.Status == MemberStatus.Active).ToList();
        var futureSessions = sessions.Where(s => s.StartTime > DateTime.UtcNow && !s.IsCancelled).ToList();

        foreach (var session in futureSessions.Take(50))
        {
            var bookingCount = random.Next(1, Math.Min(session.Capacity, 8));
            var selectedMembers = activeMembers.OrderBy(_ => random.Next()).Take(bookingCount).ToList();

            foreach (var member in selectedMembers)
            {
                var booking = new SessionBooking
                {
                    Id = Guid.NewGuid(),
                    ClubId = session.ClubId,
                    SessionId = session.Id,
                    MemberId = member.Id,
                    BookedAt = DateTime.UtcNow.AddDays(-random.Next(1, 7)),
                    Status = BookingStatus.Confirmed
                };
                _context.SessionBookings.Add(booking);
                session.CurrentBookings++;
            }

            // Add some to waitlist if session is full
            if (session.CurrentBookings >= session.Capacity && random.Next(3) == 0)
            {
                var waitlistMember = activeMembers.FirstOrDefault(m => !selectedMembers.Contains(m));
                if (waitlistMember != null)
                {
                    var waitlist = new Waitlist
                    {
                        Id = Guid.NewGuid(),
                        ClubId = session.ClubId,
                        SessionId = session.Id,
                        MemberId = waitlistMember.Id,
                        Position = 1,
                        JoinedAt = DateTime.UtcNow.AddDays(-random.Next(1, 5)),
                        NotificationSent = false
                    };
                    _context.Waitlists.Add(waitlist);
                }
            }
        }
    }

    private List<Payment> CreatePayments(Guid clubId, List<Member> members, List<MembershipType> membershipTypes)
    {
        var payments = new List<Payment>();
        var random = new Random(42);

        foreach (var member in members.Where(m => m.Status == MemberStatus.Active))
        {
            // Membership payment
            var membershipType = membershipTypes[random.Next(membershipTypes.Count)];
            var membershipPayment = new Payment
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                MemberId = member.Id,
                Amount = membershipType.AnnualFee,
                Currency = "GBP",
                Status = PaymentStatus.Completed,
                Method = random.Next(2) == 0 ? PaymentMethod.Stripe : PaymentMethod.BankTransfer,
                Type = PaymentType.Membership,
                Description = $"{membershipType.Name} membership payment",
                PaymentDate = member.JoinedDate.AddDays(random.Next(0, 7)),
                ProcessedDate = member.JoinedDate.AddDays(random.Next(0, 7)),
                StripePaymentIntentId = $"pi_{Guid.NewGuid().ToString()[..24]}",
                ReceiptNumber = $"RCP-{DateTime.UtcNow.Year}-{random.Next(10000, 99999)}"
            };
            payments.Add(membershipPayment);

            // Some additional payments
            if (random.Next(3) == 0)
            {
                var sessionPayment = new Payment
                {
                    Id = Guid.NewGuid(),
                    ClubId = clubId,
                    MemberId = member.Id,
                    Amount = random.Next(10, 50),
                    Currency = "GBP",
                    Status = PaymentStatus.Completed,
                    Method = PaymentMethod.Stripe,
                    Type = PaymentType.SessionFee,
                    Description = "Session booking payment",
                    PaymentDate = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                    ProcessedDate = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                    StripePaymentIntentId = $"pi_{Guid.NewGuid().ToString()[..24]}",
                    ReceiptNumber = $"RCP-{DateTime.UtcNow.Year}-{random.Next(10000, 99999)}"
                };
                payments.Add(sessionPayment);
            }

            if (random.Next(4) == 0)
            {
                var eventPayment = new Payment
                {
                    Id = Guid.NewGuid(),
                    ClubId = clubId,
                    MemberId = member.Id,
                    Amount = random.Next(20, 100),
                    Currency = "GBP",
                    Status = PaymentStatus.Completed,
                    Method = PaymentMethod.Stripe,
                    Type = PaymentType.EventTicket,
                    Description = "Event ticket purchase",
                    PaymentDate = DateTime.UtcNow.AddDays(-random.Next(1, 60)),
                    ProcessedDate = DateTime.UtcNow.AddDays(-random.Next(1, 60)),
                    StripePaymentIntentId = $"pi_{Guid.NewGuid().ToString()[..24]}",
                    ReceiptNumber = $"RCP-{DateTime.UtcNow.Year}-{random.Next(10000, 99999)}"
                };
                payments.Add(eventPayment);
            }
        }

        // Add some pending/failed payments for realism
        var pendingMember = members.FirstOrDefault(m => m.Status == MemberStatus.Active);
        if (pendingMember != null)
        {
            payments.Add(new Payment
            {
                Id = Guid.NewGuid(),
                ClubId = clubId,
                MemberId = pendingMember.Id,
                Amount = 39.99m,
                Currency = "GBP",
                Status = PaymentStatus.Pending,
                Method = PaymentMethod.Stripe,
                Type = PaymentType.Membership,
                Description = "Membership renewal - pending",
                PaymentDate = DateTime.UtcNow.AddDays(-2)
            });
        }

        return payments;
    }
}
