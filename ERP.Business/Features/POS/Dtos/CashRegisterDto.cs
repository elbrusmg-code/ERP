namespace ERP.Business.Features.POS.Dtos;

public sealed class CashRegisterDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
}
