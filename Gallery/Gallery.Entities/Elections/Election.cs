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
    public class Election : IElection, IElectionListing
    {
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
