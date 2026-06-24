using ERP.Business.Common.Models;
using ERP.Business.Features.Catalog.Dtos;
using ERP.Business.Features.Catalog.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogController(ICatalogService catalogService) : ControllerBase
{
    [HttpGet("categories")]
    public async Task<ActionResult<ApiResponse<List<ProductCategoryDto>>>> GetCategories(
        CancellationToken cancellationToken)
    {
        var result = await catalogService.GetCategoriesAsync(cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<ApiResponse<List<ProductBrandDto>>>> GetBrands(
        CancellationToken cancellationToken)
    {
        var result = await catalogService.GetBrandsAsync(cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("units")]
    public async Task<ActionResult<ApiResponse<List<UnitOfMeasureDto>>>> GetUnits(
        CancellationToken cancellationToken)
    {
        var result = await catalogService.GetUnitsAsync(cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("tax-rates")]
    public async Task<ActionResult<ApiResponse<List<TaxRateDto>>>> GetTaxRates(
        CancellationToken cancellationToken)
    {
        var result = await catalogService.GetTaxRatesAsync(cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("products")]
    public async Task<ActionResult<ApiResponse<List<ProductListDto>>>> GetProducts(
        CancellationToken cancellationToken)
    {
        var result = await catalogService.GetProductsAsync(cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("products/{id:int}")]
    public async Task<ActionResult<ApiResponse<ProductDetailDto>>> GetProductById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await catalogService.GetProductByIdAsync(id, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("products/by-barcode/{barcode}")]
    public async Task<ActionResult<ApiResponse<ProductDetailDto>>> GetProductByBarcode(
        string barcode,
        CancellationToken cancellationToken)
    {
        var result = await catalogService.GetProductByBarcodeAsync(barcode, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("products/{id:int}/price")]
    public async Task<ActionResult<ApiResponse<ProductPriceDto>>> GetEffectivePrice(
        int id,
        [FromQuery] int? branchId,
        CancellationToken cancellationToken)
    {
        var result = await catalogService.GetEffectivePriceAsync(id, branchId, cancellationToken);
        return ToActionResult(result);
    }

    private ActionResult<ApiResponse<T>> ToActionResult<T>(ServiceResult<T> result)
    {
        return result.Success
            ? Ok(ApiResponse<T>.Ok(result.Data!, result.Message))
            : NotFound(ApiResponse<T>.Fail(result.Message, result.Errors));
    }
}
