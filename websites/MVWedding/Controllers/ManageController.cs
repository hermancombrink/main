using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVWedding.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ManageController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        #region Angular Views
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult Wedding()
        {
            return View();
        }


        public ActionResult Bridesmaids()
        {
            return View();
        }
        public ActionResult Groomsmen()
        {
            return View();
        }

        public ActionResult Location()
        {
            return View();
        }

        public ActionResult Guests()
        {
            return View();
        }

        public ActionResult Photos()
        {
            return View();
        }

        public ActionResult SaveDate()
        {
            return View();
        }

        public ActionResult CheckList()
        {
            return View();
        }
        #endregion
    }
}
