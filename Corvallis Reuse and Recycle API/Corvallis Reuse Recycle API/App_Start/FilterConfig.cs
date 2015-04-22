using System.Web;
using System.Web.Mvc;

namespace Corvallis_Reuse_Recycle_API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
