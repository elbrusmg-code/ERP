using ERP.Core.Common;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Inventory;

public class Warehouse : SoftDeleteEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Location { get; set; }
    public WarehouseType Type { get; set; }
    public bool IsActive { get; set; }
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public ICollection<StockItem> StockItems { get; set; } = new List<StockItem>();
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    public ICollection<ProductBatch> ProductBatches { get; set; } = new List<ProductBatch>();
}
