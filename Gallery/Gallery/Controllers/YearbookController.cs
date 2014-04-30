using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gallery.Controllers
{
    public class YearbookController : Controller
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public FileResult Image(string name)
        {
            var yearbookRoot = ConfigurationManager.AppSettings["yearbookLocation"];
            var imagePath = Path.Combine(yearbookRoot, name + ".jpg");
            if (System.IO.File.Exists(imagePath))
            {
                return new FileStreamResult(new FileStream(imagePath, FileMode.Open), @"image/jpeg");
            }
            else
            {
                return null;
            }
        }
	}
}