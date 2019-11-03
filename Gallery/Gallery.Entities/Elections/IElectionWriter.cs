using System.Collections.Generic;

namespace Gallery.Entities.Elections
{
    public interface IElectionWriter
    {
        int Upsert(SingleElectionResult exemplar);
        bool AddWiners(int electionId, List<SingleElectionResult> winners);
    }
}
