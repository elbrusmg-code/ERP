using ERP.Core.Common;
using ERP.Core.Entities.Catalog;
using ERP.Core.Entities.Inventory;

namespace ERP.Core.Entities.Procurement;

public class SupplierReturnItem : AuditableEntity
{
    public int SupplierReturnId { get; set; }
    public SupplierReturn? SupplierReturn { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int? ProductBatchId { get; set; }
    public ProductBatch? ProductBatch { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal LineTotal { get; set; }
    public string? Reason { get; set; }
}
