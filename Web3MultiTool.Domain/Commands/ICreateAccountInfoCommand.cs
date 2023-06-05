using Web3MultiTool.Domain.Models;

namespace Web3MultiTool.Domain.Commands;

public interface ICreateAccountInfoCommand
{
    Task Execute(List<AccountInfo> accountInfos);
}