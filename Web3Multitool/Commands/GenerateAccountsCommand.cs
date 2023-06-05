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
        var amount = int.Parse(_viewTabViewModel.GenerateInputAmount);
        _viewTabViewModel.GenerateInputAmount = string.Empty;

        var newAccountInfos = new List<AccountInfo>();
        for (var i = 0; i < amount; i++)
        {
            var (privateKey, address) = GenerateAccount();
            
            var accInfo = new AccountInfo
            {
                PrivateKey = privateKey,
                Address = address,
                FantomInfo = new AddressChainInfo { ChainId = 250 },
                AvaxInfo = new AddressChainInfo { ChainId = 43114 },
                PolygonInfo = new AddressChainInfo { ChainId = 137 },
                ArbitrumInfo = new AddressChainInfo { ChainId = 42161 },
                OptimismInfo = new AddressChainInfo { ChainId = 10 }
            };
            
            newAccountInfos.Add(accInfo);
        }

        try
        {
            await _accountInfosStore.Add(newAccountInfos);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            throw;
        }
        finally
        {
            
        }
    }
    
    private (string, string) GenerateAccount()
    {
        var privateKey = EthECKey.GenerateKey().GetPrivateKeyAsBytes().ToHex();
        var address = EthECKey.GetPublicAddress(privateKey);

        return (privateKey, address);
    }
}