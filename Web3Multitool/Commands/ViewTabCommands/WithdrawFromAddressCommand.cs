using System.Diagnostics;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using Web3Multitool.Dialogs;
using Web3MultiTool.Domain.Models;
using Web3Multitool.Stores;
using Web3Multitool.ViewModels;

namespace Web3Multitool.Commands;

public class WithdrawFromAddressCommand : AsyncCommandBase
{
    private readonly ViewTabViewModel _viewTabViewModel;
    private readonly AccountInfosStore _accountInfosStore;

    public WithdrawFromAddressCommand(ViewTabViewModel viewTabViewModel, AccountInfosStore accountInfosStore)
    {
        _viewTabViewModel = viewTabViewModel;
        _accountInfosStore = accountInfosStore;
    }
    
    public override async Task ExecuteAsync(object parameter)
    {
        var address = (parameter as AccountInfo)?.Address;
        var view = new WithdrawToCexDialog()
        {
            DataContext = new WithdrawToCexDialogViewModel(address)
        };
        
        var result = await DialogHost.Show(view, "RootDialog");
        
        // Todo: Add logic to withdraw funds from address
        
        
        
        Debug.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
    }
}