using Microsoft.ServiceBus.Messaging;
using MsdnDataIntegrationPatterns.DataIntegration.Entities;
using System;
using System.Activities;

namespace MsdnDataIntegrationPatterns.DataIntegration
{
    public class Aggregation
    {
        public void Execute()
        {
            var client = SubscriptionClient.CreateFromConnectionString(connectionString, topicName, subscriptionName);
            client.OnMessage(message => {
                ProductEntity product = EntityMap.Instance.MapToEntity<ProductEntity>(message);

                // Persist the product
                var exists = Find(product.Id) != null;
                if (exists)
                    Update(product);
                else
                    Create(product);
            });
        }

        private readonly string connectionString = "<ConnectionString>";
        private readonly string topicName = "<TopicName>";
        private readonly string subscriptionName = "<SubscriptionName>";
    }
}
