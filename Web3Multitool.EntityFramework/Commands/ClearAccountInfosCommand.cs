using Microsoft.EntityFrameworkCore;
using Web3MultiTool.Domain.Commands;

namespace Web3Multitool.EntityFramework.Commands;

public class ClearAccountInfosCommand : IClearAccountInfosCommand
{
    private readonly AccountInfoDbContextFactory _contextFactory;

    public ClearAccountInfosCommand(AccountInfoDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task Execute()
    {
        await using var context = _contextFactory.Create();
        await context.AccountInfos.ExecuteDeleteAsync();
    }
}