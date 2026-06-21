using ERP.Core.Common;
using ERP.Core.Entities.HR;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.POS;

public class CashShift : SoftDeleteEntity
{
    public string ShiftNumber { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public int CashRegisterId { get; set; }
    public CashRegister? CashRegister { get; set; }
    public int CashierEmployeeId { get; set; }
    public Employee? CashierEmployee { get; set; }
    public CashShiftStatus Status { get; set; }
    public DateTime OpenedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public decimal OpeningCashAmount { get; set; }
    public decimal ExpectedCashAmount { get; set; }
    public decimal ActualCashAmount { get; set; }
    public decimal CashDifference { get; set; }
    public decimal TotalCashSales { get; set; }
    public decimal TotalCardSales { get; set; }
    public decimal TotalMixedSales { get; set; }
    public decimal TotalRefunds { get; set; }
    public string? OpeningNote { get; set; }
    public string? ClosingNote { get; set; }
    public string? ClosedBy { get; set; }
    public ICollection<POSReceipt> Receipts { get; set; } = new List<POSReceipt>();
}
