using ERP.Core.Common;
using ERP.Core.Entities.Inventory;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Procurement;

public class GoodsReceipt : SoftDeleteEntity
{
    public string ReceiptNumber { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public int WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public int? PurchaseOrderId { get; set; }
    public PurchaseOrder? PurchaseOrder { get; set; }
    public GoodsReceiptStatus Status { get; set; }
    public DateTime ReceivedDate { get; set; }
    public string? ReceivedBy { get; set; }
    public string? Note { get; set; }
    public ICollection<GoodsReceiptItem> Items { get; set; } = new List<GoodsReceiptItem>();
}
