using System;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
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

            Console.ReadLine();
        }

        public static void CreateDB()
        {
            var cnStr = 
            Fluently.Configure()
                .Database(MsSqlConfiguration
                  .MsSql2012
                  .ConnectionString(ConnectionString))
                  .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                  .ExposeConfiguration(c => new SchemaUpdate(c).Execute(true, true))
                  .BuildSessionFactory();
        }

        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration
                  .MsSql2012
                  .ConnectionString(ConnectionString))
                  .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                  .BuildSessionFactory();
        }
    }
}
