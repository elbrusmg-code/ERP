using ERP.Core.Common;
using ERP.Core.Entities.Organization;

namespace ERP.Core.Entities.Catalog;

public class BranchProductPrice : SoftDeleteEntity
{
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
    public decimal SalePrice { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public bool IsActive { get; set; }
}
