using ERP.Core.Common;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Finance;

public class JournalEntry : SoftDeleteEntity
{
    public string EntryNumber { get; set; } = string.Empty;
    public JournalEntryStatus Status { get; set; }
    public JournalEntrySource Source { get; set; }
    public int? BranchId { get; set; }
    public Branch? Branch { get; set; }
    public DateTime EntryDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? ReferenceNumber { get; set; }
    public string? PostedBy { get; set; }
    public DateTime? PostedAt { get; set; }
    public ICollection<JournalEntryLine> Lines { get; set; } = new List<JournalEntryLine>();
}
