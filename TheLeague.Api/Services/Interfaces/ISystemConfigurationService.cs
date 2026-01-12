using TheLeague.Api.DTOs;
using TheLeague.Core.Entities;

namespace TheLeague.Api.Services.Interfaces;

/// <summary>
/// Service for managing system configuration
/// </summary>
public interface ISystemConfigurationService
{
    /// <summary>
    /// Gets the current system configuration
    /// </summary>
    Task<SystemConfigurationDto> GetConfigurationAsync();

    /// <summary>
    /// Updates the system configuration
    /// </summary>
    Task<SystemConfigurationDto> UpdateConfigurationAsync(UpdateSystemConfigurationRequest request, string updatedBy, string? ipAddress);

    /// <summary>
    /// Gets the configuration audit log
    /// </summary>
    Task<List<ConfigurationAuditLogDto>> GetAuditLogAsync(int? limit = null);

    /// <summary>
    /// Tests the payment provider connection
    /// </summary>
    Task<ProviderTestResult> TestPaymentProviderAsync();

    /// <summary>
    /// Tests the email provider connection
    /// </summary>
    Task<ProviderTestResult> TestEmailProviderAsync();

    /// <summary>
    /// Sends a test email
    /// </summary>
    Task<ProviderTestResult> SendTestEmailAsync(string toEmail);
}
