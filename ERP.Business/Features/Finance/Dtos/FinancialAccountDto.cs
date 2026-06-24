namespace ERP.Business.Features.Finance.Dtos;

public sealed class FinancialAccountDto
{
    public int Id { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int? BranchId { get; set; }
    public string? BranchName { get; set; }
    public bool IsSystemAccount { get; set; }
    public bool IsActive { get; set; }
}
