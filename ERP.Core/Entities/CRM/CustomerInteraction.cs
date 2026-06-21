using ERP.Core.Common;
using ERP.Core.Enums;

namespace ERP.Core.Entities.CRM;

public class CustomerInteraction : AuditableEntity
{
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public CRMInteractionType Type { get; set; }
    public DateTime InteractionDate { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CreatedByUserId { get; set; }
}
