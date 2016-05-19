using System.Web.Mvc;

namespace AdventureWorks.WebServices.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {
            return "You've setup Adventureworks Webservices!";
        }
    }
}