using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

using Gallery.Entities.Elections;
using Gallery.Entities.SetLists;

namespace setlist
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstArg = args[0].ToLower();

            if (firstArg == "list" || firstArg == "l")
            {
                // run in list mode.
            }
            else
            {
                string listName = firstArg;
                var elections = new List<IElection>();
                string cnStr = ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString;

                for (int i = 1; i < args.Length; i++)
                {
                    int id;
                    IElection election;
                    if (Int32.TryParse(args[i], out id) && id.ToString() == args[i])
                    {
                        election = SqlBackedElection.FromId(id, cnStr);
                    }
                    else
                    {
                        election = SqlBackedElection.FromName(args[i], cnStr);
                    }
                    if (null == election)
                    {
                        Console.WriteLine(String.Format("There is no election named {0}", args[i]));
                        return;
                    }
                    elections.Add(election);
                }

                var theSetList = SetList.FromElections(elections, listName);
                var dbSetList = SqlBackedSetlist.CloneFrom(theSetList, cnStr);
                if (dbSetList.Store())
                {
                    Console.WriteLine("Setlist created.");
                }
                else
                {
                    Console.WriteLine("Failed to create setlist.");
                }
            }

            Console.ReadLine();
        }
    }
}
