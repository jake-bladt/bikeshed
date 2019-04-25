using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace sneakers
{
    class Program
    {
        static void Main(string[] args)
        {
            if(1 == args.Length)
            {
                var path = args[0];
                var subjects = GetSneakers(GetDbConnectionString());
                if (CreateContest(subjects, ConfigurationManager.AppSettings["yearbookSource"], path))
                {
                    Console.WriteLine("Contest created.");
                }
                else
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
            Console.WriteLine("Usage: sneakers [directory]");
        }

        static List<String> GetSneakers(string cnStr)
        {
            var ret = new List<String>();

            var cn = new SqlConnection(cnStr);
            cn.Open();
            var cmd = new SqlCommand("getSneakIns", cn) { CommandType = CommandType.StoredProcedure };
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ret.Add(rdr["SubjectName"].ToString());
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

        private static string GetDbConnectionString()
        {
            var env = Environment.GetEnvironmentVariable("GALLERY_CONSTR");
            return String.IsNullOrEmpty(env) ?
                ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString : env;
        }

    }
}
