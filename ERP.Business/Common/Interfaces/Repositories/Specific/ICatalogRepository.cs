using ERP.Core.Entities.Catalog;

namespace ERP.Business.Common.Interfaces.Repositories.Specific;

public interface ICatalogRepository
{
    Task<List<ProductCategory>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);
    Task<List<ProductBrand>> GetActiveBrandsAsync(CancellationToken cancellationToken = default);
    Task<List<UnitOfMeasure>> GetActiveUnitsAsync(CancellationToken cancellationToken = default);
    Task<List<TaxRate>> GetActiveTaxRatesAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetProductDetailsAsync(int productId, CancellationToken cancellationToken = default);
    Task<Product?> GetProductBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task<Product?> GetProductByBarcodeAsync(string barcode, CancellationToken cancellationToken = default);
    Task<bool> SkuExistsAsync(string sku, int? excludeProductId = null, CancellationToken cancellationToken = default);
    Task<bool> BarcodeExistsAsync(string barcode, int? excludeBarcodeId = null, CancellationToken cancellationToken = default);
    Task<decimal?> GetEffectiveSalePriceAsync(int productId, int? branchId = null, DateTime? date = null, CancellationToken cancellationToken = default);
}
