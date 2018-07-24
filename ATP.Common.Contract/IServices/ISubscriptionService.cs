using ATP.Common.Contract.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ATP.Common.Contract.IServices
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<Subscriber>> GetSubscribersAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
