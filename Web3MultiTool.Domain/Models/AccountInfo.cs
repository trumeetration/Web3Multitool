using System.ComponentModel;
using System.Runtime.CompilerServices;
using Web3MultiTool.Domain.Utils;

namespace Web3MultiTool.Domain.Models;

public class AccountInfo : INotifyPropertyChanged
{
    private int _totalTxAmount;
    private Guid _id;
    private string _address;
    private string? _cexAddress;
    private string _privateKey;
    private AddressChainInfo _fantomInfo;
    private AddressChainInfo _avaxInfo;
    private AddressChainInfo _polygonInfo;
    private AddressChainInfo _arbitrumInfo;
    private AddressChainInfo _optimismInfo;
    private AddressChainInfo _bnbInfo;
    private AddressChainInfo _harmonyInfo;
    private AddressChainInfo _coredaoInfo;
    private double _totalBalanceUsd;
    private bool _isSelected;

    public AccountInfo()
    {
        FantomInfo = new AddressChainInfo { ChainId = (int)Chain.Fantom, Id = Guid.NewGuid() };
        AvaxInfo = new AddressChainInfo { ChainId = (int)Chain.Avalanche, Id = Guid.NewGuid() };
        PolygonInfo = new AddressChainInfo { ChainId = (int)Chain.Polygon, Id = Guid.NewGuid() };
        ArbitrumInfo = new AddressChainInfo { ChainId = (int)Chain.Arbitrum, Id = Guid.NewGuid() };
        OptimismInfo = new AddressChainInfo { ChainId = (int)Chain.Optimism, Id = Guid.NewGuid() };
        BnbInfo = new AddressChainInfo { ChainId = (int)Chain.Binance, Id = Guid.NewGuid() };
        HarmonyInfo = new AddressChainInfo { ChainId = (int)Chain.Harmony, Id = Guid.NewGuid() };
        CoredaoInfo = new AddressChainInfo { ChainId = (int)Chain.Coredao, Id = Guid.NewGuid() };
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetField(ref _isSelected, value);
    }

    public Guid Id
    {
        get => _id;
        set => SetField(ref _id, value);
    }

    public string Address
    {
        get => _address;
        set => SetField(ref _address, value);
    }

    public string? CexAddress
    {
        get => _cexAddress;
        set => SetField(ref _cexAddress, value);
    }

    public string PrivateKey
    {
        get => _privateKey;
        set => SetField(ref _privateKey, value);
    }

    public AddressChainInfo FantomInfo
    {
        get => _fantomInfo;
        set => SetField(ref _fantomInfo, value);
    }

    public AddressChainInfo AvaxInfo
    {
        get => _avaxInfo;
        set => SetField(ref _avaxInfo, value);
    }

    public AddressChainInfo PolygonInfo
    {
        get => _polygonInfo;
        set => SetField(ref _polygonInfo, value);
    }

    public AddressChainInfo ArbitrumInfo
    {
        get => _arbitrumInfo;
        set => SetField(ref _arbitrumInfo, value);
    }

    public AddressChainInfo OptimismInfo
    {
        get => _optimismInfo;
        set => SetField(ref _optimismInfo, value);
    }
    
    public AddressChainInfo BnbInfo
    {
        get => _bnbInfo;
        set => SetField(ref _bnbInfo, value);
    }
    
    public AddressChainInfo HarmonyInfo
    {
        get => _harmonyInfo;
        set => SetField(ref _harmonyInfo, value);
    }
    
    public AddressChainInfo CoredaoInfo
    {
        get => _coredaoInfo;
        set => SetField(ref _coredaoInfo, value);
    }

    public double TotalBalanceUsd
    {
        get => _totalBalanceUsd;
        set => SetField(ref _totalBalanceUsd, value);
    }

    public int TotalTxAmount
    {
        get => _totalTxAmount;
        set => SetField(ref _totalTxAmount, value);
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