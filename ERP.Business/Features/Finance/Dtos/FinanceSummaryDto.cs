namespace ERP.Business.Features.Finance.Dtos;

public sealed class FinanceSummaryDto
{
    public int BranchId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetProfit { get; set; }
}
