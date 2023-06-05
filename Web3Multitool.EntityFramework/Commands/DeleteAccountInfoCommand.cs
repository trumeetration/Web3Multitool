using Web3MultiTool.Domain.Commands;
using Web3Multitool.EntityFramework.DTOs;

namespace Web3Multitool.EntityFramework.Commands;

public class DeleteAccountInfoCommand : IDeleteAccountInfoCommand
{
    private readonly AccountInfoDbContextFactory _contextFactory;

    public DeleteAccountInfoCommand (AccountInfoDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }
    
    public async Task Execute(string address)
    {
        await using var context = _contextFactory.Create();

        var accountInfoDto = new AccountInfoDto
        {
            Address = address
        };

        context.AccountInfos.Remove(accountInfoDto);
        await context.SaveChangesAsync();
    }   
}