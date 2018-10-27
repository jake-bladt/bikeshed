using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Gallery.Entities.Subjects;

namespace Gallery.Entities.Candidates
{
    public class RiderCategoryChooser : ICandidateChooser
    {
        protected string ConnectionString;
        protected string Determinant;

        public string Name { get; set; }

        public RiderCategoryChooser(String determinant, String cnStr)
        {
            Determinant = determinant;
            ConnectionString = cnStr;
        }

        public List<ISubject> GetCandidates()
        {
            var ret = new List<ISubject>();
            SqlCommand cmd;

            var cn = new SqlConnection(ConnectionString);
            if (Determinant.StartsWith("sp:"))
            {
                var determinantParts = Determinant.Split(':');
                var spName = determinantParts[1];
                cmd = new SqlCommand(spName, cn) { CommandType = CommandType.StoredProcedure };
            }
            else
            {
                cmd = new SqlCommand("getSubjectsByCategory", cn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@name", Determinant);
            }


            try
            {
                cn.Open();
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
