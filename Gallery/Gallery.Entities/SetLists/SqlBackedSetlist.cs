using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Gallery.Entities.Subjects;

namespace Gallery.Entities.SetLists
{
    public class SqlBackedSetlist : SetList
    {
        public string ConnectionString { get; protected set; }

        public static SqlBackedSetlist CloneFrom(SetList source, string cn) 
        {
            var ret = new SqlBackedSetlist(cn, source.Name);
            source.ToList().ForEach(kvp => ret[kvp.Key] = kvp.Value);
            return ret;
        }

        public static SqlBackedSetlist ByName(string name, string cnStr)
        {
            var ret = new SqlBackedSetlist(cnStr, name);
            ret.Fetch();
            return ret;
        }

        public static SqlBackedSetlist ById(int Id, string cnStr)
        {
            var ret = new SqlBackedSetlist(cnStr, String.Empty);
            ret.Fetch(Id);
            return ret;
        }

        public SqlBackedSetlist(string cn, string name) : base(name)
        {
            ConnectionString = cn;
            Name = name;
        }

        public bool Fetch(int? id = null)
        {
            var cn = new SqlConnection(ConnectionString);
            cn.Open();
            var cmd = new SqlCommand("getSubjectSetMembers", cn) { CommandType = CommandType.StoredProcedure };
            if (null == id)
            {
                cmd.Parameters.Add(new SqlParameter("setName", Name));
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("setId", id));
            }
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var subject = new SqlBackedSubject
                {
                    ID = (int)rdr["Id"],
                    Name = (string)rdr["Name"],
                    DisplayName = (string)rdr["DisplayName"],
                    ImageCount = (int)rdr["ImageCount"]
                };
                this[subject.Name] = subject;
            }
            return true;
        }

        public bool Store()
        {
            return StoreSet() && StoreMembers();
        }

        protected bool StoreSet()
        {
            var cn = new SqlConnection(ConnectionString);
            cn.Open();
            var cmd = new SqlCommand("addSubjectSet", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("name", Name));
            var ret = cmd.ExecuteScalar();
            Id = Convert.ToInt32(ret);
            return true;
        }

        protected bool StoreMembers()
        {
            var ret = true;
            Values.ToList().ForEach(subj => ret &= StoreMember(subj));
            return ret;
        }

        protected bool StoreMember(ISubject subject)
        {
            var cn = new SqlConnection(ConnectionString);
            cn.Open();
            var cmd = new SqlCommand("addSubjectSetMember", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("setId", Id));
            cmd.Parameters.Add(new SqlParameter("subjectId", subject.ID));
            cmd.ExecuteNonQuery();
            return true;
        }
    
    }
}
