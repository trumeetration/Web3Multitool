namespace LayerzeroMultitool.Models;

public class AccountInfo
{
    public string Address { get; set; }
    public string PrivateKey { get; set; }
    public double BinanceSmartChainBalance { get; set; }
    public double AvalancheBalance { get; set; }
    public double PolygonBalance { get; set; }
    public double ArbitrumBalance { get; set; }
    public double FantomBalance { get; set; }
    public double UsdtBalance { get; set; }
    public double TotalBalanceUsd { get; set; }
}