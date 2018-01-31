using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsdnDataIntegrationPatterns.DataIntegration.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
    }
}
