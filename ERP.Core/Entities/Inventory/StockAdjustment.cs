using ERP.Core.Common;
using ERP.Core.Entities.Catalog;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Inventory;

public class StockAdjustment : AuditableEntity
{
    public string AdjustmentNumber { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public int WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
    public int? ProductBatchId { get; set; }
    public ProductBatch? ProductBatch { get; set; }
    public decimal QuantityBefore { get; set; }
    public decimal AdjustmentQuantity { get; set; }
    public decimal QuantityAfter { get; set; }
    public StockAdjustmentReason Reason { get; set; }
    public string? Note { get; set; }
    public DateTime AdjustmentDate { get; set; }
    public string? ApprovedBy { get; set; }
}
