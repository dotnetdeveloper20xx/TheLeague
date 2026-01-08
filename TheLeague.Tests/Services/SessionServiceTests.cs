using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Entities;
using TheLeague.Core.Enums;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Tests.Services;

public class SessionServiceTests
{
    private ApplicationDbContext CreateDbContext(Guid tenantId)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var mockTenantService = new Mock<ITenantService>();
        mockTenantService.Setup(x => x.CurrentTenantId).Returns(tenantId);

        return new ApplicationDbContext(options, mockTenantService.Object);
    }

    [Fact]
    public async Task GetSessionsAsync_ShouldReturnOnlyTenantSessions()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        var otherClubId = Guid.NewGuid();

        using var context = CreateDbContext(clubId);

        context.Clubs.Add(new Club { Id = clubId, Name = "Test Club", Slug = "test" });

        context.Sessions.Add(new Session
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Title = "Test Session 1",
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            Capacity = 10,
            Category = SessionCategory.AllAges
        });
        context.Sessions.Add(new Session
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Title = "Test Session 2",
            StartTime = DateTime.UtcNow.AddDays(2),
            EndTime = DateTime.UtcNow.AddDays(2).AddHours(1),
            Capacity = 10,
            Category = SessionCategory.AllAges
        });
        context.Sessions.Add(new Session
        {
            Id = Guid.NewGuid(),
            ClubId = otherClubId,
            Title = "Other Club Session",
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            Capacity = 10,
            Category = SessionCategory.AllAges
        });

        await context.SaveChangesAsync();

        var service = new SessionService(context);
        var filter = new SessionFilterRequest();

        // Act
        var result = await service.GetSessionsAsync(clubId, filter);

        // Assert
        result.Items.Should().HaveCount(2);
        result.Items.Should().AllSatisfy(s => s.Title.Should().NotBe("Other Club Session"));
    }

    [Fact]
    public async Task CreateSessionAsync_ShouldCreateSession()
    {
        // Arrange
        var clubId = Guid.NewGuid();

        using var context = CreateDbContext(clubId);
        context.Clubs.Add(new Club { Id = clubId, Name = "Test Club", Slug = "test" });
        await context.SaveChangesAsync();

        var service = new SessionService(context);
        var request = new SessionCreateRequest(
            Title: "New Session",
            Description: "A new training session",
            Category: SessionCategory.Juniors,
            StartTime: DateTime.UtcNow.AddDays(7),
            EndTime: DateTime.UtcNow.AddDays(7).AddHours(1),
            Capacity: 15,
            VenueId: null,
            SessionFee: 10.00m
        );

        // Act
        var result = await service.CreateSessionAsync(clubId, request);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("New Session");
        result.Capacity.Should().Be(15);
        result.Category.Should().Be(SessionCategory.Juniors);
    }

    [Fact]
    public async Task BookSessionAsync_ShouldCreateBooking()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        using var context = CreateDbContext(clubId);
        context.Clubs.Add(new Club { Id = clubId, Name = "Test Club", Slug = "test" });
        context.Sessions.Add(new Session
        {
            Id = sessionId,
            ClubId = clubId,
            Title = "Test Session",
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            Capacity = 10,
            CurrentBookings = 0,
            Category = SessionCategory.AllAges
        });
        context.Members.Add(new Member
        {
            Id = memberId,
            ClubId = clubId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@test.com",
            Status = MemberStatus.Active
        });
        await context.SaveChangesAsync();

        var service = new SessionService(context);
        var request = new BookSessionRequest(null, null);

        // Act
        var result = await service.BookSessionAsync(clubId, sessionId, memberId, request);

        // Assert
        result.Should().NotBeNull();
        result!.MemberId.Should().Be(memberId);
        result.Status.Should().Be(BookingStatus.Confirmed);

        var session = await context.Sessions.FindAsync(sessionId);
        session!.CurrentBookings.Should().Be(1);
    }

    [Fact]
    public async Task BookSessionAsync_ShouldThrowException_WhenSessionFull()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        using var context = CreateDbContext(clubId);
        context.Clubs.Add(new Club { Id = clubId, Name = "Test Club", Slug = "test" });
        context.Sessions.Add(new Session
        {
            Id = sessionId,
            ClubId = clubId,
            Title = "Full Session",
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            Capacity = 5,
            CurrentBookings = 5,
            Category = SessionCategory.AllAges
        });
        context.Members.Add(new Member
        {
            Id = memberId,
            ClubId = clubId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@test.com",
            Status = MemberStatus.Active
        });
        await context.SaveChangesAsync();

        var service = new SessionService(context);
        var request = new BookSessionRequest(null, null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.BookSessionAsync(clubId, sessionId, memberId, request));
    }

    [Fact]
    public async Task GetSessionsAsync_WithDateFilter_ShouldFilterCorrectly()
    {
        // Arrange
        var clubId = Guid.NewGuid();

        using var context = CreateDbContext(clubId);
        context.Clubs.Add(new Club { Id = clubId, Name = "Test Club", Slug = "test" });

        var tomorrow = DateTime.UtcNow.Date.AddDays(1);
        var nextWeek = DateTime.UtcNow.Date.AddDays(7);

        context.Sessions.Add(new Session
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Title = "Tomorrow Session",
            StartTime = tomorrow.AddHours(10),
            EndTime = tomorrow.AddHours(11),
            Capacity = 10,
            Category = SessionCategory.AllAges
        });
        context.Sessions.Add(new Session
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Title = "Next Week Session",
            StartTime = nextWeek.AddHours(10),
            EndTime = nextWeek.AddHours(11),
            Capacity = 10,
            Category = SessionCategory.AllAges
        });

        await context.SaveChangesAsync();

        var service = new SessionService(context);
        var filter = new SessionFilterRequest(DateFrom: tomorrow, DateTo: tomorrow.AddDays(1));

        // Act
        var result = await service.GetSessionsAsync(clubId, filter);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items.First().Title.Should().Be("Tomorrow Session");
    }
}
