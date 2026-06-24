namespace ERP.Business.Features.POS.Dtos;

public sealed class POSReceiptDto
{
    public int Id { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public int CashRegisterId { get; set; }
    public string CashRegisterCode { get; set; } = string.Empty;
    public int CashShiftId { get; set; }
    public int CashierEmployeeId { get; set; }
    public string CashierName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime ReceiptDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountTotal { get; set; }
    public decimal TaxTotal { get; set; }
    public decimal GrandTotal { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal ChangeAmount { get; set; }
    public string DiscountType { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }
    public string? CustomerName { get; set; }
    public string? Note { get; set; }
    public List<POSReceiptItemDto> Items { get; set; } = new();
    public List<POSPaymentDto> Payments { get; set; } = new();
    public List<SalesReturnDto> Returns { get; set; } = new();
}
