using ATP.Common.Contract.IRepositories;
using ATP.Common.Contract.IServices;
using ATP.Common.Contract.Models;
using ATP.Common.DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ATP.Common.Logic.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionService()
        {
            _subscriptionRepository = new SubscriptionRepository();
        }

        public async Task<IEnumerable<Subscriber>> GetSubscribersAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var subscribers = await _subscriptionRepository.GetSubscribersAsync(cancellationToken).ConfigureAwait(false);
            return subscribers;
        }
    }
}
