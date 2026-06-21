using ERP.Core.Common;
using ERP.Core.Entities.POS;
using ERP.Core.Enums;

namespace ERP.Core.Entities.CRM;

public class LoyaltyPointTransaction : AuditableEntity
{
    public string TransactionNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int? LoyaltyCardId { get; set; }
    public LoyaltyCard? LoyaltyCard { get; set; }
    public int? POSReceiptId { get; set; }
    public POSReceipt? POSReceipt { get; set; }
    public LoyaltyTransactionType Type { get; set; }
    public decimal Points { get; set; }
    public decimal BalanceAfter { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Note { get; set; }
}
