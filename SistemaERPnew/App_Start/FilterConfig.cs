using System.Web;
using System.Web.Mvc;
using SistemaERPnew.Models;
namespace SistemaERPnew
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new Authorize());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
