using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Gallery.Entities.Taxonomy
{
    public class SubjectCategoryRepository
    {
        protected string _ConnectionString;

        public SubjectCategoryRepository(string cnStr)
        {
            _ConnectionString = cnStr;
        }

        public List<String> GetSubjectCategories(string subjectName)
        {
            var cn = new SqlConnection(_ConnectionString);
            var cmd = new SqlCommand("getSubjectCategories", cn) { CommandType = CommandType.StoredProcedure };
            var ret = new List<String>();

            try
            {
                cn.Open();
                cmd.Parameters.AddWithValue("@name", subjectName);
                var rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    ret.Add(rdr["CategoryName"].ToString());
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
