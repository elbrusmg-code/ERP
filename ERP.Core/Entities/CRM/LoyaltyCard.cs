using ERP.Core.Common;
using ERP.Core.Enums;

namespace ERP.Core.Entities.CRM;

public class LoyaltyCard : SoftDeleteEntity
{
    public string CardNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public LoyaltyCardStatus Status { get; set; }
    public DateTime IssuedAt { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public decimal PointsBalance { get; set; }
    public bool IsPrimary { get; set; }
    public string? Note { get; set; }
}
