using Microsoft.ServiceBus.Messaging;
using MsdnDataIntegrationPatterns.DataIntegration.Entities;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MsdnDataIntegrationPatterns.DataIntegration
{
    public class Broadcast
    {
        public async Task Execute(Entity entity)
        {
            var client = TopicClient.CreateFromConnectionString(connectionString, topicName);
            var message = new BrokeredMessage(JsonConvert.SerializeObject(entity));

            await client.SendAsync(message);
        }

        private readonly string connectionString = "<ConnectionString>";
        private readonly string topicName = "<TopicName>";
    }
}
