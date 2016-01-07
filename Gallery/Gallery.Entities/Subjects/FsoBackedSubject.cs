using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Entities.Subjects
{
    public class FsoBackedSubject : ISubject
    {
        public FsoBackedSubject(FileInfo subjectInfo, int id)
        {
            ID = id;
            Name = subjectInfo.Name;
            DisplayName = subjectInfo.Name;
            ImageCount = 1;
            FullPath = subjectInfo.FullName;
        }

        public int ID { get; protected set; }
        public string Name { get; protected set; }
        public string DisplayName { get; protected set; }
        public int ImageCount { get; protected set; }
        public string FullPath { get; protected set; }
    }
}
