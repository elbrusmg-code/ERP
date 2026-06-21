using ERP.Core.Entities.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Data.Configurations.Finance;

public sealed class FinancialAccountConfiguration : EntityConfiguration<FinancialAccount>
{
    protected override void ConfigureEntity(EntityTypeBuilder<FinancialAccount> builder)
    {
        builder.Property(x => x.AccountCode).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.HasIndex(x => x.AccountCode).IsUnique();
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class FinancialTransactionConfiguration : EntityConfiguration<FinancialTransaction>
{
    protected override void ConfigureEntity(EntityTypeBuilder<FinancialTransaction> builder)
    {
        builder.Property(x => x.TransactionNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.SourceModule).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ReferenceNumber).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.CreatedByUserId).HasMaxLength(450);
        builder.HasIndex(x => x.TransactionNumber).IsUnique();
        builder.HasIndex(x => new { x.BranchId, x.TransactionDate });
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.FinancialAccount).WithMany(x => x.Transactions).HasForeignKey(x => x.FinancialAccountId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ExpenseConfiguration : EntityConfiguration<Expense>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Expense> builder)
    {
        builder.Property(x => x.ExpenseNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.Description).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.ReferenceNumber).HasMaxLength(100);
        builder.Property(x => x.ApprovedBy).HasMaxLength(450);
        builder.HasIndex(x => x.ExpenseNumber).IsUnique();
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.FinancialTransaction).WithMany().HasForeignKey(x => x.FinancialTransactionId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class SalaryPaymentConfiguration : EntityConfiguration<SalaryPayment>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SalaryPayment> builder)
    {
        builder.Property(x => x.PaymentNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.GrossAmount).HasPrecision(18, 2);
        builder.Property(x => x.DeductionAmount).HasPrecision(18, 2);
        builder.Property(x => x.NetAmount).HasPrecision(18, 2);
        builder.Property(x => x.ReferenceNumber).HasMaxLength(100);
        builder.Property(x => x.Note).HasMaxLength(1000);
        builder.HasIndex(x => x.PaymentNumber).IsUnique();
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Employee).WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.FinancialTransaction).WithMany().HasForeignKey(x => x.FinancialTransactionId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class DailySalesSummaryConfiguration : EntityConfiguration<DailySalesSummary>
{
    protected override void ConfigureEntity(EntityTypeBuilder<DailySalesSummary> builder)
    {
        builder.Property(x => x.SummaryNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.TotalSales).HasPrecision(18, 2);
        builder.Property(x => x.TotalCashSales).HasPrecision(18, 2);
        builder.Property(x => x.TotalCardSales).HasPrecision(18, 2);
        builder.Property(x => x.TotalMixedSales).HasPrecision(18, 2);
        builder.Property(x => x.TotalRefunds).HasPrecision(18, 2);
        builder.Property(x => x.NetSales).HasPrecision(18, 2);
        builder.Property(x => x.CalculatedBy).HasMaxLength(450);
        builder.Property(x => x.ApprovedBy).HasMaxLength(450);
        builder.HasIndex(x => x.SummaryNumber).IsUnique();
        builder.HasIndex(x => new { x.BranchId, x.SalesDate });
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class DailySalesSummaryShiftConfiguration : EntityConfiguration<DailySalesSummaryShift>
{
    protected override void ConfigureEntity(EntityTypeBuilder<DailySalesSummaryShift> builder)
    {
        builder.Property(x => x.TotalSales).HasPrecision(18, 2);
        builder.Property(x => x.CashSales).HasPrecision(18, 2);
        builder.Property(x => x.CardSales).HasPrecision(18, 2);
        builder.Property(x => x.MixedSales).HasPrecision(18, 2);
        builder.Property(x => x.Refunds).HasPrecision(18, 2);
        builder.Property(x => x.ExpectedCash).HasPrecision(18, 2);
        builder.Property(x => x.ActualCash).HasPrecision(18, 2);
        builder.Property(x => x.CashDifference).HasPrecision(18, 2);
        builder.HasIndex(x => new { x.DailySalesSummaryId, x.CashShiftId }).IsUnique();
        builder.HasOne(x => x.DailySalesSummary).WithMany(x => x.Shifts).HasForeignKey(x => x.DailySalesSummaryId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CashShift).WithMany().HasForeignKey(x => x.CashShiftId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class SupplierPayableConfiguration : EntityConfiguration<SupplierPayable>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SupplierPayable> builder)
    {
        builder.Property(x => x.PayableNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.OriginalAmount).HasPrecision(18, 2);
        builder.Property(x => x.PaidAmount).HasPrecision(18, 2);
        builder.Property(x => x.RemainingAmount).HasPrecision(18, 2);
        builder.Property(x => x.Note).HasMaxLength(1000);
        builder.HasIndex(x => x.PayableNumber).IsUnique();
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Supplier).WithMany().HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.SupplierInvoice).WithMany().HasForeignKey(x => x.SupplierInvoiceId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class JournalEntryConfiguration : EntityConfiguration<JournalEntry>
{
    protected override void ConfigureEntity(EntityTypeBuilder<JournalEntry> builder)
    {
        builder.Property(x => x.EntryNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.ReferenceNumber).HasMaxLength(100);
        builder.Property(x => x.PostedBy).HasMaxLength(450);
        builder.HasIndex(x => x.EntryNumber).IsUnique();
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class JournalEntryLineConfiguration : EntityConfiguration<JournalEntryLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<JournalEntryLine> builder)
    {
        builder.Property(x => x.Debit).HasPrecision(18, 2);
        builder.Property(x => x.Credit).HasPrecision(18, 2);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.HasOne(x => x.JournalEntry).WithMany(x => x.Lines).HasForeignKey(x => x.JournalEntryId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.FinancialAccount).WithMany(x => x.JournalEntryLines).HasForeignKey(x => x.FinancialAccountId).OnDelete(DeleteBehavior.Restrict);
    }
}
