using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsdnDataIntegrationPatterns.DataIntegration
{
    internal class StateMachine
    {
        private StateMachine()
        {
        }

        private static StateMachine _instance;
        public static StateMachine Current => _instance ?? (_instance = new StateMachine());

        // Topic - Count
        protected ConcurrentDictionary<string, int> transactions = new ConcurrentDictionary<string, int>();

        public async Task<bool> SuccessAsync(BrokeredMessage message, string topicName)
        {
            bool done = await EndTransactionAsync(topicName);
            int count = Current.transactions[topicName];

            // All concurrent transactions are done
            if (done && count == 0)
            {
                await message.CompleteAsync();
            }

            return done;
        }

        public async Task<bool> CancelAsync(BrokeredMessage message, string topicName)
        {
            // Cancel the message broadcast -> Remove all concurrent transactions
            int count = Current.transactions[topicName];
            bool done = Current.transactions.TryUpdate(topicName, 0, count);

            if (done)
            {
                await message.DeadLetterAsync();
            }

            return done;
        }

        public async Task<bool> BeginTransactionAsync(string topicName)
        {
            if (Current.transactions.ContainsKey(topicName))
            {
                int count = Current.transactions[topicName];
                return Current.transactions.TryUpdate(topicName, count + 1, count);
            }
            else
            {
                return Current.transactions.TryAdd(topicName, 1);
            }
        }

        public async Task<bool> EndTransactionAsync(string topicName)
        {
            if (Current.transactions.ContainsKey(topicName))
            {
                int count = Current.transactions[topicName];
                return Current.transactions.TryUpdate(topicName, count - 1, count);
            }
            else
            {
                return false;
            }
        }
    }
}
