using ERP.Business.Common.Models;
using ERP.Business.Features.POS.Dtos;

namespace ERP.Business.Features.POS.Interfaces;

public interface IPosService
{
    Task<ServiceResult<List<CashRegisterDto>>> GetCashRegistersAsync(
        int? branchId = null,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<CashRegisterDto>> GetCashRegisterByIdAsync(
        int cashRegisterId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<List<CashShiftDto>>> GetOpenShiftsAsync(
        int? branchId = null,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<List<POSReceiptDto>>> GetReceiptsByShiftAsync(
        int cashShiftId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<POSReceiptDto>> GetReceiptByIdAsync(
        int receiptId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<POSReceiptDto>> GetReceiptByNumberAsync(
        string receiptNumber,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<List<POSReceiptDto>>> GetReceiptsAsync(
        int branchId,
        DateTime? date = null,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<List<SalesReturnDto>>> GetReturnsAsync(
        int branchId,
        DateTime? date = null,
        CancellationToken cancellationToken = default);
}
