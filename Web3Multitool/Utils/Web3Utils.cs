using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Nethereum.Web3;

namespace Web3Multitool.Utils;

public class Web3Utils
{
    public Dictionary<int, Web3> Providers { get; set; } = new();

    public Web3Utils(Dictionary<int, string> rpcDictionary)
    {
        foreach (var (chainId, rpcUrl) in rpcDictionary)
        {
            try
            {
                var w3 = new Web3(rpcUrl);
                var networkId = w3.Net.Version.SendRequestAsync().Result;
                Debug.WriteLine(networkId);
                Providers.Add(chainId, w3);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Problem with rpc {rpcUrl}: {e}");
            }
        }
    }
}