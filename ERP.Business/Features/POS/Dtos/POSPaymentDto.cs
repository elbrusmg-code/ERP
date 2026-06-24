namespace ERP.Business.Features.POS.Dtos;

public sealed class POSPaymentDto
{
    public int Id { get; set; }
    public string PaymentNumber { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Note { get; set; }
    public List<PaymentTerminalTransactionDto> TerminalTransactions { get; set; } = new();
}
