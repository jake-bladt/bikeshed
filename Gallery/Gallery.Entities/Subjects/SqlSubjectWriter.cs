using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Gallery.Entities.Subjects
{
    public class SqlSubjectWriter : ISubjectWriter
    {
        public string ConnectionString { get; protected set; }

        public SqlSubjectWriter(string cnStr)
        {
            ConnectionString = cnStr;
        }

        public bool UpdateImageCount(string name, int imageCount)
        {
            var cn = new SqlConnection(ConnectionString);
            cn.Open();
            var cmd = new SqlCommand("updateSubject", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("name", name);
            cmd.Parameters.Add(new SqlParameter("imageCount", imageCount));
            cmd.ExecuteNonQuery();
            return true;
        }
    }
}
