using ERP.Core.Common;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Procurement;

public class SupplierInvoice : SoftDeleteEntity
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public int? PurchaseOrderId { get; set; }
    public PurchaseOrder? PurchaseOrder { get; set; }
    public SupplierInvoiceStatus Status { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TaxTotal { get; set; }
    public decimal DiscountTotal { get; set; }
    public decimal GrandTotal { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public string? Note { get; set; }
    public ICollection<SupplierPayment> Payments { get; set; } = new List<SupplierPayment>();
}
