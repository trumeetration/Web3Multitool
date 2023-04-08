using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LayerzeroMultitool.Models;

namespace LayerzeroMultitool.ViewModels;

public class ViewModel : INotifyPropertyChanged
{
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

    public ObservableCollection<AccountInfo> AccountInfos { get; set; }

    public ViewModel()
    {
        AccountInfos = new ObservableCollection<AccountInfo>()
        {
            new()
            {
                Address = "0x713b04EB5A9D585eC075CA65EdC3d6d7C12Be5Bb",
                ArbitrumBalance = 0.13,
                FantomBalance = 30,
                BinanceSmartChainBalance = 0.4,
                AvalancheBalance = 0.1,
                PolygonBalance = 5,
                PrivateKey = "0xprivate",
                TotalBalanceUsd = 300.15,
                UsdtBalance = 100.10
            },
            new()
            {
                Address = "0xbebra",
                ArbitrumBalance = 0,
                FantomBalance = 0,
                BinanceSmartChainBalance = 0,
                AvalancheBalance = 0,
                PolygonBalance = 0,
                PrivateKey = "0xprivate",
                TotalBalanceUsd = 0,
                UsdtBalance = 0
            },
            new()
            {
                Address = "0xbebra",
                ArbitrumBalance = 0,
                FantomBalance = 0,
                BinanceSmartChainBalance = 0,
                AvalancheBalance = 0,
                PolygonBalance = 0,
                PrivateKey = "0xprivate",
                TotalBalanceUsd = 0,
                UsdtBalance = 0
            }
        };
    }
}