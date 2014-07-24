
using log4net.logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wedding.logic;

namespace ginger.aalwyn.co.za.Controllers
{
    public class HomeController : Controller
    {
        IWeddingLogic _context;
        ILogger _logger;
        public HomeController(IWeddingLogic context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }
        public ActionResult Index()
        {
            _logger.Log.Info("Requesting initial page");           
            string domain = GetDomain();
            var weddingPersons = _context.GetWeddingByDomain(domain);
            return View(weddingPersons);
        }

        public ActionResult Main()
        {
            string domain = GetDomain();
            var weddingPersons = _context.GetWeddingByDomain(domain);
            return View(weddingPersons);
        }

        public ActionResult Guestbook()
        {
            string domain = GetDomain();
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
