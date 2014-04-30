using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gallery.Controllers
{
    public class SubjectController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(String name)
        {
            return View();
        }

	}
}