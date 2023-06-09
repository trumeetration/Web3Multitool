using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Nethereum.Util;
using Newtonsoft.Json.Linq;
using Web3Multitool.Commands;
using Web3Multitool.Dialogs;
using Web3Multitool.Models;
using Web3Multitool.Stores;
using Web3Multitool.Utils;
using Chain = Web3Multitool.Utils.Chain;

namespace Web3Multitool.ViewModels;

public class MainViewModel : BaseViewModel
{
    private Timer _timer;
    
    public ViewTabViewModel ViewTabViewModel { get; }
    public Web3Utils Web3Utils;
    
    public MainViewModel(ViewTabViewModel viewTabViewModel)
    {
        ViewTabViewModel = viewTabViewModel;
        ViewTabViewModel.MainViewModel = this;

        LoadConfigInfo();
        ConnectToRpcList();

        _timer = new Timer(60000);
        _timer.Elapsed += async (_, _) => await GetCurrenciesRate();
        _timer.Start();

        GetCurrenciesRate().Wait();
    }

    private async Task GetCurrenciesRate()
    {
        try
        {
            using (var client = new HttpClient())
            {
                var response =
                    await client.GetAsync(
                        "https://api.coingecko.com/api/v3/simple/price?ids=ethereum,binancecoin,avalanche-2,fantom,matic-network&vs_currencies=usd&precision=2").ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(content);

                if (data != null)
                {
                    AvaxPrice = (double)data["avalanche-2"]["usd"];
                    BnbPrice = (double)data["binancecoin"]["usd"];
                    EthPrice = (double)data["ethereum"]["usd"];
                    FtmPrice = (double)data["fantom"]["usd"];
                    MaticPrice = (double)data["matic-network"]["usd"];
                }
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
        }
    }

    private string _binanceAPI;

    public string BinanceAPI
    {
        get => _binanceAPI;
        set => SetField(ref _binanceAPI, value);
    }

    private string _bybitAPI;

    public string BybitAPI
    {
        get => _bybitAPI;
        set => SetField(ref _bybitAPI, value);
    }

    private string _okxAPI;

    public string OKXAPI
    {
        get => _okxAPI;
        set => SetField(ref _okxAPI, value);
    }

    private string _arbitrumRPC;

    public string ArbitrumRPC
    {
        get => _arbitrumRPC;
        set => SetField(ref _arbitrumRPC, value);
    }

    private string _fantomRPC;

    public string FantomRPC
    {
        get => _fantomRPC;
        set => SetField(ref _fantomRPC, value);
    }

    private string _avaxRPC;

    public string AVAXRPC
    {
        get => _avaxRPC;
        set => SetField(ref _avaxRPC, value);
    }

    private string _polygonRPC;

    public string PolygonRPC
    {
        get => _polygonRPC;
        set => SetField(ref _polygonRPC, value);
    }

    private string _optimismRPC;

    public string OptimismRPC
    {
        get => _optimismRPC;
        set => SetField(ref _optimismRPC, value);
    }

    private double _ethPrice;

    public double EthPrice
    {
        get => _ethPrice;
        set => SetField(ref _ethPrice, value);
    }

    private double _ftmPrice;

    public double FtmPrice
    {
        get => _ftmPrice;
        set => SetField(ref _ftmPrice, value);
    }

    private double _maticPrice;

    public double MaticPrice
    {
        get => _maticPrice;
        set => SetField(ref _maticPrice, value);
    }
    
    private double _bnbPrice;

    public double BnbPrice
    {
        get => _bnbPrice;
        set => SetField(ref _bnbPrice, value);
    }
    
    private double _avaxPrice;

    public double AvaxPrice
    {
        get => _avaxPrice;
        set => SetField(ref _avaxPrice, value);
    }

    public ICommand SaveConfigData
    {
        get => new RelayCommand(() =>
        {
            var config = new AppConfig
            {
                BinanceAPIKey = BinanceAPI,
                BybitAPIKey = BybitAPI,
                OKXAPIKey = OKXAPI,
                ArbitrumRPC = ArbitrumRPC,
                FantomRPC = FantomRPC,
                AVAXRPC = AVAXRPC,
                PolygonRPC = PolygonRPC,
                OptimismRPC = OptimismRPC
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            string configString = JsonSerializer.Serialize(config, options);
            File.WriteAllText("config.json", configString);

            Debug.WriteLine("Connecting to RPCs again");
            ConnectToRpcList();
        }, () => true);
    }

    private (string, string) GenerateAccount()
    {
        var privateKey = EthECKey.GenerateKey().GetPrivateKeyAsBytes().ToHex();
        var address = EthECKey.GetPublicAddress(privateKey);

        return (privateKey, address);
    }

    private void LoadConfigInfo()
    {
        if (!File.Exists("config.json")) return;
        var configInfoString = File.ReadAllText("config.json");
        var configInfo = JsonSerializer.Deserialize<AppConfig>(configInfoString);
        BinanceAPI = configInfo.BinanceAPIKey;
        BybitAPI = configInfo.BybitAPIKey;
        OKXAPI = configInfo.OKXAPIKey;
        ArbitrumRPC = configInfo.ArbitrumRPC;
        FantomRPC = configInfo.FantomRPC;
        AVAXRPC = configInfo.AVAXRPC;
        PolygonRPC = configInfo.PolygonRPC;
        OptimismRPC = configInfo.OptimismRPC;
    }

    private void ConnectToRpcList()
    {
        var dict = new Dictionary<Chain, string>();
        if (Uri.IsWellFormedUriString(ArbitrumRPC, UriKind.RelativeOrAbsolute))
            dict.Add(Chain.Arbitrum, ArbitrumRPC);
        if (Uri.IsWellFormedUriString(FantomRPC, UriKind.RelativeOrAbsolute))
            dict.Add(Chain.Fantom, FantomRPC);
        if (Uri.IsWellFormedUriString(PolygonRPC, UriKind.RelativeOrAbsolute))
            dict.Add(Chain.Polygon, PolygonRPC);
        if (Uri.IsWellFormedUriString(OptimismRPC, UriKind.RelativeOrAbsolute))
            dict.Add(Chain.Optimism, OptimismRPC);
        if (Uri.IsWellFormedUriString(AVAXRPC, UriKind.RelativeOrAbsolute))
            dict.Add(Chain.Avalanche, AVAXRPC);

        if (dict.Any())
            Web3Utils = new Web3Utils(dict);
    }
}