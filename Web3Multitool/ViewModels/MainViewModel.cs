using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Nethereum.Util;
using Web3Multitool.Commands;
using Web3Multitool.Dialogs;
using Web3Multitool.Models;
using Web3Multitool.Stores;
using Web3Multitool.Utils;

namespace Web3Multitool.ViewModels;

public class MainViewModel : BaseViewModel
{
    public ViewTabViewModel ViewTabViewModel { get; }
    private Web3Utils _web3Utils;
    
    public MainViewModel(ViewTabViewModel viewTabViewModel)
    {
        ViewTabViewModel = viewTabViewModel;

        LoadConfigInfo();
        ConnectToRpcList();
    }

    private string _binanceAPI;

    public string BinanceAPI
    {
        get => _binanceAPI;
        set
        {
            _binanceAPI = value?.Trim();
            OnPropertyChanged(nameof(BinanceAPI));
        }
    }

    private string _bybitAPI;

    public string BybitAPI
    {
        get => _bybitAPI;
        set
        {
            _bybitAPI = value?.Trim();
            OnPropertyChanged(nameof(BybitAPI));
        }
    }

    private string _okxAPI;

    public string OKXAPI
    {
        get => _okxAPI;
        set
        {
            _okxAPI = value?.Trim();
            OnPropertyChanged(nameof(OKXAPI));
        }
    }

    private string _arbitrumRPC;

    public string ArbitrumRPC
    {
        get => _arbitrumRPC;
        set
        {
            _arbitrumRPC = value?.Trim();
            OnPropertyChanged(nameof(ArbitrumRPC));
        }
    }

    private string _fantomRPC;

    public string FantomRPC
    {
        get => _fantomRPC;
        set
        {
            _fantomRPC = value?.Trim();
            OnPropertyChanged(nameof(FantomRPC));
        }
    }

    private string _avaxRPC;

    public string AVAXRPC
    {
        get => _avaxRPC;
        set
        {
            _avaxRPC = value.Trim();
            OnPropertyChanged(nameof(AVAXRPC));
        }
    }

    private string _polygonRPC;

    public string PolygonRPC
    {
        get => _polygonRPC;
        set
        {
            _polygonRPC = value?.Trim();
            OnPropertyChanged(nameof(PolygonRPC));
        }
    }

    private string _optimismRPC;

    public string OptimismRPC
    {
        get => _optimismRPC;
        set
        {
            _optimismRPC = value?.Trim();
            OnPropertyChanged(nameof(OptimismRPC));
        }
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
        var dict = new Dictionary<int, string>();
        if (Uri.IsWellFormedUriString(ArbitrumRPC, UriKind.RelativeOrAbsolute))
            dict.Add(42161, ArbitrumRPC);
        if (Uri.IsWellFormedUriString(FantomRPC, UriKind.RelativeOrAbsolute))
            dict.Add(250, FantomRPC);
        if (Uri.IsWellFormedUriString(PolygonRPC, UriKind.RelativeOrAbsolute))
            dict.Add(137, PolygonRPC);
        if (Uri.IsWellFormedUriString(OptimismRPC, UriKind.RelativeOrAbsolute))
            dict.Add(10, OptimismRPC);
        if (Uri.IsWellFormedUriString(AVAXRPC, UriKind.RelativeOrAbsolute))
            dict.Add(43114, AVAXRPC);

        if (dict.Any())
            _web3Utils = new Web3Utils(dict);
    }
}