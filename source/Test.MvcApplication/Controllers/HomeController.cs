using System.Web.Mvc;

namespace CacheByAttribute.Test.MvcApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DoSomething();
            DoSomethingElse();
            
            return View();
        }


        public ActionResult DropCacheOnFloor()
        {
            RemoveAllFromRegion();
            return View("Index");
        }
        [CacheRemoveAllFromRegion("ames")]
        private string RemoveAllFromRegion()
        {
            return "bob";
        }


        [Cache(Region = "ames")]
        private string DoSomething()
        {
            return "bob";
        }


        [Cache(KeyPrefix = "TheKey", Region = "ames")]
        private string DoSomethingElse()
        {
            return "bob";
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}