using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

using Gallery.Entities.Elections;

namespace setlist
{
    class Program
    {
        static void Main(string[] args)
        {
            string listName = args[0];
            var elections = new List<IElection>();

            for (int i = 1; i < args.Length; i++)
            {
                int id;
                if (Int32.TryParse(args[i], out id) && id.ToString() == args[i])
                {

                }
                else
                {

                }
            }
        }
    }
}
