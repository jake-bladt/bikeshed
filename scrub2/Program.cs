using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using ImageProcessor;
using ImageProcessor.Imaging.Formats;

namespace scrub2
{
    class Program
    {
        static string GalleryRoot;

        static void Main(string[] args)
        {
            // Load settings
            GalleryRoot = ConfigurationManager.AppSettings["GallerySource"];

            // Parse command line
            var verbose = args.Contains("-v") || args.Contains("--verbose");
            var subjects = args.ToList().Where(s => !s.StartsWith("-"));

            // Default to scrubbing entire gallery            
            if (String.IsNullOrEmpty(subjects.FirstOrDefault()))
            {
                var rootDI = new DirectoryInfo(GalleryRoot);
                var subdirs = rootDI.GetDirectories();
                subjects = subdirs.ToList().Select(d => d.Name.ToLowerInvariant());
            }

            // Scrub each target subdirectory
            subjects.ToList().ForEach(s => ScrubDir(s, verbose));

            Console.ReadLine();
        }

        static void ScrubDir(string dirName, bool verbose)
        {
            ConvertWebPs(dirName, verbose);
            var subdirPath = Path.Combine(GalleryRoot, dirName);
            var subdirDI = new DirectoryInfo(subdirPath);
            var jpegs = subdirDI.GetFiles("*.jpg");
            var jCount = jpegs.Length;

            var correctPaths = GetCorrectFilePaths(subdirDI, jCount);
            var jpegFilePaths = jpegs.ToList().Select(j => j.FullName);

            var filesToRename = jpegFilePaths.Where(s => !correctPaths.Contains(s)).ToList();
            var namesToUse = correctPaths.Where(s => !jpegFilePaths.Contains(s)).ToList();
            filesToRename.ForEach(p =>
            {
                var nextName = namesToUse.First();
                if (verbose) Console.WriteLine($"{p} => {nextName}");
                File.Move(p, nextName);
                namesToUse.Remove(nextName);
            });
        }

        static List<String> GetCorrectFilePaths(DirectoryInfo di, int count)
        {
            var ret = new List<String>();

            var subjectName = di.Name;
            var fullPath = di.FullName;

            for(int i = 1; i <= count; i++)
            {
                string filePath = String.Empty;
                if(count >= 1000)
                {
                    int vol = (i / 1000) + 1;
                    filePath = Path.Combine(fullPath, String.Format("vol{0}", vol.ToString("000")), 
                        String.Format("{0}(1}.jpg", subjectName, i.ToString("0000")));
                }
                else
                {
                    filePath = Path.Combine(fullPath, $"{subjectName}{i.ToString("000")}.jpg");
                }

                ret.Add(filePath);
            }
            return ret;
        }

        static void ConvertWebPs(string dirName, bool verbose)
        {
            var subdirPath = Path.Combine(GalleryRoot, dirName);
            var subdirDI = new DirectoryInfo(subdirPath);
            var webps = subdirDI.GetFiles("*.webp");
            var pCount = webps.Length;
            if(pCount > 0)
            {
                if (verbose) Console.WriteLine(String.Format("{0} webp {1} found.", pCount.ToString("#,##0"), pCount > 1 ? "files" : "file"));
                webps.ToList().ForEach(f => ConvertWebp(f, subdirDI, verbose));
            }
        }

        static void ConvertWebp(FileInfo fi, DirectoryInfo di, bool verbose)
        {
            var sourcePath = fi.FullName;
            var uuid = Guid.NewGuid();
            var destinationFile = String.Format("{0}.jpg", uuid.ToString().ToLowerInvariant());
            var destinationPath = Path.Combine(di.FullName, destinationFile);
            if (verbose) Console.WriteLine(String.Format("{0} => {1}", sourcePath, destinationFile));

            byte[] photoBytes = File.ReadAllBytes(sourcePath);
            var format = new JpegFormat { Quality = 100 };

            using (var inStream = new MemoryStream(photoBytes))
            {
                using (var imgFactory = new ImageFactory(true))
                {
                    var imgHolder = imgFactory.Load(inStream);
                    imgHolder
                        .Format(format)
                        .Save(destinationPath);
                }
            }
            File.Delete(sourcePath);
        }
    }
}
