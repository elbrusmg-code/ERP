using ERP.Business.Common.Models;
using ERP.Business.Features.CRM.Dtos;
using ERP.Business.Features.CRM.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/crm")]
public class CrmController(ICrmService crmService) : ControllerBase
{
    [HttpGet("customer-groups")]
    public async Task<ActionResult<ApiResponse<List<CustomerGroupDto>>>> GetCustomerGroups(
        CancellationToken cancellationToken)
    {
        var result = await crmService.GetCustomerGroupsAsync(cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("customers/{id:int}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> GetCustomerById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await crmService.GetCustomerByIdAsync(id, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("customers/by-phone/{phone}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> GetCustomerByPhone(
        string phone,
        CancellationToken cancellationToken)
    {
        var result = await crmService.GetCustomerByPhoneAsync(phone, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("customers/by-email/{email}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> GetCustomerByEmail(
        string email,
        CancellationToken cancellationToken)
    {
        var result = await crmService.GetCustomerByEmailAsync(email, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("customers/by-loyalty-card/{cardNumber}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> GetCustomerByLoyaltyCard(
        string cardNumber,
        CancellationToken cancellationToken)
    {
        var result = await crmService.GetCustomerByLoyaltyCardAsync(cardNumber, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("customers/{id:int}/transactions")]
    public async Task<ActionResult<ApiResponse<List<CustomerTransactionHistoryDto>>>> GetCustomerTransactions(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await crmService.GetCustomerTransactionsAsync(id, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("customers/{id:int}/loyalty-transactions")]
    public async Task<ActionResult<ApiResponse<List<LoyaltyPointTransactionDto>>>> GetLoyaltyTransactions(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await crmService.GetLoyaltyTransactionsAsync(id, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("customers/{id:int}/notes")]
    public async Task<ActionResult<ApiResponse<List<CustomerNoteDto>>>> GetCustomerNotes(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await crmService.GetCustomerNotesAsync(id, cancellationToken);
        return ToActionResult(result);
    }

    private ActionResult<ApiResponse<T>> ToActionResult<T>(ServiceResult<T> result)
    {
        return result.Success
            ? Ok(ApiResponse<T>.Ok(result.Data!, result.Message))
            : NotFound(ApiResponse<T>.Fail(result.Message, result.Errors));
    }
}
