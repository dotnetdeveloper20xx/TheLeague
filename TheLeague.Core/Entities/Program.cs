using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents a program offered by the club (courses, camps, classes, etc.).
/// </summary>
public class Program
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid? VenueId { get; set; }
    public Guid? FacilityId { get; set; }

    // Basic Info
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public ProgramType Type { get; set; }
    public ProgramStatus Status { get; set; } = ProgramStatus.Draft;

    // Classification
    public string? Category { get; set; }
    public string? SubCategory { get; set; }
    public string? Sport { get; set; }
    public SkillLevel SkillLevel { get; set; } = SkillLevel.AllLevels;
    public AgeGroup AgeGroup { get; set; } = AgeGroup.AllAges;
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public Gender? TargetGender { get; set; }

    // Schedule
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? DurationWeeks { get; set; }
    public int? TotalSessions { get; set; }
    public int? SessionDurationMinutes { get; set; }
    public string? Schedule { get; set; } // JSON: days and times

    // Registration
    public DateTime? RegistrationOpenDate { get; set; }
    public DateTime? RegistrationCloseDate { get; set; }
    public DateTime? EarlyBirdDeadline { get; set; }
    public bool RequiresApplication { get; set; }
    public string? ApplicationForm { get; set; } // JSON form definition

    // Capacity
    public int? MinParticipants { get; set; }
    public int? MaxParticipants { get; set; }
    public int CurrentEnrollment { get; set; }
    public int WaitlistCount { get; set; }
    public bool AllowWaitlist { get; set; } = true;
    public int? WaitlistLimit { get; set; }

    // Pricing
    public decimal? Price { get; set; }
    public decimal? EarlyBirdPrice { get; set; }
    public decimal? MemberPrice { get; set; }
    public decimal? NonMemberPrice { get; set; }
    public decimal? DepositAmount { get; set; }
    public bool DepositRequired { get; set; }
    public string Currency { get; set; } = "GBP";
    public bool AllowInstallments { get; set; }
    public int? NumberOfInstallments { get; set; }
    public Guid? FeeId { get; set; }

    // Discounts
    public decimal? SiblingDiscount { get; set; }
    public decimal? MultiProgramDiscount { get; set; }
    public string? DiscountCodes { get; set; } // JSON array

    // Instructor/Coach
    public Guid? PrimaryInstructorId { get; set; }
    public string? InstructorNames { get; set; }
    public int? StudentToInstructorRatio { get; set; }

    // Prerequisites
    public string? Prerequisites { get; set; } // JSON array
    public Guid? PrerequisiteProgramId { get; set; }
    public SkillLevel? RequiredSkillLevel { get; set; }
    public string? RequiredCertifications { get; set; } // JSON array

    // Certification
    public bool OffersQualification { get; set; }
    public string? QualificationName { get; set; }
    public string? QualificationBody { get; set; }
    public string? CertificateTemplate { get; set; }

    // Equipment
    public string? EquipmentRequired { get; set; } // JSON array
    public bool EquipmentProvided { get; set; }
    public string? EquipmentIncluded { get; set; } // JSON array

    // Membership Restrictions
    public bool MembersOnly { get; set; }
    public string? AllowedMembershipTypes { get; set; } // JSON array

    // Terms & Conditions
    public string? TermsAndConditions { get; set; }
    public string? CancellationPolicy { get; set; }
    public decimal? CancellationFee { get; set; }
    public int? CancellationNoticeDays { get; set; }

    // Media
    public string? ImageUrl { get; set; }
    public string? GalleryUrls { get; set; } // JSON array
    public string? VideoUrl { get; set; }

    // Statistics
    public int TotalEnrollmentsAllTime { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal? AverageRating { get; set; }
    public int ReviewCount { get; set; }

    // Publishing
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? PublishedBy { get; set; }
    public bool ShowOnWebsite { get; set; }
    public bool AllowOnlineRegistration { get; set; } = true;

    // Notes
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }
    public string? InstructorNotes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public Venue? Venue { get; set; }
    public Facility? Facility { get; set; }
    public Member? PrimaryInstructor { get; set; }
    public Program? PrerequisiteProgram { get; set; }
    public Fee? Fee { get; set; }
    public ICollection<ProgramSession> Sessions { get; set; } = new List<ProgramSession>();
    public ICollection<ProgramEnrollment> Enrollments { get; set; } = new List<ProgramEnrollment>();
    public ICollection<ProgramInstructor> Instructors { get; set; } = new List<ProgramInstructor>();
}

