﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
                    var arr = di.GetFiles("*.jpeg", SearchOption.AllDirectories);
                    arr.ToList().ForEach(fi => _files[fi.FullName] = fi);
                }
                return _files;
            }
        }
    }
}
