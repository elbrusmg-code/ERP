using ERP.Business.Common.Interfaces.Repositories.Specific;
using ERP.Core.Entities.POS;
using ERP.Core.Enums;
using ERP.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories.Specific;

public sealed class PosRepository(ERPDbContext context) : IPosRepository
{
    public Task<List<CashRegister>> GetCashRegistersByBranchAsync(int branchId, CancellationToken cancellationToken = default)
    {
        return context.CashRegisters.AsNoTracking()
            .Where(x => x.BranchId == branchId && x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<CashRegister?> GetCashRegisterDetailsAsync(int cashRegisterId, CancellationToken cancellationToken = default)
    {
        return context.CashRegisters.AsNoTracking()
            .AsSplitQuery()
            .Where(x => !x.IsDeleted)
            .Include(x => x.Branch)
            .Include(x => x.CashShifts.Where(shift => !shift.IsDeleted))
            .FirstOrDefaultAsync(x => x.Id == cashRegisterId, cancellationToken);
    }

    public Task<CashShift?> GetOpenShiftAsync(
        int cashRegisterId,
        int cashierEmployeeId,
        CancellationToken cancellationToken = default)
    {
        return context.CashShifts.AsNoTracking()
            .Include(x => x.CashRegister)
            .Include(x => x.CashierEmployee)
            .FirstOrDefaultAsync(
                x => x.CashRegisterId == cashRegisterId &&
                     x.CashierEmployeeId == cashierEmployeeId &&
                     x.Status == CashShiftStatus.Open &&
                     !x.IsDeleted,
                cancellationToken);
    }

    public Task<List<CashShift>> GetOpenShiftsByBranchAsync(int branchId, CancellationToken cancellationToken = default)
    {
        return context.CashShifts.AsNoTracking()
            .Where(x => x.BranchId == branchId && x.Status == CashShiftStatus.Open && !x.IsDeleted)
            .Include(x => x.CashRegister)
            .Include(x => x.CashierEmployee)
            .OrderBy(x => x.OpenedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<POSReceipt?> GetReceiptDetailsAsync(int receiptId, CancellationToken cancellationToken = default)
    {
        return ReceiptDetailsQuery().FirstOrDefaultAsync(x => x.Id == receiptId, cancellationToken);
    }

    public Task<POSReceipt?> GetReceiptByNumberAsync(string receiptNumber, CancellationToken cancellationToken = default)
    {
        return ReceiptDetailsQuery().FirstOrDefaultAsync(x => x.ReceiptNumber == receiptNumber, cancellationToken);
    }

    public Task<List<POSReceipt>> GetReceiptsByShiftAsync(int cashShiftId, CancellationToken cancellationToken = default)
    {
        return context.POSReceipts.AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.CashShiftId == cashShiftId && !x.IsDeleted)
            .Include(x => x.Items)
            .Include(x => x.Payments)
            .OrderByDescending(x => x.ReceiptDate)
            .ToListAsync(cancellationToken);
    }

    public Task<List<POSReceipt>> GetReceiptsByBranchAndDateAsync(
        int branchId,
        DateTime date,
        CancellationToken cancellationToken = default)
    {
        var start = date.Date;
        var end = start.AddDays(1);

        return context.POSReceipts.AsNoTracking()
            .Where(x => x.BranchId == branchId &&
                        x.ReceiptDate >= start &&
                        x.ReceiptDate < end &&
                        !x.IsDeleted)
            .Include(x => x.CashRegister)
            .Include(x => x.CashierEmployee)
            .OrderByDescending(x => x.ReceiptDate)
            .ToListAsync(cancellationToken);
    }

    public Task<List<SalesReturn>> GetReturnsByBranchAndDateAsync(
        int branchId,
        DateTime date,
        CancellationToken cancellationToken = default)
    {
        var start = date.Date;
        var end = start.AddDays(1);

        return context.SalesReturns.AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.BranchId == branchId &&
                        x.ReturnDate >= start &&
                        x.ReturnDate < end &&
                        !x.IsDeleted)
            .Include(x => x.OriginalReceipt)
            .Include(x => x.Items)
                .ThenInclude(x => x.Product)
            .OrderByDescending(x => x.ReturnDate)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> CashRegisterCodeExistsAsync(
        int branchId,
        string code,
        int? excludeCashRegisterId = null,
        CancellationToken cancellationToken = default)
    {
        return context.CashRegisters.AsNoTracking().AnyAsync(
            x => x.BranchId == branchId &&
                 x.Code == code &&
                 !x.IsDeleted &&
                 (!excludeCashRegisterId.HasValue || x.Id != excludeCashRegisterId.Value),
            cancellationToken);
    }

    public Task<bool> ReceiptNumberExistsAsync(string receiptNumber, CancellationToken cancellationToken = default)
    {
        return context.POSReceipts.AsNoTracking()
            .AnyAsync(x => x.ReceiptNumber == receiptNumber && !x.IsDeleted, cancellationToken);
    }

    private IQueryable<POSReceipt> ReceiptDetailsQuery()
    {
        return context.POSReceipts.AsNoTracking()
            .AsSplitQuery()
            .Where(x => !x.IsDeleted)
            .Include(x => x.Branch)
            .Include(x => x.CashRegister)
            .Include(x => x.CashShift)
            .Include(x => x.CashierEmployee)
            .Include(x => x.Items)
                .ThenInclude(x => x.Product)
            .Include(x => x.Payments)
                .ThenInclude(x => x.TerminalTransactions)
            .Include(x => x.Returns.Where(salesReturn => !salesReturn.IsDeleted));
    }
}
