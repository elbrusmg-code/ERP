using ERP.Business.Common.Models;
using ERP.Business.Features.POS.Dtos;
using ERP.Business.Features.POS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/pos")]
public class PosController(IPosService posService) : ControllerBase
{
    [HttpGet("registers")]
    public async Task<ActionResult<ApiResponse<List<CashRegisterDto>>>> GetRegisters(
        [FromQuery] int? branchId,
        CancellationToken cancellationToken)
    {
        var result = await posService.GetCashRegistersAsync(branchId, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("registers/{id:int}")]
    public async Task<ActionResult<ApiResponse<CashRegisterDto>>> GetRegisterById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await posService.GetCashRegisterByIdAsync(id, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("shifts/open")]
    public async Task<ActionResult<ApiResponse<List<CashShiftDto>>>> GetOpenShifts(
        [FromQuery] int? branchId,
        CancellationToken cancellationToken)
    {
        var result = await posService.GetOpenShiftsAsync(branchId, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("shifts/{id:int}/receipts")]
    public async Task<ActionResult<ApiResponse<List<POSReceiptDto>>>> GetShiftReceipts(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await posService.GetReceiptsByShiftAsync(id, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("receipts/{id:int}")]
    public async Task<ActionResult<ApiResponse<POSReceiptDto>>> GetReceiptById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await posService.GetReceiptByIdAsync(id, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("receipts/by-number/{receiptNumber}")]
    public async Task<ActionResult<ApiResponse<POSReceiptDto>>> GetReceiptByNumber(
        string receiptNumber,
        CancellationToken cancellationToken)
    {
        var result = await posService.GetReceiptByNumberAsync(receiptNumber, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("receipts")]
    public async Task<ActionResult<ApiResponse<List<POSReceiptDto>>>> GetReceipts(
        [FromQuery] int branchId,
        [FromQuery] DateTime? date,
        CancellationToken cancellationToken)
    {
        var result = await posService.GetReceiptsAsync(branchId, date, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("returns")]
    public async Task<ActionResult<ApiResponse<List<SalesReturnDto>>>> GetReturns(
        [FromQuery] int branchId,
        [FromQuery] DateTime? date,
        CancellationToken cancellationToken)
    {
        var result = await posService.GetReturnsAsync(branchId, date, cancellationToken);
        return ToActionResult(result);
    }

    private ActionResult<ApiResponse<T>> ToActionResult<T>(ServiceResult<T> result)
    {
        return result.Success
            ? Ok(ApiResponse<T>.Ok(result.Data!, result.Message))
            : NotFound(ApiResponse<T>.Fail(result.Message, result.Errors));
    }
}
