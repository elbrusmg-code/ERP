using ERP.Core.Common;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Procurement;

public class SupplierPayment : AuditableEntity
{
    public string PaymentNumber { get; set; } = string.Empty;
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public int? SupplierInvoiceId { get; set; }
    public SupplierInvoice? SupplierInvoice { get; set; }
    public PaymentMethod Method { get; set; }
    public SupplierPaymentStatus Status { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Note { get; set; }
}
