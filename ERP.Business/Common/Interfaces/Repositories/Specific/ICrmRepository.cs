using ERP.Core.Entities.CRM;

namespace ERP.Business.Common.Interfaces.Repositories.Specific;

public interface ICrmRepository
{
    Task<List<CustomerGroup>> GetActiveCustomerGroupsAsync(CancellationToken cancellationToken = default);
    Task<Customer?> GetCustomerDetailsAsync(int customerId, CancellationToken cancellationToken = default);
    Task<Customer?> GetCustomerByPhoneAsync(string phone, CancellationToken cancellationToken = default);
    Task<Customer?> GetCustomerByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Customer?> GetCustomerByLoyaltyCardAsync(string cardNumber, CancellationToken cancellationToken = default);
    Task<bool> CustomerCodeExistsAsync(string customerCode, int? excludeCustomerId = null, CancellationToken cancellationToken = default);
    Task<bool> LoyaltyCardExistsAsync(string cardNumber, int? excludeLoyaltyCardId = null, CancellationToken cancellationToken = default);
    Task<List<CustomerTransactionHistory>> GetCustomerTransactionHistoryAsync(int customerId, CancellationToken cancellationToken = default);
    Task<List<LoyaltyPointTransaction>> GetLoyaltyTransactionsAsync(int customerId, CancellationToken cancellationToken = default);
    Task<List<CustomerNote>> GetCustomerNotesAsync(int customerId, CancellationToken cancellationToken = default);
}
