using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YodaUrbanMyth
{
    class Program
    {
        static void Main(string[] args)
        {
            Yoda yoda = new Yoda();
            yoda.TestYodaSyntax(null);
            yoda.TestYodaSyntax("World, hello!");
        }
    }
}
