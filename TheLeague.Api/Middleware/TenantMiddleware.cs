using System.Security.Claims;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
    {
        Guid? tenantId = null;

        // 1. Try to get from JWT claim
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var clubIdClaim = context.User.FindFirst("clubId")?.Value;
            if (!string.IsNullOrEmpty(clubIdClaim) && Guid.TryParse(clubIdClaim, out var claimTenantId))
            {
                tenantId = claimTenantId;
            }
        }

        // 2. Try to get from header
        if (!tenantId.HasValue && context.Request.Headers.TryGetValue("X-Tenant-Id", out var headerTenantId))
        {
            if (Guid.TryParse(headerTenantId, out var parsedTenantId))
            {
                tenantId = parsedTenantId;
            }
        }

        // 3. Try to get from subdomain
        if (!tenantId.HasValue)
        {
            var host = context.Request.Host.Host;
            var subdomain = host.Split('.').FirstOrDefault();
            if (!string.IsNullOrEmpty(subdomain) && subdomain != "www" && subdomain != "localhost")
            {
                // Would normally look up tenant by slug here
                // For demo purposes, we'll rely on header or claim
            }
        }

        tenantService.SetCurrentTenant(tenantId);
        await _next(context);
    }
}

public static class TenantMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TenantMiddleware>();
    }
}
