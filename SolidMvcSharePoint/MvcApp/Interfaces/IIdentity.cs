using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidMvcSharePoint.Interfaces
{
    public interface IIdentity
    {
        string Username { get; set; }

        bool IsInRole(string roleName);
    }
}
