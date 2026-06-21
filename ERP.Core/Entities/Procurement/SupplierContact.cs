using ERP.Core.Common;

namespace ERP.Core.Entities.Procurement;

public class SupplierContact : SoftDeleteEntity
{
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Position { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
}
