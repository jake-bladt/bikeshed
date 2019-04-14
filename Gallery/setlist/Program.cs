using System;
using System.Collections.Generic;
using System.Configuration;

using Gallery.Entities.Elections;
using Gallery.Entities.SetLists;

namespace setlist
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstArg = args[0].ToLower();
            string cnStr = GetDbConnectionString();

            if (firstArg == "list" || firstArg == "l")
            {
                var count = args.Length > 1 ? Int32.Parse(args[1]) : 10;
                var reader = new SqlElectionReader(cnStr);
                var listings = reader.GetElectionListing(count);
                listings.ForEach(el => Console.WriteLine(String.Format("{0}. {1}", el.Id, el.Name)));
            }
            else
            {
                string listName = firstArg;
                var elections = new List<IElection>();

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

        private static string GetDbConnectionString()
        {
            var env = Environment.GetEnvironmentVariable("GALLERY_CONSTR");
            return String.IsNullOrEmpty(env) ?
                ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString : env;
        }

    }
}
