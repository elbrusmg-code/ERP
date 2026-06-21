using ERP.Core.Common;
using ERP.Core.Entities.HR;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.POS;

public class SalesReturn : SoftDeleteEntity
{
    public string ReturnNumber { get; set; } = string.Empty;
    public int OriginalReceiptId { get; set; }
    public POSReceipt? OriginalReceipt { get; set; }
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public int CashRegisterId { get; set; }
    public CashRegister? CashRegister { get; set; }
    public int CashShiftId { get; set; }
    public CashShift? CashShift { get; set; }
    public int CashierEmployeeId { get; set; }
    public Employee? CashierEmployee { get; set; }
    public SalesReturnStatus Status { get; set; }
    public DateTime ReturnDate { get; set; }
    public decimal TotalReturnAmount { get; set; }
    public PaymentMethod RefundMethod { get; set; }
    public string? Reason { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public ICollection<SalesReturnItem> Items { get; set; } = new List<SalesReturnItem>();
}
