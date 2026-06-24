namespace ERP.Business.Features.Catalog.Dtos;

public class ProductDetailDto
{
    public int Id { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public bool IsActive { get; set; }
    public bool IsPerishable { get; set; }
    public bool TrackBatch { get; set; }
    public bool AllowDecimalQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public int ProductCategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int? ProductBrandId { get; set; }
    public string? BrandName { get; set; }
    public int UnitOfMeasureId { get; set; }
    public string UnitName { get; set; } = string.Empty;
    public string UnitShortName { get; set; } = string.Empty;
    public int? TaxRateId { get; set; }
    public string? TaxRateName { get; set; }
    public decimal? TaxRate { get; set; }
    public List<ProductBarcodeDto> Barcodes { get; set; } = new();
}
