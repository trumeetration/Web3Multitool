using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using OKX.Api;
using OKX.Api.Enums;
using Web3Multitool.Dialogs;
using Web3MultiTool.Domain.Models;
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
        _viewTabViewModel.IsLoading = true;

        // Get currencies and send it to dialog viewmodel
        var api = new OKXRestApiClient();
        var apiCredentials = _viewTabViewModel.MainViewModel.OKXApiInfo;
        api.SetApiCredentials(apiCredentials.ApiKey, apiCredentials.SecretKey, apiCredentials.PassPhrase);

        var balanceResponse = await api.Funding.GetFundingBalanceAsync();
        var currenciesResponse = await api.Funding.GetCurrenciesAsync();

        _viewTabViewModel.IsLoading = false;

        if (balanceResponse.Success == false)
        {
            Debug.WriteLine($"get currencies info error: {balanceResponse.Error.Message}");
            return;
        }

        if (currenciesResponse.Success == false)
        {
            Debug.WriteLine($"get currencies info error: {currenciesResponse.Error.Message}");
            return;
        }

        var view = new DepositToManyAddressDialog()
        {
            DataContext = new DepositToManyAddressDialogViewModel(currenciesResponse.Data, balanceResponse.Data)
        };

        var result = await DialogHost.Show(view, "RootDialog");

        if (result is false) return;

        var dialogViewModel = view.DataContext as DepositToManyAddressDialogViewModel;
        var selectedChain = dialogViewModel.SelectedChain;
        var selectedCoin = dialogViewModel.SelectedCoin;
        var selectedAmountFrom = Convert.ToDouble(dialogViewModel.AmountFrom);
        var selectedAmountTo = dialogViewModel.NeedToRandomize
            ? Convert.ToDouble(dialogViewModel.AmountTo)
            : selectedAmountFrom;
        var minDelay = Convert.ToDouble(dialogViewModel.MinDelay);
        var maxDelay = Convert.ToDouble(dialogViewModel.MaxDelay);

        var selectedAccounts = _accountInfosStore.AccountInfos.Where(acc => acc.IsSelected);

        var processedAmount = 0;
        var accountInfos = selectedAccounts as AccountInfo[] ?? selectedAccounts.ToArray();
        var rng = new Random();
        var keys = accountInfos.Select(e => rng.NextDouble()).ToArray();
        Array.Sort(keys, accountInfos);
        var total = accountInfos.Length;
        foreach (var account in accountInfos)
        {
            var amountToDeposit = decimal.Round((decimal)GetDoubleWithinRange(selectedAmountFrom, selectedAmountTo), 5);

            _viewTabViewModel.MainViewModel.AppendToLog(
                $"[{processedAmount+1}/{total}] Withdrawing for {account.Address} {amountToDeposit} {selectedCoin.Symbol}");

            var withdrawResult = await api.Funding.WithdrawAsync(selectedCoin.Symbol, amountToDeposit,
                OkxWithdrawalDestination.DigitalCurrencyAddress, account.Address, selectedChain.Fee,
                selectedChain.Chain);

            if (withdrawResult.Data is null)
            {
                _viewTabViewModel.MainViewModel.AppendToLog($"[{processedAmount+1}/{total}] Fail for {account.Address} - {withdrawResult.Error?.Message}");
            }
            else
            {
                _viewTabViewModel.MainViewModel.AppendToLog($"[{processedAmount+1}/{total}] Success for {account.Address} - {withdrawResult.Data.WithdrawalId}");
            }
            
            // Sleep some time
            var sleepTime = GetDoubleWithinRange(minDelay, maxDelay);
            _viewTabViewModel.MainViewModel.AppendToLog($"Sleeping {sleepTime} seconds");
            await Task.Delay(TimeSpan.FromSeconds(sleepTime));

            processedAmount++;
        }
    }

    private double GetDoubleWithinRange(double lowerBound, double upperBound)
    {
        var random = new Random();
        return random.NextDouble() * (upperBound - lowerBound) + lowerBound;
    }
}