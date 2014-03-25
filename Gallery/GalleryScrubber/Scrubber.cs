using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using ImageGallery;

namespace GalleryScrubber
{
    public class Scrubber
    {
        protected ImageGallery.FileSystemImageGallery _gallery;
        public Scrubber(ImageGallery.FileSystemImageGallery gallery) 
        { 
            _gallery = gallery;  
        }

        public List<String> Scrub()
        {
            var ret = new List<String>();
            _gallery.Subjects.Values.ToList().ForEach(subj =>
            {
                int count = ScrubSubjectDirectory(subj);
                if (count > 0) ret.Add(String.Format("{0}: {1} file(s) renamed.", subj.Name, count));
            });
            return ret;
        }

        public string ScrubSubject(string subjectName)
        {
            string ret;
            try
            {
                var subject = _gallery.Subject(subjectName);
                int count = ScrubSubjectDirectory(subject);
                return String.Format("{0} file(s) renamed.", count);
            }
            catch(Exception ex)
            {
                ret = String.Format("Failed to scrub: {0}", ex.ToString());
            }

            return ret;
        }

        protected int ScrubSubjectDirectory(Subject subject)
        {
            var correctFileNames = GetCorrectFileNames(subject);
            var incorrectFileNames = new List<String>();
            subject.Files.Keys.ToList().ForEach(path =>
            {
                if (correctFileNames.Contains(path))
                {
                    correctFileNames.Remove(path);
                }
                else
                {
                    incorrectFileNames.Add(path);
                }
            });
            return RenameFiles(incorrectFileNames, correctFileNames);
        }

        protected List<String> GetCorrectFileNames(Subject subject)
        {
            var ret = new List<String>();
            int fileCount = subject.Files.Count;
            for (int i = 1; i <= fileCount; i++)
            {
                ret.Add(CorrectFileName(subject, i, fileCount >= 1000));
            }
            return ret;
        }

        protected string CorrectFileName(Subject subject, int ordinal, bool useSubdirectories)
        {
            string ret;
            if (useSubdirectories)
            {
                ret = String.Format(@"{0}\Vol{1:D3}\{2}(3:D3).jpg", subject.DirectoryPath, (ordinal / 1000) + 1, subject.Name, ordinal);
            }
            else
            {
                ret = String.Format(@"{0}\{1}{2:D3}.jpg", subject.DirectoryPath, subject.Name, ordinal);
            }
            return ret;
        }

        protected int RenameFiles(List<String> badNames, List<String> goodNames)
        {
            if(badNames.Count != goodNames.Count) 
            {
                throw new ArgumentException(String.Format("Can not rename {0} file(s) to (1) file names.", badNames.Count, goodNames.Count));
            }

            var ret = badNames.Count;

            var badArr = badNames.ToArray();
            var goodArr = goodNames.ToArray();

            for (int i = 0; i < badArr.Length; i++)
            {
                File.Move(badArr[i], goodArr[i]);
            }
            return ret;
        }
    }
}
