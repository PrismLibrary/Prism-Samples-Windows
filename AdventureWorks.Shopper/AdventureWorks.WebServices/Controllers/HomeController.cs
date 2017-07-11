using System.Web.Mvc;

namespace AdventureWorks.WebServices.Controllers
{
    public class HomeController : Controller {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public string Index()
        {
            return "You've setup Adventureworks Webservices!";
        }
    }
}