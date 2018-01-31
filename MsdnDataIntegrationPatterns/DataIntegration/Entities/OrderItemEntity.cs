using System;
using System.Collections.Generic;

namespace MsdnDataIntegrationPatterns.DataIntegration.Entities
{
    public class OrderItemEntity : Entity
    {
        public Guid ProductId { get; set; }

        public Guid CustomerId { get; set; }

        public int Quantity { get; set; }
    }
}