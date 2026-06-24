using ERP.Business.Common.Models;
using ERP.Business.Features.CRM.Dtos;

namespace ERP.Business.Features.CRM.Interfaces;

public interface ICrmService
{
    Task<ServiceResult<List<CustomerGroupDto>>> GetCustomerGroupsAsync(
        CancellationToken cancellationToken = default);

    Task<ServiceResult<CustomerDto>> GetCustomerByIdAsync(
        int customerId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<CustomerDto>> GetCustomerByPhoneAsync(
        string phone,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<CustomerDto>> GetCustomerByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<CustomerDto>> GetCustomerByLoyaltyCardAsync(
        string cardNumber,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<List<CustomerTransactionHistoryDto>>> GetCustomerTransactionsAsync(
        int customerId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<List<LoyaltyPointTransactionDto>>> GetLoyaltyTransactionsAsync(
        int customerId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<List<CustomerNoteDto>>> GetCustomerNotesAsync(
        int customerId,
        CancellationToken cancellationToken = default);
}
