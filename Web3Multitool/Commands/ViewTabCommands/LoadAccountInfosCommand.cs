using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Web3Multitool.Stores;
using Web3Multitool.ViewModels;

namespace Web3Multitool.Commands;

public class LoadAccountInfosCommand : AsyncCommandBase
{
    private readonly ViewTabViewModel _viewTabViewModel;
    private readonly AccountInfosStore _accountInfosStore;

    public LoadAccountInfosCommand(ViewTabViewModel viewTabViewModel, AccountInfosStore accountInfosStore)
    {
        _viewTabViewModel = viewTabViewModel;
        _accountInfosStore = accountInfosStore;
    }
    
    public override async Task ExecuteAsync(object parameter)
    {
        try
        {
            await _accountInfosStore.Load();
            double totalUsd = 0;
            var totalTx = 0;

            foreach (var acc in _accountInfosStore.AccountInfos)
            {
                totalUsd += acc.TotalBalanceUsd;
                totalTx += acc.TotalTxAmount;
            }

            _viewTabViewModel.TotalUsd = totalUsd;
            _viewTabViewModel.TotalTxAmount = totalTx;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
        finally
        {
            
        }
    }
}