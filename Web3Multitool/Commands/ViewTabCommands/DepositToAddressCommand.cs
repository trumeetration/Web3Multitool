﻿using System.Diagnostics;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using Web3Multitool.Dialogs;
using Web3Multitool.Stores;
using Web3Multitool.ViewModels;

namespace Web3Multitool.Commands;

public class DepositToAddressCommand : AsyncCommandBase
{
    private readonly ViewTabViewModel _viewTabViewModel;
    private readonly AccountInfosStore _accountInfosStore;

    public DepositToAddressCommand(ViewTabViewModel viewTabViewModel, AccountInfosStore accountInfosStore)
    {
        _viewTabViewModel = viewTabViewModel;
        _accountInfosStore = accountInfosStore;
    }
    
    public override async Task ExecuteAsync(object parameter)
    {
        var view = new DepositToAddressDialog()
        {
            DataContext = new DepositToAddressDialogViewModel()
        };

        var result = await DialogHost.Show(view, "RootDialog");
        
        // Todo: Add logic to deposit 

        Debug.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));

    }
}