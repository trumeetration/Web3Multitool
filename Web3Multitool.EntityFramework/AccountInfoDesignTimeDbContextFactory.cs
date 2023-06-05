using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Web3Multitool.EntityFramework;

public class AccountInfoDesignTimeDbContextFactory : IDesignTimeDbContextFactory<AccountInfoDbContext>
{
    public AccountInfoDbContext CreateDbContext(string[] args)
    {
        return new AccountInfoDbContext(new DbContextOptionsBuilder().UseSqlite("Data Source=Boba.db").Options);
    }
}