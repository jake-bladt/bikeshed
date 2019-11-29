using System;
using System.Data.SqlClient;
using System.Text;

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

                var oldSubject = String.Empty;
                StringBuilder lineBuilder = null;

                while(rdr.Read())
                {
                    var newSubject = rdr["Name"].ToString();
                    var catName = rdr["CategoryName"].ToString();

                    if(newSubject != oldSubject)
                    {
                        lineBuilder = new StringBuilder(newSubject);
                    }

                    lineBuilder.Append(" \"");
                    lineBuilder.Append(catName);
                    lineBuilder.Append("\"");
                    migrationCount++;

                    if (newSubject != oldSubject)
                    {
                        callback(lineBuilder.ToString());
                    }
                    oldSubject = newSubject;
                }
                callback(lineBuilder.ToString());
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
