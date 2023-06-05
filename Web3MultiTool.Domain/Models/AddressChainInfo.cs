namespace Web3MultiTool.Domain.Models;

public class AddressChainInfo
{
    public int Id { get; set; }
    public int ChainId { get; set; }
    public int TxAmount { get; set; }
    public double BaseBalance { get; set; }
    public DateTime FirstTxDate { get; set; }
    public double UsdcBalance { get; set; }
}