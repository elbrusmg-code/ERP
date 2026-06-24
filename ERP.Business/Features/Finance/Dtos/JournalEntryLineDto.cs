namespace ERP.Business.Features.Finance.Dtos;

public sealed class JournalEntryLineDto
{
    public int Id { get; set; }
    public int FinancialAccountId { get; set; }
    public string FinancialAccountCode { get; set; } = string.Empty;
    public string FinancialAccountName { get; set; } = string.Empty;
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public string? Description { get; set; }
}
