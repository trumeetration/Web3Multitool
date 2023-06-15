using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using Web3Multitool.Dialogs;
using Web3MultiTool.Domain.Models;
using Web3Multitool.Stores;
using Web3Multitool.ViewModels;

namespace Web3Multitool.Commands;

public class EditManyCexAddressesCommand : AsyncCommandBase
{
    private readonly ViewTabViewModel _viewTabViewModel;
    private readonly AccountInfosStore _accountInfosStore;

    public EditManyCexAddressesCommand(ViewTabViewModel viewTabViewModel, AccountInfosStore accountInfosStore)
    {
        _viewTabViewModel = viewTabViewModel;
        _accountInfosStore = accountInfosStore;
    }
    
    public override async Task ExecuteAsync(object data)
    {
        var view = new EditManyCexDialog()
        {
            DataContext = new EditManyCexDialogViewModel()
        };

        var result = await DialogHost.Show(view, "RootDialog");

        if (result is true)
        {
            var dialogViewModel = view.DataContext as EditManyCexDialogViewModel;
            var inputCexAddresses = dialogViewModel?.Addresses.Trim().Split(Environment.NewLine);

            if (inputCexAddresses.Length == _accountInfosStore.AccountInfos.Count(acc => acc.IsSelected))
            {
                var accountsToUpdate = _accountInfosStore.AccountInfos.Where(acc => acc.IsSelected).ToList();

                for (int i = 0; i < inputCexAddresses.Length; i++)
                {
                    var selectedAccountInfo = accountsToUpdate[i];
                    
                    var updatedAccountInfo = new AccountInfo
                    {
                        Id = selectedAccountInfo.Id,
                        Address = selectedAccountInfo.Address,
                        CexAddress = inputCexAddresses[i].Trim(),
                        PrivateKey = selectedAccountInfo.PrivateKey,
                        FantomInfo = selectedAccountInfo.FantomInfo,
                        AvaxInfo = selectedAccountInfo.AvaxInfo,
                        PolygonInfo = selectedAccountInfo.PolygonInfo,
                        ArbitrumInfo = selectedAccountInfo.ArbitrumInfo,
                        OptimismInfo = selectedAccountInfo.OptimismInfo,
                        BnbInfo = selectedAccountInfo.BnbInfo,
                        CoredaoInfo = selectedAccountInfo.CoredaoInfo,
                        HarmonyInfo = selectedAccountInfo.HarmonyInfo,
                        TotalBalanceUsd = selectedAccountInfo.TotalBalanceUsd,
                        TotalTxAmount = selectedAccountInfo.TotalTxAmount
                    };
                    
                    await _accountInfosStore.Update(updatedAccountInfo); // Temp solution. Need to make method to update all entities in 1 request
                }
            }
            else
            {
                Debug.WriteLine("Input amount differs from selected amount");
            }
        }

        Debug.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
    }
}