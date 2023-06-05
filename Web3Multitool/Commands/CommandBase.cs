using System;
using System.Windows.Input;

namespace Web3Multitool.Commands;

public abstract class CommandBase : ICommand
{
    public event EventHandler CanExecuteChanged;

    public virtual bool CanExecute(object parameter)
    {
        return true;
    }

    public abstract void Execute(object parameter);

    protected virtual void OnCanExecutedChanged()
    {
        CanExecuteChanged?.Invoke(this, new EventArgs());
    }
}