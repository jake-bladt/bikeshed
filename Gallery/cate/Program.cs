using System;
using System.Configuration;
using System.Linq;

using Gallery.Entities.Taxonomy;

namespace cate
{
    class Program
    {
        static void Main(string[] args)
        {
            var cnStr = ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString;
            var repo = new SubjectCategoryRepository(cnStr);
            var parsed = new CommandLineParser(args);

            if (0 == args.Length)
            {
                Console.WriteLine("USAGE: cate <subject name> <switches> <categories>");
                return;
            }

            if(args.Length > 1)
            {
                Console.WriteLine(parsed.SubjectName);
                Console.WriteLine($"First switch: {parsed.Switches.First()}");
                Console.WriteLine($"First category: {parsed.Categories.First()}");
            }

            var cats = repo.GetSubjectCategories(parsed.SubjectName);

            Console.WriteLine($"Categories for {parsed.SubjectName}");
            cats.ForEach(c => Console.WriteLine(c));

            Console.ReadLine();
        }
    }
}
