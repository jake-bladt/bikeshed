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
        public static SqlBackedSetlist CloneFrom(SetList source) 
        {
            var ret = new SqlBackedSetlist() { Name = source.Name };
            source.ToList().ForEach(kvp => ret[kvp.Key] = kvp.Value);
            return ret;
        }

    }
}
