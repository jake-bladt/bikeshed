using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace speccandir
{
    class Program
    {
        public static string ConnectionString;

        static void Main(string[] args)
        {
            ConnectionString = GetDbConnectionString();

            var categorizer = args[0];
            var targetPath = args[1];

            var candidates = GetCandidates(categorizer);
            var yearbook = ConfigurationManager.AppSettings["yearbookSource"];
            if(CreateContest(candidates, yearbook, targetPath))
            {
                Console.WriteLine("Directory created.");
            } 
            else
            {
                Console.WriteLine("Failed to create directory.");
            }
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

        public static List<String> GetCandidates(string categorizer)
        {
            var parts = categorizer.Split(':');
            if (parts.Length < 2) return GetCandidatesByCategory(categorizer);
            return GetCandidatesByCategorizer(parts[1]); // Should eventually fork on parts[0] == 'sp'
        }

        public static List<String> GetCandidatesByCategory(string category)
        {
            var ret = new List<String>();

            var cn = new SqlConnection(ConnectionString);
            cn.Open();
            var cmd = new SqlCommand("getSubjectsByCategory", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(
                new SqlParameter("@name", category)
            );
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ret.Add(rdr["Name"].ToString());
            }

            return ret;
        }

        public static List<String> GetCandidatesByCategorizer(string categorizer)
        {
            var ret = new List<String>();

            var cmd = CommandFromCategorizer(categorizer);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ret.Add(rdr["SubjectName"].ToString());
            }

            return ret;
        }

        // Categorizer is in the form of getSubjectsByCategoryIntersection;@cat1name=nature;@cat2name=lightning
        public static SqlCommand CommandFromCategorizer(string categorizer)
        {
            var cn = new SqlConnection(ConnectionString);
            cn.Open();

            var parts = categorizer.Split(';');
            var cmdName = parts[0];
            var ret = new SqlCommand(cmdName, cn) { CommandType = CommandType.StoredProcedure };
            for(int i=1; i < parts.Length; i++)
            {
                var argParts = parts[i].Split('=');
                var param = new SqlParameter(argParts[0], argParts[1]);
                ret.Parameters.Add(param);
            }
            return ret;
        }

        private static string GetDbConnectionString()
        {
            var env = Environment.GetEnvironmentVariable("GALLERY_CONSTR");
            return String.IsNullOrEmpty(env) ?
                ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString : env;
        }

    }
}
