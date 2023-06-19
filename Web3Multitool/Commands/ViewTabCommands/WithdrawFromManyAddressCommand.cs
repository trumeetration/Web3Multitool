using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using OKX.Api.Helpers;
using Web3Multitool.Dialogs;
using Web3MultiTool.Domain.Models;
using Web3Multitool.Stores;
using Web3Multitool.Utils;
using Web3Multitool.ViewModels;
using Chain = Web3Multitool.Utils.Chain;

namespace Web3Multitool.Commands;

public class WithdrawFromManyAddressCommand : AsyncCommandBase
{
    private readonly ViewTabViewModel _viewTabViewModel;
    private readonly AccountInfosStore _accountInfosStore;
    private MainViewModel _mainViewModel;
    private int _processedAmount = 0;
    private int _total;

    public WithdrawFromManyAddressCommand(ViewTabViewModel viewTabViewModel, AccountInfosStore accountInfosStore)
    {
        _viewTabViewModel = viewTabViewModel;
        _accountInfosStore = accountInfosStore;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        _mainViewModel = _viewTabViewModel.MainViewModel;

        var view = new WithdrawManyToCexDialog()
        {
            DataContext = new WithdrawManyToCexDialogViewModel()
        };

        var result = await DialogHost.Show(view, "RootDialog");

        // Todo: Add logic to withdraw funds from address

        if (result is false) return;

        var dialogViewModel = view.DataContext as WithdrawManyToCexDialogViewModel;
        var selectedChain = dialogViewModel.SelectedChain;
        var endpoint = _viewTabViewModel.MainViewModel.Web3Utils.ChainInfosDictionary[selectedChain.Id].Endpoint;

        // Check if RPC works
        try
        {
            var _web3 = new Web3(endpoint);
            await _web3.Eth.ChainId.SendRequestAsync();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            _mainViewModel.AppendToLog("Invalid RPC in config");
            return;
        }

        var selectedCoin = dialogViewModel.SelectedCoin;
        double.TryParse(dialogViewModel.MinRemain ?? "0", NumberStyles.Any, CultureInfo.InvariantCulture,
            out var minRemain);
        double.TryParse(dialogViewModel.MaxRemain ?? "0", NumberStyles.Any, CultureInfo.InvariantCulture,
            out var maxRemain);
        double.TryParse(dialogViewModel.MinDelay ?? "0", NumberStyles.Any, CultureInfo.InvariantCulture,
            out var minDelay);
        double.TryParse(dialogViewModel.MaxDelay ?? "0", NumberStyles.Any, CultureInfo.InvariantCulture,
            out var maxDelay);

        var selectedAccounts = _accountInfosStore.AccountInfos
            .Where(acc => acc.IsSelected)
            .ToArray();
        ShuffleAccounts(selectedAccounts);

        _total = selectedAccounts.Length;

        foreach (var account in selectedAccounts)
        {
            if (await MakeTransaction(account, selectedCoin, minRemain, maxRemain, selectedChain, endpoint))
            {
                var sleepTime = GetDoubleWithinRange(minDelay, maxDelay);
                _mainViewModel.AppendToLog($"[{_processedAmount + 1}/{_total}] - Sleeping {sleepTime:N2} seconds");
                await Task.Delay(TimeSpan.FromSeconds(sleepTime));
            }
        }
    }

