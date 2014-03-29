using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gallery.Controllers
{
    public class MigrationController : Controller
    {
        [HttpPost]
        public ActionResult DirToElection(string dirPath, string electionName, DateTime? eventDate = null)
        {
            return Json(new { Result = "OK" });
        }
	}
}