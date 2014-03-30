using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gallery.Entities.Elections;
using Gallery.Entities.ImageGallery;

namespace Gallery.Migration
{
    public class ElectionMigrationHelper
    {
        public IImageGallery Gallery { get; protected set; }

        public ElectionMigrationHelper(IImageGallery gallery )
        {
            Gallery = gallery;
        }

        public bool MigrateDirectoryToDB(string dirPath, string electionName, DateTime eventDate, ElectionType eventType)
        {
            var election = Election.FromDirectory(dirPath, electionName, eventDate, eventType, Gallery);
            return false;
        }
    }
}
