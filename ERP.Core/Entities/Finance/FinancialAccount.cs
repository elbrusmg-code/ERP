using ERP.Core.Common;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Finance;

public class FinancialAccount : SoftDeleteEntity
{
    public string AccountCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public FinancialAccountType Type { get; set; }
    public FinancialAccountStatus Status { get; set; }
    public int? BranchId { get; set; }
    public Branch? Branch { get; set; }
    public bool IsSystemAccount { get; set; }
    public bool IsActive { get; set; }
    public ICollection<FinancialTransaction> Transactions { get; set; } = new List<FinancialTransaction>();
    public ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
}
