using ERP.Core.Common;

namespace ERP.Core.Entities.Finance;

public class JournalEntryLine : AuditableEntity
{
    public int JournalEntryId { get; set; }
    public JournalEntry? JournalEntry { get; set; }
    public int FinancialAccountId { get; set; }
    public FinancialAccount? FinancialAccount { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public string? Description { get; set; }
}
