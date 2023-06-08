using System.ComponentModel.DataAnnotations.Schema;

namespace Web3Multitool.EntityFramework.DTOs;

[Table("AddressChainInfos")]
public class AddressChainInfoDto
{
    public Guid Id { get; set; }
    public int ChainId { get; set; }
    public int TxAmount { get; set; }
    public double BaseBalance { get; set; }
    public DateTime FirstTxDate { get; set; }
    public double UsdcBalance { get; set; }
    public double UsdtBalance { get; set; }
}