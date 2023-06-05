using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web3MultiTool.Domain.Commands;
using Web3MultiTool.Domain.Models;
using Web3MultiTool.Domain.Queries;

namespace Web3Multitool.Stores;

public class AccountInfosStore
{
    private readonly IGetAllAccountsQuery _getAllAccountsQuery;
    private readonly ICreateAccountInfoCommand _createAccountInfoCommand;
    private readonly IUpdateAccountInfoCommand _updateAccountInfoCommand;
    private readonly IDeleteAccountInfoCommand _deleteAccountInfoCommand;
    private readonly IClearAccountInfosCommand _clearAccountInfosCommand;

    private readonly List<AccountInfo> _accountInfos;
    public IEnumerable<AccountInfo> AccountInfos => _accountInfos;

    public event Action? AccountInfosLoaded;
    public event Action<IEnumerable<AccountInfo>>? AccountInfosAdded;
    public event Action<AccountInfo>? AccountInfoUpdated;
    public event Action<string>? AccountInfoDeleted;
    public event Action? AccountInfosCleared;
    
    public AccountInfosStore(
        IGetAllAccountsQuery getAllAccountsQuery,
        ICreateAccountInfoCommand createAccountInfoCommand,
        IUpdateAccountInfoCommand updateAccountInfoCommand,
        IDeleteAccountInfoCommand deleteAccountInfoCommand,
        IClearAccountInfosCommand clearAccountInfosCommand
    )
    {
        _getAllAccountsQuery = getAllAccountsQuery;
        _createAccountInfoCommand = createAccountInfoCommand;
        _updateAccountInfoCommand = updateAccountInfoCommand;
        _deleteAccountInfoCommand = deleteAccountInfoCommand;
        _clearAccountInfosCommand = clearAccountInfosCommand;

        _accountInfos = new List<AccountInfo>();
    }

    public async Task Add(List<AccountInfo> accountInfos)
    {
        await _createAccountInfoCommand.Execute(accountInfos);
        
        _accountInfos.AddRange(accountInfos);
        
        AccountInfosAdded?.Invoke(accountInfos);
    }

    public async Task Load()
    {
        var accountInfos = await _getAllAccountsQuery.Execute();

        _accountInfos.Clear();
        _accountInfos.AddRange(accountInfos);
        
        AccountInfosLoaded?.Invoke();
    }

    public async Task Update(AccountInfo accountInfo)
    {
        await _updateAccountInfoCommand.Execute(accountInfo);

        var currentIndex = _accountInfos.FindIndex(x => x.Address == accountInfo.Address);
        _accountInfos[currentIndex] = accountInfo;
        
        AccountInfoUpdated?.Invoke(accountInfo);
    }

    public async Task Delete(string address)
    {
        await _deleteAccountInfoCommand.Execute(address);

        _accountInfos.RemoveAll(x => x.Address == address);
        
        AccountInfoDeleted?.Invoke(address);
    }

    public async Task Clear()
    {
        await _clearAccountInfosCommand.Execute();
        
        _accountInfos.Clear();
        
        AccountInfosCleared?.Invoke();
    }
}