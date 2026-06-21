using ERP.Core.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Data.Configurations;

public abstract class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.ToTable(typeof(TEntity).Name);
        builder.HasKey(entity => entity.Id);

        if (typeof(AuditableEntity).IsAssignableFrom(typeof(TEntity)))
        {
            builder.Property(nameof(AuditableEntity.CreatedBy)).HasMaxLength(450);
            builder.Property(nameof(AuditableEntity.UpdatedBy)).HasMaxLength(450);
        }

        if (typeof(SoftDeleteEntity).IsAssignableFrom(typeof(TEntity)))
        {
            builder.Property(nameof(SoftDeleteEntity.DeletedBy)).HasMaxLength(450);
            builder.HasIndex(nameof(SoftDeleteEntity.IsDeleted));
        }

        ConfigureEntity(builder);
    }

    protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
}
