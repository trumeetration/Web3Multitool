using System;
using Web3Multitool.ViewModels;

namespace Web3Multitool.Stores;

public class ModalNavigationStore
{
    private BaseViewModel _currentViewModel;

    public BaseViewModel CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            CurrentViewModelChanged?.Invoke();
        }
    }

    public event Action CurrentViewModelChanged;
    
    public bool IsOpen => CurrentViewModel != null;

    public void Close()
    {
        CurrentViewModel = null;
    }
}