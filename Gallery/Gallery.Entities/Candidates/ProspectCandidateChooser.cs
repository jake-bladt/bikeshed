using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Gallery.Entities.Subjects;

namespace Gallery.Entities.Candidates
{
    public class ProspectCandidateChooser : ICandidateChooser
    {
        public string Name { get; set; }
        protected string ConnectionString { get; set; }

        public ProspectCandidateChooser(string cn)
        {
            ConnectionString = cn;
        }

        public List<ISubject> GetCandidates()
        {
            var ret = new List<ISubject>();

            var cn = new SqlConnection(ConnectionString);
            cn.Open();

            var cmd = new SqlCommand("getRookies", cn) { CommandType = CommandType.StoredProcedure };
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var subject = new SqlBackedSubject
                {
                    ID = Convert.ToInt32(rdr["Id"]),
                    Name = rdr["Name"].ToString(),
                    DisplayName = rdr["DisplayName"].ToString(),
                    ImageCount = Convert.ToInt32(rdr["ImageCount"])
                };
                ret.Add(subject);
            }

            return ret;
        }
    }
}
