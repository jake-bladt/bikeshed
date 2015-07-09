using System;
using FluentNHibernate.Mapping;

namespace nhc1
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string DisplayName { get; set; }
    }

    public class UserMap : ClassMap<User>
    {

    }
}
