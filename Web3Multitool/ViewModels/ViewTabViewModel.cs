using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Web3Multitool.Commands;
using Web3Multitool.Dialogs;
using Web3MultiTool.Domain.Models;
using Web3Multitool.Stores;

namespace Web3Multitool.ViewModels;

public class ViewTabViewModel : BaseViewModel
{
    private readonly AccountInfosStore _accountInfosStore;

    private readonly ObservableCollection<AccountInfo> _accountInfos;
    public IEnumerable<AccountInfo> AccountInfos => _accountInfos;

    public ICommand ImportAccountsFromFileCommand { get; }
    public ICommand ExportAccountsToFileCommand { get; }
    public ICommand ClearAccountInfosCommand { get; }
    public ICommand EditCexAddressCommand { get; }
    public ICommand GenerateAccountsCommand { get; }
    public ICommand LoadAccountInfosCommand { get; }
    

    public ViewTabViewModel(AccountInfosStore accountInfosStore)
    {
        _accountInfosStore = accountInfosStore;
        _accountInfos = new ObservableCollection<AccountInfo>();
        
        _accountInfosStore.AccountInfosAdded += AccountInfosStoreOnAccountInfosAdded;
        _accountInfosStore.AccountInfoDeleted += AccountInfosStoreOnAccountInfoDeleted;
        _accountInfosStore.AccountInfosCleared += AccountInfosStoreOnAccountInfosCleared;
        _accountInfosStore.AccountInfosLoaded += AccountInfosStoreOnAccountInfosLoaded;

        LoadAccountInfosCommand = new LoadAccountInfosCommand(this, _accountInfosStore);
        ImportAccountsFromFileCommand = new ImportAccountsFromFileCommand(this, _accountInfosStore);
        ExportAccountsToFileCommand = new ExportAccountsToFileCommand(this, _accountInfosStore);
        ClearAccountInfosCommand = new ClearAccountInfosCommand(this, _accountInfosStore);
        EditCexAddressCommand = new EditCexAddressCommand(this, _accountInfosStore);
        GenerateAccountsCommand = new GenerateAccountsCommand(this, _accountInfosStore);
    }

    protected override void Dispose()
    {
        _accountInfosStore.AccountInfosAdded -= AccountInfosStoreOnAccountInfosAdded;
        _accountInfosStore.AccountInfoDeleted -= AccountInfosStoreOnAccountInfoDeleted;
        _accountInfosStore.AccountInfosCleared -= AccountInfosStoreOnAccountInfosCleared;
        _accountInfosStore.AccountInfosLoaded -= AccountInfosStoreOnAccountInfosLoaded;

        base.Dispose();
    }

    private void AccountInfosStoreOnAccountInfosLoaded()
    {
        _accountInfos.Clear();

        foreach (var accountInfo in _accountInfosStore.AccountInfos)
        {
            _accountInfos.Add(accountInfo);
        }
    }

    private void AccountInfosStoreOnAccountInfosCleared()
    {
        _accountInfos.Clear();
    }

    private void AccountInfosStoreOnAccountInfoDeleted(string address)
    {
        var accountInfo = _accountInfos.FirstOrDefault(accInfo => accInfo.Address == address);

        if (accountInfo != null)
            _accountInfos.Remove(accountInfo);
    }

    private void AccountInfosStoreOnAccountInfosAdded(IEnumerable<AccountInfo> accountInfos)
    {
        foreach (var accountInfo in accountInfos)
        {
            _accountInfos.Add(accountInfo);
        }
    }

    private string _generateInputAmount;

    public string GenerateInputAmount
    {
        get => _generateInputAmount;
        set => SetField(ref _generateInputAmount, value);
    }

    public static ViewTabViewModel LoadViewModel(AccountInfosStore accountInfosStore)
    {
        var viewModel = new ViewTabViewModel(accountInfosStore);
        
        viewModel.LoadAccountInfosCommand.Execute(null);

        return viewModel;
    }
}