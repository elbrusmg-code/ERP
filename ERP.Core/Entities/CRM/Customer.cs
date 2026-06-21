using ERP.Core.Common;
using ERP.Core.Entities.Organization;
using ERP.Core.Enums;

namespace ERP.Core.Entities.CRM;

public class Customer : SoftDeleteEntity
{
    public string CustomerCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? TaxNumber { get; set; }
    public CustomerType Type { get; set; }
    public CustomerStatus Status { get; set; }
    public int? CustomerGroupId { get; set; }
    public CustomerGroup? CustomerGroup { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public bool IsLoyaltyEnabled { get; set; }
    public decimal CurrentLoyaltyPoints { get; set; }
    public decimal TotalSpent { get; set; }
    public DateTime? LastPurchaseDate { get; set; }
    public int? RegisteredBranchId { get; set; }
    public Branch? RegisteredBranch { get; set; }
    public ICollection<CustomerAddress> Addresses { get; set; } = new List<CustomerAddress>();
    public ICollection<LoyaltyCard> LoyaltyCards { get; set; } = new List<LoyaltyCard>();
    public ICollection<LoyaltyPointTransaction> LoyaltyPointTransactions { get; set; } = new List<LoyaltyPointTransaction>();
    public ICollection<CustomerNote> Notes { get; set; } = new List<CustomerNote>();
    public ICollection<CustomerInteraction> Interactions { get; set; } = new List<CustomerInteraction>();
    public ICollection<CustomerTransactionHistory> TransactionHistory { get; set; } = new List<CustomerTransactionHistory>();
}
