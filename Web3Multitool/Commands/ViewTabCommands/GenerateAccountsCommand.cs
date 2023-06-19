using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Web3MultiTool.Domain.Models;
using Web3Multitool.Stores;
using Web3Multitool.ViewModels;

namespace Web3Multitool.Commands;

public class GenerateAccountsCommand : AsyncCommandBase
{
    private readonly ViewTabViewModel _viewTabViewModel;
    private readonly AccountInfosStore _accountInfosStore;

    public GenerateAccountsCommand(ViewTabViewModel viewTabViewModel, AccountInfosStore accountInfosStore)
    {
        _viewTabViewModel = viewTabViewModel;
        _accountInfosStore = accountInfosStore;
    }
    
    public override async Task ExecuteAsync(object parameter)
    {
        _viewTabViewModel.IsLoading = true;
        
        var amount = int.Parse(_viewTabViewModel.GenerateInputAmount);
        _viewTabViewModel.GenerateInputAmount = string.Empty;

        var newAccountInfos = new List<AccountInfo>();
        for (var i = 0; i < amount; i++)
        {
            var (privateKey, address) = GenerateAccount();
            
            var accInfo = new AccountInfo
            {
                Id = Guid.NewGuid(),
                PrivateKey = privateKey,
                Address = address
            };
            
            newAccountInfos.Add(accInfo);
        }

        try
        {
            await Task.Delay(1000);
            await _accountInfosStore.Add(newAccountInfos);
            _viewTabViewModel.LoadAccountInfosCommand.Execute(null);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            throw;
        }
        finally
        {
            await Task.Delay(1000);
            _viewTabViewModel.IsLoading = false;
        }
    }
    
    private (string, string) GenerateAccount()
    {
        var privateKey = "0x" + EthECKey.GenerateKey().GetPrivateKeyAsBytes().ToHex();
        var address = EthECKey.GetPublicAddress(privateKey);

        return (privateKey, address);
    }
}