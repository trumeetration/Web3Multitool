using Web3MultiTool.Domain.Models;

namespace Web3MultiTool.Domain.Commands;

public interface IUpdateAccountInfoCommand
{
    Task Execute(AccountInfo accountInfo);
}