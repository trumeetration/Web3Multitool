namespace Web3MultiTool.Domain.Models;

public class AccountInfo
{
    public int Id { get; set; }
    public string Address { get; set; }
    public string? CexAddress { get; set; }
    public string PrivateKey { get; set; }
    public AddressChainInfo FantomInfo { get; set; }
    public AddressChainInfo AvaxInfo { get; set; }
    public AddressChainInfo PolygonInfo { get; set; }
    public AddressChainInfo ArbitrumInfo { get; set; }
    public AddressChainInfo OptimismInfo { get; set; }
    public double TotalBalanceUsd { get; set; }
    public int TotalTxAmount { get; set; }
}