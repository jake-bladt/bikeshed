using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Gallery.Entities.Subjects;

namespace Gallery.Entities.Candidates
{
    public class SetCandidateChooser : ICandidateChooser
    {
        protected string ConnectionString;
        protected string TargetSetName;

        public SetCandidateChooser(string setName, string cnStr)
        {
            TargetSetName = setName;
            ConnectionString = cnStr;
        }

        public string Name { get; protected set; }

        public List<ISubject> GetCandidates()
        {
            throw new NotImplementedException();
        }
    }
}
