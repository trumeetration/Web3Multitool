using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Web3Multitool.ViewModels;

public class DepositToAddressDialogViewModel : BaseViewModel
{
    public Dictionary<string, ObservableCollection<string>> AvailableCoinToDepositCollection
    {
        get;
        set;
    } = new()
    {
        {
            "ETH", new ObservableCollection<string>()
            {
                "ETH",
                "BEP20",
                "AVAXC",
                "MATIC",
                "ARBITRUM",
                "OPTIMISM"
            }
        },
        {
            "MATIC", new ObservableCollection<string>()
            {
                "ETH",
                "BEP20",
                "AVAXC",
                "MATIC",
                "ARBITRUM",
                "OPTIMISM"
            }
        },
        {
            "USDT", new ObservableCollection<string>()
            {
                "ETH",
                "BEP20",
                "AVAXC",
                "MATIC",
                "ARBITRUM",
                "OPTIMISM"
            }
        },
        {
            "USDC", new ObservableCollection<string>()
            {
                "ETH",
                "BEP20",
                "AVAXC",
                "MATIC",
                "ARBITRUM",
                "OPTIMISM"
            }
        },
        {
            "AVAX", new ObservableCollection<string>()
            {
                "ETH",
                "BEP20",
                "AVAXC",
                "MATIC",
                "ARBITRUM",
                "OPTIMISM"
            }
        },
    };
    
    private string _selectedChain;

    public string SelectedChain
    {
        get => _selectedChain;
        set => SetField(ref _selectedChain, value);
    }

    private string _selectedCoin;

    public string SelectedCoin
    {
        get => _selectedCoin;
        set
        {
            SetField(ref _selectedCoin, value);
            SelectedCoinChainCollection = AvailableCoinToDepositCollection[_selectedCoin];
            OnPropertyChanged(nameof(SelectedCoinChainCollection));
        }
    }

    public ObservableCollection<string> SelectedCoinChainCollection { get; set; } = new();

    private string _amountFrom;

    public string AmountFrom
    {
        get => _amountFrom;
        set => SetField(ref _amountFrom, value);
    }
    
    private string _amountTo;

    public string AmountTo
    {
        get => _amountTo;
        set => SetField(ref _amountTo, value);
    }

    private bool _needToRandomize;

    public bool NeedToRandomize
    {
        get => _needToRandomize;
        set => SetField(ref _needToRandomize, value);
    }
}