using System;
using System.Collections.Generic;

namespace MsdnDataIntegrationPatterns.DataIntegration.Entities
{
    public class ProductEntity : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}