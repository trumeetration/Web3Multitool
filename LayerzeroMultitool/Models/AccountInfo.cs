using System;

namespace LayerzeroMultitool.Models;

public class AccountInfo
{
    public string Address { get; set; }
    public string CexAddress { get; set; }
    public string PrivateKey { get; set; }

    public BscAddressInfo BscInfo { get; set; }
    public AvalancheAddressInfo AvaxInfo { get; set; }
    public PolygonAddressInfo PolygonInfo { get; set; }
    public ArbitrumAddressInfo ArbitrumInfo { get; set; }
    public FantomAddressInfo FantomInfo { get; set; }
    public double TotalBalanceUsd { get; set; }
    public int TotalTxAmount { get; set; }
    
}

public class AddressChainInfo
{
    public int TxAmount { get; set; }
    public double BaseBalance { get; set; }
    public DateTime FirstTxDate { get; set; }
    public double UsdtBalance { get; set; }
}

public class BscAddressInfo : AddressChainInfo
{
    
}

public class AvalancheAddressInfo : AddressChainInfo
{
    
}

public class PolygonAddressInfo : AddressChainInfo
{
    
}

public class ArbitrumAddressInfo : AddressChainInfo
{
    
}

public class FantomAddressInfo : AddressChainInfo
{
    
}