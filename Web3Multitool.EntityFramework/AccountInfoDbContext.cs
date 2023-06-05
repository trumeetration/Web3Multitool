using Microsoft.EntityFrameworkCore;
using Web3Multitool.EntityFramework.DTOs;

namespace Web3Multitool.EntityFramework;

public class AccountInfoDbContext : DbContext
{
    public AccountInfoDbContext(DbContextOptions options) : base(options) {}
    
    public DbSet<AccountInfoDto> AccountInfos { get; set; }
    public DbSet<AddressChainInfoDto> AddressChainInfos { get; set; }
}