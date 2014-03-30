using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gallery.Entities.Elections
{
    public class Election : IElection
    {
        public static Election FromDirectory(
            string dirPath,
            string name,
            DateTime date,
            ElectionType eventType)
        {

            if (!Directory.Exists(dirPath)) throw new ArgumentException(String.Format("The directory {0} does not exist.", dirPath));

            var di = new DirectoryInfo(dirPath);
            var fis = di.GetFiles("*.jpg");
            var winnerCount = fis.Count();



            var ret = new Election(name, date) { EventType = eventType, WinnerCount = winnerCount };
            return ret;
        }

        public string Name { get; set; }
        public ElectionType EventType { get; set; }
        public int WinnerCount { get; set; }
        public DateTime EventDate { get; set; }

        public Election(string name, DateTime eventDate)
        {
            Name = name;
            EventDate = eventDate;
        }

    }
}
