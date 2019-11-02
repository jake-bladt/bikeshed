using System;
using System.Collections.Generic;
using System.Data.Sql;

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
            throw new NotImplementedException();
        }
    }
}
