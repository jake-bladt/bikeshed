using System;
using System.Data.SqlClient;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Migration
{
    public class CategoryMigrationHelper
    {
        protected String _cnStr;

        public CategoryMigrationHelper(string cn)
        {
            _cnStr = cn;
        }

        public int Migrate(Action<String> callback)
        {
            var migrationCount = 0;
            var cn = new SqlConnection(_cnStr);
            try
            {
                cn.Open();
                var cmd = new SqlCommand("getSubjectCategoryList", cn);
                var rdr = cmd.ExecuteReader();
                var exportLine = String.Empty;
                var oldSubject = String.Empty;
                while(rdr.Read())
                {
                    var newSubject = rdr["Name"].ToString();
                    if(newSubject == oldSubject)
                    {
                        var catName = rdr["CategoryName"].ToString();
                        exportLine += $" \"{catName}\"";
                        migrationCount++;
                    } else
                    {
                        callback(exportLine);
                        exportLine = newSubject;
                    }
                    oldSubject = newSubject;
                }
                callback(exportLine);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cn.Close();
            }

            return migrationCount;
        }

    }
}
