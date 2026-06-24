using ERP.Business.Common.Interfaces.Repositories.Specific;
using ERP.Business.Common.Models;
using ERP.Business.Features.CRM.Dtos;
using ERP.Business.Features.CRM.Interfaces;
using ERP.Core.Entities.CRM;

namespace ERP.Business.Features.CRM.Services;

public sealed class CrmService(ICrmRepository crmRepository) : ICrmService
{
    public async Task<ServiceResult<List<CustomerGroupDto>>> GetCustomerGroupsAsync(
        CancellationToken cancellationToken = default)
    {
        var groups = await crmRepository.GetActiveCustomerGroupsAsync(cancellationToken);
        var data = groups
            .OrderBy(x => x.Name)
            .Select(MapCustomerGroup)
            .ToList();

        return ServiceResult<List<CustomerGroupDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<CustomerDto>> GetCustomerByIdAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        var customer = await crmRepository.GetCustomerDetailsAsync(customerId, cancellationToken);

        return customer is null
            ? ServiceResult<CustomerDto>.Failure("Customer not found.")
            : ServiceResult<CustomerDto>.SuccessResult(MapCustomer(customer));
    }

    public async Task<ServiceResult<CustomerDto>> GetCustomerByPhoneAsync(
        string phone,
        CancellationToken cancellationToken = default)
    {
        var customer = await crmRepository.GetCustomerByPhoneAsync(phone, cancellationToken);
        return await GetHydratedCustomerResultAsync(customer, cancellationToken);
    }

