namespace ERP.Business.Features.Finance.Dtos;

public sealed class DailySalesSummaryDto
{
    public int Id { get; set; }
    public string SummaryNumber { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public DateTime SalesDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalSales { get; set; }
    public decimal TotalCashSales { get; set; }
    public decimal TotalCardSales { get; set; }
    public decimal TotalMixedSales { get; set; }
    public decimal TotalRefunds { get; set; }
    public decimal NetSales { get; set; }
    public int ReceiptCount { get; set; }
    public int ReturnCount { get; set; }
    public string? CalculatedBy { get; set; }
    public DateTime? CalculatedAt { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public List<DailySalesSummaryShiftDto> Shifts { get; set; } = new();
}
