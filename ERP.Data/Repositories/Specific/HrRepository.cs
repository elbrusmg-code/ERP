using ERP.Business.Common.Interfaces.Repositories.Specific;
using ERP.Core.Entities.HR;
using ERP.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories.Specific;

public sealed class HrRepository(ERPDbContext context) : IHrRepository
{
    public Task<List<Department>> GetDepartmentsByBranchAsync(int branchId, CancellationToken cancellationToken = default)
    {
        return context.Departments
            .AsNoTracking()
            .Where(x => x.BranchId == branchId && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Position>> GetPositionsByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
    {
        return context.Positions
            .AsNoTracking()
            .Where(x => x.DepartmentId == departmentId && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Employee>> GetEmployeesByBranchAsync(int branchId, CancellationToken cancellationToken = default)
    {
        return context.Employees
            .AsNoTracking()
            .Where(x => x.BranchId == branchId && !x.IsDeleted)
            .Include(x => x.Department)
            .Include(x => x.Position)
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ToListAsync(cancellationToken);
    }

    public Task<Employee?> GetEmployeeDetailsAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        return EmployeeDetailsQuery()
            .FirstOrDefaultAsync(x => x.Id == employeeId, cancellationToken);
    }

    public Task<Employee?> GetEmployeeByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return EmployeeDetailsQuery()
            .FirstOrDefaultAsync(x => x.AppUserId == userId, cancellationToken);
    }

    public Task<bool> EmployeeCodeExistsAsync(
        int branchId,
        string employeeCode,
        int? excludeEmployeeId = null,
        CancellationToken cancellationToken = default)
    {
        return context.Employees.AsNoTracking().AnyAsync(
            x => x.BranchId == branchId &&
                 x.EmployeeCode == employeeCode &&
                 !x.IsDeleted &&
                 (!excludeEmployeeId.HasValue || x.Id != excludeEmployeeId.Value),
            cancellationToken);
    }

    public Task<bool> EmployeeEmailExistsAsync(
        string email,
        int? excludeEmployeeId = null,
        CancellationToken cancellationToken = default)
    {
        return context.Employees.AsNoTracking().AnyAsync(
            x => x.Email == email &&
                 !x.IsDeleted &&
                 (!excludeEmployeeId.HasValue || x.Id != excludeEmployeeId.Value),
            cancellationToken);
    }

    private IQueryable<Employee> EmployeeDetailsQuery()
    {
        return context.Employees
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => !x.IsDeleted)
            .Include(x => x.Branch)
            .Include(x => x.Department)
            .Include(x => x.Position)
            .Include(x => x.Contracts.Where(contract => !contract.IsDeleted));
    }
}
