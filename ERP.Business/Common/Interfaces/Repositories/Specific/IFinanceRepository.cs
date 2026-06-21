using ERP.Core.Entities.Finance;

namespace ERP.Business.Common.Interfaces.Repositories.Specific;

public interface IFinanceRepository
{
    Task<List<FinancialAccount>> GetActiveAccountsAsync(int? branchId = null, CancellationToken cancellationToken = default);
    Task<FinancialAccount?> GetAccountByCodeAsync(string accountCode, CancellationToken cancellationToken = default);
    Task<List<FinancialTransaction>> GetTransactionsByBranchAsync(int branchId, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default);
    Task<decimal> GetBranchRevenueAsync(int branchId, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default);
    Task<decimal> GetBranchExpensesAsync(int branchId, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default);
    Task<List<Expense>> GetExpensesByBranchAsync(int branchId, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default);
    Task<List<SalaryPayment>> GetSalaryPaymentsByBranchAsync(int branchId, DateTime? month = null, CancellationToken cancellationToken = default);
    Task<DailySalesSummary?> GetDailySalesSummaryAsync(int branchId, DateTime salesDate, CancellationToken cancellationToken = default);
    Task<List<SupplierPayable>> GetOpenSupplierPayablesAsync(int branchId, CancellationToken cancellationToken = default);
    Task<JournalEntry?> GetJournalEntryDetailsAsync(int journalEntryId, CancellationToken cancellationToken = default);
}
