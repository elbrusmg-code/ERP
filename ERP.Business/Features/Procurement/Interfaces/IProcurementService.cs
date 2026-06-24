using ERP.Business.Common.Models;
using ERP.Business.Features.Procurement.Dtos;

namespace ERP.Business.Features.Procurement.Interfaces;

public interface IProcurementService
{
    Task<ServiceResult<List<SupplierDto>>> GetSuppliersAsync(
        CancellationToken cancellationToken = default);

    Task<ServiceResult<SupplierDto>> GetSupplierByIdAsync(
        int supplierId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<List<PurchaseOrderDto>>> GetPurchaseOrdersAsync(
        int branchId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<PurchaseOrderDto>> GetPurchaseOrderByIdAsync(
        int purchaseOrderId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<PurchaseOrderDto>> GetPurchaseOrderByNumberAsync(
        string orderNumber,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<List<PurchaseOrderDto>>> GetPendingPurchaseOrdersAsync(
        int branchId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<List<GoodsReceiptDto>>> GetGoodsReceiptsByPurchaseOrderAsync(
        int purchaseOrderId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<SupplierInvoiceDto>> GetSupplierInvoiceByIdAsync(
        int supplierInvoiceId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<List<SupplierInvoiceDto>>> GetUnpaidSupplierInvoicesAsync(
        int branchId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<List<SupplierPaymentDto>>> GetSupplierPaymentsByInvoiceAsync(
        int supplierInvoiceId,
        CancellationToken cancellationToken = default);
}
