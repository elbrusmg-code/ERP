using ERP.Core.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Data.Configurations.Catalog;

public sealed class ProductCategoryConfiguration : EntityConfiguration<ProductCategory>
{
    protected override void ConfigureEntity(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.HasIndex(x => x.Name);
        builder.HasOne(x => x.ParentCategory).WithMany(x => x.SubCategories).HasForeignKey(x => x.ParentCategoryId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ProductBrandConfiguration : EntityConfiguration<ProductBrand>
{
    protected override void ConfigureEntity(EntityTypeBuilder<ProductBrand> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.HasIndex(x => x.Name).IsUnique();
    }
}

public sealed class UnitOfMeasureConfiguration : EntityConfiguration<UnitOfMeasure>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UnitOfMeasure> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ShortName).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => x.ShortName).IsUnique();
    }
}

public sealed class TaxRateConfiguration : EntityConfiguration<TaxRate>
{
    protected override void ConfigureEntity(EntityTypeBuilder<TaxRate> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Rate).HasPrecision(5, 2);
        builder.HasIndex(x => x.Name).IsUnique();
    }
}

public sealed class ProductConfiguration : EntityConfiguration<Product>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.SKU).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(250).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.ImageUrl).HasMaxLength(1000);
        builder.Property(x => x.CostPrice).HasPrecision(18, 2);
        builder.Property(x => x.SalePrice).HasPrecision(18, 2);
        builder.HasIndex(x => x.SKU).IsUnique();
        builder.HasIndex(x => x.Name);
        builder.HasOne(x => x.ProductCategory).WithMany(x => x.Products).HasForeignKey(x => x.ProductCategoryId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ProductBrand).WithMany(x => x.Products).HasForeignKey(x => x.ProductBrandId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.UnitOfMeasure).WithMany(x => x.Products).HasForeignKey(x => x.UnitOfMeasureId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.TaxRate).WithMany(x => x.Products).HasForeignKey(x => x.TaxRateId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ProductBarcodeConfiguration : EntityConfiguration<ProductBarcode>
{
    protected override void ConfigureEntity(EntityTypeBuilder<ProductBarcode> builder)
    {
        builder.Property(x => x.Barcode).HasMaxLength(100).IsRequired();
        builder.HasIndex(x => x.Barcode).IsUnique();
        builder.HasOne(x => x.Product).WithMany(x => x.Barcodes).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ProductPriceHistoryConfiguration : EntityConfiguration<ProductPriceHistory>
{
    protected override void ConfigureEntity(EntityTypeBuilder<ProductPriceHistory> builder)
    {
        builder.Property(x => x.OldCostPrice).HasPrecision(18, 2);
        builder.Property(x => x.NewCostPrice).HasPrecision(18, 2);
        builder.Property(x => x.OldSalePrice).HasPrecision(18, 2);
        builder.Property(x => x.NewSalePrice).HasPrecision(18, 2);
        builder.Property(x => x.Note).HasMaxLength(1000);
        builder.HasIndex(x => new { x.ProductId, x.BranchId, x.ChangedAt });
        builder.HasOne(x => x.Product).WithMany(x => x.PriceHistory).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class BranchProductPriceConfiguration : EntityConfiguration<BranchProductPrice>
{
    protected override void ConfigureEntity(EntityTypeBuilder<BranchProductPrice> builder)
    {
        builder.Property(x => x.SalePrice).HasPrecision(18, 2);
        builder.HasIndex(x => new { x.ProductId, x.BranchId, x.EffectiveFrom });
        builder.HasOne(x => x.Product).WithMany(x => x.BranchPrices).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
    }
}
