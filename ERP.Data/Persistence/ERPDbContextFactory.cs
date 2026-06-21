using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ERP.Data.Persistence;

public sealed class ERPDbContextFactory : IDesignTimeDbContextFactory<ERPDbContext>
{
    private const string DevelopmentConnectionString =
        @"Server=(localdb)\MSSQLLocalDB;Database=SmartMarketERPDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

    public ERPDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ERPDbContext>();
        optionsBuilder.UseSqlServer(DevelopmentConnectionString);

        return new ERPDbContext(optionsBuilder.Options);
    }
}
