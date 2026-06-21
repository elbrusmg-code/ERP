using ERP.Core.Common;
using ERP.Core.Entities.Catalog;
using ERP.Core.Entities.Organization;

namespace ERP.Core.Entities.Inventory;

public class ProductBatch : SoftDeleteEntity
{
    public string BatchNumber { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public int WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
    public DateTime? ManufactureDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public decimal InitialQuantity { get; set; }
    public decimal CurrentQuantity { get; set; }
    public decimal PurchaseCost { get; set; }
    public bool IsActive { get; set; }
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}
