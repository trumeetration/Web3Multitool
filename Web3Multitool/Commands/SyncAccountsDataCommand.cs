using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Nethereum.Web3;
using Web3Multitool.Stores;
using Web3Multitool.Utils;
using Web3Multitool.ViewModels;

namespace Web3Multitool.Commands;

public class SyncAccountsDataCommand : AsyncCommandBase
{
    private readonly ViewTabViewModel _viewTabViewModel;
    private readonly AccountInfosStore _accountInfosStore;
    private Dictionary<Chain, Web3Utils.ChainInfo> _providerDictionary;

    public SyncAccountsDataCommand(ViewTabViewModel viewTabViewModel, AccountInfosStore accountInfosStore)
    {
        _viewTabViewModel = viewTabViewModel;
        _accountInfosStore = accountInfosStore;
    }
    
    public override async Task ExecuteAsync(object parameter)
    {
        _viewTabViewModel.IsLoading = true;

        if (_providerDictionary == null)
        {
            var web3Utils = _viewTabViewModel.MainViewModel.Web3Utils;
            _providerDictionary = web3Utils.ChainInfosDictionary;
        }
        
        foreach (var accountInfo in _accountInfosStore.AccountInfos)
        {
            var address = accountInfo.Address;
            var balances = await GetBalanceInfo(address);
            // Fantom
            if (balances.TryGetValue(Chain.Fantom, out var balance))
            {
                Debug.WriteLine($"Fantom: {balance.NativeBalance} FTM, {balance.UsdtBalance} USDT, {balance.UsdcBalance} USDC");
            }
            // Avax
            if (balances.TryGetValue(Chain.Avalanche, out balance))
            {
                Debug.WriteLine($"Avalanche: {balance.NativeBalance} AVAX, {balance.UsdtBalance} USDT, {balance.UsdcBalance} USDC");
            }
            // Polygon
            if (balances.TryGetValue(Chain.Polygon, out balance))
            {
                Debug.WriteLine($"Polygon: {balance.NativeBalance} MATIC, {balance.UsdtBalance} USDT, {balance.UsdcBalance} USDC");
            }
            // Arbitrum
            if (balances.TryGetValue(Chain.Arbitrum, out balance))
            {
                Debug.WriteLine($"Arbitrum: {balance.NativeBalance} ETH, {balance.UsdtBalance} USDT, {balance.UsdcBalance} USDC");
            }
            // Optimism
            if (balances.TryGetValue(Chain.Optimism, out balance))
            {
                Debug.WriteLine($"Optimism: {balance.NativeBalance} ETH, {balance.UsdtBalance} USDT, {balance.UsdcBalance} USDC");
            }
        }
        
        _viewTabViewModel.IsLoading = false;
    }

    private async Task<Dictionary<Chain, BalanceInfo>> GetBalanceInfo(string address)
    {
        var balanceDictionary = new Dictionary<Chain, BalanceInfo>();

        await Task.WhenAll(_providerDictionary.Select(async pair =>
        {
            try
            {
                var balanceInfo = new BalanceInfo();
                var w3 = pair.Value.Provider;
                
                var nativeBalanceWei = await w3.Eth.GetBalance.SendRequestAsync(address);
                var nativeBalanceFormatted = Web3.Convert.FromWei(nativeBalanceWei);

                var usdtContract = w3.Eth.GetContract(pair.Value.UsdtInfo.ABI, pair.Value.UsdtInfo.ContractAddress);
                var usdtBalanceOf = usdtContract.GetFunction("balanceOf");
                var usdtBalanceWei = await usdtBalanceOf.CallAsync<int>(address);
                var usdtBalanceFormatted = Web3.Convert.FromWei(usdtBalanceWei);
                
                var usdcContract = w3.Eth.GetContract(pair.Value.UsdcInfo.ABI, pair.Value.UsdcInfo.ContractAddress);
                var usdcBalanceOf = usdcContract.GetFunction("balanceOf");
                var usdcBalanceWei = await usdcBalanceOf.CallAsync<int>(address);
                var usdcBalanceFormatted = Web3.Convert.FromWei(usdcBalanceWei);
                
                balanceDictionary.Add(pair.Key, new BalanceInfo
                {
                    NativeBalance = nativeBalanceFormatted,
                    UsdtBalance = usdtBalanceFormatted,
                    UsdcBalance = usdcBalanceFormatted
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }));
        
        // foreach (var (key, value) in _providerDictionary)
        // {
        //     var weiBalance = await value.Eth.GetBalance.SendRequestAsync(address);
        //     var formattedBalance = Web3.Convert.FromWei(weiBalance);
        //     balanceDictionary.Add(key, formattedBalance);
        // }

        return balanceDictionary;
    }

    public class BalanceInfo
    {
        public decimal NativeBalance { get; set; }
        public decimal UsdtBalance { get; set; }
        public decimal UsdcBalance { get; set; }
    }
}