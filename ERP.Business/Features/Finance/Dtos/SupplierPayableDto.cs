namespace ERP.Business.Features.Finance.Dtos;

public sealed class SupplierPayableDto
{
    public int Id { get; set; }
    public string PayableNumber { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public int? SupplierInvoiceId { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal OriginalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Note { get; set; }
}
