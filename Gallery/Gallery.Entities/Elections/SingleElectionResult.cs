﻿using System;

namespace Gallery.Entities.Elections
{
    public class SingleElectionResult
    {
        public string ElectionName { get; set; }
        public int ElectionId { get; set; }
        public string SubjectName { get; set; }
        public int SubjectId { get; set; }
        public int OrdinalRank { get; set; }
        public int PointValue { get; set; }
        public DateTime EventDate { get; set; }
        public ElectionType EventType { get; set; }
    }
}
