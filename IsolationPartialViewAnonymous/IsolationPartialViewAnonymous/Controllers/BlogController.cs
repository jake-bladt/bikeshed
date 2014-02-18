using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsolationPartialViewAnonymous.Controllers
{
    public class BlogController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Title(dynamic args)
        {
            return View(args);
        }

        public ActionResult Admin(string arg = "Default Value")
        {
            ViewData["arg"] = arg;
            return View();
        }

        private void DoStuff()
        {
            var x = Enumerable.Range(-4, 100).ToArray();
        }

    }
}
