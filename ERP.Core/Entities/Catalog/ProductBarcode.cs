using ERP.Core.Common;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Catalog;

public class ProductBarcode : SoftDeleteEntity
{
    public string Barcode { get; set; } = string.Empty;
    public BarcodeType Type { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
}
