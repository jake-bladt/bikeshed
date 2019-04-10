using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace annual
{
    class Program
    {
        private static string GetDbConnectionString()
        {
            var env = Environment.GetEnvironmentVariable("GALLERY_CONSTR");
            return String.IsNullOrEmpty(env) ?
                ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString : env;
        }

        static void Main(string[] args)
        {
            if(args.Length == 2)
            {
                var year = args[0];
                var path = args[1];
                var subjects = GetYearDisplayNames(year, GetDbConnectionString());
                if(CreateContest(subjects, ConfigurationManager.AppSettings["yearbookSource"], path))
                {
                    Console.WriteLine("Contest created.");
                } else
                {
                    Console.WriteLine("Failed to create contest.");
                }

                Console.ReadLine();
            }
            else
            {
                Usage();
            }
        }

        static void Usage()
        {
            Console.WriteLine("Usage: annual [year] [directory]");
        }

        static List<String> GetYearDisplayNames(string year, string cnStr)
        {
            var ret = new List<String>();

            var cn = new SqlConnection(cnStr);
            cn.Open();
            var cmd = new SqlCommand("getYearSubjects", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("year", year));
            var rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                ret.Add(rdr["name"].ToString());
            }

            return ret;
        }

        static bool CreateContest(List<String> subjects, string srcDir, string targetDir)
        {
            if (!Directory.Exists(srcDir) || !Directory.Exists(targetDir)) return false;

            subjects.ForEach(s => {
                Console.WriteLine(s);
                File.Copy(Path.Combine(srcDir, s + ".jpg"), Path.Combine(targetDir, s + ".jpg"));
            });

            return true;
        }

    }
}
