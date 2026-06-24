namespace ERP.Business.Features.CRM.Dtos;

public sealed class CustomerGroupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal DefaultDiscountPercent { get; set; }
    public bool IsActive { get; set; }
}
