using ERP.Core.Common;

namespace ERP.Core.Entities.Catalog;

public class TaxRate : SoftDeleteEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
