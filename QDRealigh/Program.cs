using System;
using System.IO;
using System.Linq;

namespace QDRealigh
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = args[0];
            var isLive = args.Length > 1 && args[1] == "live";
            var mode = isLive ? "live" : "test";

            Console.WriteLine($"Performing a {mode} realignment of {path}.");

            var di = new DirectoryInfo(path);
            var images = di.GetFiles("*.jpg");
            var newRank = 0;
            images.OrderBy(i => i.Name).ToList().ForEach(im =>
            {
                newRank++;
                var fileName = im.Name;
                var parts = fileName.Split("-");
                var rankStr = parts[0];
                var currRank = int.Parse(rankStr);

                if(currRank != newRank)
                {
                    var newFileName = fileName.Replace(rankStr, newRank.ToString("0000"));
                    Console.WriteLine($"{fileName} -> {newFileName}");
                    if(isLive)
                    {
                        var newPath = Path.Combine(path, newFileName);
                        File.Move(im.FullName, newPath);
                    }
                }

            });

        }
    }
}
