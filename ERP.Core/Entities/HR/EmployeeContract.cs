using ERP.Core.Common;
using ERP.Core.Enums;

namespace ERP.Core.Entities.HR;

public class EmployeeContract : SoftDeleteEntity
{
    public string ContractNumber { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal Salary { get; set; }
    public string? ContractType { get; set; }
    public ContractStatus Status { get; set; }
    public string? FileUrl { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}
