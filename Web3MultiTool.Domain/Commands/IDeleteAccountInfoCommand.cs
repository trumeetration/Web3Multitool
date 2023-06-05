namespace Web3MultiTool.Domain.Commands;

public interface IDeleteAccountInfoCommand
{
    Task Execute(string address);
}