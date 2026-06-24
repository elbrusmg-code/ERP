namespace ERP.Business.Features.Catalog.Dtos;

public class ProductBarcodeDto
{
    public int Id { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
}
