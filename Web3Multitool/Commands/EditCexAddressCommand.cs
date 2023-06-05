using System.Diagnostics;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using Web3Multitool.Dialogs;
using Web3Multitool.Stores;
using Web3Multitool.ViewModels;

namespace Web3Multitool.Commands;

public class EditCexAddressCommand : AsyncCommandBase
{
    private readonly ViewTabViewModel _viewTabViewModel;
    private readonly AccountInfosStore _accountInfosStore;

    public EditCexAddressCommand(ViewTabViewModel viewTabViewModel, AccountInfosStore accountInfosStore)
    {
        _viewTabViewModel = viewTabViewModel;
        _accountInfosStore = accountInfosStore;
    }
    
    public override async Task ExecuteAsync(object selectedAccountInfoDto)
    {
        var view = new EditCexDialog()
        {
            DataContext = new EditCexDialogViewModel()
        };

        var result = await DialogHost.Show(view, "RootDialog");

        if (result is true)
        {
            var dialogViewModel = view.DataContext as EditCexDialogViewModel;
            var inputCexAddress = dialogViewModel?.Address;
            Debug.WriteLine($"Entered address: {inputCexAddress}");
            // (selectedAccountInfoDto as AccountInfoDto).CexAddress = inputCexAddress;
        }

        Debug.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
    }
}