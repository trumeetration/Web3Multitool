using System;
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

    public WithdrawFromManyAddressCommand(ViewTabViewModel viewTabViewModel, AccountInfosStore accountInfosStore)
    {
        _viewTabViewModel = viewTabViewModel;
        _accountInfosStore = accountInfosStore;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        var view = new WithdrawManyToCexDialog()
        {
            DataContext = new WithdrawManyToCexDialogViewModel()
        };

        var result = await DialogHost.Show(view, "RootDialog");

        // Todo: Add logic to withdraw funds from address

        if (result is false) return;

        var dialogViewModel = view.DataContext as WithdrawManyToCexDialogViewModel;
        var selectedChain = dialogViewModel.SelectedChain;

        // Check if RPC works
        try
        {
            var _web3 = new Web3(_viewTabViewModel.MainViewModel.Web3Utils.ChainInfosDictionary[selectedChain.Id]
                .Endpoint);
            await _web3.Eth.ChainId.SendRequestAsync();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            _viewTabViewModel.MainViewModel.AppendToLog("Invalid RPC in config");
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
        var rng = new Random();
        var keys = selectedAccounts.Select(_ => rng.NextDouble()).ToArray();
        Array.Sort(keys, selectedAccounts);

        var processedAmount = 0;
        var total = selectedAccounts.Length;

        foreach (var account in selectedAccounts)
        {
            if (account.CexAddress is null)
            {
                Debug.WriteLine("Cex address is not set");
                _viewTabViewModel.MainViewModel.AppendToLog(
                    $"[{processedAmount + 1}/{total}] - {account.Address} doesn't have CEX address");
                processedAmount++;
                continue;
            }

            var remainAmount = GetDoubleWithinRange(minRemain, maxRemain);

            var endpoint = _viewTabViewModel.MainViewModel.Web3Utils.ChainInfosDictionary[selectedChain.Id].Endpoint;
            var web3Account = new Account(account.PrivateKey, (int)selectedChain.Id);
            var web3 = new Web3(web3Account, endpoint);

            if (selectedCoin == "native")
            {
                var balance = await GetNativeBalance(account.Address, web3);
                // todo
            }
            else
            {
                var coinInfo = Utils.Web3Utils.CoinInfosDictionary[selectedChain.Id][selectedCoin];
                var coinContract = web3.Eth.GetContract(coinInfo.ABI, coinInfo.ContractAddress);
                var balanceOf = coinContract.GetFunction("balanceOf");
                var balanceWei = await balanceOf.CallAsync<BigInteger>(account.Address);
                var transferValue = balanceWei - Web3.Convert.ToWei(remainAmount, selectedChain.Id);

                if (transferValue <= 0)
                {
                    Debug.WriteLine("transfer value is lower than 0... balance could be low");
                    _viewTabViewModel.MainViewModel.AppendToLog(
                        $"[{processedAmount + 1}/{total}] - 0x...{account.Address[^8..]} Invalid remain amount: {Web3.Convert.FromWei(balanceWei, selectedChain.Id)}");
                    processedAmount++;
                    continue;
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
                    _viewTabViewModel.MainViewModel.AppendToLog($"[{processedAmount + 1}/{total}] - 0x...{account.Address[^8..]} error with estimateGas");
                    processedAmount++;
                    continue;
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
                        _viewTabViewModel.MainViewModel.AppendToLog($"[{processedAmount + 1}/{total}] - 0x...{account.Address[^8..]} error with sendTransaction");
                        processedAmount++;
                        continue;
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
                        _viewTabViewModel.MainViewModel.AppendToLog($"[{processedAmount + 1}/{total}] - 0x...{account.Address[^8..]} error with sendTransaction");
                        processedAmount++;
                        continue;
                    }

                    formattedMaxFeePaid = Web3.Convert.FromWei(gasEstimate.Value * maxFeePerGas.Value);
                }

                Debug.WriteLine(txHash);
                _viewTabViewModel.MainViewModel.AppendToLog(
                    $"[{processedAmount + 1}/{total}] - 0x...{account.Address[^8..]} amount {Web3.Convert.FromWei(transferValue, selectedChain.Id):N5} fee {formattedMaxFeePaid:N5} hash {txHash}");
            }
        }
    }

    private (HexBigInteger, HexBigInteger) CalculateEip1559GasInfo(HexBigInteger baseFee, Chain chain)
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
            maxPriorityFeePerGas = (maxFeePerGasBase * 0.1).ToInt().ToHexBigInteger();
        }

        return (maxFeePerGasBase.ToInt().ToHexBigInteger(), maxPriorityFeePerGas);
    }

    private async Task<double> GetNativeBalance(string address, IWeb3 provider)
    {
        var balanceWei = await provider.Eth.GetBalance.SendRequestAsync(address);
        var balance = (double)Web3.Convert.FromWei(balanceWei);
        return balance;
    }

    private double GetDoubleWithinRange(double lowerBound, double upperBound)
    {
        var random = new Random();
        return random.NextDouble() * (upperBound - lowerBound) + lowerBound;
    }
}