using ERP.Core.Common;
using ERP.Core.Entities.Inventory;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Procurement;

public class SupplierReturn : SoftDeleteEntity
{
    public string ReturnNumber { get; set; } = string.Empty;
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public int WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
    public SupplierReturnStatus Status { get; set; }
    public DateTime ReturnDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Reason { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? Note { get; set; }
    public ICollection<SupplierReturnItem> Items { get; set; } = new List<SupplierReturnItem>();
}
