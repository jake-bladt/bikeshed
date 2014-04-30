using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Gallery.Entities.Subjects;

namespace Gallery.Controllers
{
    public class SubjectController : Controller
    {
        protected ISubjectReader _Reader;

        public SubjectController()
        {
            var cnStr = ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString;
            _Reader = new SqlSubjectReader(cnStr);
        }

        public ActionResult Index()
        {
            ViewBag.Subjects = _Reader.GetAllSubjects();
            return View();
        }

        public ActionResult Details(String name)
        {
            ViewBag.Subject = _Reader.GetSubject(name);
            return View();
        }

	}
}