namespace ERP.Business.Features.Organization.Dtos;

public class CompanyListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? LegalName { get; set; }
    public string? TaxNumber { get; set; }
    public bool IsActive { get; set; }
    public int BranchCount { get; set; }
}
