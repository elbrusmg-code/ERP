namespace ERP.Business.Features.POS.Dtos;

public sealed class PaymentTerminalTransactionDto
{
    public int Id { get; set; }
    public string TerminalTransactionNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? TerminalId { get; set; }
    public string? TerminalName { get; set; }
    public string? BankName { get; set; }
    public string? AuthorizationCode { get; set; }
    public string? RRN { get; set; }
    public string? CardLastFourDigits { get; set; }
    public decimal Amount { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? RespondedAt { get; set; }
    public string? ResponseMessage { get; set; }
}
