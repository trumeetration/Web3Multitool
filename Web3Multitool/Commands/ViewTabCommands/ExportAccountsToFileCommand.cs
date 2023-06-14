using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web3Multitool.Stores;
using Web3Multitool.ViewModels;

namespace Web3Multitool.Commands;

public class ExportAccountsToFileCommand : AsyncCommandBase
{
    private readonly ViewTabViewModel _viewTabViewModel;
    private readonly AccountInfosStore _accountInfosStore;

    public ExportAccountsToFileCommand(ViewTabViewModel viewTabViewModel, AccountInfosStore accountInfosStore)
    {
        _viewTabViewModel = viewTabViewModel;
        _accountInfosStore = accountInfosStore;
    }
    
    public override async Task ExecuteAsync(object parameter)
    {
        var content = _viewTabViewModel.AccountInfos.Aggregate(string.Empty, (current, accountInfo) => current + $"{accountInfo.PrivateKey}:{accountInfo.Address}:{accountInfo.CexAddress}\n");

        var destinationDirectory = Directory.GetCurrentDirectory() + "\\Export";

        if (!Directory.Exists(destinationDirectory))
            Directory.CreateDirectory(destinationDirectory);

        var filePath = $"{destinationDirectory}\\export_{Guid.NewGuid().ToString()}.txt";
        await File.WriteAllTextAsync(filePath, content);

        Process.Start("explorer.exe", destinationDirectory);
    }
}