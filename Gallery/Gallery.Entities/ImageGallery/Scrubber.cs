using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using GroupDocs.Conversion;
using GroupDocs.Conversion.FileTypes;
using GroupDocs.Conversion.Options.Convert;

using Gallery.Entities.Subjects;

namespace Gallery.Entities.ImageGallery
{
    public class Scrubber
    {
        protected FileSystemImageGallery _gallery;
        protected ISubjectWriter _writer = null;

        public Scrubber(FileSystemImageGallery gallery, ISubjectWriter writer = null)
        {
            _gallery = gallery;
            _writer = writer;
        }

        public List<String> Scrub(Action<String> messageCallback = null)
        {
            if (null == messageCallback) messageCallback = Scrubber.WriteAsTrace;

            var ret = new List<String>();
            _gallery.Subjects.Values.ToList().ForEach(subj =>
            {
                int count = ScrubSubjectDirectory((FileBackedSubject)subj);
                if (count > 0) ret.Add(String.Format("{0}: {1} file(s) renamed.", subj.Name, count));
            });
            return ret;
        }

        public string ScrubSubject(string subjectName, Action<String> messageCallback = null)
        {
            if (null == messageCallback) messageCallback = Scrubber.WriteAsTrace;

            string ret;
            try
            {
                var subject = _gallery.Subject(subjectName);
                int count = ScrubSubjectDirectory((FileBackedSubject)subject, messageCallback);
                return String.Format("{0} file(s) renamed.", count);
            }
            catch (Exception ex)
            {
                ret = String.Format("Failed to scrub: {0}", ex.ToString());
            }

            return ret;
        }

        protected int ScrubSubjectDirectory(FileBackedSubject subject, Action<String> messageCallback = null)
        {
            if (null == messageCallback) messageCallback = Scrubber.WriteAsTrace;

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
            int ret = RenameFiles(incorrectFileNames, correctFileNames, messageCallback);
            if (ret > 0 && null != _writer)
            {
                _writer.UpdateImageCount(subject.Name, subject.Files.Count());
            }
            return ret;
        }

        protected List<String> GetCorrectFileNames(FileBackedSubject subject)
        {
            var ret = new List<String>();
            int fileCount = subject.Files.Count;
            for (int i = 1; i <= fileCount; i++)
            {
                ret.Add(CorrectFileName(subject, i, fileCount >= 1000));
            }
            return ret;
        }

        protected string CorrectFileName(FileBackedSubject subject, int ordinal, bool useSubdirectories)
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

        public static void WriteAsTrace(string msg)
        {
            Trace.WriteLine(msg);
        }

        protected int RenameFiles(List<String> badNames, List<String> goodNames, Action<String> messageCallback = null)
        {
            if (badNames.Count != goodNames.Count)
            {
                throw new ArgumentException(String.Format("Can not rename {0} file(s) to (1) file names.", badNames.Count, goodNames.Count));
            }

            if (null == messageCallback) messageCallback = Scrubber.WriteAsTrace;

            var ret = badNames.Count;

            var badArr = badNames.ToArray();
            var goodArr = goodNames.ToArray();

            for (int i = 0; i < badArr.Length; i++)
            {
                var  srcPath = badArr[i];
                if(badArr[i].EndsWith(".webp"))
                {
                    using(var converter = new Converter(badArr[i]))
                    {
                        var options = new ImageConvertOptions { Format = ImageFileType.Jpg };
                        var nameBase = Guid.NewGuid().ToString();
                        var newName = $"{nameBase}.jpg";
                        var fi = new FileInfo(badArr[i]);
                        srcPath = badArr[i].Replace(fi.Name, newName);
                        messageCallback.Invoke($"Temporarily converting {fi.Name} -> {newName}.");
                        converter.Convert(srcPath, options);
                        fi.Delete();
                    }
                }

                File.Move(srcPath, goodArr[i]);
            }

            return ret;
        }
    }
}
