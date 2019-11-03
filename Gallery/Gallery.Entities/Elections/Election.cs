using System;
using System.Collections.Generic;

using Gallery.Entities.Subjects;

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
