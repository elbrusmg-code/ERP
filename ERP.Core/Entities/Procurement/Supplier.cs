using ERP.Core.Common;
using ERP.Core.Entities.CRM;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Procurement;

public class Supplier : SoftDeleteEntity
{
    public string CompanyName { get; set; } = string.Empty;
    public string? LegalName { get; set; }
    public string? TaxNumber { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public SupplierStatus Status { get; set; }
    public bool IsActive { get; set; }
    public ICollection<SupplierContact> Contacts { get; set; } = new List<SupplierContact>();
    public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    public ICollection<SupplierInvoice> Invoices { get; set; } = new List<SupplierInvoice>();
    public ICollection<SupplierPayment> Payments { get; set; } = new List<SupplierPayment>();
    public ICollection<SupplierReturn> Returns { get; set; } = new List<SupplierReturn>();
    public ICollection<SupplierNote> Notes { get; set; } = new List<SupplierNote>();
}
