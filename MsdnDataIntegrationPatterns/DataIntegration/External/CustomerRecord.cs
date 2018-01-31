using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsdnDataIntegrationPatterns.DataIntegration.External
{
    public class CustomerRecord
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public int NumberOfOrders { get; set; }

        public DateTime? LastOrderDate { get; set; }

        public decimal TotalAmountSpent { get; set; }
    }
}
