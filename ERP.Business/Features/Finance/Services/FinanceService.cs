using ERP.Business.Common.Interfaces.Repositories;
using ERP.Business.Common.Interfaces.Repositories.Specific;
using ERP.Business.Common.Models;
using ERP.Business.Features.Finance.Dtos;
using ERP.Business.Features.Finance.Interfaces;
using ERP.Core.Entities.Finance;
using ERP.Core.Entities.HR;
using ERP.Core.Entities.Organization;
using ERP.Core.Entities.Procurement;

namespace ERP.Business.Features.Finance.Services;

public sealed class FinanceService(
    IFinanceRepository financeRepository,
    IUnitOfWork unitOfWork) : IFinanceService
{
    public async Task<ServiceResult<List<FinancialAccountDto>>> GetAccountsAsync(
        int? branchId = null,
        CancellationToken cancellationToken = default)
    {
        var accounts = await financeRepository.GetActiveAccountsAsync(branchId, cancellationToken);
        var branches = await GetBranchesAsync(cancellationToken);

        var data = accounts
            .OrderBy(x => x.AccountCode)
            .Select(x => MapFinancialAccount(x, branches))
            .ToList();

        return ServiceResult<List<FinancialAccountDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<FinancialAccountDto>> GetAccountByCodeAsync(
        string accountCode,
        CancellationToken cancellationToken = default)
    {
        var account = await financeRepository.GetAccountByCodeAsync(accountCode, cancellationToken);

        if (account is null)
        {
            return ServiceResult<FinancialAccountDto>.Failure("Financial account not found.");
        }

        var branches = await GetBranchesAsync(cancellationToken);
        return ServiceResult<FinancialAccountDto>.SuccessResult(MapFinancialAccount(account, branches));
    }

    public async Task<ServiceResult<List<FinancialTransactionDto>>> GetTransactionsAsync(
        int branchId,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken cancellationToken = default)
    {
        if (branchId <= 0)
        {
            return ServiceResult<List<FinancialTransactionDto>>.Failure("BranchId is required.");
        }

        var transactions = await financeRepository.GetTransactionsByBranchAsync(
            branchId,
            from,
            to,
            cancellationToken);
        var branches = await GetBranchesAsync(cancellationToken);

        var data = transactions
            .OrderByDescending(x => x.TransactionDate)
            .Select(x => MapFinancialTransaction(x, branches))
            .ToList();

        return ServiceResult<List<FinancialTransactionDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<FinanceSummaryDto>> GetSummaryAsync(
        int branchId,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken cancellationToken = default)
    {
        if (branchId <= 0)
        {
            return ServiceResult<FinanceSummaryDto>.Failure("BranchId is required.");
        }

        var revenue = await financeRepository.GetBranchRevenueAsync(
            branchId,
            from,
            to,
            cancellationToken);
        var expenses = await financeRepository.GetBranchExpensesAsync(
            branchId,
            from,
            to,
            cancellationToken);

        return ServiceResult<FinanceSummaryDto>.SuccessResult(new FinanceSummaryDto
        {
            BranchId = branchId,
            From = from,
            To = to,
            TotalRevenue = revenue,
            TotalExpenses = expenses,
            NetProfit = revenue - expenses
        });
    }

    public async Task<ServiceResult<List<ExpenseDto>>> GetExpensesAsync(
        int branchId,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken cancellationToken = default)
    {
        if (branchId <= 0)
        {
            return ServiceResult<List<ExpenseDto>>.Failure("BranchId is required.");
        }

        var expenses = await financeRepository.GetExpensesByBranchAsync(
            branchId,
            from,
            to,
            cancellationToken);
        var branches = await GetBranchesAsync(cancellationToken);

        var data = expenses
            .OrderByDescending(x => x.ExpenseDate)
            .Select(x => MapExpense(x, branches))
            .ToList();

        return ServiceResult<List<ExpenseDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<List<SalaryPaymentDto>>> GetSalaryPaymentsAsync(
        int branchId,
        DateTime? month = null,
        CancellationToken cancellationToken = default)
    {
        if (branchId <= 0)
        {
            return ServiceResult<List<SalaryPaymentDto>>.Failure("BranchId is required.");
        }

        var payments = await financeRepository.GetSalaryPaymentsByBranchAsync(
            branchId,
            month,
            cancellationToken);
        var branches = await GetBranchesAsync(cancellationToken);
        var employees = await GetEmployeesAsync(cancellationToken);

        var data = payments
            .OrderByDescending(x => x.SalaryMonth)
            .ThenByDescending(x => x.PaymentDate)
            .Select(x => MapSalaryPayment(x, branches, employees))
            .ToList();

        return ServiceResult<List<SalaryPaymentDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<DailySalesSummaryDto>> GetDailySalesSummaryAsync(
        int branchId,
        DateTime salesDate,
        CancellationToken cancellationToken = default)
    {
        if (branchId <= 0)
        {
            return ServiceResult<DailySalesSummaryDto>.Failure("BranchId is required.");
        }

        var summary = await financeRepository.GetDailySalesSummaryAsync(
            branchId,
            salesDate,
            cancellationToken);

        if (summary is null)
        {
            return ServiceResult<DailySalesSummaryDto>.Failure("Daily sales summary not found.");
        }

        var branches = await GetBranchesAsync(cancellationToken);
        return ServiceResult<DailySalesSummaryDto>.SuccessResult(MapDailySalesSummary(summary, branches));
    }

    public async Task<ServiceResult<List<SupplierPayableDto>>> GetOpenSupplierPayablesAsync(
        int branchId,
        CancellationToken cancellationToken = default)
    {
        if (branchId <= 0)
        {
            return ServiceResult<List<SupplierPayableDto>>.Failure("BranchId is required.");
        }

        var payables = await financeRepository.GetOpenSupplierPayablesAsync(branchId, cancellationToken);
        var branches = await GetBranchesAsync(cancellationToken);
        var suppliers = await GetSuppliersAsync(cancellationToken);

        var data = payables
            .OrderBy(x => x.DueDate)
            .ThenBy(x => x.CreatedDate)
            .Select(x => MapSupplierPayable(x, branches, suppliers))
            .ToList();

        return ServiceResult<List<SupplierPayableDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<JournalEntryDto>> GetJournalEntryByIdAsync(
        int journalEntryId,
        CancellationToken cancellationToken = default)
    {
        var journalEntry = await financeRepository.GetJournalEntryDetailsAsync(
            journalEntryId,
            cancellationToken);

        if (journalEntry is null)
        {
            return ServiceResult<JournalEntryDto>.Failure("Journal entry not found.");
        }

        var branches = await GetBranchesAsync(cancellationToken);
        return ServiceResult<JournalEntryDto>.SuccessResult(MapJournalEntry(journalEntry, branches));
    }

    private static FinancialAccountDto MapFinancialAccount(
        FinancialAccount account,
        IReadOnlyDictionary<int, Branch> branches)
    {
        return new FinancialAccountDto
        {
            Id = account.Id,
            AccountCode = account.AccountCode,
            Name = account.Name,
            Description = account.Description,
            Type = account.Type.ToString(),
            Status = account.Status.ToString(),
            BranchId = account.BranchId,
            BranchName = account.BranchId.HasValue
                ? account.Branch?.Name ?? branches.GetValueOrDefault(account.BranchId.Value)?.Name
                : null,
            IsSystemAccount = account.IsSystemAccount,
            IsActive = account.IsActive
        };
    }

    private static FinancialTransactionDto MapFinancialTransaction(
        FinancialTransaction transaction,
        IReadOnlyDictionary<int, Branch> branches)
    {
        return new FinancialTransactionDto
        {
            Id = transaction.Id,
            TransactionNumber = transaction.TransactionNumber,
            Type = transaction.Type.ToString(),
            BranchId = transaction.BranchId,
            BranchName = transaction.Branch?.Name ??
                         branches.GetValueOrDefault(transaction.BranchId)?.Name ??
                         string.Empty,
            FinancialAccountId = transaction.FinancialAccountId,
            FinancialAccountName = transaction.FinancialAccount?.Name,
            PaymentMethod = transaction.PaymentMethod?.ToString(),
            Amount = transaction.Amount,
            TransactionDate = transaction.TransactionDate,
            SourceModule = transaction.SourceModule,
            ReferenceNumber = transaction.ReferenceNumber,
            Description = transaction.Description,
            CreatedByUserId = transaction.CreatedByUserId
        };
    }

    private static ExpenseDto MapExpense(
        Expense expense,
        IReadOnlyDictionary<int, Branch> branches)
    {
        return new ExpenseDto
        {
            Id = expense.Id,
            ExpenseNumber = expense.ExpenseNumber,
            BranchId = expense.BranchId,
            BranchName = expense.Branch?.Name ??
                         branches.GetValueOrDefault(expense.BranchId)?.Name ??
                         string.Empty,
            Category = expense.Category.ToString(),
            Method = expense.Method.ToString(),
            Amount = expense.Amount,
            ExpenseDate = expense.ExpenseDate,
            Description = expense.Description,
            ReferenceNumber = expense.ReferenceNumber,
            ApprovedBy = expense.ApprovedBy,
            ApprovedAt = expense.ApprovedAt,
            IsApproved = expense.IsApproved
        };
    }

    private static SalaryPaymentDto MapSalaryPayment(
        SalaryPayment payment,
        IReadOnlyDictionary<int, Branch> branches,
        IReadOnlyDictionary<int, Employee> employees)
    {
        return new SalaryPaymentDto
        {
            Id = payment.Id,
            PaymentNumber = payment.PaymentNumber,
            BranchId = payment.BranchId,
            BranchName = payment.Branch?.Name ??
                         branches.GetValueOrDefault(payment.BranchId)?.Name ??
                         string.Empty,
            EmployeeId = payment.EmployeeId,
            EmployeeName = GetEmployeeName(payment.Employee ?? employees.GetValueOrDefault(payment.EmployeeId)),
            Status = payment.Status.ToString(),
            Method = payment.Method.ToString(),
            GrossAmount = payment.GrossAmount,
            DeductionAmount = payment.DeductionAmount,
            NetAmount = payment.NetAmount,
            SalaryMonth = payment.SalaryMonth,
            PaymentDate = payment.PaymentDate,
            ReferenceNumber = payment.ReferenceNumber,
            Note = payment.Note
        };
    }

    private static DailySalesSummaryDto MapDailySalesSummary(
        DailySalesSummary summary,
        IReadOnlyDictionary<int, Branch> branches)
    {
        return new DailySalesSummaryDto
        {
            Id = summary.Id,
            SummaryNumber = summary.SummaryNumber,
            BranchId = summary.BranchId,
            BranchName = summary.Branch?.Name ??
                         branches.GetValueOrDefault(summary.BranchId)?.Name ??
                         string.Empty,
            SalesDate = summary.SalesDate,
            Status = summary.Status.ToString(),
            TotalSales = summary.TotalSales,
            TotalCashSales = summary.TotalCashSales,
            TotalCardSales = summary.TotalCardSales,
            TotalMixedSales = summary.TotalMixedSales,
            TotalRefunds = summary.TotalRefunds,
            NetSales = summary.NetSales,
            ReceiptCount = summary.ReceiptCount,
            ReturnCount = summary.ReturnCount,
            CalculatedBy = summary.CalculatedBy,
            CalculatedAt = summary.CalculatedAt,
            ApprovedBy = summary.ApprovedBy,
            ApprovedAt = summary.ApprovedAt,
            Shifts = summary.Shifts
                .OrderBy(x => x.Id)
                .Select(MapDailySalesSummaryShift)
                .ToList()
        };
    }

    private static DailySalesSummaryShiftDto MapDailySalesSummaryShift(
        DailySalesSummaryShift shift)
    {
        return new DailySalesSummaryShiftDto
        {
            Id = shift.Id,
            CashShiftId = shift.CashShiftId,
            TotalSales = shift.TotalSales,
            CashSales = shift.CashSales,
            CardSales = shift.CardSales,
            MixedSales = shift.MixedSales,
            Refunds = shift.Refunds,
            ExpectedCash = shift.ExpectedCash,
            ActualCash = shift.ActualCash,
            CashDifference = shift.CashDifference
        };
    }

    private static SupplierPayableDto MapSupplierPayable(
        SupplierPayable payable,
        IReadOnlyDictionary<int, Branch> branches,
        IReadOnlyDictionary<int, Supplier> suppliers)
    {
        return new SupplierPayableDto
        {
            Id = payable.Id,
            PayableNumber = payable.PayableNumber,
            BranchId = payable.BranchId,
            BranchName = payable.Branch?.Name ??
                         branches.GetValueOrDefault(payable.BranchId)?.Name ??
                         string.Empty,
            SupplierId = payable.SupplierId,
            SupplierName = payable.Supplier?.CompanyName ??
                           suppliers.GetValueOrDefault(payable.SupplierId)?.CompanyName ??
                           string.Empty,
            SupplierInvoiceId = payable.SupplierInvoiceId,
            Status = payable.Status.ToString(),
            OriginalAmount = payable.OriginalAmount,
            PaidAmount = payable.PaidAmount,
            RemainingAmount = payable.RemainingAmount,
            CreatedDate = payable.CreatedDate,
            DueDate = payable.DueDate,
            Note = payable.Note
        };
    }

    private static JournalEntryDto MapJournalEntry(
        JournalEntry journalEntry,
        IReadOnlyDictionary<int, Branch> branches)
    {
        return new JournalEntryDto
        {
            Id = journalEntry.Id,
            EntryNumber = journalEntry.EntryNumber,
            Status = journalEntry.Status.ToString(),
            Source = journalEntry.Source.ToString(),
            BranchId = journalEntry.BranchId,
            BranchName = journalEntry.BranchId.HasValue
                ? journalEntry.Branch?.Name ?? branches.GetValueOrDefault(journalEntry.BranchId.Value)?.Name
                : null,
            EntryDate = journalEntry.EntryDate,
            Description = journalEntry.Description,
            ReferenceNumber = journalEntry.ReferenceNumber,
            PostedBy = journalEntry.PostedBy,
            PostedAt = journalEntry.PostedAt,
            Lines = journalEntry.Lines
                .OrderBy(x => x.Id)
                .Select(MapJournalEntryLine)
                .ToList()
        };
    }

    private static JournalEntryLineDto MapJournalEntryLine(JournalEntryLine line)
    {
        return new JournalEntryLineDto
        {
            Id = line.Id,
            FinancialAccountId = line.FinancialAccountId,
            FinancialAccountCode = line.FinancialAccount?.AccountCode ?? string.Empty,
            FinancialAccountName = line.FinancialAccount?.Name ?? string.Empty,
            Debit = line.Debit,
            Credit = line.Credit,
            Description = line.Description
        };
    }

    private async Task<Dictionary<int, Branch>> GetBranchesAsync(
        CancellationToken cancellationToken)
    {
        return (await unitOfWork.Repository<Branch>().ListAsync(
                x => !x.IsDeleted,
                cancellationToken))
            .ToDictionary(x => x.Id);
    }

    private async Task<Dictionary<int, Employee>> GetEmployeesAsync(
        CancellationToken cancellationToken)
    {
        return (await unitOfWork.Repository<Employee>().ListAsync(
                x => !x.IsDeleted,
                cancellationToken))
            .ToDictionary(x => x.Id);
    }

    private async Task<Dictionary<int, Supplier>> GetSuppliersAsync(
        CancellationToken cancellationToken)
    {
        return (await unitOfWork.Repository<Supplier>().ListAsync(
                x => !x.IsDeleted,
                cancellationToken))
            .ToDictionary(x => x.Id);
    }

    private static string GetEmployeeName(Employee? employee)
    {
        if (employee is null)
        {
            return string.Empty;
        }

        var name = $"{employee.FirstName} {employee.LastName}".Trim();
        return string.IsNullOrWhiteSpace(name) ? employee.EmployeeCode : name;
    }
}
