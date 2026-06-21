using ERP.Core.Common;
using ERP.Core.Entities.Catalog;

namespace ERP.Core.Entities.Inventory;

public class StockTakeItem : AuditableEntity
{
    public int StockTakeId { get; set; }
    public StockTake? StockTake { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int? ProductBatchId { get; set; }
    public ProductBatch? ProductBatch { get; set; }
    public decimal SystemQuantity { get; set; }
    public decimal CountedQuantity { get; set; }
    public decimal DifferenceQuantity { get; set; }
    public string? Note { get; set; }
}
