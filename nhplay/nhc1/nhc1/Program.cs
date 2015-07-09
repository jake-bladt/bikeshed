using System;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace nhc1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create database if it doesn't exist
            CreateDB();

            Console.ReadLine();
        }

        public static void CreateDB()
        {
            var cnStr = "server=.;database=nh1;Integrated Security=SSPI;";
            Fluently.Configure()
                .Database(MsSqlConfiguration
                  .MsSql2012
                  .ConnectionString(cnStr))
                  .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                  .ExposeConfiguration(c => new SchemaUpdate(c).Execute(true, false))
                  .BuildSessionFactory();
        }
    }
}
