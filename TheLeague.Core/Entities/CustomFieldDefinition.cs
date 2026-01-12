using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Defines a custom field that can be added to member profiles for a specific club.
/// </summary>
public class CustomFieldDefinition
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }

    public string Name { get; set; } = string.Empty; // Internal name (e.g., "jersey_size")
    public string Label { get; set; } = string.Empty; // Display label (e.g., "Jersey Size")
    public string? Description { get; set; }
    public string? Placeholder { get; set; }

    public CustomFieldType FieldType { get; set; } = CustomFieldType.Text;
    public string? Options { get; set; } // JSON array for Select/MultiSelect: ["XS", "S", "M", "L", "XL"]
    public string? DefaultValue { get; set; }
    public string? ValidationRegex { get; set; }
    public string? ValidationMessage { get; set; }

    public bool IsRequired { get; set; }
    public bool IsVisibleToMember { get; set; } = true;
    public bool IsEditableByMember { get; set; }
    public bool ShowOnRegistration { get; set; }
    public bool ShowOnProfile { get; set; } = true;

    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
}
