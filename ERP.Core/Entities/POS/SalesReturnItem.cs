using ERP.Core.Common;
using ERP.Core.Entities.Catalog;
using ERP.Core.Entities.Inventory;

namespace ERP.Core.Entities.POS;

public class SalesReturnItem : AuditableEntity
{
    public int SalesReturnId { get; set; }
    public SalesReturn? SalesReturn { get; set; }
    public int POSReceiptItemId { get; set; }
    public POSReceiptItem? POSReceiptItem { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int? ProductBatchId { get; set; }
    public ProductBatch? ProductBatch { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal ReturnAmount { get; set; }
    public bool RestockToInventory { get; set; }
    public bool MarkAsDamaged { get; set; }
    public string? Reason { get; set; }
}
