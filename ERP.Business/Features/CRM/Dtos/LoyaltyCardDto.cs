namespace ERP.Business.Features.CRM.Dtos;

public sealed class LoyaltyCardDto
{
    public int Id { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public decimal PointsBalance { get; set; }
    public bool IsPrimary { get; set; }
    public string? Note { get; set; }
}
