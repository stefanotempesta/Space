using Microsoft.ServiceBus.Messaging;
using MsdnDataIntegrationPatterns.DataIntegration.Entities;
using MsdnDataIntegrationPatterns.DataIntegration.External;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MsdnDataIntegrationPatterns.DataIntegration
{
    public class Correlation
    {
        private async Task Execute(CustomerEntity customer)
        {
            // Map the Customer entity in the e-commerce application (source) to Customer record in the partner application (destination)
            CustomerRecord customerRecord = PrepareCustomerRecord(customer);

            // Create a connection to an Azure Service Bus Topic
            // Serialize the customer record and send the message to the Topic
            var client = TopicClient.CreateFromConnectionString(connectionString, topicName);
            var message = new BrokeredMessage(JsonConvert.SerializeObject(customerRecord));

            // Register the customer record with the Correlation Service and obtain a Correlation ID
            message.Properties["CorrelationId}"] = new CorrelationService().RegisterCustomer(customerRecord, subscriptionName);

            await client.SendAsync(message);
        }

        private CustomerRecord PrepareCustomerRecord(CustomerEntity customer)
        {
            return new CustomerRecord
            {
                Name = customer.Name,
                Email = customer.Email,
                NumberOfOrders = db.Orders.Where(o => o.CustomerId == customer.Id).Count(),
                LastOrderDate = db.Orders.LastOrDefault(o => o.CustomerId == customer.Id)?.CreatedOn,
                TotalAmountSpent = db.Orders.Where(o => o.CustomerId == customer.Id).Sum(o => o.TotalAmount)
            };
        }

        private readonly string connectionString = "<ConnectionString>";
        private readonly string topicName = "<TopicName>";
        private readonly string subscriptionName = "<SubscriptionName>";

        private MockDatabaseContext db = new MockDatabaseContext();
    }

    class MockDatabaseContext
    {
        public IList<OrderEntity> Orders { get; set; }
    }
}
