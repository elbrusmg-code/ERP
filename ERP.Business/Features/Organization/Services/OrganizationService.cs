using ERP.Business.Common.Interfaces.Repositories.Specific;
using ERP.Business.Common.Models;
using ERP.Business.Features.Organization.Dtos;
using ERP.Business.Features.Organization.Interfaces;
using ERP.Core.Entities.Organization;

namespace ERP.Business.Features.Organization.Services;

public sealed class OrganizationService(
    IOrganizationRepository organizationRepository,
    IHrRepository hrRepository) : IOrganizationService
{
    public async Task<ServiceResult<List<CompanyListDto>>> GetCompaniesAsync(
        CancellationToken cancellationToken = default)
    {
        var companies = await organizationRepository.GetCompaniesWithBranchesAsync(cancellationToken);
        var data = companies
            .OrderBy(x => x.Name)
            .Select(x => new CompanyListDto
            {
                Id = x.Id,
                Name = x.Name,
                LegalName = x.LegalName,
                TaxNumber = x.TaxNumber,
                IsActive = x.IsActive,
                BranchCount = x.Branches.Count
            })
            .ToList();

        return ServiceResult<List<CompanyListDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<CompanyDetailDto>> GetCompanyByIdAsync(
        int companyId,
        CancellationToken cancellationToken = default)
    {
        var company = await organizationRepository.GetCompanyWithBranchesAsync(companyId, cancellationToken);
        if (company is null)
        {
            return ServiceResult<CompanyDetailDto>.Failure("Company not found.");
        }

        var data = new CompanyDetailDto
        {
            Id = company.Id,
            Name = company.Name,
            LegalName = company.LegalName,
            TaxNumber = company.TaxNumber,
            Phone = company.Phone,
            Email = company.Email,
            Address = company.Address,
            IsActive = company.IsActive,
            Branches = company.Branches
                .OrderBy(x => x.Name)
                .Select(x => MapBranch(x, company.Name))
                .ToList()
        };

        return ServiceResult<CompanyDetailDto>.SuccessResult(data);
    }

    public async Task<ServiceResult<List<BranchListDto>>> GetActiveBranchesAsync(
        CancellationToken cancellationToken = default)
    {
        var branches = await organizationRepository.GetActiveBranchesAsync(cancellationToken);
        var companies = await organizationRepository.GetCompaniesWithBranchesAsync(cancellationToken);
        var companyNames = companies.ToDictionary(x => x.Id, x => x.Name);

        var data = branches
            .OrderBy(x => x.Name)
            .Select(x => MapBranch(
                x,
                companyNames.GetValueOrDefault(x.CompanyId, x.Company?.Name ?? string.Empty)))
            .ToList();

        return ServiceResult<List<BranchListDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<BranchDetailDto>> GetBranchByIdAsync(
        int branchId,
        CancellationToken cancellationToken = default)
    {
        var branch = await organizationRepository.GetBranchDetailsAsync(branchId, cancellationToken);
        if (branch is null)
        {
            return ServiceResult<BranchDetailDto>.Failure("Branch not found.");
        }

        var employees = await hrRepository.GetEmployeesByBranchAsync(branchId, cancellationToken);
        var data = new BranchDetailDto
        {
            Id = branch.Id,
            Name = branch.Name,
            Code = branch.Code,
            Phone = branch.Phone,
            Email = branch.Email,
            Address = branch.Address,
            IsActive = branch.IsActive,
            CompanyId = branch.CompanyId,
            CompanyName = branch.Company?.Name ?? string.Empty,
            Departments = branch.Departments
                .OrderBy(x => x.Name)
                .Select(x => new DepartmentSummaryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Type = x.Type.ToString(),
                    IsActive = x.IsActive,
                    EmployeeCount = employees.Count(employee => employee.DepartmentId == x.Id)
                })
                .ToList(),
            Employees = employees
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .Select(x => new EmployeeSummaryDto
                {
                    Id = x.Id,
                    EmployeeCode = x.EmployeeCode,
                    FullName = $"{x.FirstName} {x.LastName}".Trim(),
                    Email = x.Email,
                    Phone = x.Phone,
                    DepartmentName = x.Department?.Name ?? string.Empty,
                    PositionName = x.Position?.Name ?? string.Empty,
                    Status = x.Status.ToString()
                })
                .ToList()
        };

        return ServiceResult<BranchDetailDto>.SuccessResult(data);
    }

    private static BranchListDto MapBranch(Branch branch, string companyName)
    {
        return new BranchListDto
        {
            Id = branch.Id,
            Name = branch.Name,
            Code = branch.Code,
            Phone = branch.Phone,
            Email = branch.Email,
            Address = branch.Address,
            IsActive = branch.IsActive,
            CompanyId = branch.CompanyId,
            CompanyName = companyName
        };
    }
}
