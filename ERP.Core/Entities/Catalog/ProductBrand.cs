using ERP.Core.Common;

namespace ERP.Core.Entities.Catalog;

public class ProductBrand : SoftDeleteEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
