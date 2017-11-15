using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace annual
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 2)
            {

            }
            else
            {
                Usage();
            }
        }

        static void Usage()
        {
            Console.WriteLine("Usage: annual [year] [directory]");
        }

    }
}
