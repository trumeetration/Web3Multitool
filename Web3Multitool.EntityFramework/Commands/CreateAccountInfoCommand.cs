using Web3MultiTool.Domain.Commands;
using Web3MultiTool.Domain.Models;

namespace Web3Multitool.EntityFramework.Commands;

public class CreateAccountInfoCommand : ICreateAccountInfoCommand
{
    private readonly AccountInfoDbContextFactory _contextFactory;

    public CreateAccountInfoCommand(AccountInfoDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task Execute(List<AccountInfo> accountInfos)
    {
        await using var context = _contextFactory.Create();
        var accountInfoDtos = accountInfos.Select(info => info.AsDto());
        context.AccountInfos.AddRange(accountInfoDtos);
        await context.SaveChangesAsync();
    }
}