using Gallery.Entities.ImageGallery;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Entities.Elections
{
    public class SqlBackedElectionWriter : IElectionWriter
    {
        protected string _ConnectionString;
        protected IImageGallery _Gallery;

        public SqlBackedElectionWriter(IImageGallery gallery, string cn)
        {
            _Gallery = gallery;
            _ConnectionString = cn;
        }

        public bool AddWiners(int electionId, List<SingleElectionResult> results)
        {
            results.ForEach(res =>
            {
                var subjectId = _Gallery.Subjects[res.SubjectName].ID;
                var cn = new SqlConnection(_ConnectionString);
                cn.Open();
                var cmd = new SqlCommand("addElectionWinner", cn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add(new SqlParameter("electionId", electionId));
                cmd.Parameters.Add(new SqlParameter("winnerId", subjectId));
                cmd.Parameters.Add(new SqlParameter("rank", res.OrdinalRank));
                cmd.Parameters.Add(new SqlParameter("points", res.PointValue));
                cmd.ExecuteNonQuery();
                cn.Close();
            });
            return true;
        }

        public int Upsert(SingleElectionResult exemplar)
        {
            var cn = new SqlConnection(_ConnectionString);
            cn.Open();
            var cmd = new SqlCommand("getElection", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@name", exemplar.ElectionName);
            var rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                return (Int32)rdr["Id"];
            }
            else
            {
                var cn2 = new SqlConnection(_ConnectionString);
                cn2.Open();

                var cmd2 = new SqlCommand("addElection", cn) { CommandType = CommandType.StoredProcedure };
                cmd2.Parameters.Add(new SqlParameter("name", exemplar.ElectionName));
                cmd2.Parameters.Add(new SqlParameter("date", exemplar.EventDate));
                cmd2.Parameters.Add(new SqlParameter("winnerCount", exemplar.PointValue));
                cmd2.Parameters.Add(new SqlParameter("typeId", (int)exemplar.EventType));
                return (int)cmd.ExecuteScalar();
            }
        }
    }
}
