using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gallery.Entities.Subjects;

namespace Gallery.Entities.SetLists
{
    public class SqlBackedSetlist : SetList
    {
        public string ConnectionString { get; protected set; }

        public static SqlBackedSetlist CloneFrom(SetList source, string cn = "") 
        {
            var ret = new SqlBackedSetlist(cn, source.Name);
            source.ToList().ForEach(kvp => ret[kvp.Key] = kvp.Value);
            return ret;
        }

        public SqlBackedSetlist(string cn, string name) : base(name)
        {
            ConnectionString = cn;
        }

        public bool Store()
        {
            return false;
        }

        public bool Fetch()
        {
            return false;
        }
    }
}
