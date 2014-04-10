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
        public static SetList FromElections(IEnumerable<Election> elections, string name = "")
        {
            if (String.IsNullOrEmpty(name)) name = elections.ToList().First(e => !String.IsNullOrEmpty(e.Name)).Name;
            var ret = new SetList(name);
            elections.ToList().ForEach(el =>
            {
                el.Winners.Values.ToList().ForEach(winner => {
                    if (!ret.ContainsKey(winner.Name)) ret[winner.Name] = winner;
                });
            });
            return ret;
        }

        public int Id { get; protected set; }
        public string Name { get; protected set; }

        public SetList(string name)
        {
            Name = name;
            Id = -1;
        }

    }

}
