using System;
using System.Linq;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;

namespace nhc1
{
    class Program
    {
        protected static string ConnectionString = "server=.;database=nh1;Integrated Security=SSPI;";

        static void Main(string[] args)
        {
            // Create database if it doesn't exist
            CreateDB();
            var user = CreateUser();


            Console.ReadLine();
        }

        public static User CreateUser()
        {
            var factory = CreateSessionFactory();
            using (var session = factory.OpenSession())
            {
                var ret = session.Get<User>(1);
                if (null == ret)
                {
                    ret = new User { DisplayName = "jake" };
                    session.Save(ret);
                }
                return ret;
            }
        }

        public static void CreateDB()
        {
            GetDBConfig().ExposeConfiguration(c => new SchemaUpdate(c).Execute(true, true)).BuildSessionFactory();
        }

        public static ISessionFactory CreateSessionFactory()
        {
            return GetDBConfig().BuildSessionFactory();
        }

        public static FluentConfiguration GetDBConfig()
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration
                  .MsSql2012
                  .ConnectionString(ConnectionString))
                  .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>());
        }
    }
}
