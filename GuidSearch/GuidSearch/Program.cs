using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuidSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            GuidSearch search = new GuidSearch();
            Stopwatch watch = Stopwatch.StartNew();
            Guid duplicate = Guid.Empty;

            try
            {
                duplicate = search.FindDuplicate();
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine("Sorry, the Universe has ended...");
            }
            finally
            {
                watch.Stop();
                Console.WriteLine($"Duplicate GUID {duplicate} found after {watch.Elapsed.Days} days, {watch.Elapsed.Hours} hours and {watch.Elapsed.Minutes} minutes and {watch.Elapsed.Seconds} seconds!");
            }
        }
    }
}
