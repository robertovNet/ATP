using System.Collections.Generic;

namespace ATP.Common.Entities
{
    public class EndPointConfiguration
    {
        public string Address { get; set; }

        public IEnumerable<Policy> Policies { get; set; }
    }
}
