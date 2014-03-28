using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
                    _subjects = new Dictionary<string, ISubject>();
                    var cn = new SqlConnection(_connStr);
                    cn.Open();
                    var cmd = new SqlCommand("getAllSubjects", cn) { CommandType = CommandType.StoredProcedure };
                    var rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var subj = new SqlTrackedSubject
                        {
                            Name = rdr["Name"].ToString(),
                            DisplayName = rdr["DisplayName"].ToString(),
                            ID = (int)rdr["Id"],
                            ImageCount = (int)rdr["ImageCount"]
                        };
                        _subjects[subj.Name] = subj;
                    }

                }
                return _subjects;
            }
        }

        public bool Add(ISubject subject)
        {
            if (Subjects.ContainsKey(subject.Name))
            {
                var id = Subjects[subject.Name].ID;

            }
            else
            {

            }
        }

    }
}
