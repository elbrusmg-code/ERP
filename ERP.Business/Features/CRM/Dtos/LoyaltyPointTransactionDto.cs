namespace ERP.Business.Features.CRM.Dtos;

public sealed class LoyaltyPointTransactionDto
{
    public int Id { get; set; }
    public string TransactionNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public int? LoyaltyCardId { get; set; }
    public string? LoyaltyCardNumber { get; set; }
    public int? POSReceiptId { get; set; }
    public string? POSReceiptNumber { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Points { get; set; }
    public decimal BalanceAfter { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Note { get; set; }
}
