using System.Collections.ObjectModel;

namespace Web3Multitool.ViewModels;

public class WithdrawManyToCexDialogViewModel : BaseViewModel
{
    private Chain _selectedChain;
    private string _selectedCoin;
    private string? _minRemain;
    private string? _maxRemain;
    private string? _minDelay;
    private string? _maxDelay;

    public ObservableCollection<Chain> Chains { get; set; } = new ObservableCollection<Chain>()
    {
        new Chain
        {
            Name = "Arbitrum",
            Id = Utils.Chain.Arbitrum,
            CoinSymbolsList = new ObservableCollection<string>() { "native", "USDT", "USDC" }
        },
        new Chain
        {
            Name = "Fantom",
            Id = Utils.Chain.Fantom,
            CoinSymbolsList = new ObservableCollection<string>() { "native", "USDT", "USDC" }
        },
        new Chain
        {
            Name = "Polygon",
            Id = Utils.Chain.Polygon,
            CoinSymbolsList = new ObservableCollection<string>() { "native", "USDT", "USDC" }
        },
        new Chain
        {
            Name = "Optimism",
            Id = Utils.Chain.Optimism,
            CoinSymbolsList = new ObservableCollection<string>() { "native", "USDT", "USDC" }
        },
        new Chain
        {
            Name = "Avalanche",
            Id = Utils.Chain.Avalanche,
            CoinSymbolsList = new ObservableCollection<string>() { "native", "USDT", "USDC" }
        },
        new Chain
        {
            Name = "Binance",
            Id = Utils.Chain.Binance,
            CoinSymbolsList = new ObservableCollection<string>() { "native", "USDT", "USDC" }
        },
        new Chain
        {
            Name = "Coredao",
            Id = Utils.Chain.Coredao,
            CoinSymbolsList = new ObservableCollection<string>() { "native", "USDT", "USDC" }
        },
    };

    public Chain SelectedChain
    {
        get => _selectedChain;
        set => SetField(ref _selectedChain, value);
    }

    public string SelectedCoin
    {
        get => _selectedCoin;
        set => SetField(ref _selectedCoin, value);
    }

    public string? MinRemain
    {
        get => _minRemain;
        set => SetField(ref _minRemain, value);
    }

    public string? MaxRemain
    {
        get => _maxRemain;
        set => SetField(ref _maxRemain, value);
    }

    public string? MinDelay
    {
        get => _minDelay;
        set => SetField(ref _minDelay, value);
    }

    public string? MaxDelay
    {
        get => _maxDelay;
        set => SetField(ref _maxDelay, value);
    }

    public class Chain
    {
        public string Name { get; set; }
        public Utils.Chain Id { get; set; }
        public ObservableCollection<string> CoinSymbolsList { get; set; }
    }
}