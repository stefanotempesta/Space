using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            new ThermostatSimulator().RunSimulation().Wait();
            Console.ReadKey();
        }
    }
}
