using System.ComponentModel;
using System.Runtime.CompilerServices;

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
    private double _totalBalanceUsd;

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