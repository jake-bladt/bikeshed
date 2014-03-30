using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

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
                id = election.Id;
            }

            election.Winners.ToList().ForEach(kvp =>
            {
                if (!AddElectionWinner(election.Id, kvp.Value.ID, kvp.Key, (election.WinnerCount - kvp.Key + 1)))
                {
                    if (!CreateElection(election)) throw new ApplicationException("Failed to save election winner.");
                }               
            });

            return true;
        }

        public bool ElectionExists(Election election, out int id)
        {
            var cn = new SqlConnection(ConnectionString);
            cn.Open();
            var cmd = new SqlCommand("getElection", cn) { CommandType = CommandType.StoredProcedure };
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
            cmd.Parameters.Add(new SqlParameter("id", id));
            cmd.ExecuteNonQuery();
            return true;
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
            return true;
        }
    }
}
