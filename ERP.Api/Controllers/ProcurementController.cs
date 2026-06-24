using ERP.Business.Common.Models;
using ERP.Business.Features.Procurement.Dtos;
using ERP.Business.Features.Procurement.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProcurementController(IProcurementService procurementService) : ControllerBase
{
    [HttpGet("suppliers")]
    public async Task<ActionResult<ApiResponse<List<SupplierDto>>>> GetSuppliers(
        CancellationToken cancellationToken)
    {
        var result = await procurementService.GetSuppliersAsync(cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("suppliers/{id:int}")]
    public async Task<ActionResult<ApiResponse<SupplierDto>>> GetSupplierById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await procurementService.GetSupplierByIdAsync(id, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("purchase-orders")]
    public async Task<ActionResult<ApiResponse<List<PurchaseOrderDto>>>> GetPurchaseOrders(
        [FromQuery] int branchId,
        CancellationToken cancellationToken)
    {
        var result = await procurementService.GetPurchaseOrdersAsync(branchId, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("purchase-orders/{id:int}")]
    public async Task<ActionResult<ApiResponse<PurchaseOrderDto>>> GetPurchaseOrderById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await procurementService.GetPurchaseOrderByIdAsync(id, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("purchase-orders/by-number/{orderNumber}")]
    public async Task<ActionResult<ApiResponse<PurchaseOrderDto>>> GetPurchaseOrderByNumber(
        string orderNumber,
        CancellationToken cancellationToken)
    {
        var result = await procurementService.GetPurchaseOrderByNumberAsync(orderNumber, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("purchase-orders/pending")]
    public async Task<ActionResult<ApiResponse<List<PurchaseOrderDto>>>> GetPendingPurchaseOrders(
        [FromQuery] int branchId,
        CancellationToken cancellationToken)
    {
        var result = await procurementService.GetPendingPurchaseOrdersAsync(branchId, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("purchase-orders/{id:int}/goods-receipts")]
    public async Task<ActionResult<ApiResponse<List<GoodsReceiptDto>>>> GetGoodsReceiptsByPurchaseOrder(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await procurementService.GetGoodsReceiptsByPurchaseOrderAsync(id, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("supplier-invoices/{id:int}")]
    public async Task<ActionResult<ApiResponse<SupplierInvoiceDto>>> GetSupplierInvoiceById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await procurementService.GetSupplierInvoiceByIdAsync(id, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("supplier-invoices/unpaid")]
    public async Task<ActionResult<ApiResponse<List<SupplierInvoiceDto>>>> GetUnpaidSupplierInvoices(
        [FromQuery] int branchId,
        CancellationToken cancellationToken)
    {
        var result = await procurementService.GetUnpaidSupplierInvoicesAsync(branchId, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("supplier-invoices/{id:int}/payments")]
    public async Task<ActionResult<ApiResponse<List<SupplierPaymentDto>>>> GetSupplierPaymentsByInvoice(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await procurementService.GetSupplierPaymentsByInvoiceAsync(id, cancellationToken);
        return ToActionResult(result);
    }

    private ActionResult<ApiResponse<T>> ToActionResult<T>(ServiceResult<T> result)
    {
        return result.Success
            ? Ok(ApiResponse<T>.Ok(result.Data!, result.Message))
            : NotFound(ApiResponse<T>.Fail(result.Message, result.Errors));
    }
}
