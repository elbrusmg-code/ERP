using ERP.Business.Common.Interfaces.Repositories;
using ERP.Business.Common.Models;
using ERP.Business.Features.Inventory.Dtos;
using ERP.Business.Features.Inventory.Interfaces;
using ERP.Core.Entities.Catalog;
using ERP.Core.Entities.Inventory;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Business.Features.Inventory.Services;

public sealed class InventoryService(IUnitOfWork unitOfWork) : IInventoryService
{
    public async Task<ServiceResult<List<WarehouseDto>>> GetWarehousesAsync(
        CancellationToken cancellationToken = default)
    {
        var warehouses = await unitOfWork.Repository<Warehouse>().ListAsync(
            x => !x.IsDeleted,
            cancellationToken);
        var stockItems = await unitOfWork.Repository<StockItem>().ListAsync(cancellationToken);
        var branches = await GetBranchesAsync(cancellationToken);

        var data = warehouses
            .OrderBy(x => x.BranchId)
            .ThenBy(x => x.Name)
            .Select(x => MapWarehouse(x, branches, stockItems))
            .ToList();

        return ServiceResult<List<WarehouseDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<WarehouseDto>> GetWarehouseByIdAsync(
        int warehouseId,
        CancellationToken cancellationToken = default)
    {
        var warehouse = await unitOfWork.Repository<Warehouse>()
            .GetByIdAsync(warehouseId, cancellationToken);

        if (warehouse is null || warehouse.IsDeleted)
        {
            return ServiceResult<WarehouseDto>.Failure("Warehouse not found.");
        }

        var stockItems = await unitOfWork.Repository<StockItem>().ListAsync(
            x => x.WarehouseId == warehouseId,
            cancellationToken);
        var branches = await GetBranchesAsync(cancellationToken);

        return ServiceResult<WarehouseDto>.SuccessResult(
            MapWarehouse(warehouse, branches, stockItems));
    }

    public async Task<ServiceResult<List<StockItemDto>>> GetStockAsync(
        int? branchId = null,
        int? warehouseId = null,
        CancellationToken cancellationToken = default)
    {
        var stockItems = await unitOfWork.Repository<StockItem>().ListAsync(
            x => (!branchId.HasValue || x.BranchId == branchId.Value) &&
                 (!warehouseId.HasValue || x.WarehouseId == warehouseId.Value),
            cancellationToken);

        var data = await MapStockItemsAsync(stockItems, cancellationToken);
        return ServiceResult<List<StockItemDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<List<StockItemDto>>> GetProductStockAsync(
        int productId,
        CancellationToken cancellationToken = default)
    {
        var stockItems = await unitOfWork.Repository<StockItem>().ListAsync(
            x => x.ProductId == productId,
            cancellationToken);

        var data = await MapStockItemsAsync(stockItems, cancellationToken);
        return ServiceResult<List<StockItemDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<List<InventoryAlertDto>>> GetAlertsAsync(
        int? branchId = null,
        CancellationToken cancellationToken = default)
    {
        var alerts = await unitOfWork.Repository<InventoryAlert>().ListAsync(
            x => (!branchId.HasValue || x.BranchId == branchId.Value) &&
                 x.Status == InventoryAlertStatus.Open &&
                 (x.Type == InventoryAlertType.LowStock ||
                  x.Type == InventoryAlertType.OutOfStock),
            cancellationToken);
        var products = await GetProductsAsync(cancellationToken);
        var warehouses = await GetWarehousesDictionaryAsync(cancellationToken);
        var branches = await GetBranchesAsync(cancellationToken);

        var data = alerts
            .OrderByDescending(x => x.CreatedDate)
            .Select(x => new InventoryAlertDto
            {
                Id = x.Id,
                Type = x.Type.ToString(),
                Status = x.Status.ToString(),
                ProductId = x.ProductId,
                ProductName = products.GetValueOrDefault(x.ProductId)?.Name ?? string.Empty,
                BranchId = x.BranchId,
                BranchName = branches.GetValueOrDefault(x.BranchId)?.Name ?? string.Empty,
                WarehouseId = x.WarehouseId,
                WarehouseName = warehouses.GetValueOrDefault(x.WarehouseId)?.Name ?? string.Empty,
                CurrentQuantity = x.CurrentQuantity,
                ThresholdQuantity = x.ThresholdQuantity,
                ExpiryDate = x.ExpiryDate,
                Message = x.Message,
                CreatedDate = x.CreatedDate
            })
            .ToList();

        return ServiceResult<List<InventoryAlertDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<List<ProductBatchDto>>> GetBatchesAsync(
        int? productId = null,
        int? branchId = null,
        int? warehouseId = null,
        CancellationToken cancellationToken = default)
    {
        var batches = await unitOfWork.Repository<ProductBatch>().ListAsync(
            x => !x.IsDeleted &&
                 (!productId.HasValue || x.ProductId == productId.Value) &&
                 (!branchId.HasValue || x.BranchId == branchId.Value) &&
                 (!warehouseId.HasValue || x.WarehouseId == warehouseId.Value),
            cancellationToken);
        var products = await GetProductsAsync(cancellationToken);
        var warehouses = await GetWarehousesDictionaryAsync(cancellationToken);
        var branches = await GetBranchesAsync(cancellationToken);

        var data = batches
            .OrderBy(x => x.ExpiryDate)
            .ThenBy(x => x.BatchNumber)
            .Select(x => new ProductBatchDto
            {
                Id = x.Id,
                BatchNumber = x.BatchNumber,
                ProductId = x.ProductId,
                ProductName = products.GetValueOrDefault(x.ProductId)?.Name ?? string.Empty,
                BranchId = x.BranchId,
                BranchName = branches.GetValueOrDefault(x.BranchId)?.Name ?? string.Empty,
                WarehouseId = x.WarehouseId,
                WarehouseName = warehouses.GetValueOrDefault(x.WarehouseId)?.Name ?? string.Empty,
                ManufactureDate = x.ManufactureDate,
                ExpiryDate = x.ExpiryDate,
                InitialQuantity = x.InitialQuantity,
                CurrentQuantity = x.CurrentQuantity,
                PurchaseCost = x.PurchaseCost,
                IsActive = x.IsActive
            })
            .ToList();

        return ServiceResult<List<ProductBatchDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<List<StockMovementDto>>> GetMovementsAsync(
        int? productId = null,
        int? branchId = null,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken cancellationToken = default)
    {
        var movements = await unitOfWork.Repository<StockMovement>().ListAsync(
            x => (!productId.HasValue || x.ProductId == productId.Value) &&
                 (!branchId.HasValue || x.BranchId == branchId.Value) &&
                 (!from.HasValue || x.MovementDate >= from.Value) &&
                 (!to.HasValue || x.MovementDate <= to.Value),
            cancellationToken);
        var products = await GetProductsAsync(cancellationToken);
        var warehouses = await GetWarehousesDictionaryAsync(cancellationToken);
        var branches = await GetBranchesAsync(cancellationToken);
        var batches = (await unitOfWork.Repository<ProductBatch>().ListAsync(
                x => !x.IsDeleted,
                cancellationToken))
            .ToDictionary(x => x.Id);

        var data = movements
            .OrderByDescending(x => x.MovementDate)
            .Select(x => new StockMovementDto
            {
                Id = x.Id,
                MovementNumber = x.MovementNumber,
                MovementType = x.Type.ToString(),
                ProductId = x.ProductId,
                ProductName = products.GetValueOrDefault(x.ProductId)?.Name ?? string.Empty,
                BranchId = x.BranchId,
                BranchName = branches.GetValueOrDefault(x.BranchId)?.Name ?? string.Empty,
                SourceWarehouseId = x.WarehouseId,
                SourceWarehouseName = warehouses.GetValueOrDefault(x.WarehouseId)?.Name ?? string.Empty,
                DestinationWarehouseId = null,
                DestinationWarehouseName = null,
                ProductBatchId = x.ProductBatchId,
                BatchNumber = x.ProductBatchId.HasValue
                    ? batches.GetValueOrDefault(x.ProductBatchId.Value)?.BatchNumber
                    : null,
                Quantity = x.Quantity,
                CreatedDate = x.MovementDate
            })
            .ToList();

        return ServiceResult<List<StockMovementDto>>.SuccessResult(data);
    }

    private async Task<List<StockItemDto>> MapStockItemsAsync(
        IReadOnlyCollection<StockItem> stockItems,
        CancellationToken cancellationToken)
    {
        var products = await GetProductsAsync(cancellationToken);
        var warehouses = await GetWarehousesDictionaryAsync(cancellationToken);
        var branches = await GetBranchesAsync(cancellationToken);
        var barcodes = await unitOfWork.Repository<ProductBarcode>().ListAsync(
            x => x.IsActive && !x.IsDeleted,
            cancellationToken);
        var primaryBarcodes = barcodes
            .GroupBy(x => x.ProductId)
            .ToDictionary(
                group => group.Key,
                group => group.OrderByDescending(x => x.IsPrimary).ThenBy(x => x.Id).First().Barcode);

        return stockItems
            .OrderBy(x => products.GetValueOrDefault(x.ProductId)?.Name)
            .ThenBy(x => warehouses.GetValueOrDefault(x.WarehouseId)?.Name)
            .Select(x => new StockItemDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ProductName = products.GetValueOrDefault(x.ProductId)?.Name ?? string.Empty,
                SKU = products.GetValueOrDefault(x.ProductId)?.SKU ?? string.Empty,
                Barcode = primaryBarcodes.GetValueOrDefault(x.ProductId),
                BranchId = x.BranchId,
                BranchName = branches.GetValueOrDefault(x.BranchId)?.Name ?? string.Empty,
                WarehouseId = x.WarehouseId,
                WarehouseName = warehouses.GetValueOrDefault(x.WarehouseId)?.Name ?? string.Empty,
                Quantity = x.Quantity,
                ReservedQuantity = x.ReservedQuantity,
                AvailableQuantity = x.Quantity - x.ReservedQuantity,
                ReorderLevel = x.ReorderLevel
            })
            .ToList();
    }

    private static WarehouseDto MapWarehouse(
        Warehouse warehouse,
        IReadOnlyDictionary<int, Branch> branches,
        IReadOnlyCollection<StockItem> stockItems)
    {
        var warehouseStock = stockItems.Where(x => x.WarehouseId == warehouse.Id).ToList();
        return new WarehouseDto
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Code = warehouse.Code,
            Location = warehouse.Location,
            Type = warehouse.Type.ToString(),
            BranchId = warehouse.BranchId,
            BranchName = branches.GetValueOrDefault(warehouse.BranchId)?.Name ?? string.Empty,
            IsActive = warehouse.IsActive,
            StockCount = warehouseStock.Count,
            ProductCount = warehouseStock.Select(x => x.ProductId).Distinct().Count()
        };
    }

    private async Task<Dictionary<int, Product>> GetProductsAsync(
        CancellationToken cancellationToken)
    {
        return (await unitOfWork.Repository<Product>().ListAsync(
                x => !x.IsDeleted,
                cancellationToken))
            .ToDictionary(x => x.Id);
    }

    private async Task<Dictionary<int, Warehouse>> GetWarehousesDictionaryAsync(
        CancellationToken cancellationToken)
    {
        return (await unitOfWork.Repository<Warehouse>().ListAsync(
                x => !x.IsDeleted,
                cancellationToken))
            .ToDictionary(x => x.Id);
    }

    private async Task<Dictionary<int, Branch>> GetBranchesAsync(
        CancellationToken cancellationToken)
    {
        return (await unitOfWork.Repository<Branch>().ListAsync(
                x => !x.IsDeleted,
                cancellationToken))
            .ToDictionary(x => x.Id);
    }
}
