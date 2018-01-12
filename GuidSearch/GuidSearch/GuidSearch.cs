using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuidSearch
{
    public class GuidSearch
    {
        public Guid FindDuplicate()
        {
            var aLotOfGuids = new List<Guid>();
            Guid newGuid = Guid.Empty;
            bool found = false;

            while (!found)
            {
                newGuid = Guid.NewGuid();
                found = aLotOfGuids.Contains(newGuid);
                if (!found)
                {
                    aLotOfGuids.Add(newGuid);
                }
            };

            return newGuid;
        }
    }
}
