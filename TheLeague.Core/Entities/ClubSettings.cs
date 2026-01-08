namespace TheLeague.Core.Entities;

public class ClubSettings
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }

    // Registration Settings
    public bool AllowOnlineRegistration { get; set; } = true;
    public bool RequireEmergencyContact { get; set; } = true;
    public bool RequireMedicalInfo { get; set; } = false;
    public bool AllowFamilyAccounts { get; set; } = true;

    // Payment Settings
    public bool AllowOnlinePayments { get; set; } = true;
    public bool AllowManualPayments { get; set; } = true;
    public bool AutoSendPaymentReminders { get; set; } = true;
    public int PaymentReminderDaysBefore { get; set; } = 14;
    public int PaymentReminderFrequency { get; set; } = 7;

    // Booking Settings
    public bool AllowMemberBookings { get; set; } = true;
    public int MaxAdvanceBookingDays { get; set; } = 30;
    public int CancellationNoticePeriodHours { get; set; } = 24;
    public bool EnableWaitlist { get; set; } = true;

    // Communication Settings
    public bool SendWelcomeEmail { get; set; } = true;
    public bool SendBookingConfirmations { get; set; } = true;
    public bool SendPaymentReceipts { get; set; } = true;

    public Club Club { get; set; } = null!;
}
