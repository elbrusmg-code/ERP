using ERP.Core.Common;
using ERP.Core.Entities.POS;

namespace ERP.Core.Entities.Finance;

public class DailySalesSummaryShift : AuditableEntity
{
    public int DailySalesSummaryId { get; set; }
    public DailySalesSummary? DailySalesSummary { get; set; }
    public int CashShiftId { get; set; }
    public CashShift? CashShift { get; set; }
    public decimal TotalSales { get; set; }
    public decimal CashSales { get; set; }
    public decimal CardSales { get; set; }
    public decimal MixedSales { get; set; }
    public decimal Refunds { get; set; }
    public decimal ExpectedCash { get; set; }
    public decimal ActualCash { get; set; }
    public decimal CashDifference { get; set; }
}
