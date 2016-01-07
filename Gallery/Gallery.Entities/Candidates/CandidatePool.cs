using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gallery.Entities.ImageGallery;
using Gallery.Entities.Subjects;

namespace Gallery.Entities.Candidates
{
    public class CandidatePool : ICandidatePool
    {
        public static CandidatePool FromGallery(IImageGallery gallery)
        {
            var ret = new CandidatePool();
            gallery.Subjects.Values.ToList().ForEach(subj =>
            {
                ret.Add(subj);
            });
            return ret;
        }

        public static CandidatePool FromFileSystemDirectory(DirectoryInfo rootInfo)
        {
            var candidateImages = rootInfo.GetFiles("*.jpg");
            var ret = new CandidatePool();
            var id = 0;
            candidateImages.ToList().ForEach(img =>
            {
                var candy = new FsoBackedSubject(img, ++id);
                ret.Add(candy);
            });
            return ret;
        }

        protected Dictionary<string, ISubject> _Subjects;
        protected bool _ListIsDirty = false;
        protected List<ISubject> _Candidates;

        public CandidatePool()
        {
            _Subjects = new Dictionary<string, ISubject>();
        }

        public ISubject HasSubject(string name)
        {
            if (_Subjects.ContainsKey(name))
            {
                return _Subjects[name];
            }
            return null;
        }

        public bool Remove(ISubject subject)
        {
            if (_Subjects.ContainsKey(subject.Name))
            {
                _Subjects.Remove(subject.Name);
                _ListIsDirty = true;
                return true;
            }
            return false;
        }

        public bool Add(ISubject subject)
        {
            if (_Subjects.ContainsKey(subject.Name))
            {
                return false;
            }
            else
            {
                _Subjects[subject.Name] = subject;
                _ListIsDirty = true;
                return true;
            }
        }

        public List<ISubject> Candidates
        {
            get
            {
                if (_ListIsDirty || null == _Candidates)
                {
                    _Candidates = new List<ISubject>();
                    _Subjects.Values.ToList().ForEach(subj => _Candidates.Add(subj));
                    _ListIsDirty = false;
                }
                return _Candidates;
            }
        }
    }
}
