namespace TheLeague.Core.Enums;

public enum ClubType
{
    Cricket,
    Football,
    Rugby,
    Tennis,
    Golf,
    Hockey,
    Swimming,
    Athletics,
    MultiSport,
    CommunityGroup,
    YouthOrganization,
    Other
}

public enum PaymentProvider
{
    Stripe,
    PayPal
}

public enum MemberStatus
{
    Pending,
    Active,
    Expired,
    Suspended,
    Cancelled
}

public enum FamilyMemberRelation
{
    Spouse,
    Child,
    Sibling,
    Parent,
    Other
}

public enum MembershipPaymentType
{
    Annual,
    Monthly,
    PayAsYouGo
}

public enum MembershipStatus
{
    Active,
    PendingPayment,
    Expired,
    Cancelled
}

public enum PaymentStatus
{
    Pending,
    Processing,
    Completed,
    Failed,
    Refunded,
    Cancelled
}

public enum PaymentMethod
{
    Stripe,
    PayPal,
    BankTransfer,
    Cash,
    Cheque,
    Other
}

public enum PaymentType
{
    Membership,
    EventTicket,
    SessionFee,
    Other
}

public enum SessionCategory
{
    AllAges,
    Juniors,
    Seniors,
    U7,
    U9,
    U11,
    U13,
    U15,
    U17,
    U19,
    Ladies,
    Mens,
    Mixed,
    Beginners,
    Advanced,
    Social,
    Competition
}

public enum BookingStatus
{
    Confirmed,
    Cancelled,
    NoShow,
    Attended
}

public enum EventType
{
    Social,
    Tournament,
    AGM,
    Training,
    Fundraiser,
    Competition,
    Meeting,
    Presentation,
    Other
}

public enum RSVPResponse
{
    Attending,
    NotAttending,
    Maybe
}

public enum DocumentType
{
    ProfilePhoto,
    MedicalForm,
    ConsentForm,
    DBSCertificate,
    CoachingQualification,
    Other
}

public enum EmailType
{
    Welcome,
    PasswordReset,
    PaymentReminder,
    PaymentReceipt,
    BookingConfirmation,
    BookingCancellation,
    EventReminder,
    MembershipRenewal,
    BulkCommunication,
    WaitlistNotification
}

public enum EmailStatus
{
    Pending,
    Sent,
    Failed,
    Bounced
}

public enum CampaignStatus
{
    Draft,
    Scheduled,
    Sending,
    Completed,
    Failed
}

public enum UserRole
{
    SuperAdmin,
    ClubManager,
    Member
}
