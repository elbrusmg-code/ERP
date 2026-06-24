namespace ERP.Business.Features.Catalog.Dtos;

public class ProductPriceDto
{
    public int ProductId { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int? BranchId { get; set; }
    public decimal SalePrice { get; set; }
    public bool IsBranchSpecific { get; set; }
    public DateTime CheckedAt { get; set; }
}
