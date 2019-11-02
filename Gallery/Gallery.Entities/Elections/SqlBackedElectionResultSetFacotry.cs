using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Gallery.Entities.Elections
{
    public class SqlBackedElectionResultSetFacotry : IElectionResultSetFactory
    {
        protected readonly string _connectionString;

        public SqlBackedElectionResultSetFacotry(string cn)
        {
            _connectionString = cn;
        }

        public ElectionResultSet GetResultSet()
        {

            var ret = new ElectionResultSet();
            using(var cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                var cmd = new SqlCommand("getAllElectionWinners", cn) { CommandType = CommandType.StoredProcedure };
                var rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    var singleResult = new SingleElectionResult
                    {
                        ElectionId = (int)rdr["ElectionId"],
                        ElectionName = rdr["ElectionName"].ToString(),
                        SubjectId = (int)rdr["SubjectId"],
                        SubjectName = rdr["SubjectName"].ToString(),
                        OrdinalRank = (int)rdr["OrdinalRank"],
                        PointValue = (int)rdr["PointValue"]
                    };
                    ret.Add(singleResult);
                }
            }
            return ret;
        }
    }
}
