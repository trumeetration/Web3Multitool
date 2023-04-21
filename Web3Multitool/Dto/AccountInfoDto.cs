using Web3Multitool.Models;

namespace Web3Multitool.Dto;

public class AccountInfoDto
{
    public bool IsSelected { get; set; }
    public string Address { get; set; }
    public string CexAddress { get; set; }
    public string PrivateKey { get; set; }
    public AddressChainInfo FantomInfo { get; set; }
    public AddressChainInfo AvaxInfo { get; set; }
    public AddressChainInfo PolygonInfo { get; set; }
    public AddressChainInfo ArbitrumInfo { get; set; }
    public AddressChainInfo OptimismInfo { get; set; }
    public double TotalBalanceUsd { get; set; }
    public int TotalTxAmount { get; set; }
}