using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

using Gallery.Entities.SetLists;

namespace slideshow
{
    class Program
    {
        static void Main(string[] args)
        {
            string setName = args[0];
            string showName = args[1];
            int imageCount = Int32.Parse(args[2]);
            string cnStr = ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString;
            string subjectRoot = ConfigurationManager.AppSettings["subjectLocation"];
            string slideshowRoot = ConfigurationManager.AppSettings["slideshowLocation"];

            var setList = new SqlBackedSetlist(cnStr, setName);
            if(setList.Fetch())
            {
                var ssPath = Path.Combine(slideshowRoot, showName + ".txt");
                using (var writer = new StreamWriter(ssPath))
                {
                    setList.Values.ToList().ForEach(subj =>
                    {
                        var subjPath = Path.Combine(subjectRoot, subj.Name);
                        var di = new DirectoryInfo(subjPath);
                        var files = di.GetFiles("*.jpg", SearchOption.AllDirectories);
                        double includeChance = (double)imageCount / files.Count();
                        var rnd = new Random();
                        files.ToList().ForEach(fi =>
                        {
                            if (rnd.NextDouble() < includeChance)
                            {
                                writer.WriteLine(fi.FullName);
                            }
                        });
                    });
                }
                Console.WriteLine("Slideshow complete.");
            }
            else
            {
                Console.WriteLine("Could not load subject set");
            }
            Console.ReadLine();

        }
    }
}
