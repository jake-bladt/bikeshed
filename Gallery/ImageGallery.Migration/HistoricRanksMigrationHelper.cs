using System;
using System.Data;
using System.Data.SqlClient;

namespace Gallery.Migration
{
    public class HistoricRanksMigrationHelper
    {
        public class Result
        {
            public bool Success { get; set; }
            public int EntryCount { get; set; }
            public Exception ExceptionReported { get; set; }
        }

        protected string _connectionString;

        public HistoricRanksMigrationHelper(string cn)
        {
            _connectionString = cn;

        }

        public Result UpdateHistoricRanks()
        {
            SqlConnection cn = null;
            try
            {
                cn = new SqlConnection(_connectionString);
                cn.Open();
                var cmd = new SqlCommand("setRanksAtDates", cn) { CommandType = CommandType.StoredProcedure };
                var ret = cmd.ExecuteScalar();
                return new Result { EntryCount = (int)ret, Success = true };
            }
            catch(Exception ex)
            {
                return new Result { EntryCount = -1, ExceptionReported = ex, Success = false };
            }
            finally
            {
                cn.Close();
            }

        }



    }
}
