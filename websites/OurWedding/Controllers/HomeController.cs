using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OurWedding.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Aliezia & Herman's Wedding";
            return View();
        }
     

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult Date()
        {
            return View();
        }

        public ActionResult Event()
        {
            return View();
        }

        public ActionResult GettingThere()
        {
            return View();
        }

        public ActionResult OurStory()
        {
            return View();
        }

        public ActionResult RSVP()
        {
            return View();
        }
    }
}
