namespace ERP.Business.Features.Catalog.Dtos;

public class TaxRateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public bool IsActive { get; set; }
}
