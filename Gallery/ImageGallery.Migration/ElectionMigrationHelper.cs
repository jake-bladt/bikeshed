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
            

            return false;
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
    }
}
