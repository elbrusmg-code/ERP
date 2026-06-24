namespace ERP.Business.Features.CRM.Dtos;

public sealed class CustomerTransactionHistoryDto
{
    public int Id { get; set; }
    public string TransactionNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int? POSReceiptId { get; set; }
    public string? POSReceiptNumber { get; set; }
    public int? SalesReturnId { get; set; }
    public string? SalesReturnNumber { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Note { get; set; }
}
