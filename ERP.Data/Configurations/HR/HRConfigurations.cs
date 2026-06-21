using ERP.Core.Entities.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Data.Configurations.HR;

public sealed class DepartmentConfiguration : EntityConfiguration<Department>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Department> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.HasIndex(x => new { x.BranchId, x.Name }).IsUnique();
        builder.HasOne(x => x.Branch).WithMany(x => x.Departments).HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class PositionConfiguration : EntityConfiguration<Position>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Position> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.BaseSalary).HasPrecision(18, 2);
        builder.HasIndex(x => new { x.DepartmentId, x.Name }).IsUnique();
        builder.HasOne(x => x.Department).WithMany(x => x.Positions).HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class EmployeeConfiguration : EntityConfiguration<Employee>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(x => x.EmployeeCode).HasMaxLength(50).IsRequired();
        builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(30);
        builder.Property(x => x.AppUserId).HasMaxLength(450);
        builder.Property(x => x.Salary).HasPrecision(18, 2);
        builder.HasIndex(x => new { x.BranchId, x.EmployeeCode }).IsUnique();
        builder.HasIndex(x => x.Email);
        builder.HasOne(x => x.Branch).WithMany(x => x.Employees).HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Department).WithMany(x => x.Employees).HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Position).WithMany(x => x.Employees).HasForeignKey(x => x.PositionId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class EmployeeContractConfiguration : EntityConfiguration<EmployeeContract>
{
    protected override void ConfigureEntity(EntityTypeBuilder<EmployeeContract> builder)
    {
        builder.Property(x => x.ContractNumber).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ContractType).HasMaxLength(100);
        builder.Property(x => x.FileUrl).HasMaxLength(1000);
        builder.Property(x => x.Salary).HasPrecision(18, 2);
        builder.HasIndex(x => x.ContractNumber).IsUnique();
        builder.HasOne(x => x.Employee).WithMany(x => x.Contracts).HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}
