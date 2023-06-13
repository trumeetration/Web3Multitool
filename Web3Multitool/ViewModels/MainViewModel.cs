using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Newtonsoft.Json.Linq;
using Web3Multitool.Commands;
using Web3Multitool.Models;
using Web3Multitool.Utils;
using Chain = Web3Multitool.Utils.Chain;

namespace Web3Multitool.ViewModels;

public class MainViewModel : BaseViewModel
{
    private Timer _timer;
    
    public ViewTabViewModel ViewTabViewModel { get; }
    public Web3Utils Web3Utils;

    private string _log;
    
    private string _binanceAPI;
    private OkxApiInfo _okxApiInfo;
    private string _bybitAPI;
    
    private string _arbitrumRPC;
    private string _fantomRPC;
    private string _avaxRPC;
    private string _polygonRPC;
    private string _optimismRPC;
    private string _bscRPC;
    private string _harmonyRPC;
    private string _coredaomRPC;
    
    private double _ethPrice;
    private double _ftmPrice;
    private double _maticPrice;
    private double _bnbPrice;
    private double _avaxPrice;
    private double _harmonyPrice;
    private double _coredaoPrice;

    public MainViewModel(ViewTabViewModel viewTabViewModel)
    {
        ViewTabViewModel = viewTabViewModel;
        ViewTabViewModel.MainViewModel = this;

        LoadConfigInfo();

        if (OKXApiInfo == null)
            OKXApiInfo = new OkxApiInfo();

        ConnectToRpcList();

        _timer = new Timer(60000);
        _timer.Elapsed += async (_, _) => await GetCurrenciesRate();
        _timer.Start();

        GetCurrenciesRate().Wait();
    }

    public string Log
    {
        get => _log;
        set => SetField(ref _log, value);
    }

    public string BinanceAPI
    {
        get => _binanceAPI;
        set => SetField(ref _binanceAPI, value);
    }
    
    public string BybitAPI
    {
        get => _bybitAPI;
        set => SetField(ref _bybitAPI, value);
    }

    public OkxApiInfo OKXApiInfo
    {
        get => _okxApiInfo;
        set => SetField(ref _okxApiInfo, value);
    }

    public string ArbitrumRPC
    {
        get => _arbitrumRPC;
        set => SetField(ref _arbitrumRPC, value);
    }

    public string FantomRPC
    {
        get => _fantomRPC;
        set => SetField(ref _fantomRPC, value);
    }

    public string AVAXRPC
    {
        get => _avaxRPC;
        set => SetField(ref _avaxRPC, value);
    }
    
    public string PolygonRPC
    {
        get => _polygonRPC;
        set => SetField(ref _polygonRPC, value);
    }

    public string OptimismRPC
    {
        get => _optimismRPC;
        set => SetField(ref _optimismRPC, value);
    }

    public string BscRPC
    {
        get => _bscRPC;
        set => SetField(ref _bscRPC, value);
    }

    public string HarmonyRPC
    {
        get => _harmonyRPC;
        set => SetField(ref _harmonyRPC, value);
    }

    public string CoredaoRPC
    {
        get => _coredaomRPC;
        set => SetField(ref _coredaomRPC, value);
    }
    
    public double EthPrice
    {
        get => _ethPrice;
        set => SetField(ref _ethPrice, value);
    }

    public double FtmPrice
    {
        get => _ftmPrice;
        set => SetField(ref _ftmPrice, value);
    }
    
    public double MaticPrice
    {
        get => _maticPrice;
        set => SetField(ref _maticPrice, value);
    }

    public double BnbPrice
    {
        get => _bnbPrice;
        set => SetField(ref _bnbPrice, value);
    }
    
    public double AvaxPrice
    {
        get => _avaxPrice;
        set => SetField(ref _avaxPrice, value);
    }

    public double HarmonyPrice
    {
        get => _harmonyPrice;
        set => SetField(ref _harmonyPrice, value);
    }
    
