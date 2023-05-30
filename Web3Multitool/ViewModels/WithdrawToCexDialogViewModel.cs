using System.Collections.ObjectModel;
using Web3Multitool.Dto;
using Web3Multitool.Models;

namespace Web3Multitool.ViewModels;

public class WithdrawToCexDialogViewModel : BaseViewModel
{
    private string _selectedChain;

    public string SelectedChain
    {
        get => _selectedChain;
        set => SetField(ref _selectedChain, value);
    }

    private string _coin;

    public string Coin
    {
        get => _coin;
        set => SetField(ref _coin, value);
    }
    
    private double _amountFrom;

    public double AmountFrom
    {
        get => _amountFrom;
        set => SetField(ref _amountFrom, value);
    }
    
    private double _amountTo;

    public double AmountTo
    {
        get => _amountTo;
        set => SetField(ref _amountTo, value);
    }

    private double _availableAmount;

    public double AvailableAmount
    {
        get => _availableAmount;
        set => SetField(ref _availableAmount, value);
    }
}