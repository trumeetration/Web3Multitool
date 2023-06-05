using Microsoft.EntityFrameworkCore;

namespace Web3Multitool.EntityFramework;

public class AccountInfoDbContextFactory
{
    private readonly DbContextOptions _options;

    public AccountInfoDbContextFactory(DbContextOptions options)
    {
        _options = options;
    }
    
    public AccountInfoDbContext Create()
    {
        return new AccountInfoDbContext(_options);
    }
}