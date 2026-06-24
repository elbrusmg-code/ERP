namespace ERP.Business.Features.CRM.Dtos;

public sealed class CustomerInteractionDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime InteractionDate { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CreatedByUserId { get; set; }
}
