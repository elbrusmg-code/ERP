using ERP.Core.Entities.POS;

namespace ERP.Business.Common.Interfaces.Repositories.Specific;

public interface IPosRepository
{
    Task<List<CashRegister>> GetCashRegistersByBranchAsync(int branchId, CancellationToken cancellationToken = default);
    Task<CashRegister?> GetCashRegisterDetailsAsync(int cashRegisterId, CancellationToken cancellationToken = default);
    Task<CashShift?> GetOpenShiftAsync(int cashRegisterId, int cashierEmployeeId, CancellationToken cancellationToken = default);
    Task<List<CashShift>> GetOpenShiftsByBranchAsync(int branchId, CancellationToken cancellationToken = default);
    Task<POSReceipt?> GetReceiptDetailsAsync(int receiptId, CancellationToken cancellationToken = default);
    Task<POSReceipt?> GetReceiptByNumberAsync(string receiptNumber, CancellationToken cancellationToken = default);
    Task<List<POSReceipt>> GetReceiptsByShiftAsync(int cashShiftId, CancellationToken cancellationToken = default);
    Task<List<POSReceipt>> GetReceiptsByBranchAndDateAsync(int branchId, DateTime date, CancellationToken cancellationToken = default);
    Task<List<SalesReturn>> GetReturnsByBranchAndDateAsync(int branchId, DateTime date, CancellationToken cancellationToken = default);
    Task<bool> CashRegisterCodeExistsAsync(int branchId, string code, int? excludeCashRegisterId = null, CancellationToken cancellationToken = default);
    Task<bool> ReceiptNumberExistsAsync(string receiptNumber, CancellationToken cancellationToken = default);
}
