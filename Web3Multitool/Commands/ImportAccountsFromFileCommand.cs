using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Win32;
using Nethereum.Signer;
using Web3Multitool.Stores;
using Web3Multitool.ViewModels;

namespace Web3Multitool.Commands;

public class ImportAccountsFromFileCommand : AsyncCommandBase
{
    private readonly ViewTabViewModel _viewTabViewModel;
    private readonly AccountInfosStore _accountInfosStore;

    public ImportAccountsFromFileCommand(ViewTabViewModel viewTabViewModel, AccountInfosStore accountInfosStore)
    {
        _viewTabViewModel = viewTabViewModel;
        _accountInfosStore = accountInfosStore;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        var dialog = new OpenFileDialog()
        {
            Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
            FilterIndex = 0
        };
        if (dialog.ShowDialog() != true)
            return;
        var content = await File.ReadAllLinesAsync(dialog.FileName);

        foreach (var line in content)
        {
            var privateKey = line.Contains(':')
                ? line.Split(':')[0]
                : line.Trim().Replace(Environment.NewLine, String.Empty);

            if (privateKey.Length == 0) continue;
            var address = EthECKey.GetPublicAddress(privateKey);
            // _viewTabViewModel.AccountInfos.Add(new AccountInfoDto
            // {
            //     Address = address,
            //     PrivateKey = privateKey
            // });
        }
    }
}