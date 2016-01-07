using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gallery.Entities.Candidates;

namespace runoffs
{
    class Program
    {
        static void Main(string[] args)
        {
            var runoffRoot = args[0];
            var targetCount = Int32.Parse(args[1]);
            var contestCount = args.Length > 2 ? Int32.Parse(args[2]) : 1;
            
            string poolRoot = ConfigurationManager.AppSettings["yearbookLocation"];
            var poolInfo = new DirectoryInfo(poolRoot);


        }
    }
}
