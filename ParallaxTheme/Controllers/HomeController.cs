using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParallaxTheme.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Portfolio()
        {
            return View();
        }

        public ActionResult Career()
        {
            return View();
        }

        public ActionResult Skills()
        {
            return View();
        }

        public ActionResult References()
        {
            return View();
        }

        public ActionResult Studies()
        {
            return View();
        }

        public ActionResult Hobbies()
        {
            return View();
        }

        public ActionResult Gallery()
        {
            return View();
        }
    }
}
