using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
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
                BnbInfo = accountInfo.BnbInfo,
                HarmonyInfo = accountInfo.HarmonyInfo,
                CoredaoInfo = accountInfo.CoredaoInfo,
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
            
            // Bsc
            if (addressInfo.TryGetValue(Chain.Binance, out info))
            {
                updatedAccountInfo.BnbInfo.UsdtBalance = info.UsdtBalance;
                updatedAccountInfo.BnbInfo.UsdcBalance = info.UsdcBalance;
                updatedAccountInfo.BnbInfo.NativeBalance = info.NativeBalance;
                updatedAccountInfo.BnbInfo.TxAmount = info.TxAmount;
                
                Debug.WriteLine($"Bsc: {info.NativeBalance} ETH, {info.UsdtBalance} USDT, {info.UsdcBalance} USDC");
            }
            
            // Harmony
            if (addressInfo.TryGetValue(Chain.Harmony, out info))
            {
                updatedAccountInfo.HarmonyInfo.UsdtBalance = info.UsdtBalance;
                updatedAccountInfo.HarmonyInfo.UsdcBalance = info.UsdcBalance;
                updatedAccountInfo.HarmonyInfo.NativeBalance = info.NativeBalance;
                updatedAccountInfo.HarmonyInfo.TxAmount = info.TxAmount;
                
                Debug.WriteLine($"Harmony: {info.NativeBalance} ETH, {info.UsdtBalance} USDT, {info.UsdcBalance} USDC");
            }
            
            // Coredao
            if (addressInfo.TryGetValue(Chain.Coredao, out info))
            {
                updatedAccountInfo.CoredaoInfo.UsdtBalance = info.UsdtBalance;
                updatedAccountInfo.CoredaoInfo.UsdcBalance = info.UsdcBalance;
                updatedAccountInfo.CoredaoInfo.NativeBalance = info.NativeBalance;
                updatedAccountInfo.CoredaoInfo.TxAmount = info.TxAmount;
                
                Debug.WriteLine($"Coredao: {info.NativeBalance} ETH, {info.UsdtBalance} USDT, {info.UsdcBalance} USDC");
            }
            
            // Total
            var totalTx = updatedAccountInfo.ArbitrumInfo.TxAmount +
                          updatedAccountInfo.FantomInfo.TxAmount +
                          updatedAccountInfo.OptimismInfo.TxAmount +
                          updatedAccountInfo.PolygonInfo.TxAmount +
                          updatedAccountInfo.BnbInfo.TxAmount +
                          updatedAccountInfo.HarmonyInfo.TxAmount +
                          updatedAccountInfo.CoredaoInfo.TxAmount +
                          updatedAccountInfo.AvaxInfo.TxAmount;

            var totalUsd = GetAllChainsTotalBalanceUsd(updatedAccountInfo);

            updatedAccountInfo.TotalTxAmount = totalTx;
            updatedAccountInfo.TotalBalanceUsd = totalUsd;
            
            await _accountInfosStore.Update(updatedAccountInfo);
        }
        
        _viewTabViewModel.IsLoading = false;
    }

    private double GetChainTotalBalanceUsd(AddressChainInfo addressChainInfo, double chainCoinPrice)
    {
        return addressChainInfo.NativeBalance * chainCoinPrice +
               addressChainInfo.UsdtBalance +
               addressChainInfo.UsdcBalance;
    }

    private double GetAllChainsTotalBalanceUsd(AccountInfo updatedAccountInfo)
    {
        var ftmPrice = _viewTabViewModel.MainViewModel.FtmPrice;
        var avaxPrice = _viewTabViewModel.MainViewModel.AvaxPrice;
        var maticPrice = _viewTabViewModel.MainViewModel.MaticPrice;
        var ethPrice = _viewTabViewModel.MainViewModel.EthPrice;
        var bnbPrice = _viewTabViewModel.MainViewModel.BnbPrice;
        var harmonyPrice = _viewTabViewModel.MainViewModel.HarmonyPrice;
        var coredaoPrice = _viewTabViewModel.MainViewModel.CoredaoPrice;

        return GetChainTotalBalanceUsd(updatedAccountInfo.ArbitrumInfo, ethPrice) +
               GetChainTotalBalanceUsd(updatedAccountInfo.FantomInfo, ftmPrice) +
               GetChainTotalBalanceUsd(updatedAccountInfo.OptimismInfo, ethPrice) +
               GetChainTotalBalanceUsd(updatedAccountInfo.PolygonInfo, maticPrice) +
               GetChainTotalBalanceUsd(updatedAccountInfo.BnbInfo, bnbPrice) +
               GetChainTotalBalanceUsd(updatedAccountInfo.HarmonyInfo, harmonyPrice) +
               GetChainTotalBalanceUsd(updatedAccountInfo.CoredaoInfo, coredaoPrice) +
               GetChainTotalBalanceUsd(updatedAccountInfo.AvaxInfo, avaxPrice);
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
                var usdtBalanceWei = await usdtBalanceOf.CallAsync<BigInteger>(address);
                var usdtBalanceFormatted = (double)Web3.Convert.FromWei(usdtBalanceWei, pair.Key == Chain.Binance ? 18 : 6);
                
                var usdcContract = w3.Eth.GetContract(pair.Value.UsdcInfo.ABI, pair.Value.UsdcInfo.ContractAddress);
                var usdcBalanceOf = usdcContract.GetFunction("balanceOf");
                var usdcBalanceWei = await usdcBalanceOf.CallAsync<BigInteger>(address);
                var usdcBalanceFormatted = (double)Web3.Convert.FromWei(usdcBalanceWei, pair.Key == Chain.Binance ? 18 : 6);

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