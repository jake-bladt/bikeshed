using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sneakers
{
    class Program
    {
        static void Main(string[] args)
        {
            if(1 == args.Length)
            {
                var dir = args[0];

            }
            else
            {
                Usage();
            }
        }

    static void Usage()
    {
        Console.WriteLine("Usage: sneakers [directory]");
    }

    }
}
