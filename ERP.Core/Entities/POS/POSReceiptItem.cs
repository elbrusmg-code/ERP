using ERP.Core.Common;
using ERP.Core.Entities.Catalog;
using ERP.Core.Entities.Inventory;

namespace ERP.Core.Entities.POS;

public class POSReceiptItem : AuditableEntity
{
    public int POSReceiptId { get; set; }
    public POSReceipt? POSReceipt { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int? ProductBatchId { get; set; }
    public ProductBatch? ProductBatch { get; set; }
    public string ProductNameSnapshot { get; set; } = string.Empty;
    public string SKUSnapshot { get; set; } = string.Empty;
    public string? BarcodeSnapshot { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal CostPriceSnapshot { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal LineTotal { get; set; }
    public bool IsReturned { get; set; }
    public decimal ReturnedQuantity { get; set; }
}
