using ERP.Core.Entities.Organization;

namespace ERP.Business.Common.Interfaces.Repositories.Specific;

public interface IOrganizationRepository
{
    Task<List<Company>> GetCompaniesWithBranchesAsync(CancellationToken cancellationToken = default);
    Task<Company?> GetCompanyWithBranchesAsync(int companyId, CancellationToken cancellationToken = default);
    Task<List<Branch>> GetActiveBranchesAsync(CancellationToken cancellationToken = default);
    Task<Branch?> GetBranchDetailsAsync(int branchId, CancellationToken cancellationToken = default);
    Task<bool> BranchCodeExistsAsync(int companyId, string code, int? excludeBranchId = null, CancellationToken cancellationToken = default);
}
