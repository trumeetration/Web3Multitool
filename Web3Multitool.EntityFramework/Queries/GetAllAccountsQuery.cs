using Microsoft.EntityFrameworkCore;
using Web3MultiTool.Domain.Models;
using Web3MultiTool.Domain.Queries;

namespace Web3Multitool.EntityFramework.Queries;

public class GetAllAccountsQuery : IGetAllAccountsQuery
{
    private readonly AccountInfoDbContextFactory _contextFactory;

    public GetAllAccountsQuery(AccountInfoDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<IEnumerable<AccountInfo>> Execute()
    {
        await using AccountInfoDbContext context = _contextFactory.Create();
        var accountInfos = await context.AccountInfos
            .Include(x => x.AvaxInfo)
            .Include(x => x.ArbitrumInfo)
            .Include(x => x.FantomInfo)
            .Include(x => x.OptimismInfo)
            .Include(x => x.PolygonInfo)
            .Include(x => x.BnbInfo)
            .Include(x => x.HarmonyInfo)
            .Include(x => x.CoredaoInfo)
            .ToListAsync();
        return accountInfos.Select(info => info.FromDto());
    }
}