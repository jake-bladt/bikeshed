using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Gallery.Entities.Elections;
using Gallery.Entities.ImageGallery;
using Gallery.Entities.Utilities;

namespace Gallery.Migration
{
    public class ElectionMigrationHelper
    {
        public IImageGallery Gallery { get; protected set; }
        public IElectionSet Target { get; protected set; }

        public ElectionMigrationHelper(IImageGallery gallery, IElectionSet target )
        {
            Gallery = gallery;
            Target = target;
        }

        public bool MigrateDirectoryToDB(string dirPath, string electionName, DateTime eventDate, ElectionType eventType)
        {
            var election = Election.FromDirectory(dirPath, electionName, eventDate, eventType, Gallery);
            if (null == election) return false;
            return Target.Store(election);
        }

        public bool MigrateHistory(string rootPath)
        {
            if (!Directory.Exists(rootPath)) throw new ArgumentException("Could not find the directory " + rootPath);
            var rootDi = new DirectoryInfo(rootPath);
            rootDi.GetDirectories().ToList().ForEach(yearDi =>
            {
                if (IsAnnualFolder(yearDi))
                {
                    yearDi.GetDirectories().ToList().ForEach(monthDi =>
                    {
                        if(IsMonthlyFolder(monthDi))
                        {
                            if (!MigrateMonthlyDirectory(monthDi))
                            {
                                throw new ElectionMigrationException(
                                    String.Format("Failed to migrate election at " + monthDi.FullName));
                            }
                        }
                    });
                }
            });

            return true;
        }

        public bool MigrateSpecials(string rootPath)
        {
            var specialsPath = Path.Combine(rootPath, "special");
            if (!Directory.Exists(specialsPath)) throw new ArgumentException("Could not find the directory " + specialsPath);
            var specialsDi = new DirectoryInfo(specialsPath);
            specialsDi.GetDirectories().ToList().ForEach(electionDi =>
            {
                if (!MigrateSpecialDirectory(electionDi)) Trace.WriteLine(String.Format("Failed to migrate special election from {0}.", electionDi.Name));
            });
            return true;
        }

        public bool MigrateRunoffs(string rootPath)
        {
            var runoffsPath = Path.Combine(rootPath, "special");
            if (!Directory.Exists(runoffsPath)) throw new ArgumentException("Could not find the directory " + runoffsPath);
            var runoffsDi = new DirectoryInfo(runoffsPath);
            runoffsDi.GetDirectories().ToList().ForEach(electionDi =>
            {
                if (!MigrateRunoffDirectory(electionDi)) Trace.WriteLine(String.Format("Failed to migrate special election from {0}.", electionDi.Name));
            });
            return true;
        }

        protected Dictionary<string, ElectionType> ElectionTypesByName = new Dictionary<string, ElectionType> 
          { 
              { "travel", ElectionType.Travel },
              { "rookie", ElectionType.Rookie },
              { "star",   ElectionType.Star   },
              { "walkin", ElectionType.WalkIn },
              { "wonder", ElectionType.Wonder }
          };

        protected bool MigrateMonthlyDirectory(DirectoryInfo di)
        {
            ElectionTypesByName.ToList().ForEach(kvp =>
            {
                string fullPath = Path.Combine(di.FullName, kvp.Key);
                if (Directory.Exists(fullPath))
                {
                    DateTime eventDate = EventDateFromDirName(di.Name);
                    string name = MonthlyElectionNameFromParts(eventDate, kvp.Key);
                    var eventType = kvp.Value;
                    var migrateSuccess = MigrateDirectoryToDB(fullPath, name, eventDate, eventType);
                    if (!migrateSuccess) throw new ElectionMigrationException(
                         "Failed to migrate " + fullPath);
                }                
            });
            return true;
        }

        protected bool MigrateSpecialDirectory(DirectoryInfo di)
        {
            DateTime eventDate = di.LastWriteTime;
            string name = SpecialElectionNameFromDirectoryName(di.Name);
            return MigrateDirectoryToDB(di.FullName, name, eventDate, ElectionType.Special);
        }

        protected bool MigrateRunoffDirectory(DirectoryInfo di)
        {
            DateTime eventDate = di.CreationTime;
            int runoffNum = Int32.Parse(di.Name.Substring(9));
            var name = RunoffElectionNameFromParts(eventDate, runoffNum);
            return MigrateDirectoryToDB(di.FullName, name, eventDate, ElectionType.RunOff);
        }

        protected bool IsAnnualFolder(DirectoryInfo di)
        {
            string pattern = @"^\d{4}$";
            return Regex.IsMatch(di.Name, pattern);
        }

        protected bool IsMonthlyFolder(DirectoryInfo di)
        {
            string pattern = @"^\d{6}$";
            return Regex.IsMatch(di.Name, pattern);
        }

        protected string MonthlyElectionNameFromParts(DateTime eventDate, string subdirName)
        {
            return String.Format("{0} {1} group", eventDate.ToString("MMMM yyyy"), subdirName.ToLower());
        }

        protected string RunoffElectionNameFromParts(DateTime eventDate, int runoffNum)
        {
            return String.Format("{0} runoff #{1}", eventDate.ToString("MMMM d yyyy"), runoffNum);
        }

        protected string SpecialElectionNameFromDirectoryName(string directoryName)
        {
            var dnameArr = directoryName.ToCharArray();
            var sb = new StringBuilder(dnameArr[0]);
            for(int i = 1; i < dnameArr.Length; i++)
            {
                var c = dnameArr[i].ToString();
                if(c == c.ToUpper()) sb.Append(" ");
                sb.Append(c);
            }
            return sb.ToString();
        }

        protected DateTime EventDateFromDirName(string dirName)
        {
            var monthAndYearToken = Int32.Parse(dirName);
            int year = monthAndYearToken / 100;
            int month = monthAndYearToken % 100;
            return new DateTime(year, month, 1);
        }

    }
}
