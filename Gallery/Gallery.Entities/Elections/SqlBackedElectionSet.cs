using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Gallery.Entities.Elections
{
    public class SqlBackedElectionSet
    {
        public string ConnectionString { get; protected set; }

        public SqlBackedElectionSet(String cn)
        {
            ConnectionString = cn;
        }

        public bool Store(Election election)
        {
            var cn = new SqlConnection(ConnectionString);
            cn.Open();

            if (ElectionExists(election))
            {

            }
            else
            {
                
            }

            election.Winners.ToList().ForEach(kvp =>
            {

            });

            return true;
        }

        public bool ElectionExists(Election election)
        {
            var cn = new SqlConnection(ConnectionString);
            cn.Open();
            var cmd = new SqlCommand("getElection", cn) { CommandType = CommandType.StoredProcedure };
            var rdr = cmd.ExecuteReader();
            return rdr.Read();
        }
    }
}
