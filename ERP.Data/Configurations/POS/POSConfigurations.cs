using ERP.Core.Entities.POS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Data.Configurations.POS;

public sealed class CashRegisterConfiguration : EntityConfiguration<CashRegister>
{
    protected override void ConfigureEntity(EntityTypeBuilder<CashRegister> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.HasIndex(x => new { x.BranchId, x.Code }).IsUnique();
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class CashShiftConfiguration : EntityConfiguration<CashShift>
{
    protected override void ConfigureEntity(EntityTypeBuilder<CashShift> builder)
    {
        builder.Property(x => x.ShiftNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.OpeningCashAmount).HasPrecision(18, 2);
        builder.Property(x => x.ExpectedCashAmount).HasPrecision(18, 2);
        builder.Property(x => x.ActualCashAmount).HasPrecision(18, 2);
        builder.Property(x => x.CashDifference).HasPrecision(18, 2);
        builder.Property(x => x.TotalCashSales).HasPrecision(18, 2);
        builder.Property(x => x.TotalCardSales).HasPrecision(18, 2);
        builder.Property(x => x.TotalMixedSales).HasPrecision(18, 2);
        builder.Property(x => x.TotalRefunds).HasPrecision(18, 2);
        builder.Property(x => x.OpeningNote).HasMaxLength(1000);
        builder.Property(x => x.ClosingNote).HasMaxLength(1000);
        builder.Property(x => x.ClosedBy).HasMaxLength(450);
        builder.HasIndex(x => x.ShiftNumber).IsUnique();
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CashRegister).WithMany(x => x.CashShifts).HasForeignKey(x => x.CashRegisterId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CashierEmployee).WithMany().HasForeignKey(x => x.CashierEmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class POSReceiptConfiguration : EntityConfiguration<POSReceipt>
{
    protected override void ConfigureEntity(EntityTypeBuilder<POSReceipt> builder)
    {
        builder.Property(x => x.ReceiptNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.SubTotal).HasPrecision(18, 2);
        builder.Property(x => x.DiscountTotal).HasPrecision(18, 2);
        builder.Property(x => x.TaxTotal).HasPrecision(18, 2);
        builder.Property(x => x.GrandTotal).HasPrecision(18, 2);
        builder.Property(x => x.PaidAmount).HasPrecision(18, 2);
        builder.Property(x => x.ChangeAmount).HasPrecision(18, 2);
        builder.Property(x => x.DiscountValue).HasPrecision(18, 4);
        builder.Property(x => x.CustomerPhone).HasMaxLength(30);
        builder.Property(x => x.CustomerName).HasMaxLength(200);
        builder.Property(x => x.Note).HasMaxLength(1000);
        builder.Property(x => x.VoidReason).HasMaxLength(1000);
        builder.Property(x => x.VoidedBy).HasMaxLength(450);
        builder.HasIndex(x => x.ReceiptNumber).IsUnique();
        builder.HasIndex(x => new { x.BranchId, x.ReceiptDate });
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CashRegister).WithMany(x => x.Receipts).HasForeignKey(x => x.CashRegisterId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CashShift).WithMany(x => x.Receipts).HasForeignKey(x => x.CashShiftId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CashierEmployee).WithMany().HasForeignKey(x => x.CashierEmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class POSReceiptItemConfiguration : EntityConfiguration<POSReceiptItem>
{
    protected override void ConfigureEntity(EntityTypeBuilder<POSReceiptItem> builder)
    {
        builder.Property(x => x.ProductNameSnapshot).HasMaxLength(250).IsRequired();
        builder.Property(x => x.SKUSnapshot).HasMaxLength(100).IsRequired();
        builder.Property(x => x.BarcodeSnapshot).HasMaxLength(100);
        builder.Property(x => x.Quantity).HasPrecision(18, 3);
        builder.Property(x => x.UnitPrice).HasPrecision(18, 2);
        builder.Property(x => x.CostPriceSnapshot).HasPrecision(18, 2);
        builder.Property(x => x.DiscountAmount).HasPrecision(18, 2);
        builder.Property(x => x.TaxAmount).HasPrecision(18, 2);
        builder.Property(x => x.LineTotal).HasPrecision(18, 2);
        builder.Property(x => x.ReturnedQuantity).HasPrecision(18, 3);
        builder.HasOne(x => x.POSReceipt).WithMany(x => x.Items).HasForeignKey(x => x.POSReceiptId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ProductBatch).WithMany().HasForeignKey(x => x.ProductBatchId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class POSPaymentConfiguration : EntityConfiguration<POSPayment>
{
    protected override void ConfigureEntity(EntityTypeBuilder<POSPayment> builder)
    {
        builder.Property(x => x.PaymentNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.ReferenceNumber).HasMaxLength(100);
        builder.Property(x => x.Note).HasMaxLength(1000);
        builder.HasIndex(x => x.PaymentNumber).IsUnique();
        builder.HasOne(x => x.POSReceipt).WithMany(x => x.Payments).HasForeignKey(x => x.POSReceiptId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class PaymentTerminalTransactionConfiguration : EntityConfiguration<PaymentTerminalTransaction>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PaymentTerminalTransaction> builder)
    {
        builder.Property(x => x.TerminalTransactionNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.TerminalId).HasMaxLength(100);
        builder.Property(x => x.TerminalName).HasMaxLength(150);
        builder.Property(x => x.BankName).HasMaxLength(200);
        builder.Property(x => x.AuthorizationCode).HasMaxLength(100);
        builder.Property(x => x.RRN).HasMaxLength(100);
        builder.Property(x => x.CardLastFourDigits).HasMaxLength(4);
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.ResponseMessage).HasMaxLength(1000);
        builder.HasIndex(x => x.TerminalTransactionNumber).IsUnique();
        builder.HasOne(x => x.POSPayment).WithMany(x => x.TerminalTransactions).HasForeignKey(x => x.POSPaymentId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class SalesReturnConfiguration : EntityConfiguration<SalesReturn>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SalesReturn> builder)
    {
        builder.Property(x => x.ReturnNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.TotalReturnAmount).HasPrecision(18, 2);
        builder.Property(x => x.Reason).HasMaxLength(1000);
        builder.Property(x => x.ApprovedBy).HasMaxLength(450);
        builder.HasIndex(x => x.ReturnNumber).IsUnique();
        builder.HasOne(x => x.OriginalReceipt).WithMany(x => x.Returns).HasForeignKey(x => x.OriginalReceiptId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CashRegister).WithMany().HasForeignKey(x => x.CashRegisterId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CashShift).WithMany().HasForeignKey(x => x.CashShiftId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CashierEmployee).WithMany().HasForeignKey(x => x.CashierEmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class SalesReturnItemConfiguration : EntityConfiguration<SalesReturnItem>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SalesReturnItem> builder)
    {
        builder.Property(x => x.Quantity).HasPrecision(18, 3);
        builder.Property(x => x.UnitPrice).HasPrecision(18, 2);
        builder.Property(x => x.ReturnAmount).HasPrecision(18, 2);
        builder.Property(x => x.Reason).HasMaxLength(1000);
        builder.HasOne(x => x.SalesReturn).WithMany(x => x.Items).HasForeignKey(x => x.SalesReturnId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.POSReceiptItem).WithMany().HasForeignKey(x => x.POSReceiptItemId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ProductBatch).WithMany().HasForeignKey(x => x.ProductBatchId).OnDelete(DeleteBehavior.Restrict);
    }
}
