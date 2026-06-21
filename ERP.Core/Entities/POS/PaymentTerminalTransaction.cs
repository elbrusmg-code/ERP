using ERP.Core.Common;
using ERP.Core.Enums;

namespace ERP.Core.Entities.POS;

public class PaymentTerminalTransaction : AuditableEntity
{
    public string TerminalTransactionNumber { get; set; } = string.Empty;
    public int POSPaymentId { get; set; }
    public POSPayment? POSPayment { get; set; }
    public PaymentTerminalStatus Status { get; set; }
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