    private async Task<bool> MakeTransaction(AccountInfo account, string selectedCoin, double minRemain,
        double maxRemain,
        WithdrawManyToCexDialogViewModel.Chain selectedChain, string endpoint, int attempt = 1)
    {
        if (attempt > 5)
        {
            Debug.WriteLine("5 tries failed");
            return false;
        }

        if (account.CexAddress is null)
        {
            Debug.WriteLine("Cex address is not set");
            _mainViewModel.AppendToLog(
                $"[{_processedAmount + 1}/{_total}] - {account.Address} doesn't have CEX address");
            _processedAmount++;
            return false;
        }

        var remainAmount = GetDoubleWithinRange(minRemain, maxRemain);

        var web3Account = new Account(account.PrivateKey, (int)selectedChain.Id);
        var web3 = new Web3(web3Account, endpoint);

        if (selectedCoin == "native")
        {
            // todo
            return true;
        }
        else
        {
            var coinInfo = Web3Utils.CoinInfosDictionary[selectedChain.Id][selectedCoin];
            var coinContract = web3.Eth.GetContract(coinInfo.ABI, coinInfo.ContractAddress);
            var balanceOf = coinContract.GetFunction("balanceOf");
            var balanceWei = await balanceOf.CallAsync<BigInteger>(account.Address);
            var transferValue =
                Web3.Convert.ToWei(
                    (double)Math.Round(
                        Web3.Convert.FromWei(balanceWei - Web3.Convert.ToWei(remainAmount, selectedChain.Id),
                            selectedChain.Id), 6, MidpointRounding.ToZero), selectedChain.Id);

            if (transferValue <= 0)
            {
                Debug.WriteLine("transfer value is lower than 0... balance could be low");
                _mainViewModel.AppendToLog(
                    $"[{_processedAmount + 1}/{_total}] - 0x...{account.Address[^8..]} Invalid remain amount: {Web3.Convert.FromWei(balanceWei, selectedChain.Id)}");
                _processedAmount++;
                return false;
            }

            var gasPrice = await web3.Eth.GasPrice.SendRequestAsync();
            var parameters = new object[]
            {
                account.CexAddress,
                transferValue
            };
            var transfer = coinContract.GetFunction("transfer");

            HexBigInteger? gasEstimate = BigInteger.Zero.ToHexBigInteger();
            try
            {
                gasEstimate = await transfer.EstimateGasAsync(
                    from: account.Address,
                    gas: null,
                    value: null,
                    functionInput: parameters);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                _mainViewModel.AppendToLog(
                    $"[{_processedAmount + 1}/{_total}] - 0x...{account.Address[^8..]} error with estimateGas");
                await Task.Delay(2000);
                return await MakeTransaction(account, selectedCoin, minRemain, maxRemain, selectedChain, endpoint,
                    attempt + 1);
            }

            string txHash;
            decimal formattedMaxFeePaid;

            if (selectedChain.Id is Chain.Binance or Chain.Coredao or Chain.Fantom or Chain.Harmony)
            {
                // EIP-1559 is not supported
                try
                {
                    txHash = await transfer.SendTransactionAsync(
                        from: account.Address,
                        gas: gasEstimate,
                        gasPrice: gasPrice,
                        value: null,
                        functionInput: parameters);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    _mainViewModel.AppendToLog(
                        $"[{_processedAmount + 1}/{_total}] - 0x...{account.Address[^8..]} error with sendTransaction");
                    await Task.Delay(2000);
                    return await MakeTransaction(account, selectedCoin, minRemain, maxRemain, selectedChain, endpoint,
                        attempt + 1);
                }

                formattedMaxFeePaid = Web3.Convert.FromWei(gasPrice.Value * gasEstimate.Value);
            }
            else
            {
                // EIP-1559 is supported
                var (maxFeePerGas, maxPriorityFeePerGas) = CalculateEip1559GasInfo(gasPrice, selectedChain.Id);

                try
                {
                    txHash = await transfer.SendTransactionAsync(
                        from: account.Address,
                        gas: gasEstimate,
                        value: null,
                        maxFeePerGas: maxFeePerGas,
                        maxPriorityFeePerGas: maxPriorityFeePerGas,
                        functionInput: parameters
                    );
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    _mainViewModel.AppendToLog(
                        $"[{_processedAmount + 1}/{_total}] - 0x...{account.Address[^8..]} error with sendTransaction");
                    await Task.Delay(2000);
                    return await MakeTransaction(account, selectedCoin, minRemain, maxRemain, selectedChain, endpoint,
                        attempt + 1);
                }

                formattedMaxFeePaid = Web3.Convert.FromWei(gasEstimate.Value * maxFeePerGas.Value);
            }

            Debug.WriteLine(txHash);
            _mainViewModel.AppendToLog(
                $"[{_processedAmount + 1}/{_total}] - 0x...{account.Address[^8..]} amount {Web3.Convert.FromWei(transferValue, selectedChain.Id):N5} fee {formattedMaxFeePaid:N5} hash {txHash}");
            return true;
        }
    }

    private void ShuffleAccounts(AccountInfo[] selectedAccounts)
    {
        var rng = new Random();
        var keys = selectedAccounts.Select(_ => rng.NextDouble()).ToArray();
        Array.Sort(keys, selectedAccounts);
    }

    private static (HexBigInteger, HexBigInteger) CalculateEip1559GasInfo(HexBigInteger baseFee, Chain chain)
    {
        var maxFeePerGasBase = baseFee.Value.ToDouble();
        HexBigInteger maxPriorityFeePerGas;

        if (chain is Chain.Arbitrum)
        {
            maxFeePerGasBase *= 1.35;
            maxPriorityFeePerGas = BigInteger.Zero.ToHexBigInteger();
        }
        else
        {
            maxFeePerGasBase *= 2;
            maxPriorityFeePerGas = ((int)(maxFeePerGasBase * 0.1)).ToHexBigInteger();
        }

        return (maxFeePerGasBase.ToInt().ToHexBigInteger(), maxPriorityFeePerGas);
    }

    private static double GetDoubleWithinRange(double lowerBound, double upperBound)
    {
        var random = new Random();
        return random.NextDouble() * (upperBound - lowerBound) + lowerBound;
    }
}