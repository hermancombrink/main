using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace hermancombrink.co.za.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index(int request)
        {
            
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            Task<string> t = client.GetDataAsync(request);
            string result = "0";
            result = await t;
            ViewBag.Message = result;
            ViewBag.MyValue = 0;
            return View();

        }
      
      ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