    public double CoredaoPrice
    {
        get => _coredaoPrice;
        set => SetField(ref _coredaoPrice, value);
    }

    public void AppendToLog(string message)
    {
        var dateTime = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        Log += $"{dateTime} | {message}{Environment.NewLine}";
        
        
    }
    
    private async Task GetCurrenciesRate()
    {
        AppendToLog("Updating currencies rates...");
        
        try
        {
            using (var client = new HttpClient())
            {
                var response =
                    await client.GetAsync(
                        "https://api.coingecko.com/api/v3/simple/price?ids=ethereum,binancecoin,avalanche-2,fantom,matic-network,harmony,coredaoorg&vs_currencies=usd&precision=2").ConfigureAwait(false);
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
                    HarmonyPrice = (double)data["harmony"]["usd"];
                    CoredaoPrice = (double)data["coredaoorg"]["usd"];
                }
                
                AppendToLog("Currencies rates were updated");
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
            AppendToLog("Error with currencies rates updating!");
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
                OKXApiInfo = OKXApiInfo,
                ArbitrumRPC = ArbitrumRPC,
                FantomRPC = FantomRPC,
                AVAXRPC = AVAXRPC,
                BscRPC = BscRPC,
                HarmonyRPC = HarmonyRPC,
                CoredaoRPC = CoredaoRPC,
                PolygonRPC = PolygonRPC,
                OptimismRPC = OptimismRPC,
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            string configString = JsonSerializer.Serialize(config, options);
            File.WriteAllText("config.json", configString);

            Debug.WriteLine("Connecting to RPCs again");
            ConnectToRpcList();
        }, () => true);
    }

    private void LoadConfigInfo()
    {
        if (!File.Exists("config.json")) return;
        var configInfoString = File.ReadAllText("config.json");
        var configInfo = JsonSerializer.Deserialize<AppConfig>(configInfoString);
        
        BinanceAPI = configInfo.BinanceAPIKey;
        BybitAPI = configInfo.BybitAPIKey;
        OKXApiInfo = configInfo.OKXApiInfo;
        
        ArbitrumRPC = configInfo.ArbitrumRPC;
        FantomRPC = configInfo.FantomRPC;
        AVAXRPC = configInfo.AVAXRPC;
        PolygonRPC = configInfo.PolygonRPC;
        OptimismRPC = configInfo.OptimismRPC;
        BscRPC = configInfo.BscRPC;
        HarmonyRPC = configInfo.HarmonyRPC;
        CoredaoRPC = configInfo.CoredaoRPC;
        
        AppendToLog("Config info loaded");
    }

    private void ConnectToRpcList()
    {
        AppendToLog("Connecting to RPCs");
        
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
        if (Uri.IsWellFormedUriString(BscRPC, UriKind.RelativeOrAbsolute))
            dict.Add(Chain.Binance, BscRPC);
        if (Uri.IsWellFormedUriString(HarmonyRPC, UriKind.RelativeOrAbsolute))
            dict.Add(Chain.Harmony, HarmonyRPC);
        if (Uri.IsWellFormedUriString(CoredaoRPC, UriKind.RelativeOrAbsolute))
            dict.Add(Chain.Coredao, CoredaoRPC);

        if (dict.Any())
            Web3Utils = new Web3Utils(dict);
        
        AppendToLog($"Working RPCs amount: {dict.Count}");
    }

    public class OkxApiInfo : INotifyPropertyChanged
    {
        private string _apiKey;
        private string _secretKey;
        private string _passPhrase;
        
        public string ApiKey
        {
            get => _apiKey;
            set => SetField(ref _apiKey, value);
        }

        public string SecretKey
        {
            get => _secretKey;
            set => SetField(ref _secretKey, value);
        }

        public string PassPhrase
        {
            get => _passPhrase;
            set => SetField(ref _passPhrase, value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}