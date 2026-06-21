using ERP.Core.Entities.CRM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Data.Configurations.CRM;

public sealed class CustomerGroupConfiguration : EntityConfiguration<CustomerGroup>
{
    protected override void ConfigureEntity(EntityTypeBuilder<CustomerGroup> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.DefaultDiscountPercent).HasPrecision(5, 2);
        builder.HasIndex(x => x.Name).IsUnique();
    }
}

public sealed class CustomerConfiguration : EntityConfiguration<Customer>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(x => x.CustomerCode).HasMaxLength(100).IsRequired();
        builder.Property(x => x.FullName).HasMaxLength(250).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(30);
        builder.Property(x => x.Email).HasMaxLength(256);
        builder.Property(x => x.TaxNumber).HasMaxLength(50);
        builder.Property(x => x.CurrentLoyaltyPoints).HasPrecision(18, 4);
        builder.Property(x => x.TotalSpent).HasPrecision(18, 2);
        builder.HasIndex(x => x.CustomerCode).IsUnique();
        builder.HasIndex(x => x.Phone);
        builder.HasIndex(x => x.Email);
        builder.HasOne(x => x.CustomerGroup).WithMany(x => x.Customers).HasForeignKey(x => x.CustomerGroupId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.RegisteredBranch).WithMany().HasForeignKey(x => x.RegisteredBranchId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class CustomerAddressConfiguration : EntityConfiguration<CustomerAddress>
{
    protected override void ConfigureEntity(EntityTypeBuilder<CustomerAddress> builder)
    {
        builder.Property(x => x.Title).HasMaxLength(100);
        builder.Property(x => x.AddressLine).HasMaxLength(500).IsRequired();
        builder.Property(x => x.City).HasMaxLength(100);
        builder.Property(x => x.PostalCode).HasMaxLength(20);
        builder.HasOne(x => x.Customer).WithMany(x => x.Addresses).HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class LoyaltyCardConfiguration : EntityConfiguration<LoyaltyCard>
{
    protected override void ConfigureEntity(EntityTypeBuilder<LoyaltyCard> builder)
    {
        builder.Property(x => x.CardNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.PointsBalance).HasPrecision(18, 4);
        builder.Property(x => x.Note).HasMaxLength(1000);
        builder.HasIndex(x => x.CardNumber).IsUnique();
        builder.HasOne(x => x.Customer).WithMany(x => x.LoyaltyCards).HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class LoyaltyPointTransactionConfiguration : EntityConfiguration<LoyaltyPointTransaction>
{
    protected override void ConfigureEntity(EntityTypeBuilder<LoyaltyPointTransaction> builder)
    {
        builder.Property(x => x.TransactionNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Points).HasPrecision(18, 4);
        builder.Property(x => x.BalanceAfter).HasPrecision(18, 4);
        builder.Property(x => x.ReferenceNumber).HasMaxLength(100);
        builder.Property(x => x.Note).HasMaxLength(1000);
        builder.HasIndex(x => x.TransactionNumber).IsUnique();
        builder.HasOne(x => x.Customer).WithMany(x => x.LoyaltyPointTransactions).HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.LoyaltyCard).WithMany().HasForeignKey(x => x.LoyaltyCardId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.POSReceipt).WithMany().HasForeignKey(x => x.POSReceiptId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class CustomerNoteConfiguration : EntityConfiguration<CustomerNote>
{
    protected override void ConfigureEntity(EntityTypeBuilder<CustomerNote> builder)
    {
        builder.Property(x => x.Note).HasMaxLength(2000).IsRequired();
        builder.Property(x => x.CreatedByUserId).HasMaxLength(450);
        builder.HasOne(x => x.Customer).WithMany(x => x.Notes).HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class CustomerInteractionConfiguration : EntityConfiguration<CustomerInteraction>
{
    protected override void ConfigureEntity(EntityTypeBuilder<CustomerInteraction> builder)
    {
        builder.Property(x => x.Subject).HasMaxLength(250).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.CreatedByUserId).HasMaxLength(450);
        builder.HasOne(x => x.Customer).WithMany(x => x.Interactions).HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class CustomerTransactionHistoryConfiguration : EntityConfiguration<CustomerTransactionHistory>
{
    protected override void ConfigureEntity(EntityTypeBuilder<CustomerTransactionHistory> builder)
    {
        builder.Property(x => x.TransactionNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.ReferenceNumber).HasMaxLength(100);
        builder.Property(x => x.Note).HasMaxLength(1000);
        builder.HasIndex(x => x.TransactionNumber).IsUnique();
        builder.HasOne(x => x.Customer).WithMany(x => x.TransactionHistory).HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.POSReceipt).WithMany().HasForeignKey(x => x.POSReceiptId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.SalesReturn).WithMany().HasForeignKey(x => x.SalesReturnId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class SupplierNoteConfiguration : EntityConfiguration<SupplierNote>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SupplierNote> builder)
    {
        builder.Property(x => x.Note).HasMaxLength(2000).IsRequired();
        builder.Property(x => x.CreatedByUserId).HasMaxLength(450);
        builder.HasOne(x => x.Supplier).WithMany(x => x.Notes).HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
    }
}
