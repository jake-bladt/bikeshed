using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace imageresult.Controllers
{
    public class KittenController : Controller
    {
        public ActionResult Image(string id)
        {
            var imgDir = Server.MapPath(@"/Content/Images");
            var path = Path.Combine(imgDir, id + ".jpg");
            return File(path, "image/jpeg"); 
        }

    }
}
