using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YodaUrbanMyth
{
    public class Yoda
    {
        public void TestYodaSyntax(string obj)
        {
            if (null == obj)
            {
                Console.WriteLine("null == obj");
            }

            if (obj == null)
            {
                Console.WriteLine("obj == null");
            }
        }
    }
}
