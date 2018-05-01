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

        public int SetSubjectCategory(string subjectName, string categoryName)
        {
            var cn = new SqlConnection(_ConnectionString);
            var cmd = new SqlCommand("setSubjectCategory", cn) { CommandType = CommandType.StoredProcedure };
            var ret = new List<String>();

            try
            {
                cn.Open();
                cmd.Parameters.AddWithValue("@subjectName", subjectName);
                cmd.Parameters.AddWithValue("@categoryName", categoryName);

                var relId = new SqlParameter();
                relId.ParameterName = "@relatonshipId";
                relId.SqlDbType = SqlDbType.Int;
                relId.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(relId);

                cmd.ExecuteNonQuery();
                return (int)relId.Value;

            }
            finally
            {
                cn.Close();
            }
        }
    }
}
