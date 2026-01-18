using Microsoft.AspNetCore.Mvc;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Controllers;

/// <summary>
/// Base controller that provides common functionality for all API controllers.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected readonly ITenantService TenantService;

    protected BaseApiController(ITenantService tenantService)
    {
        TenantService = tenantService;
    }

    /// <summary>
    /// Gets the current club ID from the JWT claim or tenant service.
    /// </summary>
    /// <returns>The club ID, or Guid.Empty if not found.</returns>
    protected Guid GetClubId()
    {
        var clubIdClaim = User.FindFirst("clubId")?.Value;
        if (Guid.TryParse(clubIdClaim, out var clubId))
            return clubId;
        return TenantService.CurrentTenantId ?? Guid.Empty;
    }

    /// <summary>
    /// Gets the current user's ID from the JWT claim.
    /// </summary>
    /// <returns>The user ID, or null if not found.</returns>
    protected string? GetUserId()
    {
        return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    }

    /// <summary>
    /// Gets the current member ID from the JWT claim.
    /// </summary>
    /// <returns>The member ID, or null if not found.</returns>
    protected Guid? GetMemberId()
    {
        var memberIdClaim = User.FindFirst("memberId")?.Value;
        if (Guid.TryParse(memberIdClaim, out var memberId))
            return memberId;
        return null;
    }
}
