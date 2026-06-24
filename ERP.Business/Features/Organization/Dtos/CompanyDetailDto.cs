namespace ERP.Business.Features.Organization.Dtos;

public class CompanyDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? LegalName { get; set; }
    public string? TaxNumber { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
    public List<BranchListDto> Branches { get; set; } = new();
}
