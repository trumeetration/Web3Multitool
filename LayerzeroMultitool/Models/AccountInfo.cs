namespace LayerzeroMultitool.Models;

public class AccountInfo
{
    public string? Address { get; set; }
    public string? CexAddress { get; set; } = "Not set";
    public string? PrivateKey { get; set; }
    public double BinanceSmartChainBalance { get; set; } = 0;
    public double AvalancheBalance { get; set; } = 0;
    public double PolygonBalance { get; set; } = 0;
    public double ArbitrumBalance { get; set; } = 0;
    public double FantomBalance { get; set; } = 0;
    public double UsdtBalance { get; set; } = 0;
    public double TotalBalanceUsd { get; set; } = 0;
}