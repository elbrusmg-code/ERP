namespace ERP.Business.Features.POS.Dtos;

public sealed class CashShiftDto
{
    public int Id { get; set; }
    public string ShiftNumber { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public int CashRegisterId { get; set; }
    public string CashRegisterCode { get; set; } = string.Empty;
    public int CashierEmployeeId { get; set; }
    public string CashierName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
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
}
