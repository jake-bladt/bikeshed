using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallery
{
    public class SqlTrackedImageGallery : IImageGallery
    {
        protected Dictionary<string, ISubject> _subjects = null;
        protected string _connStr;

        public SqlTrackedImageGallery(string cn)
        {
            _connStr = cn;
        }

        public ISubject Subject(string name)
        {
            return (Subjects.ContainsKey(name) ? Subjects[name] : null);
        }

        public Dictionary<string, ISubject> Subjects
        {
            get 
            {
                if (null == _subjects)
                {

                }
                return _subjects;
            }
        }
    }
}
