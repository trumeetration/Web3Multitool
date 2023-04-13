using System;
using System.Windows.Input;

namespace Web3Multitool.Commands;

public abstract class BaseRelayCommand : ICommand
{
    public abstract void Execute(object parameter);

    public abstract bool CanExecute(object parameter);

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}