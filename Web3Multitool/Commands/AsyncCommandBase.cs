using System;
using System.Threading.Tasks;

namespace Web3Multitool.Commands;

public abstract class AsyncCommandBase : CommandBase
{
    private bool _isExecuting;

    public bool IsExecuting
    {
        get => _isExecuting;
        set
        {
            _isExecuting = value;
            OnCanExecutedChanged();
        }
    }

    public override bool CanExecute(object parameter)
    {
        return !IsExecuting && base.CanExecute(parameter);
    }

    public override async void Execute(object parameter)
    {
        IsExecuting = true;

        try
        {
            await ExecuteAsync(parameter);
        }
        catch (Exception)
        {
            // ignored
        }
        finally
        {
            IsExecuting = false;
        }
    }

    public abstract Task ExecuteAsync(object parameter);
}