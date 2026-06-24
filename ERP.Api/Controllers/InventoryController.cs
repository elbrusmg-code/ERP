using ERP.Business.Common.Models;
using ERP.Business.Features.Inventory.Dtos;
using ERP.Business.Features.Inventory.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController(IInventoryService inventoryService) : ControllerBase
{
    [HttpGet("warehouses")]
    public async Task<ActionResult<ApiResponse<List<WarehouseDto>>>> GetWarehouses(
        CancellationToken cancellationToken)
    {
        var result = await inventoryService.GetWarehousesAsync(cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("warehouses/{id:int}")]
    public async Task<ActionResult<ApiResponse<WarehouseDto>>> GetWarehouseById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await inventoryService.GetWarehouseByIdAsync(id, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("stock")]
    public async Task<ActionResult<ApiResponse<List<StockItemDto>>>> GetStock(
        [FromQuery] int? branchId,
        [FromQuery] int? warehouseId,
        CancellationToken cancellationToken)
    {
        var result = await inventoryService.GetStockAsync(branchId, warehouseId, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("stock/product/{productId:int}")]
    public async Task<ActionResult<ApiResponse<List<StockItemDto>>>> GetProductStock(
        int productId,
        CancellationToken cancellationToken)
    {
        var result = await inventoryService.GetProductStockAsync(productId, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("alerts")]
    public async Task<ActionResult<ApiResponse<List<InventoryAlertDto>>>> GetAlerts(
        [FromQuery] int? branchId,
        CancellationToken cancellationToken)
    {
        var result = await inventoryService.GetAlertsAsync(branchId, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("batches")]
    public async Task<ActionResult<ApiResponse<List<ProductBatchDto>>>> GetBatches(
        [FromQuery] int? productId,
        [FromQuery] int? branchId,
        [FromQuery] int? warehouseId,
        CancellationToken cancellationToken)
    {
        var result = await inventoryService.GetBatchesAsync(
            productId,
            branchId,
            warehouseId,
            cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("movements")]
    public async Task<ActionResult<ApiResponse<List<StockMovementDto>>>> GetMovements(
        [FromQuery] int? productId,
        [FromQuery] int? branchId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        CancellationToken cancellationToken)
    {
        var result = await inventoryService.GetMovementsAsync(
            productId,
            branchId,
            from,
            to,
            cancellationToken);
        return ToActionResult(result);
    }

    private ActionResult<ApiResponse<T>> ToActionResult<T>(ServiceResult<T> result)
    {
        return result.Success
            ? Ok(ApiResponse<T>.Ok(result.Data!, result.Message))
            : NotFound(ApiResponse<T>.Fail(result.Message, result.Errors));
    }
}
