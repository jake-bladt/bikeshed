using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Gallery.Entities.Subjects;

namespace Gallery.Entities.Candidates
{
    public class SuperannuatedRookieChooser : ICandidateChooser
    {
        protected string ConnectionString;

        public SuperannuatedRookieChooser(string cnStr)
        {
            ConnectionString = cnStr;
        }

        public string Name { get; set; }

        public List<ISubject> GetCandidates()
        {
            throw new NotImplementedException();
        }
    }
}
