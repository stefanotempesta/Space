using AutoMapper;
using Microsoft.ServiceBus.Messaging;
using MsdnDataIntegrationPatterns.DataIntegration.Entities;
using MsdnDataIntegrationPatterns.DataIntegration.External;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsdnDataIntegrationPatterns.DataIntegration
{
    internal class EntityMap
    {
        private EntityMap()
        {
        }

        protected Dictionary<(string SystemName, string PrimaryKey), (Type EntityType, Guid EntityId)> map =
            new Dictionary<(string SystemName, string PrimaryKey), (Type EntityType, Guid EntityId)>();

        private static EntityMap _instance;
        public static EntityMap Instance => _instance ?? (_instance = new EntityMap());
        
        public T MapToEntity<T>(BrokeredMessage message) where T : Entity, new()
        {
            string systemName = message.Properties["SystemName"] as string;
            string primaryKey = message.Properties["PrimaryKey"] as string;

            T entity = BuildEntity<T>(message);
            map.Add((systemName, primaryKey), (entity.GetType(), entity.Id));

            return entity;
        }

        private T BuildEntity<T>(BrokeredMessage message) where T : Entity, new()
        {
            var source = JsonConvert.DeserializeObject(message.GetBody<string>());
            T entity = Mapper.Map<T>(source);

            return entity;
        }
    }
}
