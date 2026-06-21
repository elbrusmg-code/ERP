using ERP.Business.Common.Interfaces.Repositories.Specific;
using ERP.Core.Entities.Inventory;
using ERP.Core.Enums;
using ERP.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories.Specific;

public sealed class InventoryRepository(ERPDbContext context) : IInventoryRepository
{
    public Task<List<Warehouse>> GetWarehousesByBranchAsync(int branchId, CancellationToken cancellationToken = default)
    {
        return context.Warehouses.AsNoTracking()
            .Where(x => x.BranchId == branchId && x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<Warehouse?> GetWarehouseDetailsAsync(int warehouseId, CancellationToken cancellationToken = default)
    {
        return context.Warehouses.AsNoTracking()
            .AsSplitQuery()
            .Where(x => !x.IsDeleted)
            .Include(x => x.Branch)
            .Include(x => x.StockItems)
                .ThenInclude(x => x.Product)
            .Include(x => x.ProductBatches.Where(batch => !batch.IsDeleted))
                .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == warehouseId, cancellationToken);
    }

    public Task<StockItem?> GetStockItemAsync(
        int productId,
        int branchId,
        int warehouseId,
        CancellationToken cancellationToken = default)
    {
        return context.StockItems.AsNoTracking()
            .Include(x => x.Product)
            .Include(x => x.Warehouse)
            .FirstOrDefaultAsync(
                x => x.ProductId == productId && x.BranchId == branchId && x.WarehouseId == warehouseId,
                cancellationToken);
    }

    public Task<List<StockItem>> GetStockByBranchAsync(int branchId, CancellationToken cancellationToken = default)
    {
        return context.StockItems.AsNoTracking()
            .Where(x => x.BranchId == branchId)
            .Include(x => x.Product)
                .ThenInclude(x => x!.ProductCategory)
            .Include(x => x.Warehouse)
            .OrderBy(x => x.Product!.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<List<StockItem>> GetLowStockItemsAsync(int branchId, CancellationToken cancellationToken = default)
    {
        return context.StockItems.AsNoTracking()
            .Where(x => x.BranchId == branchId &&
                        (x.Quantity <= x.ReorderLevel || x.Quantity <= x.MinimumStockLevel))
            .Include(x => x.Product)
            .Include(x => x.Warehouse)
            .OrderBy(x => x.Quantity)
            .ToListAsync(cancellationToken);
    }

    public Task<List<ProductBatch>> GetActiveBatchesAsync(
        int productId,
        int branchId,
        int warehouseId,
        CancellationToken cancellationToken = default)
    {
        return context.ProductBatches.AsNoTracking()
            .Where(x => x.ProductId == productId &&
                        x.BranchId == branchId &&
                        x.WarehouseId == warehouseId &&
                        x.CurrentQuantity > 0 &&
                        x.IsActive &&
                        !x.IsDeleted)
            .OrderBy(x => x.ExpiryDate)
            .ThenBy(x => x.BatchNumber)
            .ToListAsync(cancellationToken);
    }

    public Task<ProductBatch?> GetNextExpiringBatchAsync(
        int productId,
        int branchId,
        int warehouseId,
        CancellationToken cancellationToken = default)
    {
        return context.ProductBatches.AsNoTracking()
            .Where(x => x.ProductId == productId &&
                        x.BranchId == branchId &&
                        x.WarehouseId == warehouseId &&
                        x.ExpiryDate.HasValue &&
                        x.CurrentQuantity > 0 &&
                        x.IsActive &&
                        !x.IsDeleted)
            .OrderBy(x => x.ExpiryDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<List<InventoryAlert>> GetOpenAlertsByBranchAsync(int branchId, CancellationToken cancellationToken = default)
    {
        return context.InventoryAlerts.AsNoTracking()
            .Where(x => x.BranchId == branchId && x.Status == InventoryAlertStatus.Open)
            .Include(x => x.Product)
            .Include(x => x.Warehouse)
            .OrderBy(x => x.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public Task<List<StockMovement>> GetStockMovementsAsync(
        int productId,
        int branchId,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.StockMovements.AsNoTracking()
            .Where(x => x.ProductId == productId && x.BranchId == branchId);

        if (from.HasValue)
        {
            query = query.Where(x => x.MovementDate >= from.Value);
        }

        if (to.HasValue)
        {
            query = query.Where(x => x.MovementDate <= to.Value);
        }

        return query
            .Include(x => x.Warehouse)
            .Include(x => x.ProductBatch)
            .OrderByDescending(x => x.MovementDate)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> WarehouseCodeExistsAsync(
        int branchId,
        string code,
        int? excludeWarehouseId = null,
        CancellationToken cancellationToken = default)
    {
        return context.Warehouses.AsNoTracking().AnyAsync(
            x => x.BranchId == branchId &&
                 x.Code == code &&
                 !x.IsDeleted &&
                 (!excludeWarehouseId.HasValue || x.Id != excludeWarehouseId.Value),
            cancellationToken);
    }
}
