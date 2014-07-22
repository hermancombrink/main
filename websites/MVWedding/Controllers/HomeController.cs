using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVWedding.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string domain = GetDomain();
            var _context = new DbLogic.WeddingLogic();
            var weddingPersons = _context.GetWeddingByDomain(domain);
            return View(weddingPersons);
        }

        public ActionResult Main()
        {
            string domain = GetDomain();
            var _context = new DbLogic.WeddingLogic();
            var weddingPersons = _context.GetWeddingByDomain(domain);
            return View(weddingPersons);
        }

        public ActionResult Guestbook()
        {
            string domain = GetDomain();
            var _context = new DbLogic.WeddingLogic();
            var wedding = _context.GetAllGuests(domain);
            return View(wedding);
        }

        private String GetDomain()
        {
            string domain = this.Request.Url.DnsSafeHost;
            if (this.Request.Url.Port != 80)
                domain = string.Format("{0}:{1}", this.Request.Url.DnsSafeHost, this.Request.Url.Port.ToString());
            domain = domain.Replace("www.", "");
            domain = domain.Replace("www", "");
            return domain;
        }
    }
}
