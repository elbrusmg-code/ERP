using ERP.Business.Common.Models;
using ERP.Business.Features.Finance.Dtos;
using ERP.Business.Features.Finance.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FinanceController(IFinanceService financeService) : ControllerBase
{
    [HttpGet("accounts")]
    public async Task<ActionResult<ApiResponse<List<FinancialAccountDto>>>> GetAccounts(
        [FromQuery] int? branchId,
        CancellationToken cancellationToken)
    {
        var result = await financeService.GetAccountsAsync(branchId, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("accounts/by-code/{accountCode}")]
    public async Task<ActionResult<ApiResponse<FinancialAccountDto>>> GetAccountByCode(
        string accountCode,
        CancellationToken cancellationToken)
    {
        var result = await financeService.GetAccountByCodeAsync(accountCode, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("transactions")]
    public async Task<ActionResult<ApiResponse<List<FinancialTransactionDto>>>> GetTransactions(
        [FromQuery] int branchId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        CancellationToken cancellationToken)
    {
        var result = await financeService.GetTransactionsAsync(branchId, from, to, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("summary")]
    public async Task<ActionResult<ApiResponse<FinanceSummaryDto>>> GetSummary(
        [FromQuery] int branchId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        CancellationToken cancellationToken)
    {
        var result = await financeService.GetSummaryAsync(branchId, from, to, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("expenses")]
    public async Task<ActionResult<ApiResponse<List<ExpenseDto>>>> GetExpenses(
        [FromQuery] int branchId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        CancellationToken cancellationToken)
    {
        var result = await financeService.GetExpensesAsync(branchId, from, to, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("salary-payments")]
    public async Task<ActionResult<ApiResponse<List<SalaryPaymentDto>>>> GetSalaryPayments(
        [FromQuery] int branchId,
        [FromQuery] DateTime? month,
        CancellationToken cancellationToken)
    {
        var result = await financeService.GetSalaryPaymentsAsync(branchId, month, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("daily-sales-summary")]
    public async Task<ActionResult<ApiResponse<DailySalesSummaryDto>>> GetDailySalesSummary(
        [FromQuery] int branchId,
        [FromQuery] DateTime date,
        CancellationToken cancellationToken)
    {
        var result = await financeService.GetDailySalesSummaryAsync(branchId, date, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("supplier-payables/open")]
    public async Task<ActionResult<ApiResponse<List<SupplierPayableDto>>>> GetOpenSupplierPayables(
        [FromQuery] int branchId,
        CancellationToken cancellationToken)
    {
        var result = await financeService.GetOpenSupplierPayablesAsync(branchId, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("journal-entries/{id:int}")]
    public async Task<ActionResult<ApiResponse<JournalEntryDto>>> GetJournalEntryById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await financeService.GetJournalEntryByIdAsync(id, cancellationToken);
        return ToActionResult(result);
    }

    private ActionResult<ApiResponse<T>> ToActionResult<T>(ServiceResult<T> result)
    {
        return result.Success
            ? Ok(ApiResponse<T>.Ok(result.Data!, result.Message))
            : NotFound(ApiResponse<T>.Fail(result.Message, result.Errors));
    }
}
