namespace ERP.Business.Features.POS.Dtos;

public sealed class SalesReturnDto
{
    public int Id { get; set; }
    public string ReturnNumber { get; set; } = string.Empty;
    public int OriginalReceiptId { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public int CashRegisterId { get; set; }
    public string CashRegisterCode { get; set; } = string.Empty;
    public int CashShiftId { get; set; }
    public int CashierEmployeeId { get; set; }
    public string CashierName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime ReturnDate { get; set; }
    public decimal TotalReturnAmount { get; set; }
    public string RefundMethod { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public List<SalesReturnItemDto> Items { get; set; } = new();
}
