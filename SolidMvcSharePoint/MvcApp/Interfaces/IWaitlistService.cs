using SolidMvcSharePoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidMvcSharePoint.Interfaces
{
    public interface IWaitlistService
    {
        void Add(Registration registration);
    }
}
