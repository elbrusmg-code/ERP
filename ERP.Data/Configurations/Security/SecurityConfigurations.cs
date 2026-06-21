using ERP.Core.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Data.Configurations.Security;

public sealed class UserBranchAssignmentConfiguration : EntityConfiguration<UserBranchAssignment>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UserBranchAssignment> builder)
    {
        builder.Property(x => x.UserId).HasMaxLength(450).IsRequired();
        builder.HasIndex(x => new { x.UserId, x.BranchId }).IsUnique();
        builder.HasOne(x => x.Branch).WithMany(x => x.UserBranchAssignments).HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
    }
}
