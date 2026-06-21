using ERP.Core.Common;
using ERP.Core.Enums;

namespace ERP.Core.Entities.Catalog;

public class UnitOfMeasure : SoftDeleteEntity
{
    public string Name { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public UnitType Type { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
