using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using LayerzeroMultitool.Commands;
using LayerzeroMultitool.Models;
using Microsoft.Win32;
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
                        Address = address,
                        BscInfo = new BscAddressInfo(),
                        AvaxInfo = new AvalancheAddressInfo(),
                        PolygonInfo = new PolygonAddressInfo(),
                        ArbitrumInfo = new ArbitrumAddressInfo(),
                        FantomInfo = new FantomAddressInfo()
                    });
                }

                GenerateInputAmount = String.Empty;
            }
        }, () => GenerateInputAmount?.Length > 0);
    }

    public ICommand ClearAccountsCommand
    {
        get => new RelayCommand(() => { AccountInfos.Clear(); }, () => AccountInfos.Any());
    }

    public ICommand ImportAccountsFromFileCommand
    {
        get => new RelayCommand(() =>
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 0
            };
            if (dialog.ShowDialog() != true)
                return;
            var content = File.ReadAllLines(dialog.FileName);

            foreach (var line in content)
            {
                var privateKey = line.Contains(':')
                    ? line.Split(':')[0]
                    : line.Trim().Replace(Environment.NewLine, String.Empty);
                
                if (privateKey.Length == 0) continue;
                var address = EthECKey.GetPublicAddress(privateKey);
                AccountInfos.Add(new()
                {
                    Address = address,
                    PrivateKey = privateKey
                });
            }
        }, () => true);
    }

    public ICommand ExportAccountsToFileCommand
    {
        get => new RelayCommand(() =>
        {
            string content = String.Empty;

            foreach (var accountInfo in AccountInfos)
            {
                content += $"{accountInfo.PrivateKey}:{accountInfo.Address}\n";
            }

            var destinationDirectory = Directory.GetCurrentDirectory() + "\\Export";

            if (!Directory.Exists(destinationDirectory))
                Directory.CreateDirectory(destinationDirectory);

            var filePath = $"{destinationDirectory}\\export_{Guid.NewGuid().ToString()}.txt";
            File.WriteAllText(filePath, content);

            Process.Start("explorer.exe", destinationDirectory);
        }, () => AccountInfos.Any());
    }

    public (string, string) GenerateAccount()
    {
        var privateKey = EthECKey.GenerateKey().GetPrivateKeyAsBytes().ToHex();
        var address = EthECKey.GetPublicAddress(privateKey);

        return (privateKey, address);
    }
}