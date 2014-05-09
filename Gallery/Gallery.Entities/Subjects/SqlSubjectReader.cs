using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Entities.Subjects
{
    public class SqlSubjectReader : ISubjectReader
    {
        protected string _ConnStr;

        public SqlSubjectReader(string cn)
        {
            _ConnStr = cn;
        }

        public List<ISubject> GetAllSubjects()
        {
            var ret = new List<ISubject>();
            var cn = new SqlConnection(_ConnStr);
            cn.Open();
            var cmd = new SqlCommand("getSubjects", cn) { CommandType = CommandType.StoredProcedure };
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ret.Add(SubjectFromRow(rdr));
            }
            return ret;
        }

        protected ISubject SubjectFromRow(SqlDataReader rdr)
        {
            return new SqlBackedSubject
            {
                ID = Convert.ToInt32(rdr["Id"]),
                Name = rdr["Name"].ToString(),
                DisplayName = rdr["DisplayName"].ToString(),
                ImageCount = Convert.ToInt32(rdr["ImageCount"])
            };
        }

        public ISubject GetSubject(string Name)
        {
            ISubject ret = null;
            var cn = new SqlConnection(_ConnStr);
            cn.Open();
            var cmd = new SqlCommand("getSubject", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("name", Name));
            var rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                ret = SubjectFromRow(rdr);
            }
            return ret;
        }
    }
}
