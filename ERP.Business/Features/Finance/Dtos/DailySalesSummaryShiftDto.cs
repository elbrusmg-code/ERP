namespace ERP.Business.Features.Finance.Dtos;

public sealed class DailySalesSummaryShiftDto
{
    public int Id { get; set; }
    public int CashShiftId { get; set; }
    public decimal TotalSales { get; set; }
    public decimal CashSales { get; set; }
    public decimal CardSales { get; set; }
    public decimal MixedSales { get; set; }
    public decimal Refunds { get; set; }
    public decimal ExpectedCash { get; set; }
    public decimal ActualCash { get; set; }
    public decimal CashDifference { get; set; }
}
