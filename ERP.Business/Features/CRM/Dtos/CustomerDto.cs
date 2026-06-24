namespace ERP.Business.Features.CRM.Dtos;

public sealed class CustomerDto
{
    public int Id { get; set; }
    public string CustomerCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? TaxNumber { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int? CustomerGroupId { get; set; }
    public string? CustomerGroupName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public bool IsLoyaltyEnabled { get; set; }
    public decimal CurrentLoyaltyPoints { get; set; }
    public decimal TotalSpent { get; set; }
    public DateTime? LastPurchaseDate { get; set; }
    public int? RegisteredBranchId { get; set; }
    public string? RegisteredBranchName { get; set; }
    public List<CustomerAddressDto> Addresses { get; set; } = new();
    public List<LoyaltyCardDto> LoyaltyCards { get; set; } = new();
    public List<CustomerNoteDto> Notes { get; set; } = new();
    public List<CustomerInteractionDto> Interactions { get; set; } = new();
    public List<CustomerTransactionHistoryDto> TransactionHistory { get; set; } = new();
}
