using ERP.Core.Common;
using ERP.Core.Entities.Organization;
using ERP.Core.Entities.POS;
using ERP.Core.Enums;

namespace ERP.Core.Entities.CRM;

public class CustomerTransactionHistory : AuditableEntity
{
    public string TransactionNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public CustomerTransactionType Type { get; set; }
    public int? POSReceiptId { get; set; }
    public POSReceipt? POSReceipt { get; set; }
    public int? SalesReturnId { get; set; }
    public SalesReturn? SalesReturn { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Note { get; set; }
}
