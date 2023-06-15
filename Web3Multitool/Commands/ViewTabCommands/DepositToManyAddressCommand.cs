using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using OKX.Api;
using Web3Multitool.Dialogs;
using Web3Multitool.Stores;
using Web3Multitool.Utils;
using Web3Multitool.ViewModels;

namespace Web3Multitool.Commands;

public class DepositToManyAddressCommand : AsyncCommandBase
{
    private readonly ViewTabViewModel _viewTabViewModel;
    private readonly AccountInfosStore _accountInfosStore;

    public DepositToManyAddressCommand(ViewTabViewModel viewTabViewModel, AccountInfosStore accountInfosStore)
    {
        _viewTabViewModel = viewTabViewModel;
        _accountInfosStore = accountInfosStore;
    }
    
    public override async Task ExecuteAsync(object parameter)
    {
        var view = new DepositToManyAddressDialog()
        {
            DataContext = new DepositToManyAddressDialogViewModel()
        };

        var result = await DialogHost.Show(view, "RootDialog");

        if (result is false) return;

        var dialogViewModel = view.DataContext as DepositToManyAddressDialogViewModel;
        var selectedChain = dialogViewModel.SelectedChain;
        var selectedCoin = dialogViewModel.SelectedCoin;
        var selectedAmountFrom = Convert.ToDouble(dialogViewModel.AmountFrom);
        var selectedAmountTo = dialogViewModel.NeedToRandomize ? Convert.ToDouble(dialogViewModel.AmountTo) : selectedAmountFrom;
        var minDelay = Convert.ToDouble(dialogViewModel.MinDelay);
        var maxDelay = Convert.ToDouble(dialogViewModel.MaxDelay);
        
        var selectedAccounts = _accountInfosStore.AccountInfos.Where(acc => acc.IsSelected);
        
        var api = new OKXRestApiClient();
        var apiCredentials = _viewTabViewModel.MainViewModel.OKXApiInfo;
        api.SetApiCredentials(apiCredentials.ApiKey, apiCredentials.SecretKey, apiCredentials.PassPhrase);
        
        var apiGetCurrenciesResponse = await api.Funding.GetCurrenciesAsync();

        if (apiGetCurrenciesResponse.Success == false)
        {
            Debug.WriteLine($"get currencies info error: {apiGetCurrenciesResponse.Error.Message}");
            return;
        }
        
        var currency = apiGetCurrenciesResponse.Data.SingleOrDefault(coin => coin.Currency == selectedCoin && coin.Chain == selectedChain);
        
        foreach (var account in selectedAccounts)
        {
            var amountToDeposit = GetDoubleWithinRange(selectedAmountFrom, selectedAmountTo);
            
            _viewTabViewModel.MainViewModel.AppendToLog($"Withdrawing for {account.Address} {amountToDeposit} {selectedCoin}");
            
            // var result = await api.Funding.WithdrawAsync()
            
            _viewTabViewModel.MainViewModel.AppendToLog($"Success for {account.Address}");
            
            // Sleep some time
            var sleepTime = GetDoubleWithinRange(minDelay, maxDelay);
            await Task.Delay(TimeSpan.FromSeconds(sleepTime));
            _viewTabViewModel.MainViewModel.AppendToLog($"Slept {sleepTime} seconds");
        }
    }
    
    private double GetDoubleWithinRange(double lowerBound, double upperBound)
    {
        var random = new Random();
        return random.NextDouble() * (upperBound - lowerBound) + lowerBound;
    }
}