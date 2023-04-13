using Microsoft.EntityFrameworkCore;

namespace Web3Multitool.Models;

public class ApplicationContext : DbContext
{
    public DbSet<AccountInfo> AccountInfos { get; set; }
    public DbSet<AddressChainInfo> AddressChainInfos { get; set; }

    public ApplicationContext()
    {
        // Database.EnsureCreated();
        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source=C:\a\lol.db");
    }
}