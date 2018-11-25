using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Gallery.Entities.Subjects;

namespace Gallery.Entities.Elections
{
    public class SqlBackedElectionSet : IElectionSet
    {
        public string ConnectionString { get; protected set; }

        public SqlBackedElectionSet(String cn)
        {
            ConnectionString = cn;
        }

        public bool Store(Election election)
        {
            int id;
            if (ElectionExists(election, out id))
            {
                election.Id = id;
                if (!UpdateElection(election)) throw new ApplicationException("Failed to save election.");
            }
            else
            {
                if (!CreateElection(election)) throw new ApplicationException("Failed to save election.");
            }
            return AddAllWinners(election);
        }

        public bool ElectionExists(Election election, out int id)
        {
            var cn = new SqlConnection(ConnectionString);
            cn.Open();
            var cmd = new SqlCommand("getElection", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue ("@name", election.Name);
            var rdr = cmd.ExecuteReader();
            if(rdr.Read())
            {
                id = (Int32)rdr["Id"];
                return true;
            }
            else
            {
                id = -1;
                return false;
            }
        }

        protected bool CreateElection(IElection election)
        {
            var cn = new SqlConnection(ConnectionString);
            cn.Open();
            var cmd = new SqlCommand("addElection", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("name", election.Name));
            cmd.Parameters.Add(new SqlParameter("date", election.EventDate));
            cmd.Parameters.Add(new SqlParameter("winnerCount", election.WinnerCount));
            cmd.Parameters.Add(new SqlParameter("typeId", (int)election.EventType));
            var id = cmd.ExecuteScalar();
            election.Id = Convert.ToInt32(id);
            return true;
        }

        protected bool UpdateElection(IElection election)
        {
            var cn = new SqlConnection(ConnectionString);
            cn.Open();
            var cmd = new SqlCommand("updateElection", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("id", election.Id));
            cmd.Parameters.Add(new SqlParameter("name", election.Name));
            cmd.Parameters.Add(new SqlParameter("date", election.EventDate));
            cmd.Parameters.Add(new SqlParameter("winnerCount", election.WinnerCount));
            cmd.Parameters.Add(new SqlParameter("typeId", (int)election.EventType));
            cmd.ExecuteNonQuery();
            return ClearElectionWinners(election.Id);
        }

        protected bool ClearElectionWinners(int id)
        {
            var cn = new SqlConnection(ConnectionString);
            cn.Open();
            var cmd = new SqlCommand("clearElectionWinners", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("electionId", id));
            cmd.ExecuteNonQuery();
            return true;
        }

        protected bool AddAllWinners(IElection election)
        {
            var ret = true;
            election.Winners.ToList().ForEach(kvp =>
            {
                int rank = kvp.Key;
                ISubject subject = kvp.Value;
                ret = ret && AddElectionWinner(election.Id, subject.ID, rank, election.WinnerCount - rank + 1);
            });
            return ret;
        }

        protected bool AddElectionWinner(int electionId, int winnerId, int rank, int points)
        {
            var cn = new SqlConnection(ConnectionString);
            cn.Open();
            var cmd = new SqlCommand("addElectionWinner", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("electionId", electionId));
            cmd.Parameters.Add(new SqlParameter("winnerId", winnerId));
            cmd.Parameters.Add(new SqlParameter("rank", rank));
            cmd.Parameters.Add(new SqlParameter("points", points));
            cmd.ExecuteNonQuery();
            cn.Close();
            return true;
        }
    }
}
