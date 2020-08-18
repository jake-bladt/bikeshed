using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Gallery.Entities.Utilities;

namespace Gallery.Entities.Subjects
{
    public class FileBackedSubject : ISubject
    {
        public static readonly int UninitializedID = -42;

        public string DirectoryPath { get; protected set; }
        public string Name { get; protected set; }
        public int ID { get; set; }
        protected string _displayName = String.Empty;

        public string DisplayName
        {
            get
            {
                if (String.IsNullOrEmpty(_displayName)) _displayName = NameMapper.DirectoryNameToDisplayName(Name);
                return _displayName;
            }
        }

        public int ImageCount
        {
            get
            {
                return Files.Keys.Count;
            }
        }

        protected Dictionary<String, FileInfo> _files;

        public FileBackedSubject(string path, string name)
        {
            DirectoryPath = path;
            Name = name;
        }

        public Dictionary<String, FileInfo> Files
        {
            get
            {
                if (null == _files)
                {
                    _files = new Dictionary<string, FileInfo>();
                    var di = new DirectoryInfo(DirectoryPath);
                    var ar1 = di.GetFiles("*.jp*g", SearchOption.AllDirectories);
                    var ar2 = di.GetFiles("*.webp", SearchOption.AllDirectories);
                    var arr = new FileInfo[ar1.Length + ar2.Length];
                    if (ar1.Length > 0) Array.Copy(ar1, arr, ar1.Length);
                    if (ar2.Length > 0) Array.Copy(ar2, 0, arr, ar1.Length, ar2.Length);

                    arr.ToList().ForEach(fi => _files[fi.FullName] = fi);
                }
                return _files;
            }
        }
    }
}
