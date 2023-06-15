using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OKX.Api.Models.Funding;

namespace Web3Multitool.ViewModels;

public class DepositToManyAddressDialogViewModel : BaseViewModel
{
    public List<string> AvailableCoinToDepositCollection { get; } = new()
    {
        "ETH",
        "BNB",
        "MATIC",
        "AVAX",
        "USDC",
        "USDT"
    };

    public ObservableCollection<Coin> Coins { get; }

    private readonly IEnumerable<OkxFundingBalance> _balances;

    public DepositToManyAddressDialogViewModel(IEnumerable<OkxCurrency> currencyEnumerable,
        IEnumerable<OkxFundingBalance> balanceResponseData)
    {
        _balances = balanceResponseData;
        Coins = new ObservableCollection<Coin>();
        
        foreach (var coinSymbol in AvailableCoinToDepositCollection)
        {
            var coinInfoList = currencyEnumerable
                .Where(coin => coin.Currency == coinSymbol && coin.AllowWithdrawal)
                .Select(coin => new CoinChainInfo
                {
                    Chain = coin.Chain,
                    Fee = coin.MinimumWithdrawalFee,
                    MinWithdrawAmount = coin.MinimumWithdrawalAmount
                })
                .ToList();
            
            Coins.Add(new Coin
            {
                Symbol = coinSymbol,
                CoinChainInfos = new ObservableCollection<CoinChainInfo>(coinInfoList)
            });
        }
    }

    public ObservableCollection<CoinChainInfo> SelectedCoinChainInfosCollection => SelectedCoin?.CoinChainInfos;

    private CoinChainInfo _selectedChain;

    public CoinChainInfo SelectedChain
    {
        get => _selectedChain;
        set => SetField(ref _selectedChain, value);
    }

    private Coin _selectedCoin;

    public Coin SelectedCoin
    {
        get => _selectedCoin;
        set
        {
            SetField(ref _selectedCoin, value);
            var coin = _balances.SingleOrDefault(balance => balance.Currency == _selectedCoin.Symbol);
            CoinBalance = coin?.Available ?? 0;
            OnPropertyChanged(nameof(SelectedCoinChainInfosCollection));
        }
    }

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

    private string _minDelay;

    public string MinDelay
    {
        get => _minDelay;
        set => SetField(ref _minDelay, value);
    }

    private string _maxDelay;

    public string MaxDelay
    {
        get => _maxDelay;
        set => SetField(ref _maxDelay, value);
    }

    private decimal _coinBalance;

    public decimal CoinBalance
    {
        get => _coinBalance;
        set => SetField(ref _coinBalance, decimal.Round(value, 4));
    }

    public class Coin
    {
        public string Symbol { get; set; }
        public ObservableCollection<CoinChainInfo> CoinChainInfos { get; set; }
    }

    public class CoinChainInfo
    {

        public string Chain { get; set; }

        public decimal Fee { get; set; }

        public decimal MinWithdrawAmount { get; set; }
    }
}