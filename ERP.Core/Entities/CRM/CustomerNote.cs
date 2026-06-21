using ERP.Core.Common;

namespace ERP.Core.Entities.CRM;

public class CustomerNote : AuditableEntity
{
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public string Note { get; set; } = string.Empty;
    public DateTime NoteDate { get; set; }
    public string? CreatedByUserId { get; set; }
}
