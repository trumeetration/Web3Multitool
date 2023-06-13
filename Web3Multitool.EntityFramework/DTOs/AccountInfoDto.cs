using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Web3Multitool.EntityFramework.DTOs;

[Table("AccountInfo")]
public class AccountInfoDto
{
    public Guid Id { get; set; }
    public string Address { get; set; }
    [MaybeNull]
    public string? CexAddress { get; set; }
    public string PrivateKey { get; set; }
    public AddressChainInfoDto FantomInfo { get; set; }
    public AddressChainInfoDto AvaxInfo { get; set; }
    public AddressChainInfoDto PolygonInfo { get; set; }
    public AddressChainInfoDto ArbitrumInfo { get; set; }
    public AddressChainInfoDto OptimismInfo { get; set; }
    public AddressChainInfoDto BnbInfo { get; set; }
    public AddressChainInfoDto HarmonyInfo { get; set; }
    public AddressChainInfoDto CoredaoInfo { get; set; }
    public double TotalBalanceUsd { get; set; }
    public int TotalTxAmount { get; set; }
}