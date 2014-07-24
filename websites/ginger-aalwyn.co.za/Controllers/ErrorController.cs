using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ginger.aalwyn.co.za.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Server()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

    }
}
