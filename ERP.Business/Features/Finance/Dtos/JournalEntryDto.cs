namespace ERP.Business.Features.Finance.Dtos;

public sealed class JournalEntryDto
{
    public int Id { get; set; }
    public string EntryNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public int? BranchId { get; set; }
    public string? BranchName { get; set; }
    public DateTime EntryDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? ReferenceNumber { get; set; }
    public string? PostedBy { get; set; }
    public DateTime? PostedAt { get; set; }
    public List<JournalEntryLineDto> Lines { get; set; } = new();
}
