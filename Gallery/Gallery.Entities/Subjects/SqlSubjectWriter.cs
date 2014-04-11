using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Gallery.Entities.Subjects
{
    public class SqlSubjectWriter : ISubjectWriter
    {
        public string ConnectionString { get; protected set; }

        public SqlSubjectWriter(string cnStr)
        {
            ConnectionString = cnStr;
        }

        public bool UpdateImageCount(string name, int imageCount)
        {
            throw new NotImplementedException();
        }
    }
}
