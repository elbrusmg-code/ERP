using ERP.Core.Entities.Inventory;

namespace ERP.Business.Common.Interfaces.Repositories.Specific;

public interface IInventoryRepository
{
    Task<List<Warehouse>> GetWarehousesByBranchAsync(int branchId, CancellationToken cancellationToken = default);
    Task<Warehouse?> GetWarehouseDetailsAsync(int warehouseId, CancellationToken cancellationToken = default);
    Task<StockItem?> GetStockItemAsync(int productId, int branchId, int warehouseId, CancellationToken cancellationToken = default);
    Task<List<StockItem>> GetStockByBranchAsync(int branchId, CancellationToken cancellationToken = default);
    Task<List<StockItem>> GetLowStockItemsAsync(int branchId, CancellationToken cancellationToken = default);
    Task<List<ProductBatch>> GetActiveBatchesAsync(int productId, int branchId, int warehouseId, CancellationToken cancellationToken = default);
    Task<ProductBatch?> GetNextExpiringBatchAsync(int productId, int branchId, int warehouseId, CancellationToken cancellationToken = default);
    Task<List<InventoryAlert>> GetOpenAlertsByBranchAsync(int branchId, CancellationToken cancellationToken = default);
    Task<List<StockMovement>> GetStockMovementsAsync(int productId, int branchId, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default);
    Task<bool> WarehouseCodeExistsAsync(int branchId, string code, int? excludeWarehouseId = null, CancellationToken cancellationToken = default);
}
