﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gallery.Entities.Subjects;

namespace Gallery.Entities.ImageGallery
{
    public class SubjectPool : ISubjectPool
    {
        public static SubjectPool FromGallery(IImageGallery gallery)
        {
            var ret = new SubjectPool();
            gallery.Subjects.Values.ToList().ForEach(subj =>
            {
                ret.Add(subj);
            });
            return ret;
        }

        protected Dictionary<string, ISubject> _Subjects;

        public SubjectPool()
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
                return true;
            }
        }
    }
}