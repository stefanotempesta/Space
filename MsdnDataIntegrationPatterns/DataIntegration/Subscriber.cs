using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsdnDataIntegrationPatterns.DataIntegration
{
    class Subscriber
    {
        public async Task ReadMessageAsync()
        {
            await StateMachine.Current.BeginTransactionAsync(topicName);

            var client = SubscriptionClient.CreateFromConnectionString(connectionString, topicName, subscriptionName);
            client.OnMessageAsync(async message =>
            {
                var entity = JsonConvert.DeserializeObject(message.GetBody<string>());

                try
                {
                    Save(entity);
                    await StateMachine.Current.SuccessAsync(message, topicName);
                }
                catch
                {
                    await StateMachine.Current.CancelAsync(message, topicName);
                }
            });
        }

        private void Save(object entity)
        {
            // Persists the message
            // If fails, throws an exception
        }

        private readonly string connectionString = "<ConnectionString>";
        private readonly string topicName = "<TopicName>";
        private readonly string subscriptionName = "<SubscriptionName>";
    }
}
