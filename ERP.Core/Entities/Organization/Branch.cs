using ERP.Core.Common;
using ERP.Core.Entities.HR;
using ERP.Core.Entities.Security;

namespace ERP.Core.Entities.Organization;

public class Branch : SoftDeleteEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
    public int CompanyId { get; set; }
    public Company? Company { get; set; }
    public ICollection<Department> Departments { get; set; } = new List<Department>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<UserBranchAssignment> UserBranchAssignments { get; set; } = new List<UserBranchAssignment>();
}
