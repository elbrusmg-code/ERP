using ERP.Business.Common.Interfaces.Repositories.Specific;
using ERP.Core.Entities.Procurement;
using ERP.Core.Enums;
using ERP.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories.Specific;

public sealed class ProcurementRepository(ERPDbContext context) : IProcurementRepository
{
    public Task<List<Supplier>> GetActiveSuppliersAsync(CancellationToken cancellationToken = default)
    {
        return context.Suppliers.AsNoTracking()
            .Where(x => x.IsActive && x.Status == SupplierStatus.Active && !x.IsDeleted)
            .OrderBy(x => x.CompanyName)
            .ToListAsync(cancellationToken);
    }

    public Task<Supplier?> GetSupplierDetailsAsync(int supplierId, CancellationToken cancellationToken = default)
    {
        return context.Suppliers.AsNoTracking()
            .AsSplitQuery()
            .Where(x => !x.IsDeleted)
            .Include(x => x.Contacts.Where(contact => !contact.IsDeleted))
            .Include(x => x.PurchaseOrders.Where(order => !order.IsDeleted))
            .Include(x => x.Invoices.Where(invoice => !invoice.IsDeleted))
            .Include(x => x.Payments)
            .Include(x => x.Returns.Where(supplierReturn => !supplierReturn.IsDeleted))
            .Include(x => x.Notes)
            .FirstOrDefaultAsync(x => x.Id == supplierId, cancellationToken);
    }

    public Task<bool> SupplierTaxNumberExistsAsync(
        string taxNumber,
        int? excludeSupplierId = null,
        CancellationToken cancellationToken = default)
    {
        return context.Suppliers.AsNoTracking().AnyAsync(
            x => x.TaxNumber == taxNumber &&
                 !x.IsDeleted &&
                 (!excludeSupplierId.HasValue || x.Id != excludeSupplierId.Value),
            cancellationToken);
    }

    public Task<List<PurchaseOrder>> GetPurchaseOrdersByBranchAsync(int branchId, CancellationToken cancellationToken = default)
    {
        return context.PurchaseOrders.AsNoTracking()
            .Where(x => x.BranchId == branchId && !x.IsDeleted)
            .Include(x => x.Supplier)
            .OrderByDescending(x => x.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public Task<PurchaseOrder?> GetPurchaseOrderDetailsAsync(int purchaseOrderId, CancellationToken cancellationToken = default)
    {
        return PurchaseOrderDetailsQuery()
            .FirstOrDefaultAsync(x => x.Id == purchaseOrderId, cancellationToken);
    }

    public Task<PurchaseOrder?> GetPurchaseOrderByNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        return PurchaseOrderDetailsQuery()
            .FirstOrDefaultAsync(x => x.OrderNumber == orderNumber, cancellationToken);
    }

    public Task<List<PurchaseOrder>> GetPendingPurchaseOrdersAsync(int branchId, CancellationToken cancellationToken = default)
    {
        var pendingStatuses = new[]
        {
            PurchaseOrderStatus.Draft,
            PurchaseOrderStatus.PendingApproval,
            PurchaseOrderStatus.Approved,
            PurchaseOrderStatus.Sent,
            PurchaseOrderStatus.PartiallyReceived
        };

        return context.PurchaseOrders.AsNoTracking()
            .Where(x => x.BranchId == branchId && pendingStatuses.Contains(x.Status) && !x.IsDeleted)
            .Include(x => x.Supplier)
            .OrderBy(x => x.ExpectedDeliveryDate)
            .ThenBy(x => x.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public Task<List<GoodsReceipt>> GetGoodsReceiptsByPurchaseOrderAsync(
        int purchaseOrderId,
        CancellationToken cancellationToken = default)
    {
        return context.GoodsReceipts.AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.PurchaseOrderId == purchaseOrderId && !x.IsDeleted)
            .Include(x => x.Warehouse)
            .Include(x => x.Items)
                .ThenInclude(x => x.Product)
            .OrderByDescending(x => x.ReceivedDate)
            .ToListAsync(cancellationToken);
    }

    public Task<SupplierInvoice?> GetSupplierInvoiceDetailsAsync(
        int supplierInvoiceId,
        CancellationToken cancellationToken = default)
    {
        return context.SupplierInvoices.AsNoTracking()
            .AsSplitQuery()
            .Where(x => !x.IsDeleted)
            .Include(x => x.Supplier)
            .Include(x => x.Branch)
            .Include(x => x.PurchaseOrder)
            .Include(x => x.Payments)
            .FirstOrDefaultAsync(x => x.Id == supplierInvoiceId, cancellationToken);
    }

    public Task<List<SupplierInvoice>> GetUnpaidSupplierInvoicesAsync(
        int branchId,
        CancellationToken cancellationToken = default)
    {
        var unpaidStatuses = new[]
        {
            SupplierInvoiceStatus.Unpaid,
            SupplierInvoiceStatus.PartiallyPaid,
            SupplierInvoiceStatus.Overdue
        };

        return context.SupplierInvoices.AsNoTracking()
            .Where(x => x.BranchId == branchId && unpaidStatuses.Contains(x.Status) && !x.IsDeleted)
            .Include(x => x.Supplier)
            .OrderBy(x => x.DueDate)
            .ToListAsync(cancellationToken);
    }

    public Task<List<SupplierPayment>> GetSupplierPaymentsByInvoiceAsync(
        int supplierInvoiceId,
        CancellationToken cancellationToken = default)
    {
        return context.SupplierPayments.AsNoTracking()
            .Where(x => x.SupplierInvoiceId == supplierInvoiceId)
            .OrderByDescending(x => x.PaymentDate)
            .ToListAsync(cancellationToken);
    }

    private IQueryable<PurchaseOrder> PurchaseOrderDetailsQuery()
    {
        return context.PurchaseOrders.AsNoTracking()
            .AsSplitQuery()
            .Where(x => !x.IsDeleted)
            .Include(x => x.Supplier)
            .Include(x => x.Branch)
            .Include(x => x.Items)
                .ThenInclude(x => x.Product)
            .Include(x => x.GoodsReceipts.Where(receipt => !receipt.IsDeleted))
            .Include(x => x.SupplierInvoice);
    }
}
