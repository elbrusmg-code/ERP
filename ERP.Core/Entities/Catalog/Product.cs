using ERP.Core.Common;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Catalog;

public class Product : SoftDeleteEntity
{
    public string SKU { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ProductType Type { get; set; }
    public ProductStatus Status { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public bool IsActive { get; set; }
    public bool IsPerishable { get; set; }
    public bool TrackBatch { get; set; }
    public bool AllowDecimalQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public int ProductCategoryId { get; set; }
    public ProductCategory? ProductCategory { get; set; }
    public int? ProductBrandId { get; set; }
    public ProductBrand? ProductBrand { get; set; }
    public int UnitOfMeasureId { get; set; }
    public UnitOfMeasure? UnitOfMeasure { get; set; }
    public int? TaxRateId { get; set; }
    public TaxRate? TaxRate { get; set; }
    public ICollection<ProductBarcode> Barcodes { get; set; } = new List<ProductBarcode>();
    public ICollection<ProductPriceHistory> PriceHistory { get; set; } = new List<ProductPriceHistory>();
    public ICollection<BranchProductPrice> BranchPrices { get; set; } = new List<BranchProductPrice>();
}
