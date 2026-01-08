using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

public class MemberDocument
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberId { get; set; }

    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string FilePath { get; set; } = string.Empty;

    public DocumentType Type { get; set; }
    public string? Description { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public string? UploadedBy { get; set; }

    public Member Member { get; set; } = null!;
}