    public async Task<ServiceResult<CustomerDto>> GetCustomerByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var customer = await crmRepository.GetCustomerByEmailAsync(email, cancellationToken);
        return await GetHydratedCustomerResultAsync(customer, cancellationToken);
    }

    public async Task<ServiceResult<CustomerDto>> GetCustomerByLoyaltyCardAsync(
        string cardNumber,
        CancellationToken cancellationToken = default)
    {
        var customer = await crmRepository.GetCustomerByLoyaltyCardAsync(cardNumber, cancellationToken);
        return await GetHydratedCustomerResultAsync(customer, cancellationToken);
    }

    public async Task<ServiceResult<List<CustomerTransactionHistoryDto>>> GetCustomerTransactionsAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        if (!await CustomerExistsAsync(customerId, cancellationToken))
        {
            return ServiceResult<List<CustomerTransactionHistoryDto>>.Failure("Customer not found.");
        }

        var transactions = await crmRepository.GetCustomerTransactionHistoryAsync(
            customerId,
            cancellationToken);
        var data = transactions
            .OrderByDescending(x => x.TransactionDate)
            .Select(MapCustomerTransactionHistory)
            .ToList();

        return ServiceResult<List<CustomerTransactionHistoryDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<List<LoyaltyPointTransactionDto>>> GetLoyaltyTransactionsAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        if (!await CustomerExistsAsync(customerId, cancellationToken))
        {
            return ServiceResult<List<LoyaltyPointTransactionDto>>.Failure("Customer not found.");
        }

        var transactions = await crmRepository.GetLoyaltyTransactionsAsync(
            customerId,
            cancellationToken);
        var data = transactions
            .OrderByDescending(x => x.TransactionDate)
            .Select(MapLoyaltyPointTransaction)
            .ToList();

        return ServiceResult<List<LoyaltyPointTransactionDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<List<CustomerNoteDto>>> GetCustomerNotesAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        if (!await CustomerExistsAsync(customerId, cancellationToken))
        {
            return ServiceResult<List<CustomerNoteDto>>.Failure("Customer not found.");
        }

        var notes = await crmRepository.GetCustomerNotesAsync(customerId, cancellationToken);
        var data = notes
            .OrderByDescending(x => x.NoteDate)
            .Select(MapCustomerNote)
            .ToList();

        return ServiceResult<List<CustomerNoteDto>>.SuccessResult(data);
    }

    private async Task<ServiceResult<CustomerDto>> GetHydratedCustomerResultAsync(
        Customer? customer,
        CancellationToken cancellationToken)
    {
        if (customer is null)
        {
            return ServiceResult<CustomerDto>.Failure("Customer not found.");
        }

        var detailedCustomer = await crmRepository.GetCustomerDetailsAsync(customer.Id, cancellationToken);

        return detailedCustomer is null
            ? ServiceResult<CustomerDto>.Failure("Customer not found.")
            : ServiceResult<CustomerDto>.SuccessResult(MapCustomer(detailedCustomer));
    }

    private async Task<bool> CustomerExistsAsync(
        int customerId,
        CancellationToken cancellationToken)
    {
        return await crmRepository.GetCustomerDetailsAsync(customerId, cancellationToken) is not null;
    }

    private static CustomerGroupDto MapCustomerGroup(CustomerGroup group)
    {
        return new CustomerGroupDto
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            DefaultDiscountPercent = group.DefaultDiscountPercent,
            IsActive = group.IsActive
        };
    }

    private static CustomerDto MapCustomer(Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            CustomerCode = customer.CustomerCode,
            FullName = customer.FullName,
            Phone = customer.Phone,
            Email = customer.Email,
            TaxNumber = customer.TaxNumber,
            Type = customer.Type.ToString(),
            Status = customer.Status.ToString(),
            CustomerGroupId = customer.CustomerGroupId,
            CustomerGroupName = customer.CustomerGroup?.Name,
            DateOfBirth = customer.DateOfBirth,
            IsLoyaltyEnabled = customer.IsLoyaltyEnabled,
            CurrentLoyaltyPoints = customer.CurrentLoyaltyPoints,
            TotalSpent = customer.TotalSpent,
            LastPurchaseDate = customer.LastPurchaseDate,
            RegisteredBranchId = customer.RegisteredBranchId,
            RegisteredBranchName = customer.RegisteredBranch?.Name,
            Addresses = customer.Addresses
                .OrderByDescending(x => x.IsDefault)
                .ThenBy(x => x.Title)
                .Select(MapCustomerAddress)
                .ToList(),
            LoyaltyCards = customer.LoyaltyCards
                .OrderByDescending(x => x.IsPrimary)
                .ThenBy(x => x.CardNumber)
                .Select(MapLoyaltyCard)
                .ToList(),
            Notes = customer.Notes
                .OrderByDescending(x => x.NoteDate)
                .Select(MapCustomerNote)
                .ToList(),
            Interactions = customer.Interactions
                .OrderByDescending(x => x.InteractionDate)
                .Select(MapCustomerInteraction)
                .ToList(),
            TransactionHistory = customer.TransactionHistory
                .OrderByDescending(x => x.TransactionDate)
                .Select(MapCustomerTransactionHistory)
                .ToList()
        };
    }

    private static CustomerAddressDto MapCustomerAddress(CustomerAddress address)
    {
        return new CustomerAddressDto
        {
            Id = address.Id,
            Title = address.Title,
            AddressLine = address.AddressLine,
            City = address.City,
            PostalCode = address.PostalCode,
            IsDefault = address.IsDefault,
            IsActive = address.IsActive
        };
    }

    private static LoyaltyCardDto MapLoyaltyCard(LoyaltyCard card)
    {
        return new LoyaltyCardDto
        {
            Id = card.Id,
            CardNumber = card.CardNumber,
            Status = card.Status.ToString(),
            IssuedAt = card.IssuedAt,
            ExpiryDate = card.ExpiryDate,
            PointsBalance = card.PointsBalance,
            IsPrimary = card.IsPrimary,
            Note = card.Note
        };
    }

    private static LoyaltyPointTransactionDto MapLoyaltyPointTransaction(
        LoyaltyPointTransaction transaction)
    {
        return new LoyaltyPointTransactionDto
        {
            Id = transaction.Id,
            TransactionNumber = transaction.TransactionNumber,
            CustomerId = transaction.CustomerId,
            LoyaltyCardId = transaction.LoyaltyCardId,
            LoyaltyCardNumber = transaction.LoyaltyCard?.CardNumber,
            POSReceiptId = transaction.POSReceiptId,
            POSReceiptNumber = transaction.POSReceipt?.ReceiptNumber,
            Type = transaction.Type.ToString(),
            Points = transaction.Points,
            BalanceAfter = transaction.BalanceAfter,
            TransactionDate = transaction.TransactionDate,
            ReferenceNumber = transaction.ReferenceNumber,
            Note = transaction.Note
        };
    }

    private static CustomerNoteDto MapCustomerNote(CustomerNote note)
    {
        return new CustomerNoteDto
        {
            Id = note.Id,
            Note = note.Note,
            NoteDate = note.NoteDate,
            CreatedByUserId = note.CreatedByUserId
        };
    }

    private static CustomerInteractionDto MapCustomerInteraction(CustomerInteraction interaction)
    {
        return new CustomerInteractionDto
        {
            Id = interaction.Id,
            Type = interaction.Type.ToString(),
            InteractionDate = interaction.InteractionDate,
            Subject = interaction.Subject,
            Description = interaction.Description,
            CreatedByUserId = interaction.CreatedByUserId
        };
    }

    private static CustomerTransactionHistoryDto MapCustomerTransactionHistory(
        CustomerTransactionHistory transaction)
    {
        return new CustomerTransactionHistoryDto
        {
            Id = transaction.Id,
            TransactionNumber = transaction.TransactionNumber,
            CustomerId = transaction.CustomerId,
            BranchId = transaction.BranchId,
            BranchName = transaction.Branch?.Name ?? string.Empty,
            Type = transaction.Type.ToString(),
            POSReceiptId = transaction.POSReceiptId,
            POSReceiptNumber = transaction.POSReceipt?.ReceiptNumber,
            SalesReturnId = transaction.SalesReturnId,
            SalesReturnNumber = transaction.SalesReturn?.ReturnNumber,
            Amount = transaction.Amount,
            TransactionDate = transaction.TransactionDate,
            ReferenceNumber = transaction.ReferenceNumber,
            Note = transaction.Note
        };
    }
}
