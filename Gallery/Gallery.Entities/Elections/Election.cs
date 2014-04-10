using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Gallery.Entities.ImageGallery;
using Gallery.Entities.Subjects;
using Gallery.Entities.Utilities;

namespace Gallery.Entities.Elections
{
    public class Election : IElection
    {
        public static Election FromDirectory(
            string dirPath,
            string name,
            DateTime date,
            ElectionType eventType, 
            IImageGallery gallery)
        {

            if (!Directory.Exists(dirPath)) throw new ArgumentException(String.Format("The directory {0} does not exist.", dirPath));

            var di = new DirectoryInfo(dirPath);
            var fis = di.GetFiles("*.jpg");
            var winnerCount = fis.Count();
            var winners = new Dictionary<int, ISubject>();

            fis.ToList().ForEach(fi =>
            {
                var line = fi.Name;
                var entries = NameMapper.ElectionFileNameToElectionEntry(line);
                int pos = entries.Item1;
                string winnerName = entries.Item2;

                if (winners.ContainsKey(pos)) throw new DuplicateElectionEntryException(String.Format("Multiple entries at position {0}", pos));
                if (!gallery.Subjects.ContainsKey(winnerName)) throw new UnknownSubjectException(String.Format("Unknown subject: {0}", winnerName));
                var subject = gallery.Subjects[winnerName];
                if (winners.ContainsValue(subject)) throw new DuplicateSubjectException(String.Format("Duplicate subject: {0}", winnerName));
                winners[pos] = gallery.Subjects[winnerName];
            });

            for (int i = 1; i <= winnerCount; i++)
            {
                if (!winners.ContainsKey(i)) throw new EmptyElectionSlotException(String.Format("Slot {0} is empty", i));
            }

            
            var ret = new Election(name, date) { EventType = eventType, WinnerCount = winnerCount, Winners = winners };
            return ret;
        }

        public string Name { get; set; }
        public int Id { get; set; }
        public ElectionType EventType { get; set; }
        public int WinnerCount { get; set; }
        public DateTime EventDate { get; set; }
        public Dictionary<int, ISubject> Winners { get; set; }

        public Election(string name, DateTime eventDate)
        {
            Name = name;
            EventDate = eventDate;
            InitializeWinners();
        }

        public Election()
        {
            InitializeWinners();
        }

        protected void InitializeWinners()
        {
            Winners = new Dictionary<int, ISubject>();
        }
    }
}
