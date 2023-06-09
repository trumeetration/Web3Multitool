using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Nethereum.Web3;
using Web3MultiTool.Domain.Models;
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
    
    [SuppressMessage("ReSharper.DPA", "DPA0006: Large number of DB commands", MessageId = "count: 50")]
    public override async Task ExecuteAsync(object parameter)
    {
        _viewTabViewModel.IsLoading = true;

        if (_providerDictionary == null)
        {
            var web3Utils = _viewTabViewModel.MainViewModel.Web3Utils;
            _providerDictionary = web3Utils.ChainInfosDictionary;
        }
        
        for (int i = 0; i < _accountInfosStore.AccountInfos.Count(); i++)
        {
            var accountInfo = _accountInfosStore.AccountInfos.ElementAt(i);
            var address = accountInfo.Address;
            var addressInfo = await GetAddressInfo(address);

            var updatedAccountInfo = new AccountInfo
            {
                IsSelected = false,
                Id = accountInfo.Id,
                Address = accountInfo.Address,
                CexAddress = accountInfo.CexAddress,
                PrivateKey = accountInfo.PrivateKey,
                FantomInfo = accountInfo.FantomInfo,
                AvaxInfo = accountInfo.AvaxInfo,
                PolygonInfo = accountInfo.PolygonInfo,
                ArbitrumInfo = accountInfo.ArbitrumInfo,
                OptimismInfo = accountInfo.OptimismInfo,
                TotalBalanceUsd = 0,
                TotalTxAmount = 0
            };

            // Fantom
            if (addressInfo.TryGetValue(Chain.Fantom, out var info))
            {
                updatedAccountInfo.FantomInfo.UsdtBalance = info.UsdtBalance;
                updatedAccountInfo.FantomInfo.UsdcBalance = info.UsdcBalance;
                updatedAccountInfo.FantomInfo.NativeBalance = info.NativeBalance;
                updatedAccountInfo.FantomInfo.TxAmount = info.TxAmount;
                
                Debug.WriteLine($"Fantom: {info.NativeBalance} FTM, {info.UsdtBalance} USDT, {info.UsdcBalance} USDC");
            }
            
            // Avax
            if (addressInfo.TryGetValue(Chain.Avalanche, out info))
            {
                updatedAccountInfo.AvaxInfo.UsdtBalance = info.UsdtBalance;
                updatedAccountInfo.AvaxInfo.UsdcBalance = info.UsdcBalance;
                updatedAccountInfo.AvaxInfo.NativeBalance = info.NativeBalance;
                updatedAccountInfo.AvaxInfo.TxAmount = info.TxAmount;
                
                Debug.WriteLine($"Avalanche: {info.NativeBalance} AVAX, {info.UsdtBalance} USDT, {info.UsdcBalance} USDC");
            }
            
            // Polygon
            if (addressInfo.TryGetValue(Chain.Polygon, out info))
            {
                updatedAccountInfo.PolygonInfo.UsdtBalance = info.UsdtBalance;
                updatedAccountInfo.PolygonInfo.UsdcBalance = info.UsdcBalance;
                updatedAccountInfo.PolygonInfo.NativeBalance = info.NativeBalance;
                updatedAccountInfo.PolygonInfo.TxAmount = info.TxAmount;
                
                Debug.WriteLine($"Polygon: {info.NativeBalance} MATIC, {info.UsdtBalance} USDT, {info.UsdcBalance} USDC");
            }
            
            // Arbitrum
            if (addressInfo.TryGetValue(Chain.Arbitrum, out info))
            {
                updatedAccountInfo.ArbitrumInfo.UsdtBalance = info.UsdtBalance;
                updatedAccountInfo.ArbitrumInfo.UsdcBalance = info.UsdcBalance;
                updatedAccountInfo.ArbitrumInfo.NativeBalance = info.NativeBalance;
                updatedAccountInfo.ArbitrumInfo.TxAmount = info.TxAmount;
                
                Debug.WriteLine($"Arbitrum: {info.NativeBalance} ETH, {info.UsdtBalance} USDT, {info.UsdcBalance} USDC");
            }
            
            // Optimism
            if (addressInfo.TryGetValue(Chain.Optimism, out info))
            {
                updatedAccountInfo.OptimismInfo.UsdtBalance = info.UsdtBalance;
                updatedAccountInfo.OptimismInfo.UsdcBalance = info.UsdcBalance;
                updatedAccountInfo.OptimismInfo.NativeBalance = info.NativeBalance;
                updatedAccountInfo.OptimismInfo.TxAmount = info.TxAmount;
                
                Debug.WriteLine($"Optimism: {info.NativeBalance} ETH, {info.UsdtBalance} USDT, {info.UsdcBalance} USDC");
            }
            
            // Total
            var totalTx = updatedAccountInfo.ArbitrumInfo.TxAmount +
                          updatedAccountInfo.FantomInfo.TxAmount +
                          updatedAccountInfo.OptimismInfo.TxAmount +
                          updatedAccountInfo.PolygonInfo.TxAmount +
                          updatedAccountInfo.AvaxInfo.TxAmount;
            
            var ftmPrice = _viewTabViewModel.MainViewModel.FtmPrice;
            var avaxPrice = _viewTabViewModel.MainViewModel.AvaxPrice;
            var maticPrice = _viewTabViewModel.MainViewModel.MaticPrice;
            var ethPrice = _viewTabViewModel.MainViewModel.EthPrice;
            var bnbPrice = _viewTabViewModel.MainViewModel.BnbPrice;
            
            var totalUsd = updatedAccountInfo.ArbitrumInfo.NativeBalance * ethPrice +
                           updatedAccountInfo.ArbitrumInfo.UsdcBalance +
                           updatedAccountInfo.ArbitrumInfo.UsdtBalance +
                           updatedAccountInfo.FantomInfo.NativeBalance * ftmPrice +
                           updatedAccountInfo.FantomInfo.UsdcBalance +
                           updatedAccountInfo.FantomInfo.UsdtBalance +
                           updatedAccountInfo.OptimismInfo.NativeBalance * ethPrice +
                           updatedAccountInfo.OptimismInfo.UsdcBalance +
                           updatedAccountInfo.OptimismInfo.UsdtBalance +
                           updatedAccountInfo.PolygonInfo.NativeBalance * maticPrice +
                           updatedAccountInfo.PolygonInfo.UsdcBalance +
                           updatedAccountInfo.PolygonInfo.UsdtBalance +
                           updatedAccountInfo.AvaxInfo.NativeBalance * avaxPrice +
                           updatedAccountInfo.AvaxInfo.UsdcBalance +
                           updatedAccountInfo.AvaxInfo.UsdtBalance;

            updatedAccountInfo.TotalTxAmount = totalTx;
            updatedAccountInfo.TotalBalanceUsd = totalUsd;
            
            await _accountInfosStore.Update(updatedAccountInfo);
        }
        
        _viewTabViewModel.IsLoading = false;
    }

    private async Task<Dictionary<Chain, AddressInfo>> GetAddressInfo(string address)
    {
        var addressInfosDictionary = new Dictionary<Chain, AddressInfo>();

        await Task.WhenAll(_providerDictionary.Select(async pair =>
        {
            try
            {
                var w3 = pair.Value.Provider;
                
                var nativeBalanceWei = await w3.Eth.GetBalance.SendRequestAsync(address);
                var nativeBalanceFormatted = (double)Web3.Convert.FromWei(nativeBalanceWei);

                var usdtContract = w3.Eth.GetContract(pair.Value.UsdtInfo.ABI, pair.Value.UsdtInfo.ContractAddress);
                var usdtBalanceOf = usdtContract.GetFunction("balanceOf");
                var usdtBalanceWei = await usdtBalanceOf.CallAsync<int>(address);
                var usdtBalanceFormatted = (double)Web3.Convert.FromWei(usdtBalanceWei, 6);
                
                var usdcContract = w3.Eth.GetContract(pair.Value.UsdcInfo.ABI, pair.Value.UsdcInfo.ContractAddress);
                var usdcBalanceOf = usdcContract.GetFunction("balanceOf");
                var usdcBalanceWei = await usdcBalanceOf.CallAsync<int>(address);
                var usdcBalanceFormatted = (double)Web3.Convert.FromWei(usdcBalanceWei, 6);

                var nonce = await w3.Eth.Transactions.GetTransactionCount.SendRequestAsync(address);
                var txAmount = (int)nonce.Value;
                
                addressInfosDictionary.Add(pair.Key, new AddressInfo
                {
                    TxAmount = txAmount,
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

        return addressInfosDictionary;
    }

    public class AddressInfo
    {
        public int TxAmount { get; set; }
        public double NativeBalance { get; set; }
        public double UsdtBalance { get; set; }
        public double UsdcBalance { get; set; }
    }
}