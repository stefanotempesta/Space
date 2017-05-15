using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningServiceDisplay
{
    class Program
    {
        static void Main(string[] args)
        {
            AzureMLExperimentServiceClient.Run();

            Console.ReadKey();
        }
    }
}
