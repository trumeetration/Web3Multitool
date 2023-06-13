using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Win32;
using Nethereum.Signer;
using Web3MultiTool.Domain.Models;
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

        var newAccountInfos = new List<AccountInfo>();

        foreach (var line in content)
        {
            string? cexAddress = null;
            var privateKey = line.Contains(':')
                ? line.Split(':')[0]
                : line.Trim().Replace(Environment.NewLine, String.Empty);

            if (privateKey.Length == 0) continue;
            
            var address = EthECKey.GetPublicAddress(privateKey);
            
            if (line.Split(':').Length == 3)
                cexAddress = line.Split(':')[2]; // Also set CEX address
            
            // var privateKey = "0x";
            // var address =
            //     Nethereum.Util.AddressUtil.Current.ConvertToChecksumAddress(line.Trim()
            //         .Replace(Environment.NewLine, String.Empty));

            newAccountInfos.Add(new AccountInfo
            {
                Id = Guid.NewGuid(),
                Address = address,
                PrivateKey = privateKey,
                CexAddress = cexAddress
            });
        }

        await _accountInfosStore.Add(newAccountInfos);
    }
}