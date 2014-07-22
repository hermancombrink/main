using System.Web;
using System.Web.Mvc;

namespace hermancombrink.co.za
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}