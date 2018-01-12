using SolidMvcSharePoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidMvcSharePoint.Interfaces
{
    public interface INotificationService
    {
        void Notify(Registration registration);
    }
}
