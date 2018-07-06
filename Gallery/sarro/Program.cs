using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gallery.Entities.Candidates;
using Gallery.Entities.Subjects;

namespace sarro
{
    class Program
    {
        static void Main(string[] args)
        {
            var runoffRoot = args[0];
            var targetCount = args.Length > 1 ? Int32.Parse(args[1]) : 30;

        }
    }
}
