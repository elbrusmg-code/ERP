using ERP.Core.Common;
using ERP.Core.Enums;

namespace ERP.Core.Entities.POS;

public class POSPayment : AuditableEntity
{
    public string PaymentNumber { get; set; } = string.Empty;
    public int POSReceiptId { get; set; }
    public POSReceipt? POSReceipt { get; set; }
    public PaymentMethod Method { get; set; }
    public PosPaymentStatus Status { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Note { get; set; }
    public ICollection<PaymentTerminalTransaction> TerminalTransactions { get; set; } = new List<PaymentTerminalTransaction>();
}
