using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Enums;

namespace TheLeague.Api.Controllers;

/// <summary>
/// API controller for system configuration management.
/// All endpoints require SuperAdmin role.
/// </summary>
[ApiController]
[Route("api/admin/system-config")]
[Authorize(Roles = nameof(UserRole.SuperAdmin))]
public class SystemConfigurationController : ControllerBase
{
    private readonly ISystemConfigurationService _configService;
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly ILogger<SystemConfigurationController> _logger;

    public SystemConfigurationController(
        ISystemConfigurationService configService,
        IHostApplicationLifetime appLifetime,
        ILogger<SystemConfigurationController> logger)
    {
        _configService = configService;
        _appLifetime = appLifetime;
        _logger = logger;
    }

    /// <summary>
    /// Gets the current system configuration
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<SystemConfigurationDto>> GetConfiguration()
    {
        var config = await _configService.GetConfigurationAsync();
        return Ok(config);
    }

    /// <summary>
    /// Updates the system configuration
    /// </summary>
    [HttpPut]
    public async Task<ActionResult<SystemConfigurationDto>> UpdateConfiguration(
        [FromBody] UpdateSystemConfigurationRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Unknown";
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

        var config = await _configService.UpdateConfigurationAsync(request, userId, ipAddress);

        _logger.LogInformation(
            "System configuration updated by {User} from {IpAddress}",
            userId, ipAddress);

        return Ok(config);
    }

    /// <summary>
    /// Gets the configuration audit log
    /// </summary>
    [HttpGet("audit")]
    public async Task<ActionResult<List<ConfigurationAuditLogDto>>> GetAuditLog([FromQuery] int? limit = 50)
    {
        var logs = await _configService.GetAuditLogAsync(limit);
        return Ok(logs);
    }

    /// <summary>
    /// Tests the payment provider connection
    /// </summary>
    [HttpPost("test-payment")]
    public async Task<ActionResult<ProviderTestResult>> TestPaymentProvider()
    {
        var result = await _configService.TestPaymentProviderAsync();
        return Ok(result);
    }

    /// <summary>
    /// Tests the email provider connection
    /// </summary>
    [HttpPost("test-email")]
    public async Task<ActionResult<ProviderTestResult>> TestEmailProvider()
    {
        var result = await _configService.TestEmailProviderAsync();
        return Ok(result);
    }

    /// <summary>
    /// Sends a test email
    /// </summary>
    [HttpPost("send-test-email")]
    public async Task<ActionResult<ProviderTestResult>> SendTestEmail([FromBody] SendTestEmailRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ToEmail))
        {
            return BadRequest(new { Message = "Email address is required" });
        }

        var result = await _configService.SendTestEmailAsync(request.ToEmail);
        return Ok(result);
    }

    /// <summary>
    /// Triggers an application restart (for applying provider changes)
    /// </summary>
    [HttpPost("restart")]
    public IActionResult Restart()
    {
        var userId = User.FindFirstValue(ClaimTypes.Email) ?? "Unknown";
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

        _logger.LogWarning(
            "Application restart requested by {User} from {IpAddress}",
            userId, ipAddress);

        // Schedule the shutdown after a short delay to allow the response to be sent
        Task.Run(async () =>
        {
            await Task.Delay(1000);
            _appLifetime.StopApplication();
        });

        return Ok(new
        {
            Message = "Application restart initiated. The application will restart shortly.",
            RequestedBy = userId,
            RequestedAt = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Health check endpoint for restart polling
    /// </summary>
    [HttpGet("health")]
    [AllowAnonymous]
    public IActionResult Health()
    {
        return Ok(new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow
        });
    }
}
