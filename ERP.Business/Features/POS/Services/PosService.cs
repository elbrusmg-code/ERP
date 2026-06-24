using ERP.Business.Common.Interfaces.Repositories;
using ERP.Business.Common.Interfaces.Repositories.Specific;
using ERP.Business.Common.Models;
using ERP.Business.Features.POS.Dtos;
using ERP.Business.Features.POS.Interfaces;
using ERP.Core.Entities.Catalog;
using ERP.Core.Entities.HR;
using ERP.Core.Entities.Organization;
using ERP.Core.Entities.POS;
using ERP.Core.Enums;

namespace ERP.Business.Features.POS.Services;

public sealed class PosService(
    IPosRepository posRepository,
    IUnitOfWork unitOfWork) : IPosService
{
    public async Task<ServiceResult<List<CashRegisterDto>>> GetCashRegistersAsync(
        int? branchId = null,
        CancellationToken cancellationToken = default)
    {
        var cashRegisters = branchId.HasValue
            ? await posRepository.GetCashRegistersByBranchAsync(branchId.Value, cancellationToken)
            : await unitOfWork.Repository<CashRegister>().ListAsync(
                x => x.IsActive && !x.IsDeleted,
                cancellationToken);
        var branches = await GetBranchesAsync(cancellationToken);

        var data = cashRegisters
            .OrderBy(x => x.BranchId)
            .ThenBy(x => x.Name)
            .Select(x => MapCashRegister(x, branches))
            .ToList();

        return ServiceResult<List<CashRegisterDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<CashRegisterDto>> GetCashRegisterByIdAsync(
        int cashRegisterId,
        CancellationToken cancellationToken = default)
    {
        var cashRegister = await posRepository.GetCashRegisterDetailsAsync(cashRegisterId, cancellationToken);

        if (cashRegister is null)
        {
            return ServiceResult<CashRegisterDto>.Failure("Cash register not found.");
        }

        var branches = await GetBranchesAsync(cancellationToken);
        return ServiceResult<CashRegisterDto>.SuccessResult(MapCashRegister(cashRegister, branches));
    }

    public async Task<ServiceResult<List<CashShiftDto>>> GetOpenShiftsAsync(
        int? branchId = null,
        CancellationToken cancellationToken = default)
    {
        var shifts = branchId.HasValue
            ? await posRepository.GetOpenShiftsByBranchAsync(branchId.Value, cancellationToken)
            : await unitOfWork.Repository<CashShift>().ListAsync(
                x => x.Status == CashShiftStatus.Open && !x.IsDeleted,
                cancellationToken);

        var data = await MapCashShiftsAsync(shifts, cancellationToken);
        return ServiceResult<List<CashShiftDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<List<POSReceiptDto>>> GetReceiptsByShiftAsync(
        int cashShiftId,
        CancellationToken cancellationToken = default)
    {
        var receipts = await posRepository.GetReceiptsByShiftAsync(cashShiftId, cancellationToken);
        var data = new List<POSReceiptDto>();

        foreach (var receipt in receipts.OrderByDescending(x => x.ReceiptDate))
        {
            data.Add(await MapReceiptAsync(receipt, cancellationToken));
        }

        return ServiceResult<List<POSReceiptDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<POSReceiptDto>> GetReceiptByIdAsync(
        int receiptId,
        CancellationToken cancellationToken = default)
    {
        var receipt = await posRepository.GetReceiptDetailsAsync(receiptId, cancellationToken);

        if (receipt is null)
        {
            return ServiceResult<POSReceiptDto>.Failure("Receipt not found.");
        }

        return ServiceResult<POSReceiptDto>.SuccessResult(await MapReceiptAsync(receipt, cancellationToken));
    }

    public async Task<ServiceResult<POSReceiptDto>> GetReceiptByNumberAsync(
        string receiptNumber,
        CancellationToken cancellationToken = default)
    {
        var receipt = await posRepository.GetReceiptByNumberAsync(receiptNumber, cancellationToken);

        if (receipt is null)
        {
            return ServiceResult<POSReceiptDto>.Failure("Receipt not found.");
        }

        return ServiceResult<POSReceiptDto>.SuccessResult(await MapReceiptAsync(receipt, cancellationToken));
    }

    public async Task<ServiceResult<List<POSReceiptDto>>> GetReceiptsAsync(
        int branchId,
        DateTime? date = null,
        CancellationToken cancellationToken = default)
    {
        if (branchId <= 0)
        {
            return ServiceResult<List<POSReceiptDto>>.Failure("BranchId is required.");
        }

        var receiptDate = (date ?? DateTime.Today).Date;
        var receipts = await posRepository.GetReceiptsByBranchAndDateAsync(
            branchId,
            receiptDate,
            cancellationToken);
        var data = new List<POSReceiptDto>();

        foreach (var receipt in receipts.OrderByDescending(x => x.ReceiptDate))
        {
            data.Add(await MapReceiptAsync(receipt, cancellationToken));
        }

        return ServiceResult<List<POSReceiptDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<List<SalesReturnDto>>> GetReturnsAsync(
        int branchId,
        DateTime? date = null,
        CancellationToken cancellationToken = default)
    {
        if (branchId <= 0)
        {
            return ServiceResult<List<SalesReturnDto>>.Failure("BranchId is required.");
        }

        var returnDate = (date ?? DateTime.Today).Date;
        var returns = await posRepository.GetReturnsByBranchAndDateAsync(
            branchId,
            returnDate,
            cancellationToken);
        var data = new List<SalesReturnDto>();

        foreach (var salesReturn in returns.OrderByDescending(x => x.ReturnDate))
        {
            data.Add(await MapReturnAsync(salesReturn, cancellationToken));
        }

        return ServiceResult<List<SalesReturnDto>>.SuccessResult(data);
    }

    private async Task<List<CashShiftDto>> MapCashShiftsAsync(
        IReadOnlyCollection<CashShift> shifts,
        CancellationToken cancellationToken)
    {
        var branches = await GetBranchesAsync(cancellationToken);
        var cashRegisters = await GetCashRegistersDictionaryAsync(cancellationToken);
        var employees = await GetEmployeesAsync(cancellationToken);

        return shifts
            .OrderBy(x => x.BranchId)
            .ThenBy(x => x.OpenedAt)
            .Select(x => new CashShiftDto
            {
                Id = x.Id,
                ShiftNumber = x.ShiftNumber,
                BranchId = x.BranchId,
                BranchName = x.Branch?.Name ?? branches.GetValueOrDefault(x.BranchId)?.Name ?? string.Empty,
                CashRegisterId = x.CashRegisterId,
                CashRegisterCode = x.CashRegister?.Code ??
                                   cashRegisters.GetValueOrDefault(x.CashRegisterId)?.Code ??
                                   string.Empty,
                CashierEmployeeId = x.CashierEmployeeId,
                CashierName = GetEmployeeName(
                    x.CashierEmployee ?? employees.GetValueOrDefault(x.CashierEmployeeId)),
                Status = x.Status.ToString(),
                OpenedAt = x.OpenedAt,
                ClosedAt = x.ClosedAt,
                OpeningCashAmount = x.OpeningCashAmount,
                ExpectedCashAmount = x.ExpectedCashAmount,
                ActualCashAmount = x.ActualCashAmount,
                CashDifference = x.CashDifference,
                TotalCashSales = x.TotalCashSales,
                TotalCardSales = x.TotalCardSales,
                TotalMixedSales = x.TotalMixedSales,
                TotalRefunds = x.TotalRefunds
            })
            .ToList();
    }

    private async Task<POSReceiptDto> MapReceiptAsync(
        POSReceipt receipt,
        CancellationToken cancellationToken)
    {
        var branches = await GetBranchesAsync(cancellationToken);
        var cashRegisters = await GetCashRegistersDictionaryAsync(cancellationToken);
        var employees = await GetEmployeesAsync(cancellationToken);

        var receiptItems = receipt.Items.Count > 0
            ? receipt.Items.ToList()
            : await unitOfWork.Repository<POSReceiptItem>().ListAsync(
                x => x.POSReceiptId == receipt.Id,
                cancellationToken);
        var payments = receipt.Payments.Count > 0
            ? receipt.Payments.ToList()
            : await unitOfWork.Repository<POSPayment>().ListAsync(
                x => x.POSReceiptId == receipt.Id,
                cancellationToken);
        var returns = await unitOfWork.Repository<SalesReturn>().ListAsync(
            x => x.OriginalReceiptId == receipt.Id && !x.IsDeleted,
            cancellationToken);

        var paymentDtos = await MapPaymentsAsync(payments, cancellationToken);
        var returnDtos = new List<SalesReturnDto>();

        foreach (var salesReturn in returns.OrderByDescending(x => x.ReturnDate))
        {
            returnDtos.Add(await MapReturnAsync(salesReturn, cancellationToken));
        }

        return new POSReceiptDto
        {
            Id = receipt.Id,
            ReceiptNumber = receipt.ReceiptNumber,
            BranchId = receipt.BranchId,
            BranchName = receipt.Branch?.Name ?? branches.GetValueOrDefault(receipt.BranchId)?.Name ?? string.Empty,
            CashRegisterId = receipt.CashRegisterId,
            CashRegisterCode = receipt.CashRegister?.Code ??
                               cashRegisters.GetValueOrDefault(receipt.CashRegisterId)?.Code ??
                               string.Empty,
            CashShiftId = receipt.CashShiftId,
            CashierEmployeeId = receipt.CashierEmployeeId,
            CashierName = GetEmployeeName(
                receipt.CashierEmployee ?? employees.GetValueOrDefault(receipt.CashierEmployeeId)),
            Status = receipt.Status.ToString(),
            ReceiptDate = receipt.ReceiptDate,
            SubTotal = receipt.SubTotal,
            DiscountTotal = receipt.DiscountTotal,
            TaxTotal = receipt.TaxTotal,
            GrandTotal = receipt.GrandTotal,
            PaidAmount = receipt.PaidAmount,
            ChangeAmount = receipt.ChangeAmount,
            DiscountType = receipt.DiscountType.ToString(),
            CustomerPhone = receipt.CustomerPhone,
            CustomerName = receipt.CustomerName,
            Note = receipt.Note,
            Items = receiptItems
                .OrderBy(x => x.Id)
                .Select(MapReceiptItem)
                .ToList(),
            Payments = paymentDtos,
            Returns = returnDtos
        };
    }

    private async Task<List<POSPaymentDto>> MapPaymentsAsync(
        IReadOnlyCollection<POSPayment> payments,
        CancellationToken cancellationToken)
    {
        if (payments.Count == 0)
        {
            return new List<POSPaymentDto>();
        }

        var paymentIds = payments.Select(x => x.Id).ToList();
        var terminalTransactions = await unitOfWork.Repository<PaymentTerminalTransaction>().ListAsync(
            x => paymentIds.Contains(x.POSPaymentId),
            cancellationToken);
        var transactionsByPayment = terminalTransactions
            .GroupBy(x => x.POSPaymentId)
            .ToDictionary(x => x.Key, x => x.OrderBy(transaction => transaction.RequestedAt).ToList());

        return payments
            .OrderBy(x => x.PaymentDate)
            .Select(x => new POSPaymentDto
            {
                Id = x.Id,
                PaymentNumber = x.PaymentNumber,
                Method = x.Method.ToString(),
                Status = x.Status.ToString(),
                Amount = x.Amount,
                PaymentDate = x.PaymentDate,
                ReferenceNumber = x.ReferenceNumber,
                Note = x.Note,
                TerminalTransactions = transactionsByPayment
                    .GetValueOrDefault(x.Id, new List<PaymentTerminalTransaction>())
                    .Select(MapTerminalTransaction)
                    .ToList()
            })
            .ToList();
    }

    private async Task<SalesReturnDto> MapReturnAsync(
        SalesReturn salesReturn,
        CancellationToken cancellationToken)
    {
        var branches = await GetBranchesAsync(cancellationToken);
        var cashRegisters = await GetCashRegistersDictionaryAsync(cancellationToken);
        var employees = await GetEmployeesAsync(cancellationToken);
        var products = await GetProductsAsync(cancellationToken);
        var returnItems = salesReturn.Items.Count > 0
            ? salesReturn.Items.ToList()
            : await unitOfWork.Repository<SalesReturnItem>().ListAsync(
                x => x.SalesReturnId == salesReturn.Id,
                cancellationToken);

        return new SalesReturnDto
        {
            Id = salesReturn.Id,
            ReturnNumber = salesReturn.ReturnNumber,
            OriginalReceiptId = salesReturn.OriginalReceiptId,
            BranchId = salesReturn.BranchId,
            BranchName = salesReturn.Branch?.Name ??
                         branches.GetValueOrDefault(salesReturn.BranchId)?.Name ??
                         string.Empty,
            CashRegisterId = salesReturn.CashRegisterId,
            CashRegisterCode = salesReturn.CashRegister?.Code ??
                               cashRegisters.GetValueOrDefault(salesReturn.CashRegisterId)?.Code ??
                               string.Empty,
            CashShiftId = salesReturn.CashShiftId,
            CashierEmployeeId = salesReturn.CashierEmployeeId,
            CashierName = GetEmployeeName(
                salesReturn.CashierEmployee ?? employees.GetValueOrDefault(salesReturn.CashierEmployeeId)),
            Status = salesReturn.Status.ToString(),
            ReturnDate = salesReturn.ReturnDate,
            TotalReturnAmount = salesReturn.TotalReturnAmount,
            RefundMethod = salesReturn.RefundMethod.ToString(),
            Reason = salesReturn.Reason,
            ApprovedBy = salesReturn.ApprovedBy,
            ApprovedAt = salesReturn.ApprovedAt,
            Items = returnItems
                .OrderBy(x => x.Id)
                .Select(x => MapReturnItem(x, products))
                .ToList()
        };
    }

    private static CashRegisterDto MapCashRegister(
        CashRegister cashRegister,
        IReadOnlyDictionary<int, Branch> branches)
    {
        return new CashRegisterDto
        {
            Id = cashRegister.Id,
            Name = cashRegister.Name,
            Code = cashRegister.Code,
            Description = cashRegister.Description,
            Status = cashRegister.Status.ToString(),
            IsActive = cashRegister.IsActive,
            BranchId = cashRegister.BranchId,
            BranchName = cashRegister.Branch?.Name ??
                         branches.GetValueOrDefault(cashRegister.BranchId)?.Name ??
                         string.Empty
        };
    }

    private static POSReceiptItemDto MapReceiptItem(POSReceiptItem item)
    {
        return new POSReceiptItemDto
        {
            Id = item.Id,
            ProductId = item.ProductId,
            ProductNameSnapshot = item.ProductNameSnapshot,
            SKUSnapshot = item.SKUSnapshot,
            BarcodeSnapshot = item.BarcodeSnapshot,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            DiscountAmount = item.DiscountAmount,
            TaxAmount = item.TaxAmount,
            LineTotal = item.LineTotal,
            IsReturned = item.IsReturned,
            ReturnedQuantity = item.ReturnedQuantity
        };
    }

    private static PaymentTerminalTransactionDto MapTerminalTransaction(
        PaymentTerminalTransaction transaction)
    {
        return new PaymentTerminalTransactionDto
        {
            Id = transaction.Id,
            TerminalTransactionNumber = transaction.TerminalTransactionNumber,
            Status = transaction.Status.ToString(),
            TerminalId = transaction.TerminalId,
            TerminalName = transaction.TerminalName,
            BankName = transaction.BankName,
            AuthorizationCode = transaction.AuthorizationCode,
            RRN = transaction.RRN,
            CardLastFourDigits = transaction.CardLastFourDigits,
            Amount = transaction.Amount,
            RequestedAt = transaction.RequestedAt,
            RespondedAt = transaction.RespondedAt,
            ResponseMessage = transaction.ResponseMessage
        };
    }

    private static SalesReturnItemDto MapReturnItem(
        SalesReturnItem item,
        IReadOnlyDictionary<int, Product> products)
    {
        return new SalesReturnItemDto
        {
            Id = item.Id,
            POSReceiptItemId = item.POSReceiptItemId,
            ProductId = item.ProductId,
            ProductName = item.Product?.Name ??
                          products.GetValueOrDefault(item.ProductId)?.Name ??
                          string.Empty,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            ReturnAmount = item.ReturnAmount,
            RestockToInventory = item.RestockToInventory,
            MarkAsDamaged = item.MarkAsDamaged,
            Reason = item.Reason
        };
    }

    private async Task<Dictionary<int, Branch>> GetBranchesAsync(
        CancellationToken cancellationToken)
    {
        return (await unitOfWork.Repository<Branch>().ListAsync(
                x => !x.IsDeleted,
                cancellationToken))
            .ToDictionary(x => x.Id);
    }

    private async Task<Dictionary<int, CashRegister>> GetCashRegistersDictionaryAsync(
        CancellationToken cancellationToken)
    {
        return (await unitOfWork.Repository<CashRegister>().ListAsync(
                x => !x.IsDeleted,
                cancellationToken))
            .ToDictionary(x => x.Id);
    }

    private async Task<Dictionary<int, Employee>> GetEmployeesAsync(
        CancellationToken cancellationToken)
    {
        return (await unitOfWork.Repository<Employee>().ListAsync(
                x => !x.IsDeleted,
                cancellationToken))
            .ToDictionary(x => x.Id);
    }

    private async Task<Dictionary<int, Product>> GetProductsAsync(
        CancellationToken cancellationToken)
    {
        return (await unitOfWork.Repository<Product>().ListAsync(
                x => !x.IsDeleted,
                cancellationToken))
            .ToDictionary(x => x.Id);
    }

    private static string GetEmployeeName(Employee? employee)
    {
        if (employee is null)
        {
            return string.Empty;
        }

        var name = $"{employee.FirstName} {employee.LastName}".Trim();
        return string.IsNullOrWhiteSpace(name) ? employee.EmployeeCode : name;
    }
}
