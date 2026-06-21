using ERP.Core.Common;
using ERP.Core.Entities.Catalog;

namespace ERP.Core.Entities.Inventory;

public class StockTransferItem : AuditableEntity
{
    public int StockTransferId { get; set; }
    public StockTransfer? StockTransfer { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int? ProductBatchId { get; set; }
    public ProductBatch? ProductBatch { get; set; }
    public decimal Quantity { get; set; }
    public decimal? ReceivedQuantity { get; set; }
    public string? Note { get; set; }
}
