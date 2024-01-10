using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.EFCore.Contexts;

public class SQLServerContext : ContextBase
{
    private readonly DatabaseConnectionString connectionString;

    public SQLServerContext(DatabaseConnectionString connectionString)
    {
        this.connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder
            .UseSqlServer(connectionString.Value, providerOptions => providerOptions.CommandTimeout(60))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}