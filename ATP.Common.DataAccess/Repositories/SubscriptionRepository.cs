using ATP.Common.Contract.IRepositories;
using ATP.Common.Contract.Models;
using ATP.Common.ConnectionManagement;
using Dapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ATP.Common.DataAccess.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        public async Task<IEnumerable<Subscriber>> GetSubscribersAsync(CancellationToken cancellationToken)
        {

            const string strSql = @"
                                    SELECT [Id]
                                          ,[Email]
                                    FROM [Subscriber] 
                                   ";

            using (var conn = await ConnectionFactory.GetOpenConnectionAsync(cancellationToken).ConfigureAwait(false))
            {
                return await conn.QueryAsync<Subscriber>(strSql).ConfigureAwait(false);
            }
        }
    }
}
