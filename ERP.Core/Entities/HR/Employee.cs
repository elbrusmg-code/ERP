using ERP.Core.Common;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.HR;

public class Employee : SoftDeleteEntity
{
    public string EmployeeCode { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime HireDate { get; set; }
    public EmployeeStatus Status { get; set; }
    public decimal? Salary { get; set; }
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
    public int PositionId { get; set; }
    public Position? Position { get; set; }
    public string? AppUserId { get; set; }
    public ICollection<EmployeeContract> Contracts { get; set; } = new List<EmployeeContract>();
}
