using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web3Multitool.EntityFramework;

namespace Web3Multitool.HostBuilders;

public static class AddDbContextHostBuilderExtensions
{
    public static IHostBuilder AddDbContext(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices((context, services) =>
        {
            string connectionString = context.Configuration.GetConnectionString("sqlite");

            services.AddSingleton<DbContextOptions>(new DbContextOptionsBuilder().UseSqlite(connectionString).Options);
            services.AddSingleton<AccountInfoDbContextFactory>();
        });
                
        return hostBuilder;
    }
}