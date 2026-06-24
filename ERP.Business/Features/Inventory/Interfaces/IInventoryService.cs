using ERP.Business.Common.Models;
using ERP.Business.Features.Inventory.Dtos;

namespace ERP.Business.Features.Inventory.Interfaces;

public interface IInventoryService
{
    Task<ServiceResult<List<WarehouseDto>>> GetWarehousesAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<WarehouseDto>> GetWarehouseByIdAsync(int warehouseId, CancellationToken cancellationToken = default);
    Task<ServiceResult<List<StockItemDto>>> GetStockAsync(int? branchId = null, int? warehouseId = null, CancellationToken cancellationToken = default);
    Task<ServiceResult<List<StockItemDto>>> GetProductStockAsync(int productId, CancellationToken cancellationToken = default);
    Task<ServiceResult<List<InventoryAlertDto>>> GetAlertsAsync(int? branchId = null, CancellationToken cancellationToken = default);
    Task<ServiceResult<List<ProductBatchDto>>> GetBatchesAsync(int? productId = null, int? branchId = null, int? warehouseId = null, CancellationToken cancellationToken = default);
    Task<ServiceResult<List<StockMovementDto>>> GetMovementsAsync(int? productId = null, int? branchId = null, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default);
}
