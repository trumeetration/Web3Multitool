using Web3MultiTool.Domain.Models;
using Web3Multitool.EntityFramework.DTOs;

namespace Web3Multitool.EntityFramework;

public static class Extensions
{
    private static AddressChainInfo FromDto(this AddressChainInfoDto addressChainInfoDto)
    {
        return new AddressChainInfo
        {
            Id = addressChainInfoDto.Id,
            ChainId = addressChainInfoDto.ChainId,
            TxAmount = addressChainInfoDto.TxAmount,
            NativeBalance = addressChainInfoDto.NativeBalance,
            FirstTxDate = addressChainInfoDto.FirstTxDate,
            UsdcBalance = addressChainInfoDto.UsdcBalance,
            UsdtBalance = addressChainInfoDto.UsdtBalance,
        };
    }

    public static AccountInfo FromDto(this AccountInfoDto accountInfoDto)
    {
        return new AccountInfo
        {
            Id = accountInfoDto.Id,
            Address = accountInfoDto.Address,
            CexAddress = accountInfoDto.CexAddress,
            PrivateKey = accountInfoDto.PrivateKey,
            FantomInfo = accountInfoDto.FantomInfo.FromDto(),
            AvaxInfo = accountInfoDto.AvaxInfo.FromDto(),
            PolygonInfo = accountInfoDto.PolygonInfo.FromDto(),
            ArbitrumInfo = accountInfoDto.ArbitrumInfo.FromDto(),
            OptimismInfo = accountInfoDto.OptimismInfo.FromDto(),
            BnbInfo = accountInfoDto.BnbInfo.FromDto(),
            HarmonyInfo = accountInfoDto.HarmonyInfo.FromDto(),
            CoredaoInfo = accountInfoDto.CoredaoInfo.FromDto(),
            TotalBalanceUsd = accountInfoDto.TotalBalanceUsd,
            TotalTxAmount = accountInfoDto.TotalTxAmount
        };
    }

    public static AccountInfoDto AsDto(this AccountInfo accountInfo)
    {
        return new AccountInfoDto
        {
            Id = accountInfo.Id,
            Address = accountInfo.Address,
            CexAddress = accountInfo.CexAddress,
            PrivateKey = accountInfo.PrivateKey,
            FantomInfo = accountInfo.FantomInfo.AsDto(),
            AvaxInfo = accountInfo.AvaxInfo.AsDto(),
            PolygonInfo = accountInfo.PolygonInfo.AsDto(),
            ArbitrumInfo = accountInfo.ArbitrumInfo.AsDto(),
            OptimismInfo = accountInfo.OptimismInfo.AsDto(),
            BnbInfo = accountInfo.BnbInfo.AsDto(),
            HarmonyInfo = accountInfo.HarmonyInfo.AsDto(),
            CoredaoInfo = accountInfo.CoredaoInfo.AsDto(),
            TotalBalanceUsd = accountInfo.TotalBalanceUsd,
            TotalTxAmount = accountInfo.TotalTxAmount
        };
    }

    private static AddressChainInfoDto AsDto(this AddressChainInfo addressChainInfo)
    {
        return new AddressChainInfoDto
        {
            Id = addressChainInfo.Id,
            ChainId = addressChainInfo.ChainId,
            TxAmount = addressChainInfo.TxAmount,
            NativeBalance = addressChainInfo.NativeBalance,
            FirstTxDate = addressChainInfo.FirstTxDate,
            UsdcBalance = addressChainInfo.UsdcBalance,
            UsdtBalance = addressChainInfo.UsdtBalance
        };
    }
}