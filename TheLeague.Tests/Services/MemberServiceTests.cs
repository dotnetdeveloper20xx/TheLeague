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

public class MemberServiceTests
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
    public async Task GetMembersAsync_ShouldReturnOnlyTenantMembers()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        var otherClubId = Guid.NewGuid();

        using var context = CreateDbContext(clubId);

        context.Clubs.Add(new Club { Id = clubId, Name = "Test Club", Slug = "test" });

        context.Members.Add(new Member
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@test.com",
            Status = MemberStatus.Active
        });
        context.Members.Add(new Member
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane@test.com",
            Status = MemberStatus.Active
        });
        context.Members.Add(new Member
        {
            Id = Guid.NewGuid(),
            ClubId = otherClubId,
            FirstName = "Other",
            LastName = "Member",
            Email = "other@test.com",
            Status = MemberStatus.Active
        });

        await context.SaveChangesAsync();

        var service = new MemberService(context);
        var filter = new MemberFilterRequest(null, null, null, null, null, null);

        // Act
        var result = await service.GetMembersAsync(clubId, filter);

        // Assert
        result.Items.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetMemberByIdAsync_ShouldReturnMember_WhenExists()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        using var context = CreateDbContext(clubId);

        context.Clubs.Add(new Club { Id = clubId, Name = "Test Club", Slug = "test" });
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

        var service = new MemberService(context);

        // Act
        var result = await service.GetMemberByIdAsync(clubId, memberId);

        // Assert
        result.Should().NotBeNull();
        result!.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
    }

    [Fact]
    public async Task GetMemberByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var clubId = Guid.NewGuid();

        using var context = CreateDbContext(clubId);
        context.Clubs.Add(new Club { Id = clubId, Name = "Test Club", Slug = "test" });
        await context.SaveChangesAsync();

        var service = new MemberService(context);

        // Act
        var result = await service.GetMemberByIdAsync(clubId, Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteMemberAsync_ShouldSoftDeleteMember()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        using var context = CreateDbContext(clubId);
        context.Clubs.Add(new Club { Id = clubId, Name = "Test Club", Slug = "test" });
        context.Members.Add(new Member
        {
            Id = memberId,
            ClubId = clubId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@test.com",
            Status = MemberStatus.Active,
            IsActive = true
        });
        await context.SaveChangesAsync();

        var service = new MemberService(context);

        // Act
        var result = await service.DeleteMemberAsync(clubId, memberId);

        // Assert
        result.Should().BeTrue();
        var deletedMember = await context.Members.FindAsync(memberId);
        deletedMember.Should().NotBeNull();
        deletedMember!.IsActive.Should().BeFalse();
        deletedMember.Status.Should().Be(MemberStatus.Cancelled);
    }

    [Fact]
    public async Task GetMembersAsync_WithStatusFilter_ShouldFilterCorrectly()
    {
        // Arrange
        var clubId = Guid.NewGuid();

        using var context = CreateDbContext(clubId);
        context.Clubs.Add(new Club { Id = clubId, Name = "Test Club", Slug = "test" });

        context.Members.Add(new Member
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            FirstName = "Active",
            LastName = "Member",
            Email = "active@test.com",
            Status = MemberStatus.Active
        });
        context.Members.Add(new Member
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            FirstName = "Pending",
            LastName = "Member",
            Email = "pending@test.com",
            Status = MemberStatus.Pending
        });

        await context.SaveChangesAsync();

        var service = new MemberService(context);
        var filter = new MemberFilterRequest(null, MemberStatus.Active, null, null, null, null);

        // Act
        var result = await service.GetMembersAsync(clubId, filter);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items.First().Status.Should().Be(MemberStatus.Active);
    }

    [Fact]
    public async Task GetMembersAsync_WithSearchTerm_ShouldSearchCorrectly()
    {
        // Arrange
        var clubId = Guid.NewGuid();

        using var context = CreateDbContext(clubId);
        context.Clubs.Add(new Club { Id = clubId, Name = "Test Club", Slug = "test" });

        context.Members.Add(new Member
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            FirstName = "John",
            LastName = "Smith",
            Email = "john.smith@test.com",
            Status = MemberStatus.Active
        });
        context.Members.Add(new Member
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@test.com",
            Status = MemberStatus.Active
        });

        await context.SaveChangesAsync();

        var service = new MemberService(context);
        var filter = new MemberFilterRequest("Smith", null, null, null, null, null);

        // Act
        var result = await service.GetMembersAsync(clubId, filter);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items.First().FullName.Should().Contain("Smith");
    }
}
