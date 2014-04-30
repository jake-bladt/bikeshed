using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Gallery.Entities.Elections;
using Gallery.Entities.Subjects;

namespace Gallery.Controllers
{
    public class ElectionController : Controller
    {
        protected IElectionReader _Reader;

        public ElectionController()
        {
            var cnStr = ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString;
            _Reader = new SqlElectionReader(cnStr);
        }

        public ActionResult Index()
        {
            ViewBag.Elections = _Reader.GetAllElections();
            return View();
        }

        public ActionResult Details(int id)
        {
            return View(_Reader.GetElection(id));
        }

	}
}