using ERP.Core.Common;

namespace ERP.Core.Entities.CRM;

public class CustomerAddress : SoftDeleteEntity
{
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public string? Title { get; set; }
    public string AddressLine { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}
