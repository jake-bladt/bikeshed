using System;
using System.Collections.Generic;
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
                    string name = ElectionNameFromParts(eventDate, kvp.Key);
                    var eventType = kvp.Value;
                    var migrateSuccess = MigrateDirectoryToDB(fullPath, name, eventDate, eventType);
                    if (!migrateSuccess) throw new ElectionMigrationException(
                         "Failed to migrate " + fullPath);
                }                
            });
            return true;
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

        protected string ElectionNameFromParts(DateTime eventDate, string subdirName)
        {
            return String.Format("{0} {1} group", eventDate.ToString("MMMM yyyy"), subdirName.ToLower());
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
