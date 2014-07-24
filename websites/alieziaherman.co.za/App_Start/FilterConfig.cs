using System.Web;
using System.Web.Mvc;

namespace alieziaherman.co.za
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}