using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Entities.Candidates
{
    public class FileSystemContestWriter : IContestWriter
    {
        protected string _root;
        protected string _poolPath;

        public FileSystemContestWriter(String root, String poolPath) 
        {
            _root = root;
            _poolPath = poolPath;
        }

        public bool WriteContests(Dictionary<string,List<Subjects.ISubject>> contests)
        {
            contests.ToList().ForEach(kvp =>
            {
                var name = kvp.Key;
                var contestPath = Path.Combine(_root, name);
                if (!Directory.Exists(contestPath)) Directory.CreateDirectory(contestPath);
                kvp.Value.ForEach(subj =>
                {
                    var sourcePath = Path.Combine(_poolPath, subj.Name + ".jpg");
                    var targetPath = Path.Combine(contestPath, subj.Name + ".jpg");
                    if (File.Exists(sourcePath) && !File.Exists(targetPath))
                    {
                        File.Copy(sourcePath, targetPath);
                    }
                });
            });

            return true;
        }
    }
}
