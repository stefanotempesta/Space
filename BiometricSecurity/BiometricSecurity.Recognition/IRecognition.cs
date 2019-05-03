using System;
using System.Collections.Generic;
using System.Text;

namespace BiometricSecurity.Recognition
{
    public interface IRecognition
    {
        double Recognize(string siteId, out string name);
    }
}
