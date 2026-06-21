using ERP.Core.Common;

namespace ERP.Core.Entities.CRM;

public class CustomerGroup : SoftDeleteEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal DefaultDiscountPercent { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
