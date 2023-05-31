using System.Collections.ObjectModel;
using Web3Multitool.Dto;
using Web3Multitool.Models;

namespace Web3Multitool.ViewModels;

public class WithdrawToCexDialogViewModel : BaseViewModel
{
    private string? _address;

    public WithdrawToCexDialogViewModel(string? address)
    {
        _address = address;
    }

    public ObservableCollection<string> AvailableChainCollection { get; set; } = new()
    {
        "ethereum",
        "optimism",
        "bsc",
        "polygon",
        "arbitrum",
        "avalanche",
        "fantom"
    };

    public ObservableCollection<string> AvailableCoinCollection { get; set; } = new()
    {
        "usdt",
        "usdc",
        "native"
    };

    private string? _selectedChain;

    public string? SelectedChain
    {
        get => _selectedChain;
        set => SetField(ref _selectedChain, value);
    }

    private string? _selectedCoin;

    public string? SelectedCoin
    {
        get => _selectedCoin;
        set
        {
            SetField(ref _selectedCoin, value);
            // Todo: Get balance of chosen coin and chain and set to AvailableAmount
        }
    }

    private string? _withdrawAmount;

    public string? WithdrawAmount
    {
        get => _withdrawAmount;
        set => SetField(ref _withdrawAmount, value);
    }

    private double _availableAmount;

    public double AvailableAmount
    {
        get => _availableAmount;
        set => SetField(ref _availableAmount, value);
    }

    private bool _needToWithdrawAll;

    public bool NeedToWithdrawAll
    {
        get => _needToWithdrawAll;
        set { SetField(ref _needToWithdrawAll, value); }
    }
}