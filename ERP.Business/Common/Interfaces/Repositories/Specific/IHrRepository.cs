using ERP.Core.Entities.HR;

namespace ERP.Business.Common.Interfaces.Repositories.Specific;

public interface IHrRepository
{
    Task<List<Department>> GetDepartmentsByBranchAsync(int branchId, CancellationToken cancellationToken = default);
    Task<List<Position>> GetPositionsByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);
    Task<List<Employee>> GetEmployeesByBranchAsync(int branchId, CancellationToken cancellationToken = default);
    Task<Employee?> GetEmployeeDetailsAsync(int employeeId, CancellationToken cancellationToken = default);
    Task<Employee?> GetEmployeeByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> EmployeeCodeExistsAsync(int branchId, string employeeCode, int? excludeEmployeeId = null, CancellationToken cancellationToken = default);
    Task<bool> EmployeeEmailExistsAsync(string email, int? excludeEmployeeId = null, CancellationToken cancellationToken = default);
}
