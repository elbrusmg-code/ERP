using ERP.Core.Common;
using ERP.Core.Entities.HR;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.POS;

public class POSReceipt : SoftDeleteEntity
{
    public string ReceiptNumber { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public int CashRegisterId { get; set; }
    public CashRegister? CashRegister { get; set; }
    public int CashShiftId { get; set; }
    public CashShift? CashShift { get; set; }
    public int CashierEmployeeId { get; set; }
    public Employee? CashierEmployee { get; set; }
    public PosReceiptStatus Status { get; set; }
    public DateTime ReceiptDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountTotal { get; set; }
    public decimal TaxTotal { get; set; }
    public decimal GrandTotal { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal ChangeAmount { get; set; }
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public string? CustomerPhone { get; set; }
    public string? CustomerName { get; set; }
    public string? Note { get; set; }
    public string? VoidReason { get; set; }
    public DateTime? VoidedAt { get; set; }
    public string? VoidedBy { get; set; }
    public ICollection<POSReceiptItem> Items { get; set; } = new List<POSReceiptItem>();
    public ICollection<POSPayment> Payments { get; set; } = new List<POSPayment>();
    public ICollection<SalesReturn> Returns { get; set; } = new List<SalesReturn>();
}
