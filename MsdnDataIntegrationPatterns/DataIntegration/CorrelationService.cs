using MsdnDataIntegrationPatterns.DataIntegration.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsdnDataIntegrationPatterns.DataIntegration
{
    public class CorrelationService
    {
        public Guid RegisterCustomer(CustomerRecord record, string subscription)
        {
            return store.ContainsKey((record, subscription)) ?
                GetCustomerCorrelationId(record, subscription) :
                AddCustomer(record, subscription);
        }

        public bool CustomerExists(Guid correlationId)
        {
            return store.ContainsValue(correlationId);
        }

        private Guid GetCustomerCorrelationId(CustomerRecord record, string subscription)
        {
            return store[(record, subscription)];
        }

        private Guid AddCustomer(CustomerRecord record, string subscription)
        {
            Guid correlationId = Guid.NewGuid();
            store.Add((record, subscription), correlationId);

            return correlationId;
        }

        // This should be persisted
        Dictionary<(CustomerRecord Record, string Subscription), Guid> store = new Dictionary<(CustomerRecord, string), Guid>();
    }
}
