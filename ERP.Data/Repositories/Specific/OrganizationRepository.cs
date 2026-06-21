using ERP.Business.Common.Interfaces.Repositories.Specific;
using ERP.Core.Entities.Organization;
using ERP.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories.Specific;

public sealed class OrganizationRepository(ERPDbContext context) : IOrganizationRepository
{
    public Task<List<Company>> GetCompaniesWithBranchesAsync(CancellationToken cancellationToken = default)
    {
        return context.Companies
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Include(x => x.Branches.Where(branch => !branch.IsDeleted))
            .ToListAsync(cancellationToken);
    }

    public Task<Company?> GetCompanyWithBranchesAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return context.Companies
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Include(x => x.Branches.Where(branch => !branch.IsDeleted))
            .FirstOrDefaultAsync(x => x.Id == companyId, cancellationToken);
    }

    public Task<List<Branch>> GetActiveBranchesAsync(CancellationToken cancellationToken = default)
    {
        return context.Branches
            .AsNoTracking()
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<Branch?> GetBranchDetailsAsync(int branchId, CancellationToken cancellationToken = default)
    {
        return context.Branches
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => !x.IsDeleted)
            .Include(x => x.Company)
            .Include(x => x.Departments.Where(department => !department.IsDeleted))
            .Include(x => x.Employees.Where(employee => !employee.IsDeleted))
            .FirstOrDefaultAsync(x => x.Id == branchId, cancellationToken);
    }

    public Task<bool> BranchCodeExistsAsync(
        int companyId,
        string code,
        int? excludeBranchId = null,
        CancellationToken cancellationToken = default)
    {
        return context.Branches.AsNoTracking().AnyAsync(
            x => x.CompanyId == companyId &&
                 x.Code == code &&
                 !x.IsDeleted &&
                 (!excludeBranchId.HasValue || x.Id != excludeBranchId.Value),
            cancellationToken);
    }
}
