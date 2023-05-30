using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Nethereum.Util;
using Web3Multitool.Commands;
using Web3Multitool.Dialogs;
using Web3Multitool.Dto;
using Web3Multitool.Models;
using Web3Multitool.Utils;

namespace Web3Multitool.ViewModels;

public class ViewModel : BaseViewModel
{
    private ApplicationContext db;
    private BackgroundWorker _backgroundWorker;
    public ObservableCollection<AccountInfoDto> AccountInfos { get; set; }
    private Web3Utils _web3Utils;

    public ViewModel()
    {
        db = new ApplicationContext();
        AccountInfos = new ObservableCollection<AccountInfoDto>();
        LoadInfoFromDb();
        LoadConfigInfo();
        ConnectToRpcList();
    }

    private string _generateInputAmount;

    public string GenerateInputAmount
    {
        get => _generateInputAmount;
        set
        {
            _generateInputAmount = value.Trim();
            OnPropertyChanged(nameof(GenerateInputAmount));
        }
    }

    private string _binanceAPI;

    public string BinanceAPI
    {
        get => _binanceAPI;
        set
        {
            _binanceAPI = value?.Trim();
            OnPropertyChanged(nameof(BinanceAPI));
        }
    }

    private string _bybitAPI;

    public string BybitAPI
    {
        get => _bybitAPI;
        set
        {
            _bybitAPI = value?.Trim();
            OnPropertyChanged(nameof(BybitAPI));
        }
    }

    private string _okxAPI;

    public string OKXAPI
    {
        get => _okxAPI;
        set
        {
            _okxAPI = value?.Trim();
            OnPropertyChanged(nameof(OKXAPI));
        }
    }

    private string _arbitrumRPC;

    public string ArbitrumRPC
    {
        get => _arbitrumRPC;
        set
        {
            _arbitrumRPC = value?.Trim();
            OnPropertyChanged(nameof(ArbitrumRPC));
        }
    }

    private string _fantomRPC;

    public string FantomRPC
    {
        get => _fantomRPC;
        set
        {
            _fantomRPC = value?.Trim();
            OnPropertyChanged(nameof(FantomRPC));
        }
    }

    private string _avaxRPC;

    public string AVAXRPC
    {
        get => _avaxRPC;
        set
        {
            _avaxRPC = value.Trim();
            OnPropertyChanged(nameof(AVAXRPC));
        }
    }

    private string _polygonRPC;

    public string PolygonRPC
    {
        get => _polygonRPC;
        set
        {
            _polygonRPC = value?.Trim();
            OnPropertyChanged(nameof(PolygonRPC));
        }
    }

    private string _optimismRPC;

    public string OptimismRPC
    {
        get => _optimismRPC;
        set
        {
            _optimismRPC = value?.Trim();
            OnPropertyChanged(nameof(OptimismRPC));
        }
    }

    public ICommand GenerateAccountsCommand
    {
        get => new RelayCommand(async () =>
        {
            if (Int32.TryParse(GenerateInputAmount, out int inputAmount))
            {
                AccountInfos.Clear();

                ClearAccountInfos();

                for (int i = 0; i < inputAmount; i++)
                {
                    var (privateKey, address) = GenerateAccount();

                    var newEntity = new AccountInfo
                    {
                        PrivateKey = privateKey,
                        Address = address,
                        FantomInfo = new AddressChainInfo { ChainId = 250 },
                        AvaxInfo = new AddressChainInfo { ChainId = 43114 },
                        PolygonInfo = new AddressChainInfo { ChainId = 137 },
                        ArbitrumInfo = new AddressChainInfo { ChainId = 42161 },
                        OptimismInfo = new AddressChainInfo { ChainId = 10 }
                    };

                    var newEntityDto = new AccountInfoDto
                    {
                        PrivateKey = privateKey,
                        Address = address,
                        FantomInfo = new AddressChainInfo { ChainId = 250 },
                        AvaxInfo = new AddressChainInfo { ChainId = 43114 },
                        PolygonInfo = new AddressChainInfo { ChainId = 137 },
                        ArbitrumInfo = new AddressChainInfo { ChainId = 42161 },
                        OptimismInfo = new AddressChainInfo { ChainId = 10 },
                        IsSelected = false
                    };

                    AccountInfos.Add(newEntityDto);
                    db.AccountInfos.Add(newEntity);
                }

                db.SaveChanges();
                GenerateInputAmount = String.Empty;
            }
        }, () => GenerateInputAmount?.Length > 0);
    }

