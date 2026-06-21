using ERP.Business.Common.Interfaces.Repositories.Specific;
using ERP.Core.Entities.Finance;
using ERP.Core.Enums;
using ERP.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories.Specific;

public sealed class FinanceRepository(ERPDbContext context) : IFinanceRepository
{
    public Task<List<FinancialAccount>> GetActiveAccountsAsync(
        int? branchId = null,
        CancellationToken cancellationToken = default)
    {
        return context.FinancialAccounts.AsNoTracking()
            .Where(x => x.IsActive &&
                        x.Status == FinancialAccountStatus.Active &&
                        !x.IsDeleted &&
                        (!branchId.HasValue || !x.BranchId.HasValue || x.BranchId == branchId.Value))
            .OrderBy(x => x.AccountCode)
            .ToListAsync(cancellationToken);
    }

    public Task<FinancialAccount?> GetAccountByCodeAsync(
        string accountCode,
        CancellationToken cancellationToken = default)
    {
        return context.FinancialAccounts.AsNoTracking()
            .FirstOrDefaultAsync(x => x.AccountCode == accountCode && !x.IsDeleted, cancellationToken);
    }

    public Task<List<FinancialTransaction>> GetTransactionsByBranchAsync(
        int branchId,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken cancellationToken = default)
    {
        return FilterTransactions(branchId, from, to)
            .Include(x => x.FinancialAccount)
            .OrderByDescending(x => x.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetBranchRevenueAsync(
        int branchId,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken cancellationToken = default)
    {
        return await FilterTransactions(branchId, from, to)
            .Where(x => x.Type == FinancialTransactionType.Revenue)
            .Select(x => (decimal?)x.Amount)
            .SumAsync(cancellationToken) ?? 0m;
    }

    public async Task<decimal> GetBranchExpensesAsync(
        int branchId,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken cancellationToken = default)
    {
        var expenseTypes = new[]
        {
            FinancialTransactionType.Expense,
            FinancialTransactionType.SalaryExpense,
            FinancialTransactionType.SupplierPayment,
            FinancialTransactionType.CashOut,
            FinancialTransactionType.Refund
        };

        return await FilterTransactions(branchId, from, to)
            .Where(x => expenseTypes.Contains(x.Type))
            .Select(x => (decimal?)x.Amount)
            .SumAsync(cancellationToken) ?? 0m;
    }

    public Task<List<Expense>> GetExpensesByBranchAsync(
        int branchId,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.Expenses.AsNoTracking()
            .Where(x => x.BranchId == branchId && !x.IsDeleted);

        if (from.HasValue)
        {
            query = query.Where(x => x.ExpenseDate >= from.Value);
        }

        if (to.HasValue)
        {
            query = query.Where(x => x.ExpenseDate <= to.Value);
        }

        return query.OrderByDescending(x => x.ExpenseDate).ToListAsync(cancellationToken);
    }

    public Task<List<SalaryPayment>> GetSalaryPaymentsByBranchAsync(
        int branchId,
        DateTime? month = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.SalaryPayments.AsNoTracking()
            .Where(x => x.BranchId == branchId && !x.IsDeleted);

        if (month.HasValue)
        {
            var monthStart = new DateTime(month.Value.Year, month.Value.Month, 1);
            var nextMonth = monthStart.AddMonths(1);
            query = query.Where(x => x.SalaryMonth >= monthStart && x.SalaryMonth < nextMonth);
        }

        return query
            .Include(x => x.Employee)
            .OrderByDescending(x => x.SalaryMonth)
            .ToListAsync(cancellationToken);
    }

    public Task<DailySalesSummary?> GetDailySalesSummaryAsync(
        int branchId,
        DateTime salesDate,
        CancellationToken cancellationToken = default)
    {
        var start = salesDate.Date;
        var end = start.AddDays(1);

        return context.DailySalesSummaries.AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.BranchId == branchId &&
                        x.SalesDate >= start &&
                        x.SalesDate < end &&
                        !x.IsDeleted)
            .Include(x => x.Shifts)
                .ThenInclude(x => x.CashShift)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<List<SupplierPayable>> GetOpenSupplierPayablesAsync(
        int branchId,
        CancellationToken cancellationToken = default)
    {
        var openStatuses = new[]
        {
            SupplierPayableStatus.Open,
            SupplierPayableStatus.PartiallyPaid,
            SupplierPayableStatus.Overdue
        };

        return context.SupplierPayables.AsNoTracking()
            .Where(x => x.BranchId == branchId && openStatuses.Contains(x.Status) && !x.IsDeleted)
            .Include(x => x.Supplier)
            .Include(x => x.SupplierInvoice)
            .OrderBy(x => x.DueDate)
            .ToListAsync(cancellationToken);
    }

    public Task<JournalEntry?> GetJournalEntryDetailsAsync(
        int journalEntryId,
        CancellationToken cancellationToken = default)
    {
        return context.JournalEntries.AsNoTracking()
            .AsSplitQuery()
            .Where(x => !x.IsDeleted)
            .Include(x => x.Branch)
            .Include(x => x.Lines)
                .ThenInclude(x => x.FinancialAccount)
            .FirstOrDefaultAsync(x => x.Id == journalEntryId, cancellationToken);
    }

    private IQueryable<FinancialTransaction> FilterTransactions(
        int branchId,
        DateTime? from,
        DateTime? to)
    {
        var query = context.FinancialTransactions.AsNoTracking()
            .Where(x => x.BranchId == branchId);

        if (from.HasValue)
        {
            query = query.Where(x => x.TransactionDate >= from.Value);
        }

        if (to.HasValue)
        {
            query = query.Where(x => x.TransactionDate <= to.Value);
        }

        return query;
    }
}
