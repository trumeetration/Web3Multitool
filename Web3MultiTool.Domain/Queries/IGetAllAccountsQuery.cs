using System.Collections;
using Web3MultiTool.Domain.Models;

namespace Web3MultiTool.Domain.Queries;

public interface IGetAllAccountsQuery
{
    Task<IEnumerable<AccountInfo>> Execute();
}