using Web3Multitool.ViewModels;

namespace Web3Multitool.Models;

public class AppConfig
{
    public string BinanceAPIKey { get; set; }
    public string BybitAPIKey { get; set; }
    public MainViewModel.OkxApiInfo OKXApiInfo { get; set; }
    public string ArbitrumRPC { get; set; }
    public string PolygonRPC { get; set; }
    public string OptimismRPC { get; set; }
    public string FantomRPC { get; set; }
    public string AVAXRPC { get; set; }
    public string BscRPC { get; set; }
    public string HarmonyRPC { get; set; }
    public string CoredaoRPC { get; set; }
}