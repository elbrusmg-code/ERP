namespace ERP.Business.Features.Organization.Dtos;

public class BranchDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public List<DepartmentSummaryDto> Departments { get; set; } = new();
    public List<EmployeeSummaryDto> Employees { get; set; } = new();
}
