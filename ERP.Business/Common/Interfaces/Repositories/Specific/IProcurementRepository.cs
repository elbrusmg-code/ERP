using ERP.Core.Entities.Procurement;

namespace ERP.Business.Common.Interfaces.Repositories.Specific;

public interface IProcurementRepository
{
    Task<List<Supplier>> GetActiveSuppliersAsync(CancellationToken cancellationToken = default);
    Task<Supplier?> GetSupplierDetailsAsync(int supplierId, CancellationToken cancellationToken = default);
    Task<bool> SupplierTaxNumberExistsAsync(string taxNumber, int? excludeSupplierId = null, CancellationToken cancellationToken = default);
    Task<List<PurchaseOrder>> GetPurchaseOrdersByBranchAsync(int branchId, CancellationToken cancellationToken = default);
    Task<PurchaseOrder?> GetPurchaseOrderDetailsAsync(int purchaseOrderId, CancellationToken cancellationToken = default);
    Task<PurchaseOrder?> GetPurchaseOrderByNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    Task<List<PurchaseOrder>> GetPendingPurchaseOrdersAsync(int branchId, CancellationToken cancellationToken = default);
    Task<List<GoodsReceipt>> GetGoodsReceiptsByPurchaseOrderAsync(int purchaseOrderId, CancellationToken cancellationToken = default);
    Task<SupplierInvoice?> GetSupplierInvoiceDetailsAsync(int supplierInvoiceId, CancellationToken cancellationToken = default);
    Task<List<SupplierInvoice>> GetUnpaidSupplierInvoicesAsync(int branchId, CancellationToken cancellationToken = default);
    Task<List<SupplierPayment>> GetSupplierPaymentsByInvoiceAsync(int supplierInvoiceId, CancellationToken cancellationToken = default);
}
