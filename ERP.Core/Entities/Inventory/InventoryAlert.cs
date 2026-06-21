using ERP.Core.Common;
using ERP.Core.Entities.Catalog;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Inventory;

public class InventoryAlert : AuditableEntity
{
    public InventoryAlertType Type { get; set; }
    public InventoryAlertStatus Status { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public int WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
    public decimal CurrentQuantity { get; set; }
    public decimal? ThresholdQuantity { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? ResolvedDate { get; set; }
    public string? ResolvedBy { get; set; }
}
