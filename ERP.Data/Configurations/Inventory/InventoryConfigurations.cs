using ERP.Core.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Data.Configurations.Inventory;

public sealed class WarehouseConfiguration : EntityConfiguration<Warehouse>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Warehouse> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Location).HasMaxLength(500);
        builder.HasIndex(x => new { x.BranchId, x.Code }).IsUnique();
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class StockItemConfiguration : EntityConfiguration<StockItem>
{
    protected override void ConfigureEntity(EntityTypeBuilder<StockItem> builder)
    {
        builder.Property(x => x.Quantity).HasPrecision(18, 3);
        builder.Property(x => x.ReservedQuantity).HasPrecision(18, 3);
        builder.Property(x => x.MinimumStockLevel).HasPrecision(18, 3);
        builder.Property(x => x.MaximumStockLevel).HasPrecision(18, 3);
        builder.Property(x => x.ReorderLevel).HasPrecision(18, 3);
        builder.HasIndex(x => new { x.ProductId, x.BranchId, x.WarehouseId }).IsUnique();
        builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Warehouse).WithMany(x => x.StockItems).HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class StockMovementConfiguration : EntityConfiguration<StockMovement>
{
    protected override void ConfigureEntity(EntityTypeBuilder<StockMovement> builder)
    {
        builder.Property(x => x.MovementNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Quantity).HasPrecision(18, 3);
        builder.Property(x => x.QuantityBefore).HasPrecision(18, 3);
        builder.Property(x => x.QuantityAfter).HasPrecision(18, 3);
        builder.Property(x => x.ReferenceNumber).HasMaxLength(100);
        builder.Property(x => x.SourceModule).HasMaxLength(100);
        builder.Property(x => x.Reason).HasMaxLength(1000);
        builder.HasIndex(x => x.MovementNumber).IsUnique();
        builder.HasIndex(x => new { x.BranchId, x.ProductId, x.MovementDate });
        builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Warehouse).WithMany(x => x.StockMovements).HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ProductBatch).WithMany(x => x.StockMovements).HasForeignKey(x => x.ProductBatchId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ProductBatchConfiguration : EntityConfiguration<ProductBatch>
{
    protected override void ConfigureEntity(EntityTypeBuilder<ProductBatch> builder)
    {
        builder.Property(x => x.BatchNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.InitialQuantity).HasPrecision(18, 3);
        builder.Property(x => x.CurrentQuantity).HasPrecision(18, 3);
        builder.Property(x => x.PurchaseCost).HasPrecision(18, 2);
        builder.HasIndex(x => new { x.ProductId, x.BranchId, x.WarehouseId, x.BatchNumber });
        builder.HasIndex(x => x.ExpiryDate);
        builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Warehouse).WithMany(x => x.ProductBatches).HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class InventoryAlertConfiguration : EntityConfiguration<InventoryAlert>
{
    protected override void ConfigureEntity(EntityTypeBuilder<InventoryAlert> builder)
    {
        builder.Property(x => x.CurrentQuantity).HasPrecision(18, 3);
        builder.Property(x => x.ThresholdQuantity).HasPrecision(18, 3);
        builder.Property(x => x.Message).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.ResolvedBy).HasMaxLength(450);
        builder.HasIndex(x => new { x.BranchId, x.ProductId, x.Status });
        builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Warehouse).WithMany().HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class StockAdjustmentConfiguration : EntityConfiguration<StockAdjustment>
{
    protected override void ConfigureEntity(EntityTypeBuilder<StockAdjustment> builder)
    {
        builder.Property(x => x.AdjustmentNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.QuantityBefore).HasPrecision(18, 3);
        builder.Property(x => x.AdjustmentQuantity).HasPrecision(18, 3);
        builder.Property(x => x.QuantityAfter).HasPrecision(18, 3);
        builder.Property(x => x.Note).HasMaxLength(1000);
        builder.Property(x => x.ApprovedBy).HasMaxLength(450);
        builder.HasIndex(x => x.AdjustmentNumber).IsUnique();
        builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Warehouse).WithMany().HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ProductBatch).WithMany().HasForeignKey(x => x.ProductBatchId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class StockTakeConfiguration : EntityConfiguration<StockTake>
{
    protected override void ConfigureEntity(EntityTypeBuilder<StockTake> builder)
    {
        builder.Property(x => x.StockTakeNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Notes).HasMaxLength(2000);
        builder.HasIndex(x => x.StockTakeNumber).IsUnique();
        builder.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Warehouse).WithMany().HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class StockTakeItemConfiguration : EntityConfiguration<StockTakeItem>
{
    protected override void ConfigureEntity(EntityTypeBuilder<StockTakeItem> builder)
    {
        builder.Property(x => x.SystemQuantity).HasPrecision(18, 3);
        builder.Property(x => x.CountedQuantity).HasPrecision(18, 3);
        builder.Property(x => x.DifferenceQuantity).HasPrecision(18, 3);
        builder.Property(x => x.Note).HasMaxLength(1000);
        builder.HasIndex(x => new { x.StockTakeId, x.ProductId, x.ProductBatchId });
        builder.HasOne(x => x.StockTake).WithMany(x => x.Items).HasForeignKey(x => x.StockTakeId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ProductBatch).WithMany().HasForeignKey(x => x.ProductBatchId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class StockTransferConfiguration : EntityConfiguration<StockTransfer>
{
    protected override void ConfigureEntity(EntityTypeBuilder<StockTransfer> builder)
    {
        builder.Property(x => x.TransferNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Notes).HasMaxLength(2000);
        builder.HasIndex(x => x.TransferNumber).IsUnique();
        builder.HasOne(x => x.FromBranch).WithMany().HasForeignKey(x => x.FromBranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ToBranch).WithMany().HasForeignKey(x => x.ToBranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.FromWarehouse).WithMany().HasForeignKey(x => x.FromWarehouseId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ToWarehouse).WithMany().HasForeignKey(x => x.ToWarehouseId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class StockTransferItemConfiguration : EntityConfiguration<StockTransferItem>
{
    protected override void ConfigureEntity(EntityTypeBuilder<StockTransferItem> builder)
    {
        builder.Property(x => x.Quantity).HasPrecision(18, 3);
        builder.Property(x => x.ReceivedQuantity).HasPrecision(18, 3);
        builder.Property(x => x.Note).HasMaxLength(1000);
        builder.HasOne(x => x.StockTransfer).WithMany(x => x.Items).HasForeignKey(x => x.StockTransferId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ProductBatch).WithMany().HasForeignKey(x => x.ProductBatchId).OnDelete(DeleteBehavior.Restrict);
    }
}
