using ERP.Business.Common.Models;
using ERP.Business.Features.Catalog.Dtos;

namespace ERP.Business.Features.Catalog.Interfaces;

public interface ICatalogService
{
    Task<ServiceResult<List<ProductCategoryDto>>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<List<ProductBrandDto>>> GetBrandsAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<List<UnitOfMeasureDto>>> GetUnitsAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<List<TaxRateDto>>> GetTaxRatesAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<List<ProductListDto>>> GetProductsAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<ProductDetailDto>> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default);
    Task<ServiceResult<ProductDetailDto>> GetProductByBarcodeAsync(string barcode, CancellationToken cancellationToken = default);
    Task<ServiceResult<ProductPriceDto>> GetEffectivePriceAsync(int productId, int? branchId = null, CancellationToken cancellationToken = default);
}