/// <summary>
/// Represents a single session/class within a program.
/// </summary>
public class ProgramSession
{
    public Guid Id { get; set; }
    public Guid ProgramId { get; set; }
    public Guid? VenueId { get; set; }
    public Guid? FacilityId { get; set; }

    // Session Details
    public int SessionNumber { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Topic { get; set; }

    // Schedule
    public DateTime Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int DurationMinutes { get; set; }

    // Status
    public bool IsCancelled { get; set; }
    public string? CancellationReason { get; set; }
    public DateTime? RescheduledFrom { get; set; }
    public bool IsMakeUp { get; set; }

    // Instructor
    public Guid? InstructorId { get; set; }
    public string? InstructorName { get; set; }

    // Capacity
    public int? Capacity { get; set; }
    public int AttendanceCount { get; set; }

    // Notes
    public string? Notes { get; set; }
    public string? InstructorNotes { get; set; }
    public string? LearningObjectives { get; set; }
    public string? MaterialsCovered { get; set; }

    // Resources
    public string? ResourceUrls { get; set; } // JSON array

    // Navigation
    public Program Program { get; set; } = null!;
    public Venue? Venue { get; set; }
    public Facility? Facility { get; set; }
    public Member? Instructor { get; set; }
    public ICollection<ProgramAttendance> Attendance { get; set; } = new List<ProgramAttendance>();
}

/// <summary>
/// Represents a member's enrollment in a program.
/// </summary>
public class ProgramEnrollment
{
    public Guid Id { get; set; }
    public Guid ProgramId { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberId { get; set; }
    public Guid? FamilyMemberId { get; set; }

    // Enrollment Details
    public string EnrollmentNumber { get; set; } = string.Empty;
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Pending;
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
    public DateTime? ConfirmedDate { get; set; }
    public int? WaitlistPosition { get; set; }

    // Start Info
    public DateTime? StartDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public int SessionsAttended { get; set; }
    public int SessionsMissed { get; set; }
    public decimal AttendanceRate { get; set; }

    // Assessment
    public SkillLevel? SkillLevelAtStart { get; set; }
    public SkillLevel? SkillLevelAtEnd { get; set; }
    public string? AssessmentNotes { get; set; }
    public bool Passed { get; set; }
    public decimal? FinalScore { get; set; }
    public string? GradeAwarded { get; set; }

    // Pricing
    public decimal Price { get; set; }
    public decimal? DiscountAmount { get; set; }
    public string? DiscountCode { get; set; }
    public string? DiscountReason { get; set; }
    public decimal FinalPrice { get; set; }
    public string Currency { get; set; } = "GBP";

    // Payment
    public bool IsPaid { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal BalanceDue { get; set; }
    public DateTime? PaidDate { get; set; }
    public string? PaymentReference { get; set; }
    public Guid? PaymentId { get; set; }
    public Guid? InvoiceId { get; set; }

    // Deposit
    public bool DepositPaid { get; set; }
    public decimal? DepositAmount { get; set; }
    public DateTime? DepositPaidDate { get; set; }

    // Withdrawal/Cancellation
    public DateTime? WithdrawnDate { get; set; }
    public string? WithdrawnBy { get; set; }
    public string? WithdrawalReason { get; set; }
    public decimal? RefundAmount { get; set; }
    public bool RefundProcessed { get; set; }

    // Transfer
    public bool IsTransferred { get; set; }
    public Guid? TransferredFromId { get; set; }
    public Guid? TransferredToId { get; set; }
    public DateTime? TransferDate { get; set; }

    // Certification
    public bool CertificateAwarded { get; set; }
    public DateTime? CertificateDate { get; set; }
    public string? CertificateNumber { get; set; }
    public string? CertificateUrl { get; set; }

    // Emergency Contact
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? EmergencyContactRelation { get; set; }

    // Medical/Consent
    public bool MedicalFormSubmitted { get; set; }
    public bool ConsentFormSubmitted { get; set; }
    public string? MedicalNotes { get; set; }
    public string? SpecialRequirements { get; set; }

    // Notes
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Program Program { get; set; } = null!;
    public Club Club { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public FamilyMember? FamilyMember { get; set; }
    public Payment? Payment { get; set; }
    public Invoice? Invoice { get; set; }
    public ProgramEnrollment? TransferredFrom { get; set; }
    public ICollection<ProgramAttendance> Attendance { get; set; } = new List<ProgramAttendance>();
}

/// <summary>
/// Represents an instructor assigned to a program.
/// </summary>
public class ProgramInstructor
{
    public Guid Id { get; set; }
    public Guid ProgramId { get; set; }
    public Guid MemberId { get; set; }

    // Role
    public bool IsPrimary { get; set; }
    public string? Role { get; set; } // Lead Instructor, Assistant, etc.
    public string? Specialization { get; set; }

    // Assignment
    public DateTime AssignedDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Qualifications
    public string? Qualifications { get; set; } // JSON array
    public string? Certifications { get; set; } // JSON array

    // Compensation
    public decimal? HourlyRate { get; set; }
    public decimal? SessionRate { get; set; }
    public string Currency { get; set; } = "GBP";

    // Notes
    public string? Notes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }

    // Navigation
    public Program Program { get; set; } = null!;
    public Member Member { get; set; } = null!;
}

/// <summary>
/// Represents attendance for a program session.
/// </summary>
public class ProgramAttendance
{
    public Guid Id { get; set; }
    public Guid ProgramSessionId { get; set; }
    public Guid EnrollmentId { get; set; }

    // Attendance
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Pending;
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public string? CheckedInBy { get; set; }

    // Late/Early
    public int? MinutesLate { get; set; }
    public int? MinutesEarly { get; set; }

    // Excuse
    public string? ExcuseReason { get; set; }
    public bool ExcuseApproved { get; set; }
    public string? ApprovedBy { get; set; }

    // Make-up
    public bool IsMakeUp { get; set; }
    public Guid? MakeUpForSessionId { get; set; }
    public Guid? MakeUpScheduledId { get; set; }

    // Performance
    public string? PerformanceNotes { get; set; }
    public decimal? Score { get; set; }
    public string? InstructorFeedback { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public ProgramSession ProgramSession { get; set; } = null!;
    public ProgramEnrollment Enrollment { get; set; } = null!;
    public ProgramSession? MakeUpForSession { get; set; }
}

/// <summary>
/// Represents a certificate or qualification awarded to a member.
/// </summary>
public class MemberCertificate
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public Guid ClubId { get; set; }
    public Guid? ProgramId { get; set; }
    public Guid? EnrollmentId { get; set; }

    // Certificate Details
    public string Name { get; set; } = string.Empty;
    public string? CertificateNumber { get; set; }
    public string? Description { get; set; }
    public CertificateStatus Status { get; set; } = CertificateStatus.NotStarted;

    // Award Info
    public string? AwardingBody { get; set; }
    public DateTime? AwardedDate { get; set; }
    public string? AwardedBy { get; set; }
    public string? Level { get; set; }
    public string? Grade { get; set; }

    // Validity
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool NeverExpires { get; set; }
    public int? ValidityYears { get; set; }

    // Renewal
    public bool RequiresRenewal { get; set; }
    public DateTime? RenewalDate { get; set; }
    public decimal? RenewalFee { get; set; }
    public bool RenewalReminderSent { get; set; }

    // Verification
    public string? VerificationUrl { get; set; }
    public string? VerificationCode { get; set; }

    // Documents
    public string? CertificateUrl { get; set; }
    public string? BadgeUrl { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Member Member { get; set; } = null!;
    public Club Club { get; set; } = null!;
    public Program? Program { get; set; }
    public ProgramEnrollment? Enrollment { get; set; }
}
