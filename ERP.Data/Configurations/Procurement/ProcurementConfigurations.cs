using ERP.Core.Entities.Procurement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Data.Configurations.Procurement;

public sealed class SupplierConfiguration : EntityConfiguration<Supplier>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Supplier> builder)
    {
        builder.Property(x => x.CompanyName).HasMaxLength(250).IsRequired();
        builder.Property(x => x.LegalName).HasMaxLength(250);
        builder.Property(x => x.TaxNumber).HasMaxLength(50);
        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(30);
        builder.Property(x => x.Address).HasMaxLength(500);
        builder.HasIndex(x => x.CompanyName);
        builder.HasIndex(x => x.TaxNumber).IsUnique().HasFilter("[TaxNumber] IS NOT NULL");
    }
}

public sealed class SupplierContactConfiguration : EntityConfiguration<SupplierContact>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SupplierContact> builder)
    {
        builder.Property(x => x.FullName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Position).HasMaxLength(150);
        builder.Property(x => x.Email).HasMaxLength(256);
        builder.Property(x => x.Phone).HasMaxLength(30);
        builder.HasOne(x => x.Supplier).WithMany(x => x.Contacts).HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class PurchaseOrderConfiguration : EntityConfiguration<PurchaseOrder>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.Property(x => x.OrderNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.SubTotal).HasPrecision(18, 2);
        builder.Property(x => x.TaxTotal).HasPrecision(18, 2);
        builder.Property(x => x.DiscountTotal).HasPrecision(18, 2);
        builder.Property(x => x.GrandTotal).HasPrecision(18, 2);
        builder.Property(x => x.Note).HasMaxLength(2000);
        builder.Property(x => x.ApprovedBy).HasMaxLength(450);
        builder.HasIndex(x => x.OrderNumber).IsUnique();
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Supplier).WithMany(x => x.PurchaseOrders).HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class PurchaseOrderItemConfiguration : EntityConfiguration<PurchaseOrderItem>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PurchaseOrderItem> builder)
    {
        builder.Property(x => x.OrderedQuantity).HasPrecision(18, 3);
        builder.Property(x => x.ReceivedQuantity).HasPrecision(18, 3);
        builder.Property(x => x.UnitCost).HasPrecision(18, 2);
        builder.Property(x => x.DiscountAmount).HasPrecision(18, 2);
        builder.Property(x => x.TaxAmount).HasPrecision(18, 2);
        builder.Property(x => x.LineTotal).HasPrecision(18, 2);
        builder.Property(x => x.Note).HasMaxLength(1000);
        builder.HasOne(x => x.PurchaseOrder).WithMany(x => x.Items).HasForeignKey(x => x.PurchaseOrderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class GoodsReceiptConfiguration : EntityConfiguration<GoodsReceipt>
{
    protected override void ConfigureEntity(EntityTypeBuilder<GoodsReceipt> builder)
    {
        builder.Property(x => x.ReceiptNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ReceivedBy).HasMaxLength(450);
        builder.Property(x => x.Note).HasMaxLength(2000);
        builder.HasIndex(x => x.ReceiptNumber).IsUnique();
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Warehouse).WithMany().HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Supplier).WithMany().HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.PurchaseOrder).WithMany(x => x.GoodsReceipts).HasForeignKey(x => x.PurchaseOrderId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class GoodsReceiptItemConfiguration : EntityConfiguration<GoodsReceiptItem>
{
    protected override void ConfigureEntity(EntityTypeBuilder<GoodsReceiptItem> builder)
    {
        builder.Property(x => x.BatchNumber).HasMaxLength(100);
        builder.Property(x => x.ReceivedQuantity).HasPrecision(18, 3);
        builder.Property(x => x.UnitCost).HasPrecision(18, 2);
        builder.Property(x => x.LineTotal).HasPrecision(18, 2);
        builder.Property(x => x.Note).HasMaxLength(1000);
        builder.HasOne(x => x.GoodsReceipt).WithMany(x => x.Items).HasForeignKey(x => x.GoodsReceiptId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.PurchaseOrderItem).WithMany().HasForeignKey(x => x.PurchaseOrderItemId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ProductBatch).WithMany().HasForeignKey(x => x.ProductBatchId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class SupplierInvoiceConfiguration : EntityConfiguration<SupplierInvoice>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SupplierInvoice> builder)
    {
        builder.Property(x => x.InvoiceNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.SubTotal).HasPrecision(18, 2);
        builder.Property(x => x.TaxTotal).HasPrecision(18, 2);
        builder.Property(x => x.DiscountTotal).HasPrecision(18, 2);
        builder.Property(x => x.GrandTotal).HasPrecision(18, 2);
        builder.Property(x => x.PaidAmount).HasPrecision(18, 2);
        builder.Property(x => x.RemainingAmount).HasPrecision(18, 2);
        builder.Property(x => x.Note).HasMaxLength(2000);
        builder.HasIndex(x => x.InvoiceNumber).IsUnique();
        builder.HasOne(x => x.Supplier).WithMany(x => x.Invoices).HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.PurchaseOrder).WithOne(x => x.SupplierInvoice).HasForeignKey<SupplierInvoice>(x => x.PurchaseOrderId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class SupplierPaymentConfiguration : EntityConfiguration<SupplierPayment>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SupplierPayment> builder)
    {
        builder.Property(x => x.PaymentNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.ReferenceNumber).HasMaxLength(100);
        builder.Property(x => x.Note).HasMaxLength(1000);
        builder.HasIndex(x => x.PaymentNumber).IsUnique();
        builder.HasOne(x => x.Supplier).WithMany(x => x.Payments).HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.SupplierInvoice).WithMany(x => x.Payments).HasForeignKey(x => x.SupplierInvoiceId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class SupplierReturnConfiguration : EntityConfiguration<SupplierReturn>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SupplierReturn> builder)
    {
        builder.Property(x => x.ReturnNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
        builder.Property(x => x.Reason).HasMaxLength(1000);
        builder.Property(x => x.ApprovedBy).HasMaxLength(450);
        builder.Property(x => x.Note).HasMaxLength(2000);
        builder.HasIndex(x => x.ReturnNumber).IsUnique();
        builder.HasOne(x => x.Supplier).WithMany(x => x.Returns).HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Warehouse).WithMany().HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class SupplierReturnItemConfiguration : EntityConfiguration<SupplierReturnItem>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SupplierReturnItem> builder)
    {
        builder.Property(x => x.Quantity).HasPrecision(18, 3);
        builder.Property(x => x.UnitCost).HasPrecision(18, 2);
        builder.Property(x => x.LineTotal).HasPrecision(18, 2);
        builder.Property(x => x.Reason).HasMaxLength(1000);
        builder.HasOne(x => x.SupplierReturn).WithMany(x => x.Items).HasForeignKey(x => x.SupplierReturnId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ProductBatch).WithMany().HasForeignKey(x => x.ProductBatchId).OnDelete(DeleteBehavior.Restrict);
    }
}
