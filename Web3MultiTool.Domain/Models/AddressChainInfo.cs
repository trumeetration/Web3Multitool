﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Web3MultiTool.Domain.Models;

public class AddressChainInfo : INotifyPropertyChanged
{
    private Guid _id;
    private int _chainId;
    private int _txAmount;
    private double _baseBalance;
    private DateTime _firstTxDate;
    private double _usdcBalance;

    public Guid Id
    {
        get => _id;
        set => SetField(ref _id, value);
    }

    public int ChainId
    {
        get => _chainId;
        set => SetField(ref _chainId, value);
    }

    public int TxAmount
    {
        get => _txAmount;
        set => SetField(ref _txAmount, value);
    }

    public double BaseBalance
    {
        get => _baseBalance;
        set => SetField(ref _baseBalance, value);
    }

    public DateTime FirstTxDate
    {
        get => _firstTxDate;
        set => SetField(ref _firstTxDate, value);
    }

    public double UsdcBalance
    {
        get => _usdcBalance;
        set => SetField(ref _usdcBalance, value);
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