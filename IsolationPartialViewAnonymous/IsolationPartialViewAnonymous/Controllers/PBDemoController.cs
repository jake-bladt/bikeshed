using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsolationPartialViewAnonymous.Controllers
{
    public class PBDemoController : Controller
    {
        //
        // GET: /PBDemo/

        [HttpGet]
        public ActionResult Index()
        {
            ViewData["ViewType"] = "First view (GET)";
            return View();
        }

        [HttpPost]
        public ActionResult Index(int neverUsedForAnything = 0)
        {
            ViewData["ViewType"] = "Postback (POST)";
            return View();
        }


    }
}
