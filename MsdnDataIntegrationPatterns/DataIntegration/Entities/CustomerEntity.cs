using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsdnDataIntegrationPatterns.DataIntegration.Entities
{
    public class CustomerEntity : Entity
    {
        public string Name { get; set; }

        public string Email { get; set; }
    }
}
