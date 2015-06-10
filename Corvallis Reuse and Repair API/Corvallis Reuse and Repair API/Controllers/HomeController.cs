using System.Web.Mvc;

namespace Corvallis_Reuse_and_Repair_API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
