using Microsoft.ServiceBus.Messaging;
using MsdnDataIntegrationPatterns.DataIntegration.External;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsdnDataIntegrationPatterns.DataIntegration
{
    class Partner
    {
        public void ReceiveCustomerRecord()
        {
            var client = SubscriptionClient.CreateFromConnectionString(connectionString, topicName, subscriptionName);
            client.OnMessageAsync(async message =>
            {
                CustomerRecord customerRecord = JsonConvert.DeserializeObject<CustomerRecord>(message.GetBody<string>());
                Guid correlationId = (Guid)message.Properties["CorrelationId"];

                if (new CorrelationService().CustomerExists(correlationId))
                {
                    await SaveAsync(customerRecord);
                }
            });
        }

        private Task SaveAsync(CustomerRecord customerRecord)
        {
            throw new NotImplementedException();
        }

        private readonly string connectionString = "<ConnectionString>";
        private readonly string topicName = "<TopicName>";
        private readonly string subscriptionName = "<SubscriptionName>";
    }
}
