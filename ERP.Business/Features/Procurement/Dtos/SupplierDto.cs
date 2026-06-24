namespace ERP.Business.Features.Procurement.Dtos;

public sealed class SupplierDto
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? LegalName { get; set; }
    public string? TaxNumber { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<SupplierContactDto> Contacts { get; set; } = new();
    public List<PurchaseOrderDto> PurchaseOrders { get; set; } = new();
    public List<SupplierInvoiceDto> Invoices { get; set; } = new();
    public List<SupplierPaymentDto> Payments { get; set; } = new();
    public List<SupplierReturnDto> Returns { get; set; } = new();
    public List<string> Notes { get; set; } = new();
}