    public ICommand ClearAccountsCommand
    {
        get => new RelayCommand(() =>
        {
            AccountInfos.Clear();
            ClearAccountInfos();
        }, () => AccountInfos.Any());
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

    public ICommand SaveConfigData
    {
        get => new RelayCommand(() =>
        {
            var config = new AppConfig
            {
                BinanceAPIKey = BinanceAPI,
                BybitAPIKey = BybitAPI,
                OKXAPIKey = OKXAPI,
                ArbitrumRPC = ArbitrumRPC,
                FantomRPC = FantomRPC,
                AVAXRPC = AVAXRPC,
                PolygonRPC = PolygonRPC,
                OptimismRPC = OptimismRPC
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            string configString = JsonSerializer.Serialize(config, options);
            File.WriteAllText("config.json", configString);

            Debug.WriteLine("Connecting to RPCs again");
            ConnectToRpcList();
        }, () => true);
    }

    public ICommand RunEditCexAddressDialogCommand => new AnotherCommandImplementation(ExecuteRunEditCexAddressDialog);
    private async void ExecuteRunEditCexAddressDialog(object? _)
    {
        //let's set up a little MVVM, cos that's what the cool kids are doing:
        var view = new EditCexDialog()
        {
            DataContext = new EditCexDialogViewModel()
        };

        //show the dialog
        var result = await DialogHost.Show(view, "RootDialog");

        if (result is true)
        {
            var dialogViewModel = view.DataContext as EditCexDialogViewModel;
            var inputCexAddress = dialogViewModel?.Address;
            Debug.WriteLine($"Entered address: {inputCexAddress}");
            var accountInfoDto = AccountInfos.SingleOrDefault((x) => x.Address == (_ as AccountInfoDto)?.Address);
            accountInfoDto.CexAddress = inputCexAddress;
        }

        //check the result...
        Debug.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
    }
    
    public ICommand RunDepositToAddressDialog => new AnotherCommandImplementation(ExecuteRunDepositToAddressDialog);
    private async void ExecuteRunDepositToAddressDialog(object? _)
    {
        //let's set up a little MVVM, cos that's what the cool kids are doing:
        var view = new DepositToAddressDialog()
        {
            DataContext = new DepositToAddressDialogViewModel()
        };

        //show the dialog
        var result = await DialogHost.Show(view, "RootDialog");

        //check the result...
        Debug.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
    }

    private (string, string) GenerateAccount()
    {
        var privateKey = EthECKey.GenerateKey().GetPrivateKeyAsBytes().ToHex();
        var address = EthECKey.GetPublicAddress(privateKey);

        return (privateKey, address);
    }

    private void LoadInfoFromDb()
    {
        var accountInfosDb = db
            .AccountInfos
            .Include(x => x.ArbitrumInfo)
            .Include(x => x.AvaxInfo)
            .Include(x => x.FantomInfo)
            .Include(x => x.OptimismInfo)
            .Include(x => x.PolygonInfo)
            .ToList();

        foreach (var accountInfoDb in accountInfosDb)
        {
            AccountInfos.Add(new AccountInfoDto
            {
                Address = accountInfoDb.Address,
                ArbitrumInfo = accountInfoDb.ArbitrumInfo,
                AvaxInfo = accountInfoDb.AvaxInfo,
                CexAddress = accountInfoDb.CexAddress,
                FantomInfo = accountInfoDb.FantomInfo,
                OptimismInfo = accountInfoDb.OptimismInfo,
                IsSelected = false,
                PolygonInfo = accountInfoDb.PolygonInfo,
                PrivateKey = accountInfoDb.PrivateKey,
                TotalBalanceUsd = accountInfoDb.TotalBalanceUsd,
                TotalTxAmount = accountInfoDb.TotalTxAmount
            });
        }
    }

    private void LoadConfigInfo()
    {
        if (!File.Exists("config.json")) return;
        var configInfoString = File.ReadAllText("config.json");
        var configInfo = JsonSerializer.Deserialize<AppConfig>(configInfoString);
        BinanceAPI = configInfo.BinanceAPIKey;
        BybitAPI = configInfo.BybitAPIKey;
        OKXAPI = configInfo.OKXAPIKey;
        ArbitrumRPC = configInfo.ArbitrumRPC;
        FantomRPC = configInfo.FantomRPC;
        AVAXRPC = configInfo.AVAXRPC;
        PolygonRPC = configInfo.PolygonRPC;
        OptimismRPC = configInfo.OptimismRPC;
    }

    private void ClearAccountInfos()
    {
        foreach (var info in db.AccountInfos)
        {
            db.AccountInfos.Remove(info);
        }

        foreach (var chainInfo in db.AddressChainInfos)
        {
            db.AddressChainInfos.Remove(chainInfo);
        }

        db.SaveChanges();
    }

    private void ConnectToRpcList()
    {
        var dict = new Dictionary<int, string>();
        if (Uri.IsWellFormedUriString(ArbitrumRPC, UriKind.RelativeOrAbsolute))
            dict.Add(42161, ArbitrumRPC);
        if (Uri.IsWellFormedUriString(FantomRPC, UriKind.RelativeOrAbsolute))
            dict.Add(250, FantomRPC);
        if (Uri.IsWellFormedUriString(PolygonRPC, UriKind.RelativeOrAbsolute))
            dict.Add(137, PolygonRPC);
        if (Uri.IsWellFormedUriString(OptimismRPC, UriKind.RelativeOrAbsolute))
            dict.Add(10, OptimismRPC);
        if (Uri.IsWellFormedUriString(AVAXRPC, UriKind.RelativeOrAbsolute))
            dict.Add(43114, AVAXRPC);

        if (dict.Any())
            _web3Utils = new Web3Utils(dict);
    }
}