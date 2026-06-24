using ERP.Business.Common.Models;
using ERP.Business.Features.Organization.Dtos;

namespace ERP.Business.Features.Organization.Interfaces;

public interface IOrganizationService
{
    Task<ServiceResult<List<CompanyListDto>>> GetCompaniesAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<CompanyDetailDto>> GetCompanyByIdAsync(int companyId, CancellationToken cancellationToken = default);
    Task<ServiceResult<List<BranchListDto>>> GetActiveBranchesAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<BranchDetailDto>> GetBranchByIdAsync(int branchId, CancellationToken cancellationToken = default);
}
