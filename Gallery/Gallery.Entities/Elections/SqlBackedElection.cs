using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gallery.Entities.Subjects;

namespace Gallery.Entities.Elections
{
    public class SqlBackedElection : Election
    {
        public static SqlBackedElection FromId(int id, string cnStr)
        {
            SqlBackedElection ret = null;
            var cn = new SqlConnection(cnStr);
            cn.Open();
            var cmd = GetElectionRetrievalCommand(cn);
            cmd.Parameters.Add(new SqlParameter("id", id));
            var rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                ret = FromReader(rdr, cnStr);
                LoadWinners(ret, cnStr);
            }
            return ret;
        }

        public static SqlBackedElection FromName(string name, string cnStr)
        {
            var ret = new SqlBackedElection(cnStr);
            var cn = new SqlConnection(cnStr);
            cn.Open();
            var cmd = GetElectionRetrievalCommand(cn);
            cmd.Parameters.Add(new SqlParameter("name", name));
            var rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                ret = FromReader(rdr, cnStr);
                LoadWinners(ret, cnStr);
            }
            return ret;
        }

        protected static SqlCommand GetElectionRetrievalCommand(SqlConnection cn)
        {
            return new SqlCommand("getElection", cn) { CommandType = CommandType.StoredProcedure };
        }

        protected static SqlBackedElection FromReader(SqlDataReader rdr, string cnStr)
        {
            return new SqlBackedElection(cnStr)
            {
                Id = Convert.ToInt32(rdr["Id"]),
                Name = rdr["ElectionName"].ToString(),
                WinnerCount = Convert.ToInt32(rdr["WinnerCount"]),
                EventType = (ElectionType)rdr["ElectionTypeId"],
                EventDate = Convert.ToDateTime(rdr["EventDate"])
            };
        }

        protected static bool LoadWinners(IElection election, string cnStr)
        {
            var cn = new SqlConnection(cnStr);
            cn.Open();
            var cmd = new SqlCommand("getElectionWinners", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("electionId", election.Id));
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var winner = WinnerFromReader(rdr);
                var position = Convert.ToInt32(rdr["OrdinalRank"]);
                election.Winners[position] = winner;
            }

            return election.Winners.Count == election.WinnerCount;
        }

        protected static ISubject WinnerFromReader(SqlDataReader rdr)
        {
            return new SqlBackedSubject
            {
                ID = Convert.ToInt32(rdr["WinnerId"]),
                Name = rdr["WinnerName"].ToString(),
                DisplayName = rdr["DisplayName"].ToString(),
                ImageCount = Convert.ToInt32(rdr["ImageCount"])
            };
        }

        public string ConnectionString { get; protected set; }

        public SqlBackedElection(string cn) : base()
        {
            ConnectionString = cn;
        }

    }
}
