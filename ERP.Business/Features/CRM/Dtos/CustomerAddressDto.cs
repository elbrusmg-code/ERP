namespace ERP.Business.Features.CRM.Dtos;

public sealed class CustomerAddressDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string AddressLine { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}
