using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Entities.Elections
{
    public class SqlElectionReader : IElectionReader
    {
        protected string _ConnectionString;

        public SqlElectionReader(string strCn)
        {
            _ConnectionString = strCn;
        }

        public List<IElection> GetAllElections()
        {
            var ret = new List<IElection>();
            var cn = new SqlConnection(_ConnectionString);
            cn.Open();
            var cmd = new SqlCommand("getElections", cn) { CommandType = CommandType.StoredProcedure };
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ret.Add(ElectionFromRow(rdr));
            }
            return ret;
        }

        public IElection GetElection(int id)
        {
            Election ret = null;
            var cn = new SqlConnection(_ConnectionString);
            cn.Open();
            var cmd = new SqlCommand("getElection", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("id", id));
            var rdr = cmd.ExecuteReader();

            if (rdr.Read())
            {
                ret = new Election
                {
                    Id = Convert.ToInt32(rdr["Id"]),
                    EventDate = Convert.ToDateTime(rdr["EventDate"]),
                    Name = rdr["ElectionName"].ToString(),
                    WinnerCount = Convert.ToInt32(rdr["WinnerCount"]),
                    EventType = (ElectionType)(Convert.ToInt32(rdr["ElectionTypeId"])),
                    Winners = new Dictionary<int,Subjects.ISubject>()
                };
            }
            return ret;
        }

        protected IElection ElectionFromRow(SqlDataReader rdr)
        {
            var ret = new Election
            {
                Id = Convert.ToInt32(rdr["ElectionId"]),
                EventDate = Convert.ToDateTime(rdr["EventDate"]),
                Name = rdr["Name"].ToString(),
                WinnerCount = Convert.ToInt32(rdr["WinnerCount"]),
                EventType =  (ElectionType)(Convert.ToInt32(rdr["ElectionTypeId"]))
            };
            return ret;
        }
    }
}
