using ERP.Core.Common;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Catalog;

public class ProductPriceHistory : AuditableEntity
{
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int? BranchId { get; set; }
    public Branch? Branch { get; set; }
    public decimal OldCostPrice { get; set; }
    public decimal NewCostPrice { get; set; }
    public decimal OldSalePrice { get; set; }
    public decimal NewSalePrice { get; set; }
    public PriceChangeReason Reason { get; set; }
    public DateTime ChangedAt { get; set; }
    public string? Note { get; set; }
}
