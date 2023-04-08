using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LayerzeroMultitool.Commands;
using LayerzeroMultitool.Models;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Nethereum.Util;

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
        AccountInfos = new ObservableCollection<AccountInfo>();
    }

    private string _generateInputAmount;

    public string GenerateInputAmount
    {
        get => _generateInputAmount;
        set
        {
            _generateInputAmount = value;
            OnPropertyChanged(nameof(GenerateInputAmount));
        }
    }

    public ICommand GenerateAccountsCommand
    {
        get => new RelayCommand(() =>
        {
            if (Int32.TryParse(GenerateInputAmount, out int inputAmount))
            {
                AccountInfos.Clear();
                
                for (int i = 0; i < inputAmount; i++)
                {
                    var (privateKey, address) = GenerateAccount();
                    
                    AccountInfos.Add(new()
                    {
                        PrivateKey = privateKey,
                        Address = address
                    });
                }
                
                GenerateInputAmount = String.Empty;
            }
        }, () => GenerateInputAmount?.Length > 0);
    }

    public ICommand ClearAccountsCommand
    {
        get => new RelayCommand(() =>
        {
            AccountInfos.Clear();
        }, () => AccountInfos.Count > 0);
    }

    public (string, string) GenerateAccount()
    {
        var privateKey = EthECKey.GenerateKey().GetPrivateKeyAsBytes().ToHex();
        var address = EthECKey.GetPublicAddress(privateKey);
        
        return (privateKey, address);
    } 
}