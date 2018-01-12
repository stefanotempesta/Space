using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalStringOrdering
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = new List<string>()
            {
                "File 1.txt",
                "File 2.jpg",
                "File 3.doc",
                "File 10.txt",
                "File 11.csv",
                "File 20.xls",
                "File 21.ppt"
            };

            Program program = new Program();
            program.DefaultOrderBy(files);
            program.NaturalOrderBy(files);
            program.ExtensionOrderBy(files);
            program.ExtensionOrderByDescending(files);
        }

        void DefaultOrderBy(List<string> strings)
        {
            Console.WriteLine("Default OrderBy");
            foreach (var s in strings.OrderBy(n => n))
            {
                Console.WriteLine(s);
            }
            Console.WriteLine();
        }

        void NaturalOrderBy(List<string> strings)
        {
            Console.WriteLine("Ascending Natural OrderBy with explicit NaturalStringComparer");
            foreach (var s in strings.OrderBy(n => n, new NaturalStringComparer()))
            {
                Console.WriteLine(s);
            }
            Console.WriteLine();
        }

        void ExtensionOrderBy(List<string> strings)
        {
            Console.WriteLine("Ascending Natural OrderBy with implicit NaturalStringComparer");
            foreach (var s in strings.OrderByNatural(n => n))
            {
                Console.WriteLine(s);
            }
            Console.WriteLine();
        }

        void ExtensionOrderByDescending(List<string> strings)
        {
            Console.WriteLine("Descending Natural OrderBy with implicit NaturalStringComparer");
            foreach (var s in strings.OrderByNaturalDescending(n => n))
            {
                Console.WriteLine(s);
            }
            Console.WriteLine();
        }
    }
}
