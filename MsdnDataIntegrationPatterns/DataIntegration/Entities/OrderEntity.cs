using System;
using System.Collections.Generic;

namespace MsdnDataIntegrationPatterns.DataIntegration.Entities
{
    public class OrderEntity : Entity
    {
        public string OrderNumber { get; set; }

        public Guid CustomerId { get; set; }

        public DateTime CreatedOn { get; set; }

        public decimal TotalAmount { get; set; }

        public ICollection<OrderItemEntity> Items { get; set; }
    }
}