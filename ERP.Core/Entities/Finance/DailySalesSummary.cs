using ERP.Core.Common;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Finance;

public class DailySalesSummary : SoftDeleteEntity
{
    public string SummaryNumber { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public DateTime SalesDate { get; set; }
    public DailySalesSummaryStatus Status { get; set; }
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
    public ICollection<DailySalesSummaryShift> Shifts { get; set; } = new List<DailySalesSummaryShift>();
}
