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

        public List<IElectionListing> GetElectionListing(int count)
        {
            var ret = new List<IElectionListing>();

            return ret;
        }

        public IElection GetElection(int id)
        {
            return SqlBackedElection.FromId(id, _ConnectionString);
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
