using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gallery.Entities.Subjects;

namespace Gallery.Entities.Elections
{
    public enum ElectionType
    {
        Travel = 1
      , Rookie = 2
      , Star = 3
      , WalkIn = 4
      , Wonder = 5
      , RunOff = 6
      , Special = 7
    }

    public interface IElection
    {
        int Id { get; set; }
        string Name { get; set; }
        ElectionType EventType { get; set; }
        int WinnerCount { get; set; }
        DateTime EventDate { get; set; }
        Dictionary<int, ISubject> Winners { get; set; }
    }
}
