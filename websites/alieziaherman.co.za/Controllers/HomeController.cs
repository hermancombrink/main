using log4net.logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wedding.logic;

namespace alieziaherman.co.za.Controllers
{
    public class HomeController : Controller
    {

        IWeddingLogic _context;
        ILogger _logger;
        private const string TAG = "HomeController";
        private string _weddingIdentifier
        {
            get
            {
                var result = new System.Configuration.AppSettingsReader().GetValue("weddingIdentifier", typeof(string));
                if (result == null)
                {
                    throw _logger.GetRaiseException("Failed to get wedding configuration", TAG);
                }
                return result.ToString();
            }
        }

        public HomeController(IWeddingLogic context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Aliezia & Herman's Wedding";
            
            return View();
        }


        public ActionResult Main()
        {
            return View(_context.GetWeddingByDomain(_weddingIdentifier));
        }

        public ActionResult Date()
        {
            return View(_context.GetWeddingByDomain(_weddingIdentifier));
        }

        public ActionResult ContactUs()
        {
            return View(_context.GetWeddingByDomain(_weddingIdentifier));
        }

        public ActionResult GettingThere()
        {
            return View(_context.GetWeddingByDomain(_weddingIdentifier));
        }

        public ActionResult OurStory()
        {
            return View(_context.GetWeddingByDomain(_weddingIdentifier));
        }
    }
}
