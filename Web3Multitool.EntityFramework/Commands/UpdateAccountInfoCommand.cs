using Web3MultiTool.Domain.Commands;
using Web3MultiTool.Domain.Models;

namespace Web3Multitool.EntityFramework.Commands;

public class UpdateAccountInfoCommand : IUpdateAccountInfoCommand
{
    private readonly AccountInfoDbContextFactory _contextFactory;

    public UpdateAccountInfoCommand(AccountInfoDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }
    
    public async Task Execute(AccountInfo accountInfo)
    {
        await using var context = _contextFactory.Create();

        var accountInfoDto = accountInfo.AsDto();
        context.AccountInfos.Update(accountInfoDto);
        await context.SaveChangesAsync();
    }
}