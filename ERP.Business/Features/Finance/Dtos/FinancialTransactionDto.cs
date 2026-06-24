namespace ERP.Business.Features.Finance.Dtos;

public sealed class FinancialTransactionDto
{
    public int Id { get; set; }
    public string TransactionNumber { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public int? FinancialAccountId { get; set; }
    public string? FinancialAccountName { get; set; }
    public string? PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string SourceModule { get; set; } = string.Empty;
    public string? ReferenceNumber { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? CreatedByUserId { get; set; }
}
