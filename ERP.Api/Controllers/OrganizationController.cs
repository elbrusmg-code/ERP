using ERP.Business.Common.Models;
using ERP.Business.Features.Organization.Dtos;
using ERP.Business.Features.Organization.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganizationController(IOrganizationService organizationService) : ControllerBase
{
    [HttpGet("companies")]
    public async Task<ActionResult<ApiResponse<List<CompanyListDto>>>> GetCompanies(
        CancellationToken cancellationToken)
    {
        var result = await organizationService.GetCompaniesAsync(cancellationToken);
        return result.Success
            ? Ok(ApiResponse<List<CompanyListDto>>.Ok(result.Data!, result.Message))
            : NotFound(ApiResponse<List<CompanyListDto>>.Fail(result.Message, result.Errors));
    }

    [HttpGet("companies/{id:int}")]
    public async Task<ActionResult<ApiResponse<CompanyDetailDto>>> GetCompanyById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await organizationService.GetCompanyByIdAsync(id, cancellationToken);
        return result.Success
            ? Ok(ApiResponse<CompanyDetailDto>.Ok(result.Data!, result.Message))
            : NotFound(ApiResponse<CompanyDetailDto>.Fail(result.Message, result.Errors));
    }

    [HttpGet("branches")]
    public async Task<ActionResult<ApiResponse<List<BranchListDto>>>> GetBranches(
        CancellationToken cancellationToken)
    {
        var result = await organizationService.GetActiveBranchesAsync(cancellationToken);
        return result.Success
            ? Ok(ApiResponse<List<BranchListDto>>.Ok(result.Data!, result.Message))
            : NotFound(ApiResponse<List<BranchListDto>>.Fail(result.Message, result.Errors));
    }

    [HttpGet("branches/{id:int}")]
    public async Task<ActionResult<ApiResponse<BranchDetailDto>>> GetBranchById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await organizationService.GetBranchByIdAsync(id, cancellationToken);
        return result.Success
            ? Ok(ApiResponse<BranchDetailDto>.Ok(result.Data!, result.Message))
            : NotFound(ApiResponse<BranchDetailDto>.Fail(result.Message, result.Errors));
    }
}
