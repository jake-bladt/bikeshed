using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Entities.Subjects
{
    public class SqlBackedSubject : ISubject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int ImageCount { get; set; }

        public SqlBackedSubject() { }

        public SqlBackedSubject(ISubject source, int id = -1)
        {
            ID = (id == -1 ? source.ID : id);
            Name = source.Name;
            DisplayName = source.DisplayName;
            ImageCount = source.ImageCount;
        }
    }
}
