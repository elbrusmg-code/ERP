using ERP.Core.Common;
using ERP.Core.Entities.Procurement;

namespace ERP.Core.Entities.CRM;

public class SupplierNote : AuditableEntity
{
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public string Note { get; set; } = string.Empty;
    public DateTime NoteDate { get; set; }
    public string? CreatedByUserId { get; set; }
}
