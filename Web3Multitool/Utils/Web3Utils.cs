using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Numerics;
using Nethereum.Util;
using Nethereum.Web3;

namespace Web3Multitool.Utils;

public class Web3Utils
{
    private static readonly string BaseAbi =
        @"[{""inputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""name"":""balanceOf"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""},{""internalType"":""address"",""name"":"""",""type"":""address""}],""name"":""allowance"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""value"",""type"":""uint256""}],""name"":""transfer"",""outputs"":[{""internalType"":""bool"",""name"":"""",""type"":""bool""}],""stateMutability"":""nonpayable"",""type"":""function""}]"; 
    
    public static Dictionary<Chain, Dictionary<string, CoinInfo>> CoinInfosDictionary { get; } = new()
    {
        {
            Chain.Fantom, new()
            {
                { "USDT", new() { ContractAddress = "0x049d68029688eabf473097a2fc38ef61633a3c7a", ABI = BaseAbi } },
                { "USDC", new() { ContractAddress = "0x04068DA6C83AFCFA0e13ba15A6696662335D5B75", ABI = BaseAbi } }
            }
        },
        {
            Chain.Arbitrum, new()
            {
                { "USDT", new() { ContractAddress = "0xFd086bC7CD5C481DCC9C85ebE478A1C0b69FCbb9", ABI = BaseAbi } },
                { "USDC", new() { ContractAddress = "0xFF970A61A04b1cA14834A43f5dE4533eBDDB5CC8", ABI = BaseAbi } }
            }
        },
        {
            Chain.Avalanche, new()
            {
                { "USDT", new() { ContractAddress = "0x9702230A8Ea53601f5cD2dc00fDBc13d4dF4A8c7", ABI = BaseAbi } },
                { "USDC", new() { ContractAddress = "0xB97EF9Ef8734C71904D8002F8b6Bc66Dd9c48a6E", ABI = BaseAbi } }
            }
        },
        {
            Chain.Optimism, new()
            {
                { "USDT", new() { ContractAddress = "0x94b008aa00579c1307b0ef2c499ad98a8ce58e58", ABI = BaseAbi } },
                { "USDC", new() { ContractAddress = "0x7f5c764cbc14f9669b88837ca1490cca17c31607", ABI = BaseAbi } }
            }
        },
        {
            Chain.Polygon, new()
            {
                { "USDT", new() { ContractAddress = "0xc2132d05d31c914a87c6611c10748aeb04b58e8f", ABI = BaseAbi } },
                { "USDC", new() { ContractAddress = "0x2791bca1f2de4661ed88a30c99a7a9449aa84174", ABI = BaseAbi } }
            }
        },
        {
            Chain.Binance, new()
            {
                { "USDT", new() { ContractAddress = "0x55d398326f99059ff775485246999027b3197955", ABI = BaseAbi } },
                { "USDC", new() { ContractAddress = "0x8ac76a51cc950d9822d68b83fe1ad97b32cd580d", ABI = BaseAbi } }
            }
        },
        {
            Chain.Harmony, new()
            {
                { "USDT", new() { ContractAddress = "0x3c2b8be99c50593081eaa2a724f0b8285f5aba8f", ABI = BaseAbi } },
                { "USDC", new() { ContractAddress = "0x985458e523db3d53125813ed68c274899e9dfab4", ABI = BaseAbi } }
            }
        },
        {
            Chain.Coredao, new()
            {
                { "USDT", new() { ContractAddress = "0x900101d06A7426441Ae63e9AB3B9b0F63Be145F1", ABI = BaseAbi } },
                { "USDC", new() { ContractAddress = "0xeab3ac417c4d6df6b143346a46fee1b847b50296", ABI = BaseAbi } }
            }
        }
    };
    
    public Dictionary<Chain, ChainInfo> ChainInfosDictionary { get; set; } = new();
    
    public Web3Utils(Dictionary<Chain, string> rpcDictionary)
    {
        foreach (var (chainId, rpcUrl) in rpcDictionary)
        {
            try
            {
                var w3 = new Web3(rpcUrl);
                var networkId = w3.Net.Version.SendRequestAsync().Result;
                Debug.WriteLine(networkId);
                ChainInfosDictionary.Add(chainId, new ChainInfo
                {
                    Endpoint = rpcUrl,
                    Provider = w3,
                    UsdtInfo = CoinInfosDictionary[chainId]["USDT"],
                    UsdcInfo = CoinInfosDictionary[chainId]["USDC"]
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Problem with rpc {rpcUrl}: {e}");
            }
        }
    }

    public class ChainInfo
    {
        public string Endpoint { get; set; }
        public Web3 Provider { get; set; }
        public CoinInfo UsdtInfo { get; set; }
        public CoinInfo UsdcInfo { get; set; }
    }

    public class CoinInfo
    {
        public string? ContractAddress { get; set; }
        public string? ABI { get; set; }
    }
}

public static class Web3Additions
{
    public static decimal FromWei(this UnitConversion _, BigInteger value, Chain chain)
    {
        return _.FromWei(value, chain is Chain.Binance ? 18: 6);
    }
    
    public static BigInteger ToWei(this UnitConversion _, double value, Chain chain)
    {
        return _.ToWei(value, chain is Chain.Binance ? 18: 6);
    }
}