

using System.Web.Mvc;

namespace AdventureWorks.WebServices
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            if (filters != null)
            {
                filters.Add(new HandleErrorAttribute());
            }
        }
    }
}