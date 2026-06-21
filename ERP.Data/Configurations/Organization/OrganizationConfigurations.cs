using ERP.Core.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Data.Configurations.Organization;

public sealed class CompanyConfiguration : EntityConfiguration<Company>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Company> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.LegalName).HasMaxLength(250);
        builder.Property(x => x.TaxNumber).HasMaxLength(50);
        builder.Property(x => x.Phone).HasMaxLength(30);
        builder.Property(x => x.Email).HasMaxLength(256);
        builder.Property(x => x.Address).HasMaxLength(500);
        builder.HasIndex(x => x.TaxNumber).IsUnique().HasFilter("[TaxNumber] IS NOT NULL");
    }
}

public sealed class BranchConfiguration : EntityConfiguration<Branch>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Branch> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(30);
        builder.Property(x => x.Email).HasMaxLength(256);
        builder.Property(x => x.Address).HasMaxLength(500);
        builder.HasIndex(x => new { x.CompanyId, x.Code }).IsUnique();
        builder.HasOne(x => x.Company).WithMany(x => x.Branches).HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
    }
}
