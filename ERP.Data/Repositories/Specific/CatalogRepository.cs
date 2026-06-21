using ERP.Business.Common.Interfaces.Repositories.Specific;
using ERP.Core.Entities.Catalog;
using ERP.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories.Specific;

public sealed class CatalogRepository(ERPDbContext context) : ICatalogRepository
{
    public Task<List<ProductCategory>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return context.ProductCategories.AsNoTracking()
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<List<ProductBrand>> GetActiveBrandsAsync(CancellationToken cancellationToken = default)
    {
        return context.ProductBrands.AsNoTracking()
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<List<UnitOfMeasure>> GetActiveUnitsAsync(CancellationToken cancellationToken = default)
    {
        return context.UnitsOfMeasure.AsNoTracking()
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<List<TaxRate>> GetActiveTaxRatesAsync(CancellationToken cancellationToken = default)
    {
        return context.TaxRates.AsNoTracking()
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<Product?> GetProductDetailsAsync(int productId, CancellationToken cancellationToken = default)
    {
        return ProductDetailsQuery().FirstOrDefaultAsync(x => x.Id == productId, cancellationToken);
    }

    public Task<Product?> GetProductBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        return ProductDetailsQuery().FirstOrDefaultAsync(x => x.SKU == sku, cancellationToken);
    }

    public async Task<Product?> GetProductByBarcodeAsync(string barcode, CancellationToken cancellationToken = default)
    {
        var productBarcode = await context.ProductBarcodes
            .AsNoTracking()
            .Where(x => x.Barcode == barcode && x.IsActive && !x.IsDeleted)
            .Include(x => x.Product)
                .ThenInclude(x => x!.ProductCategory)
            .Include(x => x.Product)
                .ThenInclude(x => x!.ProductBrand)
            .Include(x => x.Product)
                .ThenInclude(x => x!.UnitOfMeasure)
            .Include(x => x.Product)
                .ThenInclude(x => x!.TaxRate)
            .FirstOrDefaultAsync(cancellationToken);

        return productBarcode?.Product is { IsDeleted: false } product ? product : null;
    }

    public Task<bool> SkuExistsAsync(
        string sku,
        int? excludeProductId = null,
        CancellationToken cancellationToken = default)
    {
        return context.Products.AsNoTracking().AnyAsync(
            x => x.SKU == sku &&
                 !x.IsDeleted &&
                 (!excludeProductId.HasValue || x.Id != excludeProductId.Value),
            cancellationToken);
    }

    public Task<bool> BarcodeExistsAsync(
        string barcode,
        int? excludeBarcodeId = null,
        CancellationToken cancellationToken = default)
    {
        return context.ProductBarcodes.AsNoTracking().AnyAsync(
            x => x.Barcode == barcode &&
                 !x.IsDeleted &&
                 (!excludeBarcodeId.HasValue || x.Id != excludeBarcodeId.Value),
            cancellationToken);
    }

    public async Task<decimal?> GetEffectiveSalePriceAsync(
        int productId,
        int? branchId = null,
        DateTime? date = null,
        CancellationToken cancellationToken = default)
    {
        var effectiveDate = date ?? DateTime.UtcNow;

        if (branchId.HasValue)
        {
            var branchPrice = await context.BranchProductPrices
                .AsNoTracking()
                .Where(x => x.ProductId == productId &&
                            x.BranchId == branchId.Value &&
                            x.IsActive &&
                            !x.IsDeleted &&
                            x.EffectiveFrom <= effectiveDate &&
                            (!x.EffectiveTo.HasValue || x.EffectiveTo >= effectiveDate))
                .OrderByDescending(x => x.EffectiveFrom)
                .Select(x => (decimal?)x.SalePrice)
                .FirstOrDefaultAsync(cancellationToken);

            if (branchPrice.HasValue)
            {
                return branchPrice;
            }
        }

        return await context.Products
            .AsNoTracking()
            .Where(x => x.Id == productId && !x.IsDeleted)
            .Select(x => (decimal?)x.SalePrice)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private IQueryable<Product> ProductDetailsQuery()
    {
        return context.Products
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => !x.IsDeleted)
            .Include(x => x.ProductCategory)
            .Include(x => x.ProductBrand)
            .Include(x => x.UnitOfMeasure)
            .Include(x => x.TaxRate)
            .Include(x => x.Barcodes.Where(barcode => !barcode.IsDeleted))
            .Include(x => x.BranchPrices.Where(price => !price.IsDeleted));
    }
}
