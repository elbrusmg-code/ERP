using ERP.Business.Common.Interfaces.Repositories;
using ERP.Business.Common.Interfaces.Repositories.Specific;
using ERP.Business.Common.Models;
using ERP.Business.Features.Procurement.Dtos;
using ERP.Business.Features.Procurement.Interfaces;
using ERP.Core.Entities.Catalog;
using ERP.Core.Entities.Inventory;
using ERP.Core.Entities.Organization;
using ERP.Core.Entities.Procurement;

namespace ERP.Business.Features.Procurement.Services;

public sealed class ProcurementService(
    IProcurementRepository procurementRepository,
    IUnitOfWork unitOfWork) : IProcurementService
{
    public async Task<ServiceResult<List<SupplierDto>>> GetSuppliersAsync(
        CancellationToken cancellationToken = default)
    {
        var suppliers = await procurementRepository.GetActiveSuppliersAsync(cancellationToken);
        var supplierIds = suppliers.Select(x => x.Id).ToList();
        var contacts = supplierIds.Count == 0
            ? new List<SupplierContact>()
            : await unitOfWork.Repository<SupplierContact>().ListAsync(
                x => supplierIds.Contains(x.SupplierId) && !x.IsDeleted,
                cancellationToken);
        var contactsBySupplier = contacts
            .GroupBy(x => x.SupplierId)
            .ToDictionary(x => x.Key, x => x.OrderByDescending(contact => contact.IsPrimary).ThenBy(contact => contact.FullName).ToList());

        var data = suppliers
            .OrderBy(x => x.CompanyName)
            .Select(x => MapSupplier(
                x,
                contactsBySupplier.GetValueOrDefault(x.Id, new List<SupplierContact>())))
            .ToList();

        return ServiceResult<List<SupplierDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<SupplierDto>> GetSupplierByIdAsync(
        int supplierId,
        CancellationToken cancellationToken = default)
    {
        var supplier = await procurementRepository.GetSupplierDetailsAsync(supplierId, cancellationToken);

        if (supplier is null)
        {
            return ServiceResult<SupplierDto>.Failure("Supplier not found.");
        }

        return ServiceResult<SupplierDto>.SuccessResult(await MapSupplierDetailsAsync(supplier, cancellationToken));
    }

    public async Task<ServiceResult<List<PurchaseOrderDto>>> GetPurchaseOrdersAsync(
        int branchId,
        CancellationToken cancellationToken = default)
    {
        if (branchId <= 0)
        {
            return ServiceResult<List<PurchaseOrderDto>>.Failure("BranchId is required.");
        }

        var purchaseOrders = await procurementRepository.GetPurchaseOrdersByBranchAsync(branchId, cancellationToken);
        var data = new List<PurchaseOrderDto>();

        foreach (var purchaseOrder in purchaseOrders.OrderByDescending(x => x.OrderDate))
        {
            data.Add(await MapPurchaseOrderAsync(purchaseOrder, includeDetails: false, cancellationToken));
        }

        return ServiceResult<List<PurchaseOrderDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<PurchaseOrderDto>> GetPurchaseOrderByIdAsync(
        int purchaseOrderId,
        CancellationToken cancellationToken = default)
    {
        var purchaseOrder = await procurementRepository.GetPurchaseOrderDetailsAsync(purchaseOrderId, cancellationToken);

        if (purchaseOrder is null)
        {
            return ServiceResult<PurchaseOrderDto>.Failure("Purchase order not found.");
        }

        return ServiceResult<PurchaseOrderDto>.SuccessResult(
            await MapPurchaseOrderAsync(purchaseOrder, includeDetails: true, cancellationToken));
    }

    public async Task<ServiceResult<PurchaseOrderDto>> GetPurchaseOrderByNumberAsync(
        string orderNumber,
        CancellationToken cancellationToken = default)
    {
        var purchaseOrder = await procurementRepository.GetPurchaseOrderByNumberAsync(orderNumber, cancellationToken);

        if (purchaseOrder is null)
        {
            return ServiceResult<PurchaseOrderDto>.Failure("Purchase order not found.");
        }

        return ServiceResult<PurchaseOrderDto>.SuccessResult(
            await MapPurchaseOrderAsync(purchaseOrder, includeDetails: true, cancellationToken));
    }

    public async Task<ServiceResult<List<PurchaseOrderDto>>> GetPendingPurchaseOrdersAsync(
        int branchId,
        CancellationToken cancellationToken = default)
    {
        if (branchId <= 0)
        {
            return ServiceResult<List<PurchaseOrderDto>>.Failure("BranchId is required.");
        }

        var purchaseOrders = await procurementRepository.GetPendingPurchaseOrdersAsync(branchId, cancellationToken);
        var data = new List<PurchaseOrderDto>();

        foreach (var purchaseOrder in purchaseOrders)
        {
            data.Add(await MapPurchaseOrderAsync(purchaseOrder, includeDetails: false, cancellationToken));
        }

        return ServiceResult<List<PurchaseOrderDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<List<GoodsReceiptDto>>> GetGoodsReceiptsByPurchaseOrderAsync(
        int purchaseOrderId,
        CancellationToken cancellationToken = default)
    {
        var purchaseOrder = await procurementRepository.GetPurchaseOrderDetailsAsync(
            purchaseOrderId,
            cancellationToken);

        if (purchaseOrder is null)
        {
            return ServiceResult<List<GoodsReceiptDto>>.Failure("Purchase order not found.");
        }

        var goodsReceipts = await procurementRepository.GetGoodsReceiptsByPurchaseOrderAsync(
            purchaseOrderId,
            cancellationToken);
        var data = new List<GoodsReceiptDto>();

        foreach (var goodsReceipt in goodsReceipts.OrderByDescending(x => x.ReceivedDate))
        {
            data.Add(await MapGoodsReceiptAsync(goodsReceipt, cancellationToken));
        }

        return ServiceResult<List<GoodsReceiptDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<SupplierInvoiceDto>> GetSupplierInvoiceByIdAsync(
        int supplierInvoiceId,
        CancellationToken cancellationToken = default)
    {
        var invoice = await procurementRepository.GetSupplierInvoiceDetailsAsync(
            supplierInvoiceId,
            cancellationToken);

        if (invoice is null)
        {
            return ServiceResult<SupplierInvoiceDto>.Failure("Supplier invoice not found.");
        }

        return ServiceResult<SupplierInvoiceDto>.SuccessResult(await MapSupplierInvoiceAsync(invoice, cancellationToken));
    }

    public async Task<ServiceResult<List<SupplierInvoiceDto>>> GetUnpaidSupplierInvoicesAsync(
        int branchId,
        CancellationToken cancellationToken = default)
    {
        if (branchId <= 0)
        {
            return ServiceResult<List<SupplierInvoiceDto>>.Failure("BranchId is required.");
        }

        var invoices = await procurementRepository.GetUnpaidSupplierInvoicesAsync(branchId, cancellationToken);
        var data = new List<SupplierInvoiceDto>();

        foreach (var invoice in invoices.OrderBy(x => x.DueDate).ThenBy(x => x.InvoiceDate))
        {
            data.Add(await MapSupplierInvoiceAsync(invoice, cancellationToken));
        }

        return ServiceResult<List<SupplierInvoiceDto>>.SuccessResult(data);
    }

    public async Task<ServiceResult<List<SupplierPaymentDto>>> GetSupplierPaymentsByInvoiceAsync(
        int supplierInvoiceId,
        CancellationToken cancellationToken = default)
    {
        var invoice = await procurementRepository.GetSupplierInvoiceDetailsAsync(
            supplierInvoiceId,
            cancellationToken);

        if (invoice is null)
        {
            return ServiceResult<List<SupplierPaymentDto>>.Failure("Supplier invoice not found.");
        }

        var payments = await procurementRepository.GetSupplierPaymentsByInvoiceAsync(
            supplierInvoiceId,
            cancellationToken);
        var suppliers = await GetSuppliersDictionaryAsync(cancellationToken);
        var branches = await GetBranchesAsync(cancellationToken);

        var data = payments
            .OrderByDescending(x => x.PaymentDate)
            .Select(x => MapSupplierPayment(x, suppliers, branches))
            .ToList();

        return ServiceResult<List<SupplierPaymentDto>>.SuccessResult(data);
    }

    private async Task<SupplierDto> MapSupplierDetailsAsync(
        Supplier supplier,
        CancellationToken cancellationToken)
    {
        var contacts = supplier.Contacts.OrderByDescending(x => x.IsPrimary).ThenBy(x => x.FullName).ToList();
        var dto = MapSupplier(supplier, contacts);

        foreach (var purchaseOrder in supplier.PurchaseOrders.OrderByDescending(x => x.OrderDate))
        {
            dto.PurchaseOrders.Add(await MapPurchaseOrderAsync(purchaseOrder, includeDetails: false, cancellationToken));
        }

        foreach (var invoice in supplier.Invoices.OrderByDescending(x => x.InvoiceDate))
        {
            dto.Invoices.Add(await MapSupplierInvoiceAsync(invoice, cancellationToken));
        }

        var suppliers = await GetSuppliersDictionaryAsync(cancellationToken);
        var branches = await GetBranchesAsync(cancellationToken);
        var warehouses = await GetWarehousesAsync(cancellationToken);

        dto.Payments = supplier.Payments
            .OrderByDescending(x => x.PaymentDate)
            .Select(x => MapSupplierPayment(x, suppliers, branches))
            .ToList();
        dto.Returns = supplier.Returns
            .OrderByDescending(x => x.ReturnDate)
            .Select(x => MapSupplierReturn(x, suppliers, branches, warehouses))
            .ToList();
        dto.Notes = supplier.Notes
            .OrderByDescending(x => x.NoteDate)
            .Select(x => x.Note)
            .ToList();

        return dto;
    }

    private async Task<PurchaseOrderDto> MapPurchaseOrderAsync(
        PurchaseOrder purchaseOrder,
        bool includeDetails,
        CancellationToken cancellationToken)
    {
        var branches = await GetBranchesAsync(cancellationToken);
        var suppliers = await GetSuppliersDictionaryAsync(cancellationToken);
        var products = await GetProductsAsync(cancellationToken);

        var items = includeDetails && purchaseOrder.Items.Count == 0
            ? await unitOfWork.Repository<PurchaseOrderItem>().ListAsync(
                x => x.PurchaseOrderId == purchaseOrder.Id,
                cancellationToken)
            : purchaseOrder.Items.ToList();

        var dto = new PurchaseOrderDto
        {
            Id = purchaseOrder.Id,
            OrderNumber = purchaseOrder.OrderNumber,
            BranchId = purchaseOrder.BranchId,
            BranchName = purchaseOrder.Branch?.Name ??
                         branches.GetValueOrDefault(purchaseOrder.BranchId)?.Name ??
                         string.Empty,
            SupplierId = purchaseOrder.SupplierId,
            SupplierName = purchaseOrder.Supplier?.CompanyName ??
                           suppliers.GetValueOrDefault(purchaseOrder.SupplierId)?.CompanyName ??
                           string.Empty,
            Status = purchaseOrder.Status.ToString(),
            Priority = purchaseOrder.Priority.ToString(),
            OrderDate = purchaseOrder.OrderDate,
            ExpectedDeliveryDate = purchaseOrder.ExpectedDeliveryDate,
            SubTotal = purchaseOrder.SubTotal,
            TaxTotal = purchaseOrder.TaxTotal,
            DiscountTotal = purchaseOrder.DiscountTotal,
            GrandTotal = purchaseOrder.GrandTotal,
            Note = purchaseOrder.Note,
            ApprovedBy = purchaseOrder.ApprovedBy,
            ApprovedAt = purchaseOrder.ApprovedAt,
            SentAt = purchaseOrder.SentAt,
            Items = items
                .OrderBy(x => x.Id)
                .Select(x => MapPurchaseOrderItem(x, products))
                .ToList()
        };

        if (!includeDetails)
        {
            return dto;
        }

        var goodsReceipts = purchaseOrder.GoodsReceipts.Count > 0
            ? purchaseOrder.GoodsReceipts.ToList()
            : await procurementRepository.GetGoodsReceiptsByPurchaseOrderAsync(purchaseOrder.Id, cancellationToken);

        foreach (var goodsReceipt in goodsReceipts.OrderByDescending(x => x.ReceivedDate))
        {
            dto.GoodsReceipts.Add(await MapGoodsReceiptAsync(goodsReceipt, cancellationToken));
        }

        if (purchaseOrder.SupplierInvoice is not null)
        {
            dto.SupplierInvoice = await MapSupplierInvoiceAsync(purchaseOrder.SupplierInvoice, cancellationToken);
        }

        return dto;
    }

    private async Task<GoodsReceiptDto> MapGoodsReceiptAsync(
        GoodsReceipt goodsReceipt,
        CancellationToken cancellationToken)
    {
        var branches = await GetBranchesAsync(cancellationToken);
        var suppliers = await GetSuppliersDictionaryAsync(cancellationToken);
        var warehouses = await GetWarehousesAsync(cancellationToken);
        var purchaseOrders = await GetPurchaseOrdersDictionaryAsync(cancellationToken);
        var products = await GetProductsAsync(cancellationToken);
        var items = goodsReceipt.Items.Count > 0
            ? goodsReceipt.Items.ToList()
            : await unitOfWork.Repository<GoodsReceiptItem>().ListAsync(
                x => x.GoodsReceiptId == goodsReceipt.Id,
                cancellationToken);

        return new GoodsReceiptDto
        {
            Id = goodsReceipt.Id,
            ReceiptNumber = goodsReceipt.ReceiptNumber,
            BranchId = goodsReceipt.BranchId,
            BranchName = goodsReceipt.Branch?.Name ??
                         branches.GetValueOrDefault(goodsReceipt.BranchId)?.Name ??
                         string.Empty,
            WarehouseId = goodsReceipt.WarehouseId,
            WarehouseName = goodsReceipt.Warehouse?.Name ??
                            warehouses.GetValueOrDefault(goodsReceipt.WarehouseId)?.Name ??
                            string.Empty,
            SupplierId = goodsReceipt.SupplierId,
            SupplierName = goodsReceipt.Supplier?.CompanyName ??
                           suppliers.GetValueOrDefault(goodsReceipt.SupplierId)?.CompanyName ??
                           string.Empty,
            PurchaseOrderId = goodsReceipt.PurchaseOrderId,
            PurchaseOrderNumber = goodsReceipt.PurchaseOrder?.OrderNumber ??
                                  (goodsReceipt.PurchaseOrderId.HasValue
                                      ? purchaseOrders.GetValueOrDefault(goodsReceipt.PurchaseOrderId.Value)?.OrderNumber
                                      : null),
            Status = goodsReceipt.Status.ToString(),
            ReceivedDate = goodsReceipt.ReceivedDate,
            ReceivedBy = goodsReceipt.ReceivedBy,
            Note = goodsReceipt.Note,
            Items = items
                .OrderBy(x => x.Id)
                .Select(x => MapGoodsReceiptItem(x, products))
                .ToList()
        };
    }

    private async Task<SupplierInvoiceDto> MapSupplierInvoiceAsync(
        SupplierInvoice invoice,
        CancellationToken cancellationToken)
    {
        var suppliers = await GetSuppliersDictionaryAsync(cancellationToken);
        var branches = await GetBranchesAsync(cancellationToken);
        var purchaseOrders = await GetPurchaseOrdersDictionaryAsync(cancellationToken);
        var payments = invoice.Payments.Count > 0
            ? invoice.Payments.ToList()
            : await unitOfWork.Repository<SupplierPayment>().ListAsync(
                x => x.SupplierInvoiceId == invoice.Id,
                cancellationToken);

        return new SupplierInvoiceDto
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            SupplierId = invoice.SupplierId,
            SupplierName = invoice.Supplier?.CompanyName ??
                           suppliers.GetValueOrDefault(invoice.SupplierId)?.CompanyName ??
                           string.Empty,
            BranchId = invoice.BranchId,
            BranchName = invoice.Branch?.Name ??
                         branches.GetValueOrDefault(invoice.BranchId)?.Name ??
                         string.Empty,
            PurchaseOrderId = invoice.PurchaseOrderId,
            PurchaseOrderNumber = invoice.PurchaseOrder?.OrderNumber ??
                                  (invoice.PurchaseOrderId.HasValue
                                      ? purchaseOrders.GetValueOrDefault(invoice.PurchaseOrderId.Value)?.OrderNumber
                                      : null),
            Status = invoice.Status.ToString(),
            InvoiceDate = invoice.InvoiceDate,
            DueDate = invoice.DueDate,
            SubTotal = invoice.SubTotal,
            TaxTotal = invoice.TaxTotal,
            DiscountTotal = invoice.DiscountTotal,
            GrandTotal = invoice.GrandTotal,
            PaidAmount = invoice.PaidAmount,
            RemainingAmount = invoice.RemainingAmount,
            Note = invoice.Note,
            Payments = payments
                .OrderByDescending(x => x.PaymentDate)
                .Select(x => MapSupplierPayment(x, suppliers, branches))
                .ToList()
        };
    }

    private static SupplierDto MapSupplier(
        Supplier supplier,
        IReadOnlyCollection<SupplierContact> contacts)
    {
        return new SupplierDto
        {
            Id = supplier.Id,
            CompanyName = supplier.CompanyName,
            LegalName = supplier.LegalName,
            TaxNumber = supplier.TaxNumber,
            Email = supplier.Email,
            Phone = supplier.Phone,
            Address = supplier.Address,
            Status = supplier.Status.ToString(),
            IsActive = supplier.IsActive,
            Contacts = contacts
                .OrderByDescending(x => x.IsPrimary)
                .ThenBy(x => x.FullName)
                .Select(MapSupplierContact)
                .ToList()
        };
    }

    private static SupplierContactDto MapSupplierContact(SupplierContact contact)
    {
        return new SupplierContactDto
        {
            Id = contact.Id,
            FullName = contact.FullName,
            Position = contact.Position,
            Email = contact.Email,
            Phone = contact.Phone,
            IsPrimary = contact.IsPrimary,
            IsActive = contact.IsActive
        };
    }

    private static PurchaseOrderItemDto MapPurchaseOrderItem(
        PurchaseOrderItem item,
        IReadOnlyDictionary<int, Product> products)
    {
        var product = item.Product ?? products.GetValueOrDefault(item.ProductId);

        return new PurchaseOrderItemDto
        {
            Id = item.Id,
            ProductId = item.ProductId,
            ProductName = product?.Name ?? string.Empty,
            SKU = product?.SKU ?? string.Empty,
            OrderedQuantity = item.OrderedQuantity,
            ReceivedQuantity = item.ReceivedQuantity,
            UnitCost = item.UnitCost,
            DiscountAmount = item.DiscountAmount,
            TaxAmount = item.TaxAmount,
            LineTotal = item.LineTotal,
            Note = item.Note
        };
    }

    private static GoodsReceiptItemDto MapGoodsReceiptItem(
        GoodsReceiptItem item,
        IReadOnlyDictionary<int, Product> products)
    {
        var product = item.Product ?? products.GetValueOrDefault(item.ProductId);

        return new GoodsReceiptItemDto
        {
            Id = item.Id,
            ProductId = item.ProductId,
            ProductName = product?.Name ?? string.Empty,
            SKU = product?.SKU ?? string.Empty,
            PurchaseOrderItemId = item.PurchaseOrderItemId,
            ProductBatchId = item.ProductBatchId,
            BatchNumber = item.BatchNumber,
            ManufactureDate = item.ManufactureDate,
            ExpiryDate = item.ExpiryDate,
            ReceivedQuantity = item.ReceivedQuantity,
            UnitCost = item.UnitCost,
            LineTotal = item.LineTotal,
            Note = item.Note
        };
    }

    private static SupplierPaymentDto MapSupplierPayment(
        SupplierPayment payment,
        IReadOnlyDictionary<int, Supplier> suppliers,
        IReadOnlyDictionary<int, Branch> branches)
    {
        return new SupplierPaymentDto
        {
            Id = payment.Id,
            PaymentNumber = payment.PaymentNumber,
            SupplierId = payment.SupplierId,
            SupplierName = payment.Supplier?.CompanyName ??
                           suppliers.GetValueOrDefault(payment.SupplierId)?.CompanyName ??
                           string.Empty,
            BranchId = payment.BranchId,
            BranchName = payment.Branch?.Name ??
                         branches.GetValueOrDefault(payment.BranchId)?.Name ??
                         string.Empty,
            SupplierInvoiceId = payment.SupplierInvoiceId,
            Method = payment.Method.ToString(),
            Status = payment.Status.ToString(),
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            ReferenceNumber = payment.ReferenceNumber,
            Note = payment.Note
        };
    }

    private static SupplierReturnDto MapSupplierReturn(
        SupplierReturn supplierReturn,
        IReadOnlyDictionary<int, Supplier> suppliers,
        IReadOnlyDictionary<int, Branch> branches,
        IReadOnlyDictionary<int, Warehouse> warehouses)
    {
        return new SupplierReturnDto
        {
            Id = supplierReturn.Id,
            ReturnNumber = supplierReturn.ReturnNumber,
            SupplierId = supplierReturn.SupplierId,
            SupplierName = supplierReturn.Supplier?.CompanyName ??
                           suppliers.GetValueOrDefault(supplierReturn.SupplierId)?.CompanyName ??
                           string.Empty,
            BranchId = supplierReturn.BranchId,
            BranchName = supplierReturn.Branch?.Name ??
                         branches.GetValueOrDefault(supplierReturn.BranchId)?.Name ??
                         string.Empty,
            WarehouseId = supplierReturn.WarehouseId,
            WarehouseName = supplierReturn.Warehouse?.Name ??
                            warehouses.GetValueOrDefault(supplierReturn.WarehouseId)?.Name ??
                            string.Empty,
            Status = supplierReturn.Status.ToString(),
            ReturnDate = supplierReturn.ReturnDate,
            TotalAmount = supplierReturn.TotalAmount,
            Reason = supplierReturn.Reason,
            ApprovedBy = supplierReturn.ApprovedBy,
            ApprovedAt = supplierReturn.ApprovedAt,
            Note = supplierReturn.Note
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

    private async Task<Dictionary<int, Supplier>> GetSuppliersDictionaryAsync(
        CancellationToken cancellationToken)
    {
        return (await unitOfWork.Repository<Supplier>().ListAsync(
                x => !x.IsDeleted,
                cancellationToken))
            .ToDictionary(x => x.Id);
    }

    private async Task<Dictionary<int, Warehouse>> GetWarehousesAsync(
        CancellationToken cancellationToken)
    {
        return (await unitOfWork.Repository<Warehouse>().ListAsync(
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

    private async Task<Dictionary<int, PurchaseOrder>> GetPurchaseOrdersDictionaryAsync(
        CancellationToken cancellationToken)
    {
        return (await unitOfWork.Repository<PurchaseOrder>().ListAsync(
                x => !x.IsDeleted,
                cancellationToken))
            .ToDictionary(x => x.Id);
    }
}
