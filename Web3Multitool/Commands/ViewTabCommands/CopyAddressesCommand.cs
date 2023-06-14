using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Web3Multitool.Stores;
using Web3Multitool.ViewModels;

namespace Web3Multitool.Commands;

public class CopyAddressesCommand : CommandBase
{
    private readonly ViewTabViewModel _viewTabViewModel;
    private readonly AccountInfosStore _accountInfosStore;

    public CopyAddressesCommand(ViewTabViewModel viewTabViewModel, AccountInfosStore accountInfosStore)
    {
        _viewTabViewModel = viewTabViewModel;
        _accountInfosStore = accountInfosStore;
    }

    public override void Execute(object parameter)
    {
        var addressList = string.Join(Environment.NewLine, _accountInfosStore.AccountInfos
            .Where(acc => acc.IsSelected)
            .Select(acc => acc.Address));
        
        Clipboard.SetText(addressList);
        
        _viewTabViewModel.MainViewModel.AppendToLog("Addresses are in clipboard now!");
    }
}