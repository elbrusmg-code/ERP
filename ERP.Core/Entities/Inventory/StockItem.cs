using ERP.Core.Common;
using ERP.Core.Entities.Catalog;
using ERP.Core.Entities.Organization;

namespace ERP.Core.Entities.Inventory;

public class StockItem : AuditableEntity
{
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public int WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
    public decimal Quantity { get; set; }
    public decimal ReservedQuantity { get; set; }
    public decimal MinimumStockLevel { get; set; }
    public decimal MaximumStockLevel { get; set; }
    public decimal ReorderLevel { get; set; }
    public DateTime? LastStockUpdateAt { get; set; }
}
