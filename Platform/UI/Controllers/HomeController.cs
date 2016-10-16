namespace UI.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.title = "Logistics Platform";
            return View();
        }
    }
}