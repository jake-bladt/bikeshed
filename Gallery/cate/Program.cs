using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cate
{
    class Program
    {
        static void Main(string[] args)
        {
            if(0 == args.Length)
            {
                Console.WriteLine("USAGE: cate <subject name> <switches> <categories>");
            }

            if(args.Length > 1)
            {
                var parsed = new CommandLineParser(args);

                Console.WriteLine(parsed.SubjectName);
                Console.WriteLine($"First switch: {parsed.Switches.First()}");
                Console.WriteLine($"First category: {parsed.Categories.First()}");
            }



            Console.ReadLine();
        }
    }
}
