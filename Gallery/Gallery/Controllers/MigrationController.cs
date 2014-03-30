using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Gallery.Entities.Elections;
using Gallery.Entities.ImageGallery;
using Gallery.Migration;

namespace Gallery.Controllers
{
    public class MigrationController : Controller
    {
        [HttpPost]
        public ActionResult DirToElection(string dirPath, string electionName, DateTime? eventDate = null)
        {

            var galleryDbConn = ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString;

            var gallery = new SqlTrackedImageGallery(galleryDbConn);
            var helper = new ElectionMigrationHelper(gallery);
            var date = eventDate ?? DateTime.Now;
            bool result = helper.MigrateDirectoryToDB(dirPath, electionName, date, ElectionType.RunOff);

            return Json(new { Success = result });
        }
	}
}