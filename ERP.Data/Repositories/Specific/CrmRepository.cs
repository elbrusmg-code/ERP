using ERP.Business.Common.Interfaces.Repositories.Specific;
using ERP.Core.Entities.CRM;
using ERP.Core.Enums;
using ERP.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories.Specific;

public sealed class CrmRepository(ERPDbContext context) : ICrmRepository
{
    public Task<List<CustomerGroup>> GetActiveCustomerGroupsAsync(CancellationToken cancellationToken = default)
    {
        return context.CustomerGroups.AsNoTracking()
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<Customer?> GetCustomerDetailsAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return context.Customers.AsNoTracking()
            .AsSplitQuery()
            .Where(x => !x.IsDeleted)
            .Include(x => x.CustomerGroup)
            .Include(x => x.RegisteredBranch)
            .Include(x => x.Addresses.Where(address => !address.IsDeleted))
            .Include(x => x.LoyaltyCards.Where(card => !card.IsDeleted))
            .Include(x => x.Notes)
            .Include(x => x.Interactions)
            .Include(x => x.TransactionHistory)
            .FirstOrDefaultAsync(x => x.Id == customerId, cancellationToken);
    }

    public Task<Customer?> GetCustomerByPhoneAsync(string phone, CancellationToken cancellationToken = default)
    {
        return context.Customers.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Phone == phone && !x.IsDeleted, cancellationToken);
    }

    public Task<Customer?> GetCustomerByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return context.Customers.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted, cancellationToken);
    }

    public async Task<Customer?> GetCustomerByLoyaltyCardAsync(
        string cardNumber,
        CancellationToken cancellationToken = default)
    {
        var loyaltyCard = await context.LoyaltyCards.AsNoTracking()
            .Where(x => x.CardNumber == cardNumber &&
                        x.Status == LoyaltyCardStatus.Active &&
                        !x.IsDeleted)
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(cancellationToken);

        return loyaltyCard?.Customer is { IsDeleted: false } customer ? customer : null;
    }

    public Task<bool> CustomerCodeExistsAsync(
        string customerCode,
        int? excludeCustomerId = null,
        CancellationToken cancellationToken = default)
    {
        return context.Customers.AsNoTracking().AnyAsync(
            x => x.CustomerCode == customerCode &&
                 !x.IsDeleted &&
                 (!excludeCustomerId.HasValue || x.Id != excludeCustomerId.Value),
            cancellationToken);
    }

    public Task<bool> LoyaltyCardExistsAsync(
        string cardNumber,
        int? excludeLoyaltyCardId = null,
        CancellationToken cancellationToken = default)
    {
        return context.LoyaltyCards.AsNoTracking().AnyAsync(
            x => x.CardNumber == cardNumber &&
                 !x.IsDeleted &&
                 (!excludeLoyaltyCardId.HasValue || x.Id != excludeLoyaltyCardId.Value),
            cancellationToken);
    }

    public Task<List<CustomerTransactionHistory>> GetCustomerTransactionHistoryAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        return context.CustomerTransactionHistory.AsNoTracking()
            .Where(x => x.CustomerId == customerId)
            .Include(x => x.Branch)
            .Include(x => x.POSReceipt)
            .Include(x => x.SalesReturn)
            .OrderByDescending(x => x.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public Task<List<LoyaltyPointTransaction>> GetLoyaltyTransactionsAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        return context.LoyaltyPointTransactions.AsNoTracking()
            .Where(x => x.CustomerId == customerId)
            .Include(x => x.LoyaltyCard)
            .Include(x => x.POSReceipt)
            .OrderByDescending(x => x.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public Task<List<CustomerNote>> GetCustomerNotesAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        return context.CustomerNotes.AsNoTracking()
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.NoteDate)
            .ToListAsync(cancellationToken);
    }
}
