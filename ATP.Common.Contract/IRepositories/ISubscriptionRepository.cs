using ATP.Common.Contract.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ATP.Common.Contract.IRepositories
{
    public interface ISubscriptionRepository
    {
        Task<IEnumerable<Subscriber>> GetSubscribersAsync(CancellationToken cancellationToken);
    }
}
