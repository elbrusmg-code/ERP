using ERP.Core.Common;
using ERP.Core.Entities.Organization;
using ERP.Core.Entities.Procurement;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Finance;

public class SupplierPayable : SoftDeleteEntity
{
    public string PayableNumber { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public int? SupplierInvoiceId { get; set; }
    public SupplierInvoice? SupplierInvoice { get; set; }
    public SupplierPayableStatus Status { get; set; }
    public decimal OriginalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Note { get; set; }
}
