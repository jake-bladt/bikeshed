using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace annual
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 2)
            {
                var year = args[0];
                var path = args[1];
                var subjects = GetYearDisplayNames(year, 
                    ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString);
                subjects.ForEach(s => Console.WriteLine(s));

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

    }
}
