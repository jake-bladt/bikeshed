using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gallery.Entities.Elections;
using Gallery.Entities.Subjects;

namespace Gallery.Entities.SetLists
{
    public class SetList : Dictionary<String, ISubject>
    {
        public static SetList FromElections(IEnumerable<Election> elections)
        {
            var ret = new SetList();
            elections.ToList().ForEach(el =>
            {
                el.Winners.Values.ToList().ForEach(winner =>
                {
                    if (!ret.ContainsKey(winner.Name)) ret[winner.Name] = winner;
                });
            });
            return ret;
        }

    }

}
