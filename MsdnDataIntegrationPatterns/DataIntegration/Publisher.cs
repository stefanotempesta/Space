using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsdnDataIntegrationPatterns.DataIntegration
{
    class Publisher
    {
        public async Task SendMessageToServiceBus(object obj)
        {
            var client = TopicClient.CreateFromConnectionString(connectionString, topicName);
            var message = new BrokeredMessage(JsonConvert.SerializeObject(obj));

            message.Properties["SystemName"] = "Publisher System Name";
            message.Properties["PrimaryKey"] = "Object Primary Key";

            await client.SendAsync(message);
        }

        private readonly string connectionString = "<ConnectionString>";
        private readonly string topicName = "<TopicName>";
    }
}
