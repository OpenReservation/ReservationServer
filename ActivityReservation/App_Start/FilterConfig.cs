using System.Web.Mvc;
using ActivityReservation.Filters;

namespace ActivityReservation
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //disable applicationinsights
            //filters.Add(new ErrorHandler.AiHandleErrorAttribute());
            //custom filter
            filters.Add(new ErrorHandlerAttribute()); //
        }
    }
}