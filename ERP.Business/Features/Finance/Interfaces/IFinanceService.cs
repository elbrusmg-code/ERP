using ERP.Business.Common.Models;
using ERP.Business.Features.Finance.Dtos;

namespace ERP.Business.Features.Finance.Interfaces;

public interface IFinanceService
{
    Task<ServiceResult<List<FinancialAccountDto>>> GetAccountsAsync(
        int? branchId = null,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<FinancialAccountDto>> GetAccountByCodeAsync(
        string accountCode,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<List<FinancialTransactionDto>>> GetTransactionsAsync(
        int branchId,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<FinanceSummaryDto>> GetSummaryAsync(
        int branchId,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<List<ExpenseDto>>> GetExpensesAsync(
        int branchId,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<List<SalaryPaymentDto>>> GetSalaryPaymentsAsync(
        int branchId,
        DateTime? month = null,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<DailySalesSummaryDto>> GetDailySalesSummaryAsync(
        int branchId,
        DateTime salesDate,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<List<SupplierPayableDto>>> GetOpenSupplierPayablesAsync(
        int branchId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<JournalEntryDto>> GetJournalEntryByIdAsync(
        int journalEntryId,
        CancellationToken cancellationToken = default);
}
