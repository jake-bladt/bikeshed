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
            var ret = new List<ISubject>();

            var cn = new SqlConnection(ConnectionString);
            try
            {
                cn.Open();
                var cmd = new SqlCommand("getSuperannuatedRookies", cn) { CommandType = CommandType.StoredProcedure };
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
            }
            finally
            {
                cn.Close();
            }
            return ret;
        }
    }
}
