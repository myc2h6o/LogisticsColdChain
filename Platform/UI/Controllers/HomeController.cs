namespace UI.Controllers
{
    using System.Configuration;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.title = "Logistics Platform";
            ViewBag.endpoint = ConfigurationManager.AppSettings["endpoint"];
            return View();
        }
    }
}