using ERP.Business.Common.Interfaces.Repositories;
using ERP.Business.Common.Interfaces.Repositories.Specific;
using ERP.Business.Common.Models;
using ERP.Business.Features.Catalog.Dtos;
using ERP.Business.Features.Catalog.Interfaces;
using ERP.Core.Entities.Catalog;

namespace ERP.Business.Features.Catalog.Services;

public sealed class CatalogService(
    ICatalogRepository catalogRepository,
    IUnitOfWork unitOfWork) : ICatalogService
{
    public async Task<ServiceResult<List<ProductCategoryDto>>> GetCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await catalogRepository.GetActiveCategoriesAsync(cancellationToken);
        var data = entities.Select(x => new ProductCategoryDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            IsActive = x.IsActive,
            ParentCategoryId = x.ParentCategoryId
        }).ToList();

        return ServiceResult<List<ProductCategoryDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<List<ProductBrandDto>>> GetBrandsAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await catalogRepository.GetActiveBrandsAsync(cancellationToken);
        var data = entities.Select(x => new ProductBrandDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            IsActive = x.IsActive
        }).ToList();

        return ServiceResult<List<ProductBrandDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<List<UnitOfMeasureDto>>> GetUnitsAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await catalogRepository.GetActiveUnitsAsync(cancellationToken);
        var data = entities.Select(x => new UnitOfMeasureDto
        {
            Id = x.Id,
            Name = x.Name,
            ShortName = x.ShortName,
            Type = x.Type.ToString(),
            IsActive = x.IsActive
        }).ToList();

        return ServiceResult<List<UnitOfMeasureDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<List<TaxRateDto>>> GetTaxRatesAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await catalogRepository.GetActiveTaxRatesAsync(cancellationToken);
        var data = entities.Select(x => new TaxRateDto
        {
            Id = x.Id,
            Name = x.Name,
            Rate = x.Rate,
            IsActive = x.IsActive
        }).ToList();

        return ServiceResult<List<TaxRateDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<List<ProductListDto>>> GetProductsAsync(
        CancellationToken cancellationToken = default)
    {
        var products = await unitOfWork.Repository<Product>().ListAsync(
            x => x.IsActive && !x.IsDeleted,
            cancellationToken);
        var barcodes = await unitOfWork.Repository<ProductBarcode>().ListAsync(
            x => x.IsActive && !x.IsDeleted,
            cancellationToken);
        var categories = (await catalogRepository.GetActiveCategoriesAsync(cancellationToken))
            .ToDictionary(x => x.Id);
        var brands = (await catalogRepository.GetActiveBrandsAsync(cancellationToken))
            .ToDictionary(x => x.Id);
        var units = (await catalogRepository.GetActiveUnitsAsync(cancellationToken))
            .ToDictionary(x => x.Id);
        var primaryBarcodes = barcodes
            .GroupBy(x => x.ProductId)
            .ToDictionary(
                group => group.Key,
                group => group.OrderByDescending(x => x.IsPrimary).ThenBy(x => x.Id).First().Barcode);

        var data = products
            .OrderBy(x => x.Name)
            .Select(x => new ProductListDto
            {
                Id = x.Id,
                SKU = x.SKU,
                Name = x.Name,
                Description = x.Description,
                Type = x.Type.ToString(),
                Status = x.Status.ToString(),
                CostPrice = x.CostPrice,
                SalePrice = x.SalePrice,
                IsActive = x.IsActive,
                IsPerishable = x.IsPerishable,
                TrackBatch = x.TrackBatch,
                AllowDecimalQuantity = x.AllowDecimalQuantity,
                CategoryName = categories.GetValueOrDefault(x.ProductCategoryId)?.Name ?? string.Empty,
                BrandName = x.ProductBrandId.HasValue
                    ? brands.GetValueOrDefault(x.ProductBrandId.Value)?.Name
                    : null,
                UnitName = units.GetValueOrDefault(x.UnitOfMeasureId)?.Name ?? string.Empty,
                UnitShortName = units.GetValueOrDefault(x.UnitOfMeasureId)?.ShortName ?? string.Empty,
                PrimaryBarcode = primaryBarcodes.GetValueOrDefault(x.Id)
            })
            .ToList();

        return ServiceResult<List<ProductListDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<ProductDetailDto>> GetProductByIdAsync(
        int productId,
        CancellationToken cancellationToken = default)
    {
        var product = await catalogRepository.GetProductDetailsAsync(productId, cancellationToken);
        return product is null
            ? ServiceResult<ProductDetailDto>.Failure("Product not found.")
            : ServiceResult<ProductDetailDto>.SuccessResult(MapProductDetail(product));
    }

    public async Task<ServiceResult<ProductDetailDto>> GetProductByBarcodeAsync(
        string barcode,
        CancellationToken cancellationToken = default)
    {
        var product = await catalogRepository.GetProductByBarcodeAsync(barcode, cancellationToken);
        return product is null
            ? ServiceResult<ProductDetailDto>.Failure("Product not found for barcode.")
            : ServiceResult<ProductDetailDto>.SuccessResult(MapProductDetail(product));
    }

    public async Task<ServiceResult<ProductPriceDto>> GetEffectivePriceAsync(
        int productId,
        int? branchId = null,
        CancellationToken cancellationToken = default)
    {
        var product = await catalogRepository.GetProductDetailsAsync(productId, cancellationToken);
        if (product is null)
        {
            return ServiceResult<ProductPriceDto>.Failure("Product not found.");
        }

        var checkedAt = DateTime.UtcNow;
        var salePrice = await catalogRepository.GetEffectiveSalePriceAsync(
            productId,
            branchId,
            checkedAt,
            cancellationToken);

        if (!salePrice.HasValue)
        {
            return ServiceResult<ProductPriceDto>.Failure("Product not found.");
        }

        var isBranchSpecific = branchId.HasValue && product.BranchPrices.Any(
            x => x.BranchId == branchId.Value &&
                 x.IsActive &&
                 !x.IsDeleted &&
                 x.EffectiveFrom <= checkedAt &&
                 (!x.EffectiveTo.HasValue || x.EffectiveTo >= checkedAt));

        return ServiceResult<ProductPriceDto>.SuccessResult(new ProductPriceDto
        {
            ProductId = product.Id,
            SKU = product.SKU,
            ProductName = product.Name,
            BranchId = branchId,
            SalePrice = salePrice.Value,
            IsBranchSpecific = isBranchSpecific,
            CheckedAt = checkedAt
        });
    }

    private static ProductDetailDto MapProductDetail(Product product)
    {
        return new ProductDetailDto
        {
            Id = product.Id,
            SKU = product.SKU,
            Name = product.Name,
            Description = product.Description,
            Type = product.Type.ToString(),
            Status = product.Status.ToString(),
            CostPrice = product.CostPrice,
            SalePrice = product.SalePrice,
            IsActive = product.IsActive,
            IsPerishable = product.IsPerishable,
            TrackBatch = product.TrackBatch,
            AllowDecimalQuantity = product.AllowDecimalQuantity,
            ImageUrl = product.ImageUrl,
            ProductCategoryId = product.ProductCategoryId,
            CategoryName = product.ProductCategory?.Name ?? string.Empty,
            ProductBrandId = product.ProductBrandId,
            BrandName = product.ProductBrand?.Name,
            UnitOfMeasureId = product.UnitOfMeasureId,
            UnitName = product.UnitOfMeasure?.Name ?? string.Empty,
            UnitShortName = product.UnitOfMeasure?.ShortName ?? string.Empty,
            TaxRateId = product.TaxRateId,
            TaxRateName = product.TaxRate?.Name,
            TaxRate = product.TaxRate?.Rate,
            Barcodes = product.Barcodes
                .OrderByDescending(x => x.IsPrimary)
                .ThenBy(x => x.Barcode)
                .Select(x => new ProductBarcodeDto
                {
                    Id = x.Id,
                    Barcode = x.Barcode,
                    Type = x.Type.ToString(),
                    IsPrimary = x.IsPrimary,
                    IsActive = x.IsActive
                })
                .ToList()
        };
    }
}
