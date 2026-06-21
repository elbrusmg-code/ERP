using ERP.Core.Common;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Finance;

public class FinancialTransaction : AuditableEntity
{
    public string TransactionNumber { get; set; } = string.Empty;
    public FinancialTransactionType Type { get; set; }
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public int? FinancialAccountId { get; set; }
    public FinancialAccount? FinancialAccount { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string SourceModule { get; set; } = string.Empty;
    public string? ReferenceNumber { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? CreatedByUserId { get; set; }
}
