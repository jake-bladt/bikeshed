using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace pele
{
    class Program
    {
        public static string Path { get; set; }

        public static void Usage()
        {
            Console.WriteLine("Usage: pele <directory-to-watch>");
        }

        public static void ScanElection(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Change in {e.Name}.");

            var di = new DirectoryInfo(Path);
            var files = di.GetFiles("*.jpg");
            var totalFileCount = files.Length;
            Console.WriteLine($"{totalFileCount} files.");

            var startsWithRank = @"^\d+";
            var allFileList = files.ToList();
            var rankedFiles = allFileList.Where(f => Regex.IsMatch(f.Name, startsWithRank));

            var arr = rankedFiles.ToArray();
            var rankedFileCount = arr.Length;
            if (rankedFileCount > 0)
            {
                Console.WriteLine($"{rankedFileCount} ranked files.");
                var expectedRanks = new List<int>();
                for(int i = 0; i < rankedFileCount; i++)
                {
                    expectedRanks.Add(totalFileCount - i);
                }

                var errList = new List<string>();
                rankedFiles.ToList().ForEach(f =>
                {
                    var r = Regex.Match(f.Name, startsWithRank).Value;
                    var rank = int.Parse(r);
                    if(expectedRanks.Contains(rank))
                    {
                        expectedRanks.Remove(rank);
                    }
                    else
                    {
                        var minExpectedRank = totalFileCount - rankedFileCount;
                        if (rank >= minExpectedRank && rank <= totalFileCount)
                        {
                            errList.Add($"Duplicate rank {rank}.");
                        } 
                        else
                        {
                            errList.Add($"Unexpected rank {rank}.");
                        }
                    }

                    if(errList.ToArray().Length > 0)
                    {
                        errList.ForEach(e => Console.WriteLine(e));
                    }

                });

            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                Usage();
                return;
            }

            Path = args[0];
            Console.WriteLine($"Watching {Path}.");
            var watcher = new FileSystemWatcher(Path);
            watcher.Created += ScanElection;
            watcher.Deleted += ScanElection;
            watcher.Renamed += ScanElection;
            watcher.EnableRaisingEvents = true;

            Console.ReadLine();
        }
    }
}
